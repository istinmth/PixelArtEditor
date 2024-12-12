using System;
using System.Collections.Generic;

namespace PixelArtEditor.API.Models
{
    public partial class Pixel
    {
        public int Id { get; set; }
        public int? FrameId { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public string? ColorHex { get; set; }

        public virtual Frame? Frame { get; set; }
    }
}
