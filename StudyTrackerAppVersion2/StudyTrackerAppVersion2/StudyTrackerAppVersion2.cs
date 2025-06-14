// StudyTracker Version 2.0
// Features: Refactored with StudySession class, summary by subject

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StudyTrackerAppVersion2
{
    public partial class Form1 : Form
    {
        // Stores study session objects
        private List<StudySession> sessions = new List<StudySession>();

        // File path for saving/loading session data
        private string filePath = "studysessions.txt";

        public Form1()
        {
            InitializeComponent(); // Initialize form controls
        }

        // Handles the "Add Session" button click
        private void btnAddSession_Click(object sender, EventArgs e)
        {
            string subject = txtSubject.Text.Trim();
            string timeSpent = txtTime.Text.Trim();

            // Validate input fields
            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(timeSpent))
            {
                MessageBox.Show("Please enter both subject and time spent.");
                return;
            }

            // Ensure time is a positive number
            if (!int.TryParse(timeSpent, out int minutes) || minutes <= 0)
            {
                MessageBox.Show("Please enter a valid number of minutes.");
                return;
            }

            // Create new session and add to list
            var session = new StudySession(DateTime.Now, subject, minutes);
            sessions.Add(session);

            // Display in ListBox
            lstSessions.Items.Add(session.ToString());

            // Clear input fields
            txtSubject.Clear();
            txtTime.Clear();
        }

        // Handles the "Save" button click
        private void btnSave_Click(object sender, EventArgs e)
        {
            List<string> lines = new List<string>();

            // Convert each session to a savable string
            foreach (var session in sessions)
            {
                lines.Add(session.ToFileString());
            }

            // Save to file
            File.WriteAllLines(filePath, lines);
            MessageBox.Show("Sessions saved successfully.");
        }

        // Handles the "Load" button click
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (File.Exists(filePath))
            {
                // Clear current data
                sessions.Clear();
                lstSessions.Items.Clear();

                // Read lines from file and recreate sessions
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var session = StudySession.FromFileString(line);
                    sessions.Add(session);
                    lstSessions.Items.Add(session.ToString());
                }

                MessageBox.Show("Sessions loaded successfully.");
            }
            else
            {
                MessageBox.Show("No saved file found.");
            }
        }

        // Handles the "Summary" button click
        private void btnSummary_Click(object sender, EventArgs e)
        {
            // Group by subject and sum minutes
            var summary = sessions
                .GroupBy(s => s.Subject)
                .Select(g => $"{g.Key}: {g.Sum(s => s.Minutes)} minutes")
                .ToArray();

            // Display summary in a message box
            MessageBox.Show(string.Join("\n", summary), "Summary by Subject");
        }
    }

    // Class to represent a single study session
    public class StudySession
    {
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public int Minutes { get; set; }

        // Constructor
        public StudySession(DateTime date, string subject, int minutes)
        {
            Date = date;
            Subject = subject;
            Minutes = minutes;
        }

        // Format for display in UI
        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd HH:mm} - {Subject} - {Minutes} mins";
        }

        // Format for saving to file
        public string ToFileString()
        {
            return $"{Date.Ticks}|{Subject}|{Minutes}";
        }

        // Create a StudySession from a saved line
        public static StudySession FromFileString(string line)
        {
            var parts = line.Split('|');
            return new StudySession(
                new DateTime(long.Parse(parts[0])), // Convert ticks back to DateTime
                parts[1],
                int.Parse(parts[2])
            );
        }
    }
}
