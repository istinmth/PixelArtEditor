using ClosedXML.Excel;
using PixelArtEditor.WinForms.Models;
using System.Drawing;

namespace PixelArtEditor.WinForms
{
    public partial class MainForm : Form
    {
        private Panel mainContainer;
        private MenuStrip menuStrip;
        private Button btnEditor;
        private Button btnPreview;
        private Button btnStats;

        public MainForm()
        {
            InitializeComponent();
            InitializeUI();
            this.FormClosing += MainForm_FormClosing;
        }

        private void InitializeUI()
        {
            // Menu Strip
            menuStrip = new MenuStrip();
            var fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("New Project", null, NewProject_Click);
            fileMenu.DropDownItems.Add("Export", null, Export_Click);
            fileMenu.DropDownItems.Add("Exit", null, Exit_Click);
            menuStrip.Items.Add(fileMenu);
            this.Controls.Add(menuStrip);

            // Navigation Buttons Panel
            var buttonPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 100
            };

            btnEditor = new Button
            {
                Text = "Editor",
                Dock = DockStyle.Top,
                Height = 40
            };
            btnEditor.Click += BtnEditor_Click;

            btnPreview = new Button
            {
                Text = "Preview",
                Dock = DockStyle.Top,
                Height = 40
            };
            btnPreview.Click += BtnPreview_Click;

            btnStats = new Button
            {
                Text = "Statistics",
                Dock = DockStyle.Top,
                Height = 40
            };
            btnStats.Click += BtnStats_Click;

            buttonPanel.Controls.AddRange(new Control[] { btnEditor, btnPreview, btnStats });

            // Main Container
            mainContainer = new Panel
            {
                Dock = DockStyle.Fill
            };

            // Add controls
            this.Controls.Add(buttonPanel);
            this.Controls.Add(mainContainer);

            // Set form properties
            this.Size = new Size(800, 600);
            this.Text = "Pixel Art Editor";

            // Set the menu strip at the top
            menuStrip.Dock = DockStyle.Top;
            this.MainMenuStrip = menuStrip;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit",
                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        // Event handlers
        private void BtnEditor_Click(object sender, EventArgs e)
        {
            LoadUserControl(new PixelEditorControl());
        }

        private void BtnPreview_Click(object sender, EventArgs e)
        {
            LoadUserControl(new PreviewControl());
        }

        private void BtnStats_Click(object sender, EventArgs e)
        {
            LoadUserControl(new StatsControl());
        }

        private void LoadUserControl(UserControl userControl)
        {
            mainContainer.Controls.Clear();
            userControl.Dock = DockStyle.Fill;
            mainContainer.Controls.Add(userControl);
        }

        private void NewProject_Click(object sender, EventArgs e)
        {
            var newProjectForm = new NewProjectForm();
            if (newProjectForm.ShowDialog() == DialogResult.OK)
            {
                // Switch to the editor view with the new project
                var editorControl = new PixelEditorControl();
                LoadUserControl(editorControl);

                // Can add logic here to set the initial project settings
                MessageBox.Show("New project created successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Export_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                FileName = "PixelArtProject.xlsx"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var context = new PixelArtDBContext();
                    // Create Excel export
                    using var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Project Data");

                    // Add project info
                    worksheet.Cell("A1").Value = "Project Statistics";
                    worksheet.Cell("A2").Value = "Total Frames";
                    worksheet.Cell("B2").Value = context.Frames.Count();
                    worksheet.Cell("A3").Value = "Total Pixels";
                    worksheet.Cell("B3").Value = context.Pixels.Count();

                    // Add color usage
                    var colorStats = context.Pixels
                        .GroupBy(p => p.ColorHex)
                        .Select(g => new { Color = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count)
                        .ToList();

                    worksheet.Cell("A5").Value = "Color Usage Statistics";
                    worksheet.Cell("A6").Value = "Color";
                    worksheet.Cell("B6").Value = "Usage Count";

                    int row = 7;
                    foreach (var stat in colorStats)
                    {
                        worksheet.Cell(row, 1).Value = stat.Color;
                        worksheet.Cell(row, 2).Value = stat.Count;
                        row++;
                    }

                    // Save the file
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Export completed successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during export: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Load the editor by default
            BtnEditor_Click(this, EventArgs.Empty);
        }
    }
}
