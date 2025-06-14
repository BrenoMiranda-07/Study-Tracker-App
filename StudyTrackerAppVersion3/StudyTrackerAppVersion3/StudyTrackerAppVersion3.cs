// StudyTracker Version 3.0
// Features: Added chart visualization, weekly filter

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace StudyTrackerAppVersion3
{
    public partial class Form1 : Form
    {
        // List to store the user's study sessions
        private List<StudySession> sessions = new List<StudySession>();

        // Path to the file where study sessions are saved
        private string filePath = "studysessions.txt";

        public Form1()
        {
            InitializeComponent();
            InitializeChart();  // Initialize the chart for visualization
        }

        // Event handler for the "Add Session" button click
        private void btnAddSession_Click(object sender, EventArgs e)
        {
            // Get the subject and time spent from input fields
            string subject = txtSubject.Text.Trim();
            string timeSpent = txtTime.Text.Trim();

            // Validate input
            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(timeSpent))
            {
                MessageBox.Show("Please enter both subject and time spent.");
                return;
            }

            // Validate that time spent is a valid integer
            if (!int.TryParse(timeSpent, out int minutes) || minutes <= 0)
            {
                MessageBox.Show("Please enter a valid number of minutes.");
                return;
            }

            // Create a new study session and add it to the list
            var session = new StudySession(DateTime.Now, subject, minutes);
            sessions.Add(session);

            // Add session to the listbox and update chart
            lstSessions.Items.Add(session.ToString());
            UpdateChart();

            // Clear input fields
            txtSubject.Clear();
            txtTime.Clear();
        }

        // Event handler for the "Save" button click
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Prepare session data to be written to file
            List<string> lines = new List<string>();
            foreach (var session in sessions)
            {
                lines.Add(session.ToFileString());
            }

            // Write the session data to the file
            File.WriteAllLines(filePath, lines);
            MessageBox.Show("Sessions saved successfully.");
        }

        // Event handler for the "Load" button click
        private void btnLoad_Click(object sender, EventArgs e)
        {
            // Check if the session file exists
            if (File.Exists(filePath))
            {
                // Clear existing session data
                sessions.Clear();
                lstSessions.Items.Clear();

                // Read the file and load sessions
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var session = StudySession.FromFileString(line);
                    sessions.Add(session);
                    lstSessions.Items.Add(session.ToString());
                }

                // Update the chart with the loaded sessions
                UpdateChart();
                MessageBox.Show("Sessions loaded successfully.");
            }
            else
            {
                MessageBox.Show("No saved file found.");
            }
        }

        // Event handler for the "Summary" button click
        private void btnSummary_Click(object sender, EventArgs e)
        {
            // Group sessions by subject and calculate total minutes for each subject
            var summary = sessions
                .GroupBy(s => s.Subject)
                .Select(g => $"{g.Key}: {g.Sum(s => s.Minutes)} minutes")
                .ToArray();

            // Display summary as a message
            MessageBox.Show(string.Join("\n", summary), "Summary by Subject");
        }

        // Event handler for the "Filter Week" button click
        private void btnFilterWeek_Click(object sender, EventArgs e)
        {
            // Get the date for one week ago from today
            var oneWeekAgo = DateTime.Now.AddDays(-7);

            // Filter sessions to include only those from the last week
            var recent = sessions.Where(s => s.Date >= oneWeekAgo).ToList();

            // Clear the session listbox and display filtered sessions
            lstSessions.Items.Clear();
            foreach (var session in recent)
            {
                lstSessions.Items.Add(session.ToString());
            }

            // Update the chart to display only the filtered sessions
            UpdateChart(recent);
        }

        // Initializes the chart with default settings
        private void InitializeChart()
        {
            chartSummary.Series.Clear();  // Clear any existing series
            chartSummary.ChartAreas.Add(new ChartArea("MainArea"));  // Add chart area
            chartSummary.Series.Add("StudyMinutes");  // Add series to track study minutes
            chartSummary.Series["StudyMinutes"].ChartType = SeriesChartType.Column;  // Use column chart type
            chartSummary.Series["StudyMinutes"].XValueType = ChartValueType.String;  // Set X values as strings (subjects)
        }

        // Updates the chart with the provided data (or all sessions if no data is provided)
        private void UpdateChart(List<StudySession> data = null)
        {
            var source = data ?? sessions;  // Use provided data or default to all sessions

            // Group sessions by subject and sum the total minutes for each subject
            var summary = source
                .GroupBy(s => s.Subject)
                .Select(g => new { Subject = g.Key, TotalMinutes = g.Sum(s => s.Minutes) })
                .ToList();

            // Clear existing points in the chart
            chartSummary.Series["StudyMinutes"].Points.Clear();

            // Add the grouped data points to the chart
            foreach (var item in summary)
            {
                chartSummary.Series["StudyMinutes"].Points.AddXY(item.Subject, item.TotalMinutes);
            }
        }
    }

    // Represents a study session with date, subject, and time spent (in minutes)
    public class StudySession
    {
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public int Minutes { get; set; }

        // Constructor to initialize a new study session
        public StudySession(DateTime date, string subject, int minutes)
        {
            Date = date;
            Subject = subject;
            Minutes = minutes;
        }

        // Converts the session data to a string representation
        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd HH:mm} - {Subject} - {Minutes} mins";
        }

        // Converts the session to a file-compatible string format
        public string ToFileString()
        {
            return $"{Date.Ticks}|{Subject}|{Minutes}";
        }

        // Creates a study session from a file string
        public static StudySession FromFileString(string line)
        {
            var parts = line.Split('|');
            return new StudySession(new DateTime(long.Parse(parts[0])), parts[1], int.Parse(parts[2]));
        }
    }
}