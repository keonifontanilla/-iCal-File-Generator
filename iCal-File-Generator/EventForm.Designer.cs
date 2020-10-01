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
            this.timezoneComboBox = new System.Windows.Forms.ComboBox();
            this.timezoneLabel = new System.Windows.Forms.Label();
            this.classificationComboBox = new System.Windows.Forms.ComboBox();
            this.classificationLabel = new System.Windows.Forms.Label();
            this.viewButton = new System.Windows.Forms.Button();
            this.generateButton = new System.Windows.Forms.Button();
            this.updateButton = new System.Windows.Forms.Button();
            this.clearInputsButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.organizerLabel = new System.Windows.Forms.Label();
            this.organizerTextBox = new System.Windows.Forms.TextBox();
            this.repeatsLinkLabel = new System.Windows.Forms.LinkLabel();
            this.locationLabel = new System.Windows.Forms.Label();
            this.locationTextBox = new System.Windows.Forms.TextBox();
            this.eventTabControl = new System.Windows.Forms.TabControl();
            this.eventListTab = new System.Windows.Forms.TabPage();
            this.attendeeListTab = new System.Windows.Forms.TabPage();
            this.eventTabControl.SuspendLayout();
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
            this.submitButton.Location = new System.Drawing.Point(122, 447);
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
            this.startDatePicker.Location = new System.Drawing.Point(122, 351);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(200, 20);
            this.startDatePicker.TabIndex = 5;
            this.startDatePicker.Value = new System.DateTime(2020, 5, 23, 14, 18, 50, 0);
            this.startDatePicker.ValueChanged += new System.EventHandler(this.startDatePicker_ValueChanged);
            // 
            // startTimePicker
            // 
            this.startTimePicker.CustomFormat = "";
            this.startTimePicker.Location = new System.Drawing.Point(342, 351);
            this.startTimePicker.Name = "startTimePicker";
            this.startTimePicker.Size = new System.Drawing.Size(98, 20);
            this.startTimePicker.TabIndex = 6;
            // 
            // startDateLabel
            // 
            this.startDateLabel.AutoSize = true;
            this.startDateLabel.Location = new System.Drawing.Point(22, 351);
            this.startDateLabel.Name = "startDateLabel";
            this.startDateLabel.Size = new System.Drawing.Size(58, 13);
            this.startDateLabel.TabIndex = 8;
            this.startDateLabel.Text = "Start Date:";
            // 
            // endDateLabel
            // 
            this.endDateLabel.AutoSize = true;
            this.endDateLabel.Location = new System.Drawing.Point(22, 377);
            this.endDateLabel.Name = "endDateLabel";
            this.endDateLabel.Size = new System.Drawing.Size(55, 13);
            this.endDateLabel.TabIndex = 11;
            this.endDateLabel.Text = "End Date:";
            // 
            // endTimePicker
            // 
            this.endTimePicker.CustomFormat = "";
            this.endTimePicker.Location = new System.Drawing.Point(342, 377);
            this.endTimePicker.Name = "endTimePicker";
            this.endTimePicker.Size = new System.Drawing.Size(98, 20);
            this.endTimePicker.TabIndex = 10;
            // 
            // endDatePicker
            // 
            this.endDatePicker.CustomFormat = "";
            this.endDatePicker.Location = new System.Drawing.Point(122, 377);
            this.endDatePicker.Name = "endDatePicker";
            this.endDatePicker.Size = new System.Drawing.Size(200, 20);
            this.endDatePicker.TabIndex = 9;
            this.endDatePicker.Value = new System.DateTime(2020, 5, 23, 14, 18, 50, 0);
            // 
            // timezoneComboBox
            // 
            this.timezoneComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.timezoneComboBox.FormattingEnabled = true;
            this.timezoneComboBox.Location = new System.Drawing.Point(122, 312);
            this.timezoneComboBox.Name = "timezoneComboBox";
            this.timezoneComboBox.Size = new System.Drawing.Size(244, 21);
            this.timezoneComboBox.TabIndex = 13;
            // 
            // timezoneLabel
            // 
            this.timezoneLabel.AutoSize = true;
            this.timezoneLabel.Location = new System.Drawing.Point(22, 312);
            this.timezoneLabel.Name = "timezoneLabel";
            this.timezoneLabel.Size = new System.Drawing.Size(56, 13);
            this.timezoneLabel.TabIndex = 14;
            this.timezoneLabel.Text = "Timezone:";
            // 
            // classificationComboBox
            // 
            this.classificationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classificationComboBox.FormattingEnabled = true;
            this.classificationComboBox.Location = new System.Drawing.Point(356, 231);
            this.classificationComboBox.Name = "classificationComboBox";
            this.classificationComboBox.Size = new System.Drawing.Size(84, 21);
            this.classificationComboBox.TabIndex = 15;
            // 
            // classificationLabel
            // 
            this.classificationLabel.AutoSize = true;
            this.classificationLabel.Location = new System.Drawing.Point(279, 232);
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
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(773, 447);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(75, 23);
            this.generateButton.TabIndex = 20;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
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
            // clearInputsButton
            // 
            this.clearInputsButton.Location = new System.Drawing.Point(291, 447);
            this.clearInputsButton.Name = "clearInputsButton";
            this.clearInputsButton.Size = new System.Drawing.Size(75, 23);
            this.clearInputsButton.TabIndex = 21;
            this.clearInputsButton.Text = "Clear";
            this.clearInputsButton.UseVisualStyleBackColor = true;
            this.clearInputsButton.Click += new System.EventHandler(this.clearInputsButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(692, 447);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 22;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // organizerLabel
            // 
            this.organizerLabel.AutoSize = true;
            this.organizerLabel.Location = new System.Drawing.Point(22, 275);
            this.organizerLabel.Name = "organizerLabel";
            this.organizerLabel.Size = new System.Drawing.Size(55, 13);
            this.organizerLabel.TabIndex = 23;
            this.organizerLabel.Text = "Organizer:";
            // 
            // organizerTextBox
            // 
            this.organizerTextBox.Location = new System.Drawing.Point(122, 275);
            this.organizerTextBox.MaxLength = 255;
            this.organizerTextBox.Name = "organizerTextBox";
            this.organizerTextBox.Size = new System.Drawing.Size(244, 20);
            this.organizerTextBox.TabIndex = 24;
            // 
            // repeatsLinkLabel
            // 
            this.repeatsLinkLabel.AutoSize = true;
            this.repeatsLinkLabel.Location = new System.Drawing.Point(119, 402);
            this.repeatsLinkLabel.Name = "repeatsLinkLabel";
            this.repeatsLinkLabel.Size = new System.Drawing.Size(47, 13);
            this.repeatsLinkLabel.TabIndex = 26;
            this.repeatsLinkLabel.TabStop = true;
            this.repeatsLinkLabel.Text = "Repeats";
            this.repeatsLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.repeatsLinkLabel_LinkClicked);
            // 
            // locationLabel
            // 
            this.locationLabel.AutoSize = true;
            this.locationLabel.Location = new System.Drawing.Point(26, 231);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Size = new System.Drawing.Size(51, 13);
            this.locationLabel.TabIndex = 27;
            this.locationLabel.Text = "Location:";
            // 
            // locationTextBox
            // 
            this.locationTextBox.Location = new System.Drawing.Point(122, 232);
            this.locationTextBox.MaxLength = 255;
            this.locationTextBox.Name = "locationTextBox";
            this.locationTextBox.Size = new System.Drawing.Size(151, 20);
            this.locationTextBox.TabIndex = 28;
            // 
            // eventTabControl
            // 
            this.eventTabControl.Controls.Add(this.eventListTab);
            this.eventTabControl.Controls.Add(this.attendeeListTab);
            this.eventTabControl.Location = new System.Drawing.Point(520, 25);
            this.eventTabControl.Name = "eventTabControl";
            this.eventTabControl.SelectedIndex = 0;
            this.eventTabControl.Size = new System.Drawing.Size(414, 390);
            this.eventTabControl.TabIndex = 30;
            // 
            // eventListTab
            // 
            this.eventListTab.Location = new System.Drawing.Point(4, 22);
            this.eventListTab.Name = "eventListTab";
            this.eventListTab.Padding = new System.Windows.Forms.Padding(3);
            this.eventListTab.Size = new System.Drawing.Size(406, 364);
            this.eventListTab.TabIndex = 0;
            this.eventListTab.Text = "Events";
            this.eventListTab.UseVisualStyleBackColor = true;
            // 
            // attendeeListTab
            // 
            this.attendeeListTab.Location = new System.Drawing.Point(4, 22);
            this.attendeeListTab.Name = "attendeeListTab";
            this.attendeeListTab.Padding = new System.Windows.Forms.Padding(3);
            this.attendeeListTab.Size = new System.Drawing.Size(406, 364);
            this.attendeeListTab.TabIndex = 1;
            this.attendeeListTab.Text = "Attendees";
            this.attendeeListTab.UseVisualStyleBackColor = true;
            // 
            // EventForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 635);
            this.Controls.Add(this.eventTabControl);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.locationTextBox);
            this.Controls.Add(this.locationLabel);
            this.Controls.Add(this.repeatsLinkLabel);
            this.Controls.Add(this.organizerTextBox);
            this.Controls.Add(this.organizerLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.clearInputsButton);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.viewButton);
            this.Controls.Add(this.classificationLabel);
            this.Controls.Add(this.classificationComboBox);
            this.Controls.Add(this.timezoneLabel);
            this.Controls.Add(this.timezoneComboBox);
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
            this.eventTabControl.ResumeLayout(false);
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
        private System.Windows.Forms.ComboBox timezoneComboBox;
        private System.Windows.Forms.Label timezoneLabel;
        private System.Windows.Forms.ComboBox classificationComboBox;
        private System.Windows.Forms.Label classificationLabel;
        private System.Windows.Forms.Button viewButton;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button clearInputsButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Label organizerLabel;
        private System.Windows.Forms.TextBox organizerTextBox;
        private System.Windows.Forms.LinkLabel repeatsLinkLabel;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.TextBox locationTextBox;
        private System.Windows.Forms.TabControl eventTabControl;
        private System.Windows.Forms.TabPage eventListTab;
        private System.Windows.Forms.TabPage attendeeListTab;
    }
}

