
using BCnEncoder.Encoder;
using BCnEncoder.Shared;
using BCnEncoder.Shared.ImageFiles;
using CommunityToolkit.HighPerformance;
using Pfim;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SysImaging = System.Drawing.Imaging;

namespace MESTool
{
    public class DDSUtil    
    {
        public static Bitmap LoadTexture(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            string extension = Path.GetExtension(filePath).ToLower();

            byte[] ddsData;

            if (extension == ".wtb")
            {
                ddsData = ExtractDdsFromWtb(fileData);
            }
            else if (extension == ".dds")
            {
                ddsData = fileData;
            }
            else
            {
                return new Bitmap(filePath);
            }

            if (ddsData == null || ddsData.Length == 0)
            {
                throw new Exception("Texture data is empty or could not be extracted.");
            }

            using (var stream = new MemoryStream(ddsData))
            using (var image = Pfimage.FromStream(stream))
            {
                SysImaging.PixelFormat format;
                switch (image.Format)
                {
                    case Pfim.ImageFormat.Rgba32:
                        format = SysImaging.PixelFormat.Format32bppArgb;
                        break;
                    case Pfim.ImageFormat.Rgb24:
                        format = SysImaging.PixelFormat.Format24bppRgb;
                        break;
                    default:
                        throw new NotImplementedException($"Unsupported image format: {image.Format}");
                }

                var bitmap = new Bitmap(image.Width, image.Height, format);
                var bmpData = bitmap.LockBits(new Rectangle(0, 0, image.Width, image.Height), SysImaging.ImageLockMode.WriteOnly, format);

                Marshal.Copy(image.Data, 0, bmpData.Scan0, image.DataLen);

                bitmap.UnlockBits(bmpData);
                return bitmap;
            }
        }

        public static byte[] ExtractDdsFromWtb(byte[] wtbData)
        {
            const uint ddsSignature = 0x20534444; // "DDS " в little-endian   
            for (int i = 0; i <= wtbData.Length - 4; i++)
            {
                if (BitConverter.ToUInt32(wtbData, i) == ddsSignature)
                {
                    int ddsLength = wtbData.Length - i;
                    byte[] ddsData = new byte[ddsLength];
                    Array.Copy(wtbData, i, ddsData, 0, ddsLength);
                    return ddsData;
                }
            }

            throw new Exception("DDS signature not found in WTB file.");
        }

        public static void SaveTextureAsDds(Bitmap texture, string outputPath)
        {
            if (texture == null)
            {
                throw new ArgumentNullException(nameof(texture), "Texture to save cannot be null.");
            }

            bool hasAlpha = texture.PixelFormat == SysImaging.PixelFormat.Format32bppArgb ||
                            texture.PixelFormat == SysImaging.PixelFormat.Format32bppPArgb;

            var pixels = new ColorRgba32[texture.Width * texture.Height];

            var rect = new Rectangle(0, 0, texture.Width, texture.Height);
            BitmapData bmpData = null;
          
            try
            {
                using (var tempBitmap = new Bitmap(texture.Width, texture.Height, SysImaging.PixelFormat.Format32bppArgb))
                {
                    using (var g = Graphics.FromImage(tempBitmap))
                    {
                        g.DrawImage(texture, rect);
                    }

                    bmpData = tempBitmap.LockBits(rect, ImageLockMode.ReadOnly, SysImaging.PixelFormat.Format32bppArgb);

                    int byteCount = bmpData.Stride * bmpData.Height;
                    byte[] bmpBytes = new byte[byteCount];
                    Marshal.Copy(bmpData.Scan0, bmpBytes, 0, byteCount);

                    var byteSpan = new ReadOnlySpan<byte>(bmpBytes);
                    MemoryMarshal.Cast<byte, ColorRgba32>(byteSpan).CopyTo(pixels);
                }
            }
            finally
            {
               
            }

            ReadOnlyMemory2D<ColorRgba32> memory2D = pixels.AsMemory().AsMemory2D(texture.Height, texture.Width);
            var encoder = new BcEncoder();

            encoder.OutputOptions.FileFormat = OutputFileFormat.Dds;

            encoder.OutputOptions.Format = CompressionFormat.Rgba;
            encoder.OutputOptions.GenerateMipMaps = false;
            DdsFile ddsFile = encoder.EncodeToDds(memory2D);
            using (var fs = new FileStream(outputPath, FileMode.Create))
            {
                ddsFile.Write(fs);
            }
        }
    }
}
