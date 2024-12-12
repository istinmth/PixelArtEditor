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
    public partial class PreviewControl : UserControl
    {
        private PictureBox previewBox;
        private System.Windows.Forms.Timer animationTimer;
        private TrackBar speedControl;
        private Button btnPlay;
        private Button btnStop;
        private List<Frame> frames;
        private int currentFrame = 0;

        public PreviewControl()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            // Preview Box
            previewBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Speed Control
            speedControl = new TrackBar
            {
                Dock = DockStyle.Bottom,
                Minimum = 50,
                Maximum = 1000,
                Value = 200,
                TickFrequency = 50
            };

            // Buttons
            btnPlay = new Button
            {
                Text = "Play",
                Dock = DockStyle.Bottom
            };
            btnPlay.Click += BtnPlay_Click;

            btnStop = new Button
            {
                Text = "Stop",
                Dock = DockStyle.Bottom
            };
            btnStop.Click += BtnStop_Click;

            // Timer
            animationTimer = new System.Windows.Forms.Timer
            {
                Interval = speedControl.Value
            };
            animationTimer.Tick += AnimationTimer_Tick;

            this.Controls.AddRange(new Control[] { previewBox, speedControl, btnPlay, btnStop });
            LoadFrames();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (frames == null || frames.Count == 0) return;

            currentFrame = (currentFrame + 1) % frames.Count;
            DisplayFrame(currentFrame);
        }

        private void LoadFrames()
        {
            using var context = new PixelArtDBContext();
            frames = context.Frames
                .Include(f => f.Pixels)
                .Include(f => f.Project)  // Include Project data
                .OrderBy(f => f.FrameNumber)
                .ToList();
        }

        private void DisplayFrame(int frameIndex)
        {
            if (frames == null || frameIndex >= frames.Count) return;

            var frame = frames[frameIndex];
            if (frame.Project == null || !frame.Project.CanvasWidth.HasValue || !frame.Project.CanvasHeight.HasValue) return;

            // Create bitmap for the frame
            var bitmap = new Bitmap(frame.Project.CanvasWidth.Value, frame.Project.CanvasHeight.Value);
            using (var g = Graphics.FromImage(bitmap))
            {
                // Fill background
                g.Clear(Color.White);

                // Draw pixels
                foreach (var pixel in frame.Pixels)
                {
                    if (pixel.X.HasValue && pixel.Y.HasValue && !string.IsNullOrEmpty(pixel.ColorHex))
                    {
                        using var brush = new SolidBrush(ColorTranslator.FromHtml(pixel.ColorHex));
                        g.FillRectangle(brush, pixel.X.Value, pixel.Y.Value, 1, 1);
                    }
                }
            }

            // Display in picture box
            previewBox.Image?.Dispose();
            previewBox.Image = bitmap;
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            animationTimer.Start();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            animationTimer.Stop();
        }
    }
}
