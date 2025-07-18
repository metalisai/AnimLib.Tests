using AnimLib;
using SixLabors.ImageSharp.PixelFormats;

namespace AnimLib.Tests;

public static class ImageExtensions
{
    public static SixLabors.ImageSharp.Image ToImage(this CapturedFrame ss)
    {
        switch (ss.format)
        {
            case Texture2D.TextureFormat.RGB16:
                {
                    SixLabors.ImageSharp.Image<Rgb48> image = new SixLabors.ImageSharp.Image<Rgb48>(ss.width, ss.height);
                    int i = 0;
                    for (int y = 0; y < ss.height; y++)
                    {
                        for (int x = 0; x < ss.width; x++)
                        {
                            ushort r = (ushort)(ss.data[i++] | (ss.data[i++] << 8));
                            ushort g = (ushort)(ss.data[i++] | (ss.data[i++] << 8));
                            ushort b = (ushort)(ss.data[i++] | (ss.data[i++] << 8));
                            image[x, y] = new Rgb48(r, g, b);
                        }
                    }
                    return image;
                }
            default:
                throw new NotImplementedException();
        }
    }
}