using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace iCal_File_Generator.Controls
{
    public partial class RecurrenceInputView : UserControl
    {
        ComboBox frequencyComboBox;
        RadioButton neverRecurRadioButton;
        RadioButton untilRecurRadioButton;
        DateTimePicker untilDate;
        DateTimePicker untilTime;

        public RecurrenceInputView()
        {
            InitializeComponent();
            InitializeRecurrenceForm();
        }

        private void InitializeRecurrenceForm()
        {
            Label frequencyLabel = new Label();
            neverRecurRadioButton = new RadioButton();
            untilRecurRadioButton = new RadioButton();
            untilDate = new DateTimePicker();
            untilTime = new DateTimePicker();

            frequencyComboBox = new ComboBox();

            List<string> frequencies = new List<string>()
            {
                "Once", "Daily", "Weekly", "Monthly", "Yearly"
            };

            frequencyLabel.Text = "Frequency";
            frequencyLabel.Name = "frequencyLabel";
            frequencyComboBox.Name = "frequencyComboBox";
            this.Controls.Add(frequencyLabel);
            frequencyComboBox.DataSource = frequencies;
            frequencyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            frequencyComboBox.Location = new Point(frequencyLabel.Right, frequencyLabel.Location.Y);
            this.Controls.Add(frequencyComboBox);

            frequencyComboBox.SelectedIndexChanged += new EventHandler(frequencyComboBox_OnChange);
        }

        public string RecurFrequency
        {
            get { return frequencyComboBox.SelectedItem.ToString(); }
        }

        private void frequencyComboBox_OnChange(object send, EventArgs e)
        {
            Label endLabel = new Label();

            if (frequencyComboBox.SelectedItem.ToString() != "Once")
            {
                endLabel.Text = "End";
                endLabel.Location = new Point(0, frequencyComboBox.Bottom + 10);
                endLabel.Visible = true;
                this.Controls.Add(endLabel);

                neverRecurRadioButton.Text = "Never";
                neverRecurRadioButton.Checked = true;
                neverRecurRadioButton.Visible = true;
                neverRecurRadioButton.Location = new Point(endLabel.Right, endLabel.Location.Y);
                this.Controls.Add(neverRecurRadioButton);

                untilRecurRadioButton.Text = "Until";
                untilRecurRadioButton.Visible = true;
                untilRecurRadioButton.Location = new Point(endLabel.Right, endLabel.Bottom);
                this.Controls.Add(untilRecurRadioButton);

                untilDate.Visible = true;
                untilDate.Location = new Point(untilRecurRadioButton.Location.X, untilRecurRadioButton.Bottom);
                this.Controls.Add(untilDate);

                untilTime.Visible = true;
                untilTime.Location = new Point(untilRecurRadioButton.Location.X, untilDate.Bottom);
                untilTime.Size = new Size(98, 20);
                untilTime.ShowUpDown = true;
                untilTime.Format = DateTimePickerFormat.Time;
                this.Controls.Add(untilTime);
            }
            else
            {
                foreach (Control item in this.Controls)
                {
                    if (item.Name != "frequencyComboBox" && item.Name != "frequencyLabel") { item.Visible = false; }
                }
            }
        }

        public string RecurUntil
        {
            get { return (untilRecurRadioButton != null && untilRecurRadioButton.Checked) ? untilDate.Value.ToString("yyyy/MM/dd") + " " + untilTime.Value.TimeOfDay.ToString() : ""; }
        }

        public void UpdateRecurrence(DataAccess db, int index)
        {
            frequencyComboBox.SelectedItem = db.GetEvents()[index].recurFrequency;
            if (db.GetEvents()[index].recurUntil != "")
            {
                untilRecurRadioButton.Checked = true;
                untilDate.Value = DateTime.Parse(db.GetEvents()[index].recurUntil);
                untilTime.Value = DateTime.Parse(db.GetEvents()[index].recurUntil);
            }
        }
    }
}
