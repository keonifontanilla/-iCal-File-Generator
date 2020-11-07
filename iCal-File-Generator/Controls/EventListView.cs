using System.Drawing;
using System.Windows.Forms;

namespace iCal_File_Generator.Controls
{
    /// <summary>
    /// This partial class provides a user control that adds events to a listBox and displays all events.
    /// </summary>
    public partial class EventListView : UserControl
    {
        DataAccess db;

        public EventListView(DataAccess db)
        {
            this.db = db;

            InitializeComponent();
            InitializeListbox();
        }

        private void InitializeListbox()
        {
            eventsListBox.DataSource = db.ListEvents();
            eventsListBox.DrawMode = DrawMode.OwnerDrawVariable;
            eventsListBox.MeasureItem += new MeasureItemEventHandler(eventsListBox_MeasureItem);
            eventsListBox.DrawItem += new DrawItemEventHandler(eventsListBox_DrawItem);
            this.Controls.Add(this.eventsListBox);
        }

        private void eventsListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            foreach (string item in eventsListBox.Items)
            {
                //Set the Height of the item at index 2 to 50
                if (e.Index == eventsListBox.Items.IndexOf(item)) { e.ItemHeight = 100; }
            }
        }

        private void eventsListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index != -1) { e.Graphics.DrawString(eventsListBox.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds); }
        }

        /// <summary>
        /// Gets the index of the selected event in the listBox.
        /// </summary>
        /// <returns>Returns the index of the selected lisBox item.</returns>
        public int Index()
        {
            return eventsListBox.SelectedIndex;
        }
    }
}
