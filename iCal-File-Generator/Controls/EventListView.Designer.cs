namespace iCal_File_Generator.Controls
{
    partial class EventListView
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
            this.eventsListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // eventsListBox
            // 
            this.eventsListBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eventsListBox.FormattingEnabled = true;
            this.eventsListBox.ItemHeight = 19;
            this.eventsListBox.Location = new System.Drawing.Point(0, 0);
            this.eventsListBox.Name = "eventsListBox";
            this.eventsListBox.Size = new System.Drawing.Size(402, 365);
            this.eventsListBox.TabIndex = 0;
            // 
            // EventListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.eventsListBox);
            this.Name = "EventListView";
            this.Size = new System.Drawing.Size(402, 364);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox eventsListBox;
    }
}
