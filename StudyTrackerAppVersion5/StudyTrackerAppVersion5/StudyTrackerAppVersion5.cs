// Required namespaces for basic system operations, collections, file handling, LINQ, forms, charts, and dialogs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.VisualBasic;

namespace StudyTrackerAppVersion5
{
    public partial class Form1 : Form
    {
        // Stores all study sessions for the currently logged-in user
        private List<StudySession> sessions = new List<StudySession>();

        // Stores the username of the current user
        private string currentUser = "";

        // List of subjects approved by NCEA
        private readonly List<string> approvedSubjects = new List<string>
        {
            "English", "Maths", "Biology", "Chemistry", "Physics",
            "History", "Geography", "Economics", "Accounting", "Business Studies",
            "Digital Technologies", "Classical Studies", "Art History", "Drama",
            "Music", "Health", "Physical Education", "Te Reo Māori", "Japanese",
            "Chinese", "French", "Spanish", "German", "Samoan", "Cook Islands Māori"
        };

        // Constructor to initialize the form and wire up events
        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(MainForm_Load); // Load event for form initialization

            // Connect Save and Load button click events to their respective methods
            btnSave.Click += btnSave_Click;
            btnLoad.Click += btnLoad_Click;
        }

        // ---------------- FORM LOAD ----------------

        // Executed when the form loads
        private void MainForm_Load(object sender, EventArgs e)
        {
            cmbCategory.SelectedIndex = 0; // Set default selected index for category dropdown
            lblUserStatus.Text = "Not logged in."; // Display default user status
        }

        // ---------------- USER AUTHENTICATION ----------------

