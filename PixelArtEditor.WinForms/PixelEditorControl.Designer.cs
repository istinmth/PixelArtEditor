namespace PixelArtEditor.WinForms
{
    partial class PixelEditorControl
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
            frameList = new ListBox();
            btnNewFrame = new Button();
            btnDeleteFrame = new Button();
            pixelGrid = new DataGridView();
            colorPicker = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)pixelGrid).BeginInit();
            SuspendLayout();
            // 
            // frameList
            // 
            frameList.FormattingEnabled = true;
            frameList.ItemHeight = 15;
            frameList.Location = new Point(457, 90);
            frameList.Name = "frameList";
            frameList.Size = new Size(149, 394);
            frameList.TabIndex = 2;
            frameList.SelectedIndexChanged += frameList_SelectedIndexChanged_1;
            // 
            // btnNewFrame
            // 
            btnNewFrame.Location = new Point(457, 32);
            btnNewFrame.Name = "btnNewFrame";
            btnNewFrame.Size = new Size(149, 23);
            btnNewFrame.TabIndex = 0;
            btnNewFrame.Text = "Új frame";
            btnNewFrame.UseVisualStyleBackColor = true;
            // 
            // btnDeleteFrame
            // 
            btnDeleteFrame.Location = new Point(457, 61);
            btnDeleteFrame.Name = "btnDeleteFrame";
            btnDeleteFrame.Size = new Size(149, 23);
            btnDeleteFrame.TabIndex = 1;
            btnDeleteFrame.Text = "Frame törlése";
            btnDeleteFrame.UseVisualStyleBackColor = true;
            // 
            // pixelGrid
            // 
            pixelGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            pixelGrid.Location = new Point(3, 3);
            pixelGrid.Name = "pixelGrid";
            pixelGrid.RowTemplate.Height = 25;
            pixelGrid.Size = new Size(448, 568);
            pixelGrid.TabIndex = 1;
            // 
            // colorPicker
            // 
            colorPicker.FormattingEnabled = true;
            colorPicker.Location = new Point(457, 3);
            colorPicker.Name = "colorPicker";
            colorPicker.Size = new Size(121, 23);
            colorPicker.TabIndex = 0;
            // 
            // PixelEditorControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnNewFrame);
            Controls.Add(frameList);
            Controls.Add(btnDeleteFrame);
            Controls.Add(colorPicker);
            Controls.Add(pixelGrid);
            Name = "PixelEditorControl";
            Size = new Size(618, 574);
            ((System.ComponentModel.ISupportInitialize)pixelGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private DataGridView pixelGrid;
        private ListBox frameList;
        private Button btnNewFrame;
        private Button btnDeleteFrame;
        private ComboBox colorPicker;
    }
}
