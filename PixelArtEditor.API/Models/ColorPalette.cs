using System;
using System.Collections.Generic;

namespace PixelArtEditor.API.Models
{
    public partial class ColorPalette
    {
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public string? ColorHex { get; set; }
        public string? ColorName { get; set; }

        public virtual Project? Project { get; set; }
    }
}
