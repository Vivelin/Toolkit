using System;
using System.Collections.Generic;
using System.Text;

namespace Vivelin.Toolkit
{
    public static class ImageFormatExtensions
    {
        public static string GetDefaultExtension(this ImageFormat value)
        {
            return value switch
            {
                ImageFormat.Png => ".png",
                ImageFormat.Jpeg => ".jpg",
                _ => throw new NotSupportedException($"The image format '{value}' is not recognized or supported.")
            };
        }
    }
}
