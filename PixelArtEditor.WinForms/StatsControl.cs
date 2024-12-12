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
using System.Windows.Forms.DataVisualization.Charting;

namespace PixelArtEditor.WinForms
{
    public partial class StatsControl : UserControl
    {
        private Chart colorChart;
        private Button btnExport;

        public StatsControl()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            // Chart
            colorChart = new Chart
            {
                Dock = DockStyle.Fill
            };
            var chartArea = new ChartArea();
            colorChart.ChartAreas.Add(chartArea);

            // Export Button
            btnExport = new Button
            {
                Text = "Export to Excel",
                Dock = DockStyle.Bottom
            };
            btnExport.Click += BtnExport_Click;

            this.Controls.AddRange(new Control[] { colorChart, btnExport });
            LoadColorStats();
        }

        private void LoadColorStats()
        {
            using var context = new PixelArtDBContext();
            var colorUsage = context.Pixels
                .GroupBy(p => p.ColorHex)
                .Select(g => new { Color = g.Key, Count = g.Count() })
                .ToList();

            var series = new Series("Color Usage");
            foreach (var usage in colorUsage)
            {
                series.Points.AddXY(usage.Color, usage.Count);
            }
            colorChart.Series.Add(series);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            // Export to Excel
        }
    }
}
