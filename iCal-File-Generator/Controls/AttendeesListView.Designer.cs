namespace iCal_File_Generator.Controls
{
    partial class AttendeesListView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.attendeePanel = new System.Windows.Forms.Panel();
            this.addAttendeeButton = new System.Windows.Forms.Button();
            this.attendeePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // attendeePanel
            // 
            this.attendeePanel.Controls.Add(this.addAttendeeButton);
            this.attendeePanel.Location = new System.Drawing.Point(3, 3);
            this.attendeePanel.Name = "attendeePanel";
            this.attendeePanel.Size = new System.Drawing.Size(396, 358);
            this.attendeePanel.TabIndex = 0;
            // 
            // addAttendeeButton
            // 
            this.addAttendeeButton.AutoSize = true;
            this.addAttendeeButton.Location = new System.Drawing.Point(0, 0);
            this.addAttendeeButton.Name = "addAttendeeButton";
            this.addAttendeeButton.Size = new System.Drawing.Size(82, 23);
            this.addAttendeeButton.TabIndex = 0;
            this.addAttendeeButton.Text = "Add Attendee";
            this.addAttendeeButton.UseVisualStyleBackColor = true;
            this.addAttendeeButton.Click += new System.EventHandler(this.addAttendeeButton_Click);
            // 
            // AttendeesListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.attendeePanel);
            this.Name = "AttendeesListView";
            this.Size = new System.Drawing.Size(402, 364);
            this.attendeePanel.ResumeLayout(false);
            this.attendeePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel attendeePanel;
        private System.Windows.Forms.Button addAttendeeButton;
    }
}
