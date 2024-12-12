using System;
using System.Collections.Generic;

namespace PixelArtEditor.API.Models
{
    public partial class Frame
    {
        public Frame()
        {
            Pixels = new HashSet<Pixel>();
        }

        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public int? FrameNumber { get; set; }
        public int? DelayMs { get; set; }

        public virtual Project? Project { get; set; }
        public virtual ICollection<Pixel> Pixels { get; set; }
    }
}
