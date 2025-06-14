// StudyTracker Version 1.0
// Features: Log study sessions, basic input validation, save/load from file

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Study_Tracker_App
{
    public partial class Form1 : Form
    {
        // List to store session entries in memory
        private List<string> sessions = new List<string>();

        // Path to the text file where sessions are saved/loaded
        private string filePath = "studysessions.txt";

        public Form1()
        {
            InitializeComponent(); // Initializes form controls
        }

        // Handles "Add Session" button click
        private void btnAddSession_Click(object sender, EventArgs e)
        {
            string subject = txtSubject.Text.Trim();
            string timeSpent = txtTime.Text.Trim();

            // Check that both fields are filled
            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(timeSpent))
            {
                MessageBox.Show("Please enter both subject and time spent.");
                return;
            }

            // Ensure the time is a valid positive number
            if (!int.TryParse(timeSpent, out int minutes) || minutes <= 0)
            {
                MessageBox.Show("Please enter a valid number of minutes.");
                return;
            }

            // Format the session entry with timestamp, subject, and minutes
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm} - {subject} - {minutes} mins";

            // Add to internal list and update ListBox
            sessions.Add(entry);
            lstSessions.Items.Add(entry);

            // Clear input fields
            txtSubject.Clear();
            txtTime.Clear();
        }

        // Handles "Save" button click - saves all sessions to file
        private void btnSave_Click(object sender, EventArgs e)
        {
            File.WriteAllLines(filePath, sessions);
            MessageBox.Show("Sessions saved successfully.");
        }

        // Handles "Load" button click - loads sessions from file if it exists
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (File.Exists(filePath))
            {
                // Clear current data
                sessions.Clear();
                lstSessions.Items.Clear();

                // Load from file and update UI
                sessions.AddRange(File.ReadAllLines(filePath));
                lstSessions.Items.AddRange(sessions.ToArray());
                MessageBox.Show("Sessions loaded successfully.");
            }
            else
            {
                MessageBox.Show("No saved file found.");
            }
        }
    }
}