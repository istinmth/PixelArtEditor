using Microsoft.EntityFrameworkCore;
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
    public partial class PixelEditorControl : UserControl
    {
        public PixelEditorControl()
        {
            InitializeComponent();
            SetupGridAndColors();
        }

        private void SetupGridAndColors()
        {
            // Setup DataGridView
            pixelGrid.AllowUserToAddRows = false;
            pixelGrid.AllowUserToDeleteRows = false;
            pixelGrid.RowHeadersVisible = false;
            pixelGrid.ColumnHeadersVisible = false;
            pixelGrid.MultiSelect = false;
            pixelGrid.ScrollBars = ScrollBars.None;
            pixelGrid.CellClick += PixelGrid_CellClick;

            // Setup ColorPicker
            colorPicker.DrawMode = DrawMode.OwnerDrawFixed;
            colorPicker.DrawItem += ColorPicker_DrawItem;

            // Setup buttons
            btnNewFrame.Click += BtnNewFrame_Click;
            btnDeleteFrame.Click += BtnDeleteFrame_Click;

            InitializeGrid(32, 32);
            LoadColors();
        }

        private void InitializeGrid(int width, int height)
        {
            pixelGrid.Rows.Clear();
            pixelGrid.Columns.Clear();

            for (int i = 0; i < width; i++)
            {
                pixelGrid.Columns.Add(new DataGridViewTextBoxColumn());
            }

            for (int i = 0; i < height; i++)
            {
                pixelGrid.Rows.Add();
            }

            // Make cells square
            var cellSize = pixelGrid.Width / width;
            foreach (DataGridViewColumn col in pixelGrid.Columns)
            {
                col.Width = cellSize;
            }
            foreach (DataGridViewRow row in pixelGrid.Rows)
            {
                row.Height = cellSize;
            }
        }

        private void LoadColors()
        {
            using var context = new PixelArtDBContext();
            var colors = context.ColorPalettes.ToList();
            colorPicker.DataSource = colors;
            colorPicker.DisplayMember = "ColorName";
            colorPicker.ValueMember = "ColorHex";
        }

        private void ColorPicker_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();
            var color = (ColorPalette)colorPicker.Items[e.Index];

            using (var brush = new SolidBrush(ColorTranslator.FromHtml(color.ColorHex)))
            {
                e.Graphics.FillRectangle(brush, e.Bounds.Left + 2, e.Bounds.Top + 2,
                    20, e.Bounds.Height - 4);
            }

            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(color.ColorName, e.Font,
                    brush, e.Bounds.Left + 25, e.Bounds.Top + 2);
            }
        }

        private void PixelGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && colorPicker.SelectedItem != null)
            {
                var color = (ColorPalette)colorPicker.SelectedItem;
                pixelGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                    ColorTranslator.FromHtml(color.ColorHex);
                SavePixel(e.RowIndex, e.ColumnIndex, color.ColorHex);
            }
        }

        private void BtnNewFrame_Click(object sender, EventArgs e)
        {
            using var context = new PixelArtDBContext();
            var currentProject = context.Projects.FirstOrDefault(); // For now, just use first project

            if (currentProject != null)
            {
                var frameCount = context.Frames.Count(f => f.ProjectId == currentProject.Id);
                var newFrame = new Frame
                {
                    ProjectId = currentProject.Id,
                    FrameNumber = frameCount + 1,
                    DelayMs = 200 // Default delay
                };

                context.Frames.Add(newFrame);
                context.SaveChanges();
                LoadFrames();
            }
        }

        private void BtnDeleteFrame_Click(object sender, EventArgs e)
        {
            if (frameList.SelectedItem != null)
            {
                var frame = (Frame)frameList.SelectedItem;
                using var context = new PixelArtDBContext();

                // First, delete all pixels associated with this frame
                var pixels = context.Pixels.Where(p => p.FrameId == frame.Id);
                context.Pixels.RemoveRange(pixels);

                // Then delete the frame
                context.Frames.Remove(frame);
                context.SaveChanges();
                LoadFrames();
            }
        }

        private void SavePixel(int x, int y, string colorHex)
        {
            using var context = new PixelArtDBContext();
            var currentFrame = (Frame)frameList.SelectedItem;

            if (currentFrame == null) return;

            var pixel = context.Pixels.FirstOrDefault(p =>
                p.FrameId == currentFrame.Id &&
                p.X == x &&
                p.Y == y);

            if (pixel != null)
            {
                pixel.ColorHex = colorHex;
            }
            else
            {
                pixel = new Pixel
                {
                    FrameId = currentFrame.Id,
                    X = x,
                    Y = y,
                    ColorHex = colorHex
                };
                context.Pixels.Add(pixel);
            }

            context.SaveChanges();
        }

        private void LoadFrames()
        {
            using var context = new PixelArtDBContext();
            var frames = context.Frames
                .Include(f => f.Project)
                .OrderBy(f => f.FrameNumber)
                .ToList();

            frameList.DataSource = frames;
            frameList.DisplayMember = "FrameNumber";
        }

        private void rightPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LoadFramePixels(Frame frame)
        {
            // Clear current grid
            foreach (DataGridViewRow row in pixelGrid.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }
            }

            // Load frame pixels
            using var context = new PixelArtDBContext();
            var pixels = context.Pixels.Where(p => p.FrameId == frame.Id).ToList();

            // Apply pixels to grid
            foreach (var pixel in pixels)
            {
                if (pixel.X.HasValue && pixel.Y.HasValue && !string.IsNullOrEmpty(pixel.ColorHex))
                {
                    pixelGrid.Rows[pixel.Y.Value].Cells[pixel.X.Value].Style.BackColor =
                        ColorTranslator.FromHtml(pixel.ColorHex);
                }
            }
        }

        private void frameList_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (frameList.SelectedItem != null)
            {
                var frame = (Frame)frameList.SelectedItem;
                LoadFramePixels(frame);
            }
        }
    }
}
