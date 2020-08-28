namespace iCal_File_Generator
{
    partial class EventForm
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.submitButton = new System.Windows.Forms.Button();
            this.startDatePicker = new System.Windows.Forms.DateTimePicker();
            this.startTimePicker = new System.Windows.Forms.DateTimePicker();
            this.startDateLabel = new System.Windows.Forms.Label();
            this.endDateLabel = new System.Windows.Forms.Label();
            this.endTimePicker = new System.Windows.Forms.DateTimePicker();
            this.endDatePicker = new System.Windows.Forms.DateTimePicker();
            this.eventsListBox = new System.Windows.Forms.ListBox();
            this.timezoneComboBox = new System.Windows.Forms.ComboBox();
            this.timezoneLabel = new System.Windows.Forms.Label();
            this.classificationComboBox = new System.Windows.Forms.ComboBox();
            this.classificationLabel = new System.Windows.Forms.Label();
            this.viewButton = new System.Windows.Forms.Button();
            this.eventInfoTextBox = new System.Windows.Forms.TextBox();
            this.viewPanel = new System.Windows.Forms.Panel();
            this.panelCloseButton = new System.Windows.Forms.Button();
            this.updateButton = new System.Windows.Forms.Button();
            this.viewPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(22, 65);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(33, 13);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Title: ";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new System.Drawing.Point(122, 65);
            this.titleTextBox.MaxLength = 255;
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(244, 20);
            this.titleTextBox.TabIndex = 1;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(122, 106);
            this.descriptionTextBox.MaxLength = 500;
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(244, 105);
            this.descriptionTextBox.TabIndex = 3;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(22, 106);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(66, 13);
            this.descriptionLabel.TabIndex = 2;
            this.descriptionLabel.Text = "Description: ";
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(186, 377);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 4;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // startDatePicker
            // 
            this.startDatePicker.CustomFormat = "";
            this.startDatePicker.Location = new System.Drawing.Point(122, 305);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(200, 20);
            this.startDatePicker.TabIndex = 5;
            this.startDatePicker.Value = new System.DateTime(2020, 5, 23, 14, 18, 50, 0);
            this.startDatePicker.ValueChanged += new System.EventHandler(this.startDatePicker_ValueChanged);
            // 
            // startTimePicker
            // 
            this.startTimePicker.CustomFormat = "";
            this.startTimePicker.Location = new System.Drawing.Point(342, 305);
            this.startTimePicker.Name = "startTimePicker";
            this.startTimePicker.Size = new System.Drawing.Size(98, 20);
            this.startTimePicker.TabIndex = 6;
            // 
            // startDateLabel
            // 
            this.startDateLabel.AutoSize = true;
            this.startDateLabel.Location = new System.Drawing.Point(22, 305);
            this.startDateLabel.Name = "startDateLabel";
            this.startDateLabel.Size = new System.Drawing.Size(58, 13);
            this.startDateLabel.TabIndex = 8;
            this.startDateLabel.Text = "Start Date:";
            // 
            // endDateLabel
            // 
            this.endDateLabel.AutoSize = true;
            this.endDateLabel.Location = new System.Drawing.Point(22, 331);
            this.endDateLabel.Name = "endDateLabel";
            this.endDateLabel.Size = new System.Drawing.Size(55, 13);
            this.endDateLabel.TabIndex = 11;
            this.endDateLabel.Text = "End Date:";
            // 
            // endTimePicker
            // 
            this.endTimePicker.CustomFormat = "";
            this.endTimePicker.Location = new System.Drawing.Point(342, 331);
            this.endTimePicker.Name = "endTimePicker";
            this.endTimePicker.Size = new System.Drawing.Size(98, 20);
            this.endTimePicker.TabIndex = 10;
            // 
            // endDatePicker
            // 
            this.endDatePicker.CustomFormat = "";
            this.endDatePicker.Location = new System.Drawing.Point(122, 331);
            this.endDatePicker.Name = "endDatePicker";
            this.endDatePicker.Size = new System.Drawing.Size(200, 20);
            this.endDatePicker.TabIndex = 9;
            this.endDatePicker.Value = new System.DateTime(2020, 5, 23, 14, 18, 50, 0);
            // 
            // eventsListBox
            // 
            this.eventsListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eventsListBox.FormattingEnabled = true;
            this.eventsListBox.ItemHeight = 24;
            this.eventsListBox.Location = new System.Drawing.Point(527, 65);
            this.eventsListBox.Name = "eventsListBox";
            this.eventsListBox.Size = new System.Drawing.Size(402, 364);
            this.eventsListBox.TabIndex = 12;
            // 
            // timezoneComboBox
            // 
            this.timezoneComboBox.FormattingEnabled = true;
            this.timezoneComboBox.Location = new System.Drawing.Point(122, 228);
            this.timezoneComboBox.Name = "timezoneComboBox";
            this.timezoneComboBox.Size = new System.Drawing.Size(244, 21);
            this.timezoneComboBox.TabIndex = 13;
            // 
            // timezoneLabel
            // 
            this.timezoneLabel.AutoSize = true;
            this.timezoneLabel.Location = new System.Drawing.Point(22, 228);
            this.timezoneLabel.Name = "timezoneLabel";
            this.timezoneLabel.Size = new System.Drawing.Size(56, 13);
            this.timezoneLabel.TabIndex = 14;
            this.timezoneLabel.Text = "Timezone:";
            // 
            // classificationComboBox
            // 
            this.classificationComboBox.FormattingEnabled = true;
            this.classificationComboBox.Location = new System.Drawing.Point(122, 266);
            this.classificationComboBox.Name = "classificationComboBox";
            this.classificationComboBox.Size = new System.Drawing.Size(121, 21);
            this.classificationComboBox.TabIndex = 15;
            // 
            // classificationLabel
            // 
            this.classificationLabel.AutoSize = true;
            this.classificationLabel.Location = new System.Drawing.Point(24, 266);
            this.classificationLabel.Name = "classificationLabel";
            this.classificationLabel.Size = new System.Drawing.Size(71, 13);
            this.classificationLabel.TabIndex = 16;
            this.classificationLabel.Text = "Classification:";
            // 
            // viewButton
            // 
            this.viewButton.Location = new System.Drawing.Point(527, 447);
            this.viewButton.Name = "viewButton";
            this.viewButton.Size = new System.Drawing.Size(75, 23);
            this.viewButton.TabIndex = 17;
            this.viewButton.Text = "View";
            this.viewButton.UseVisualStyleBackColor = true;
            this.viewButton.Click += new System.EventHandler(this.viewButton_Click);
            // 
            // eventInfoTextBox
            // 
            this.eventInfoTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eventInfoTextBox.Location = new System.Drawing.Point(3, 0);
            this.eventInfoTextBox.Multiline = true;
            this.eventInfoTextBox.Name = "eventInfoTextBox";
            this.eventInfoTextBox.ReadOnly = true;
            this.eventInfoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.eventInfoTextBox.Size = new System.Drawing.Size(396, 306);
            this.eventInfoTextBox.TabIndex = 18;
            // 
            // viewPanel
            // 
            this.viewPanel.Controls.Add(this.panelCloseButton);
            this.viewPanel.Controls.Add(this.eventInfoTextBox);
            this.viewPanel.Location = new System.Drawing.Point(527, 65);
            this.viewPanel.Name = "viewPanel";
            this.viewPanel.Size = new System.Drawing.Size(402, 364);
            this.viewPanel.TabIndex = 19;
            this.viewPanel.Visible = false;
            // 
            // panelCloseButton
            // 
            this.panelCloseButton.Location = new System.Drawing.Point(165, 327);
            this.panelCloseButton.Name = "panelCloseButton";
            this.panelCloseButton.Size = new System.Drawing.Size(75, 23);
            this.panelCloseButton.TabIndex = 19;
            this.panelCloseButton.Text = "Close";
            this.panelCloseButton.UseVisualStyleBackColor = true;
            this.panelCloseButton.Click += new System.EventHandler(this.panelCloseButton_Click);
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(608, 447);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 20;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // EventForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 635);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.viewPanel);
            this.Controls.Add(this.viewButton);
            this.Controls.Add(this.classificationLabel);
            this.Controls.Add(this.classificationComboBox);
            this.Controls.Add(this.timezoneLabel);
            this.Controls.Add(this.timezoneComboBox);
            this.Controls.Add(this.eventsListBox);
            this.Controls.Add(this.endDateLabel);
            this.Controls.Add(this.endTimePicker);
            this.Controls.Add(this.endDatePicker);
            this.Controls.Add(this.startDateLabel);
            this.Controls.Add(this.startTimePicker);
            this.Controls.Add(this.startDatePicker);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.titleLabel);
            this.Name = "EventForm";
            this.Text = "Event File Generator";
            this.viewPanel.ResumeLayout(false);
            this.viewPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.DateTimePicker startDatePicker;
        private System.Windows.Forms.DateTimePicker startTimePicker;
        private System.Windows.Forms.Label startDateLabel;
        private System.Windows.Forms.Label endDateLabel;
        private System.Windows.Forms.DateTimePicker endTimePicker;
        private System.Windows.Forms.DateTimePicker endDatePicker;
        private System.Windows.Forms.ListBox eventsListBox;
        private System.Windows.Forms.ComboBox timezoneComboBox;
        private System.Windows.Forms.Label timezoneLabel;
        private System.Windows.Forms.ComboBox classificationComboBox;
        private System.Windows.Forms.Label classificationLabel;
        private System.Windows.Forms.Button viewButton;
        private System.Windows.Forms.TextBox eventInfoTextBox;
        private System.Windows.Forms.Panel viewPanel;
        private System.Windows.Forms.Button panelCloseButton;
        private System.Windows.Forms.Button updateButton;
    }
}

