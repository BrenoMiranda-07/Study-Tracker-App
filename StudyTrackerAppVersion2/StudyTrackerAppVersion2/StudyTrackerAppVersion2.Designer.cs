namespace StudyTrackerAppVersion2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.btnAddSession = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSummary = new System.Windows.Forms.Button();
            this.lstSessions = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Subject:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Time (min):";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(176, 39);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(100, 20);
            this.txtSubject.TabIndex = 2;
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(176, 71);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(100, 20);
            this.txtTime.TabIndex = 3;
            // 
            // btnAddSession
            // 
            this.btnAddSession.Location = new System.Drawing.Point(176, 106);
            this.btnAddSession.Name = "btnAddSession";
            this.btnAddSession.Size = new System.Drawing.Size(75, 23);
            this.btnAddSession.TabIndex = 4;
            this.btnAddSession.Text = "Add Session";
            this.btnAddSession.UseVisualStyleBackColor = true;
            this.btnAddSession.Click += new System.EventHandler(this.btnAddSession_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(48, 159);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save Session";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(176, 159);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "Load Session";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSummary
            // 
            this.btnSummary.Location = new System.Drawing.Point(314, 159);
            this.btnSummary.Name = "btnSummary";
            this.btnSummary.Size = new System.Drawing.Size(75, 23);
            this.btnSummary.TabIndex = 7;
            this.btnSummary.Text = "View Summary";
            this.btnSummary.UseVisualStyleBackColor = true;
            this.btnSummary.Click += new System.EventHandler(this.btnSummary_Click);
            // 
            // lstSessions
            // 
            this.lstSessions.FormattingEnabled = true;
            this.lstSessions.Location = new System.Drawing.Point(51, 228);
            this.lstSessions.Name = "lstSessions";
            this.lstSessions.Size = new System.Drawing.Size(345, 212);
            this.lstSessions.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lstSessions);
            this.Controls.Add(this.btnSummary);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAddSession);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.txtSubject);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.Button btnAddSession;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSummary;
        private System.Windows.Forms.ListBox lstSessions;
    }
}

