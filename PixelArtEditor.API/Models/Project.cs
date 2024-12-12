using System;
using System.Collections.Generic;

namespace PixelArtEditor.API.Models
{
    public partial class Project
    {
        public Project()
        {
            ColorPalettes = new HashSet<ColorPalette>();
            Frames = new HashSet<Frame>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? CreationDate { get; set; }
        public int? CanvasWidth { get; set; }
        public int? CanvasHeight { get; set; }

        public virtual ICollection<ColorPalette> ColorPalettes { get; set; }
        public virtual ICollection<Frame> Frames { get; set; }
    }
}
