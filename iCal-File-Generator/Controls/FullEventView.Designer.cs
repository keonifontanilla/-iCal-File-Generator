namespace iCal_File_Generator.Controls
{
    partial class FullEventView
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
            this.eventInfoTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
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
            this.eventInfoTextBox.TabIndex = 22;
            // 
            // FullEventView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.eventInfoTextBox);
            this.Name = "FullEventView";
            this.Size = new System.Drawing.Size(402, 364);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox eventInfoTextBox;
    }
}