        // Handles user login
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (AuthenticateUser(username, password))
            {
                currentUser = username;
                lblUserStatus.Text = $"Logged in as: {currentUser}";
                LoadSessionsForUser(); // Load sessions for this user
            }
            else
            {
                MessageBox.Show("Invalid login. Try again.");
            }
        }

        // Handles user registration
        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Enter both username and password.");
                return;
            }

            if (UsernameExists(username))
            {
                MessageBox.Show("Username already exists.");
                return;
            }

            RegisterUser(username, password);
            MessageBox.Show("Registration successful!");
        }

        // Validates user credentials
        private bool AuthenticateUser(string username, string password)
        {
            if (!File.Exists("users.txt")) return false;

            return File.ReadAllLines("users.txt")
                       .Any(line => line == $"{username},{password}");
        }

        // Registers a new user by writing to file
        private void RegisterUser(string username, string password)
        {
            if (!File.Exists("users.txt")) File.Create("users.txt").Close();

            File.AppendAllText("users.txt", $"{username},{password}{Environment.NewLine}");
        }

        // Checks if a username already exists
        private bool UsernameExists(string username)
        {
            if (!File.Exists("users.txt")) return false;

            return File.ReadAllLines("users.txt")
                       .Any(line => line.StartsWith(username + ","));
        }

        // ---------------- SESSION MANAGEMENT ----------------

        // Loads sessions from file for the current user
        private void LoadSessionsForUser()
        {
            sessions.Clear();
            lstSessions.Items.Clear();

            string path = $"{currentUser}_sessions.txt";
            if (!File.Exists(path)) return;

            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 4)
                {
                    sessions.Add(new StudySession
                    {
                        Date = DateTime.Parse(parts[0]),
                        Subject = parts[1],
                        Category = parts[2],
                        Minutes = int.Parse(parts[3])
                    });
                }
            }

            UpdateSessionList(); // Update list view
            UpdateChart();       // Update chart
        }

        // Adds a new session
        private void btnAddSession_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSubject.Text) ||
                string.IsNullOrWhiteSpace(txtTime.Text) ||
                cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!int.TryParse(txtTime.Text, out int time) || time <= 0)
            {
                MessageBox.Show("Enter a valid number of minutes.");
                return;
            }

            string subject = txtSubject.Text.Trim();

            // Validate that the subject is NCEA-approved
            if (!approvedSubjects.Contains(subject, StringComparer.OrdinalIgnoreCase))
            {
                MessageBox.Show("Invalid subject. Please enter an NCEA-approved subject.");
                return;
            }

            // Create a new study session
            var session = new StudySession
            {
                Date = DateTime.Now,
                Subject = subject,
                Category = cmbCategory.SelectedItem.ToString(),
                Minutes = time
            };

            sessions.Add(session);
            SaveAllSessions();
            UpdateSessionList();
            UpdateChart();
        }

        // Saves all sessions to a file
        private void SaveAllSessions()
        {
            string path = $"{currentUser}_sessions.txt";
            var lines = sessions.Select(s => $"{s.Date},{s.Subject},{s.Category},{s.Minutes}");
            File.WriteAllLines(path, lines);
        }

        // Saves sessions manually via Save button
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentUser))
            {
                MessageBox.Show("You must be logged in to save sessions.");
                return;
            }

            SaveAllSessions();
            MessageBox.Show("Sessions saved successfully.");
        }

        // Loads sessions via Load button
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentUser))
            {
                MessageBox.Show("You must be logged in to load sessions.");
                return;
            }

            LoadSessionsForUser();
            MessageBox.Show("Sessions loaded successfully.");
        }

        // Updates the session list in the UI
        private void UpdateSessionList(List<StudySession> listToShow = null)
        {
            lstSessions.Items.Clear();
            var source = listToShow ?? sessions;

            foreach (var session in source)
            {
                lstSessions.Items.Add($"{session.Date.ToShortDateString()} - {session.Subject} ({session.Category}) - {session.Minutes} min");
            }
        }

        // Updates the chart based on current or filtered sessions
        private void UpdateChart(List<StudySession> listToShow = null)
        {
            var source = listToShow ?? sessions;
            chartSummary.Series.Clear();

            var series = new Series("Minutes")
            {
                ChartType = SeriesChartType.Column
            };

            var summary = source
                .GroupBy(s => s.Subject)
                .Select(g => new { Subject = g.Key, Total = g.Sum(s => s.Minutes) });

            foreach (var item in summary)
            {
                series.Points.AddXY(item.Subject, item.Total);
            }

            chartSummary.Series.Add(series);
        }

        // Filters sessions from the last 7 days
        private void btnFilterWeek_Click(object sender, EventArgs e)
        {
            var filtered = sessions.Where(s => s.Date >= DateTime.Now.AddDays(-7)).ToList();
            UpdateSessionList(filtered);
            UpdateChart(filtered);
        }

        // Filters sessions within a selected date range
        private void btnFilterRange_Click(object sender, EventArgs e)
        {
            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date;

            var filtered = sessions.Where(s => s.Date.Date >= from && s.Date.Date <= to).ToList();
            UpdateSessionList(filtered);
            UpdateChart(filtered);
        }

        // Displays total time studied per subject
        private void btnSummary_Click(object sender, EventArgs e)
        {
            var summary = sessions
                .GroupBy(s => s.Subject)
                .Select(g => new { Subject = g.Key, Total = g.Sum(s => s.Minutes) });

            string msg = "Summary:\n";
            foreach (var item in summary)
            {
                msg += $"{item.Subject}: {item.Total} min\n";
            }

            MessageBox.Show(msg);
        }

        // Allows the user to edit a selected session
        private void btnEdit_Click(object sender, EventArgs e)
        {
            int index = lstSessions.SelectedIndex;

            if (index < 0 || index >= sessions.Count)
            {
                MessageBox.Show("Please select a session to edit.");
                return;
            }

            StudySession selected = sessions[index];

            // Prompt user for new subject
            string newSubject = PromptInput("Edit Subject:", selected.Subject);
            if (string.IsNullOrWhiteSpace(newSubject)) return;

            // Validate subject against approved list
            if (!approvedSubjects.Contains(newSubject, StringComparer.OrdinalIgnoreCase))
            {
                MessageBox.Show("Invalid subject. Please enter an NCEA-approved subject.");
                return;
            }

            // Prompt and validate category
            string newCategory = PromptInput("Edit Category:", selected.Category);
            if (string.IsNullOrWhiteSpace(newCategory)) return;

            List<string> allowedCategories = new List<string> { "Maths", "English", "Science", "Other" };
            if (!allowedCategories.Contains(newCategory, StringComparer.OrdinalIgnoreCase))
            {
                MessageBox.Show("Invalid category. Please choose from: Maths, English, Science, Other.");
                return;
            }

            // Prompt and validate minutes
            string newMinutes = PromptInput("Edit Minutes:", selected.Minutes.ToString());
            if (!int.TryParse(newMinutes, out int updatedMinutes) || updatedMinutes <= 0)
            {
                MessageBox.Show("Invalid minutes.");
                return;
            }

            // Apply changes
            selected.Subject = newSubject;
            selected.Category = newCategory;
            selected.Minutes = updatedMinutes;

            SaveAllSessions();
            UpdateSessionList();
            UpdateChart();
        }

        // Shows an input dialog and returns the user's input
        private string PromptInput(string title, string currentValue)
        {
            return Microsoft.VisualBasic.Interaction.InputBox(title, "Edit", currentValue);
        }

        // Deletes a selected session
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = lstSessions.SelectedIndex;

            if (index < 0 || index >= sessions.Count)
            {
                MessageBox.Show("Please select a session to delete.");
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this session?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                sessions.RemoveAt(index);
                SaveAllSessions();
                UpdateSessionList();
                UpdateChart();
            }
        }
    }

    // Class representing a study session
    public class StudySession
    {
        public DateTime Date { get; set; }     // When the session took place
        public string Subject { get; set; }    // Subject studied
        public string Category { get; set; }   // Category (e.g., Maths, English)
        public int Minutes { get; set; }       // Time spent in minutes
    }
}
