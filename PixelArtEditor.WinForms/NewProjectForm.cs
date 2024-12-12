using PixelArtEditor.WinForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelArtEditor.WinForms
{
    public partial class NewProjectForm : Form
    {
        private TextBox txtName;
        private NumericUpDown numWidth;
        private NumericUpDown numHeight;
        private Button btnOK;
        private Button btnCancel;
        private ErrorProvider errorProvider;

        public NewProjectForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "New Project";
            this.Size = new Size(300, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Controls
            var lblName = new Label { Text = "Project Name:", Location = new Point(10, 10) };
            txtName = new TextBox { Location = new Point(10, 30), Width = 260 };

            var lblWidth = new Label { Text = "Width:", Location = new Point(10, 60) };
            numWidth = new NumericUpDown
            {
                Location = new Point(10, 80),
                Minimum = 1,
                Maximum = 64,
                Value = 32
            };

            var lblHeight = new Label { Text = "Height:", Location = new Point(150, 60) };
            numHeight = new NumericUpDown
            {
                Location = new Point(150, 80),
                Minimum = 1,
                Maximum = 64,
                Value = 32
            };

            btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(10, 120)
            };
            btnOK.Click += BtnOK_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(100, 120)
            };

            errorProvider = new ErrorProvider();

            this.Controls.AddRange(new Control[] {
            lblName, txtName,
            lblWidth, numWidth,
            lblHeight, numHeight,
            btnOK, btnCancel
        });
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errorProvider.SetError(txtName, "Project name is required");
                return;
            }

            // Create new project
            using var context = new PixelArtDBContext();
            var project = new Project
            {
                Name = txtName.Text,
                CanvasWidth = (int)numWidth.Value,
                CanvasHeight = (int)numHeight.Value,
                CreationDate = DateTime.Now
            };
            context.Projects.Add(project);
            context.SaveChanges();
        }
    }
}
