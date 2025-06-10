
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


        /* public static Bitmap LoadTexture(string filePath)
         {
             // Ініціалізуємо DevIL один раз (якщо ще не ініціалізовано)
             IL.Init();

             byte[] fileData = File.ReadAllBytes(filePath);
             string extension = Path.GetExtension(filePath).ToLower();
             ImageType imageType = ImageType.Unkwown;

             if (extension == ".wtb")
             {
                 fileData = ExtractDdsFromWtb(fileData); // Припускаємо, що цей метод повертає byte[]
                 imageType = ImageType.Dds;
             }
             else if (extension == ".dds")
             {

                 imageType = ImageType.Dds;
             }
             else // Для звичайних зображень
             {

                 // Дозволяємо DevIL самому визначити тип
                 imageType = IL.DetermineType(filePath);
             }

             if (fileData == null || fileData.Length == 0)
             {
                 throw new Exception("Texture data is empty or could not be extracted.");
             }

             // Створюємо зображення в DevIL з масиву байтів
             int imageId = IL.GenImage();
             IL.BindImage(imageId);

             if (!IL.LoadStreamWithType(imageType, new MemoryStream(fileData)))
             {
                 // Якщо сталася помилка, звільняємо ресурси і кидаємо виняток
                 IL.DeleteImage(imageId);
                 throw new Exception($"DevIL could not load the image. Error: {IL.GetError()}");
             }

             int imageWidth = IL.GetInteger(IntName.ImageWidth);
             int imageHeight = IL.GetInteger(IntName.ImageHeight);

             Bitmap bitmap = new Bitmap(imageWidth, imageHeight, 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, IL.GetData());
             bitmap.LockBits(new Rectangle(0, 0, imageWidth, imageHeight), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

             IL.DeleteImage(imageId);
             IL.BindImage(0); // Звільняємо поточне зображення

             return bitmap;
         }*/

        private static byte[] ExtractDdsFromWtb(byte[] wtbData)
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

            // --- Етап 1: Конвертація System.Drawing.Bitmap у Memory2D<ColorRgba32> ---

            // Перевіряємо, чи має текстура альфа-канал, щоб вибрати правильний формат стиснення.
            // Image.IsAlphaPixelFormat() є ненадійним, тому перевіримо вручну.
            bool hasAlpha = texture.PixelFormat == SysImaging.PixelFormat.Format32bppArgb ||
                            texture.PixelFormat == SysImaging.PixelFormat.Format32bppPArgb;

            // Створюємо масив для зберігання пікселів у форматі RGBA32
            var pixels = new ColorRgba32[texture.Width * texture.Height];

            // Блокуємо біти Bitmap для прямого доступу до пам'яті
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

                    // --- ВИПРАВЛЕНА ЛОГІКА КОПІЮВАННЯ ---

                    // 1. Створюємо масив байтів відповідного розміру.
                    int byteCount = bmpData.Stride * bmpData.Height;
                    byte[] bmpBytes = new byte[byteCount];

                    // 2. Копіюємо дані з Bitmap в наш масив байтів.
                    Marshal.Copy(bmpData.Scan0, bmpBytes, 0, byteCount);

                    // 3. Копіюємо дані з масиву байтів у наш масив пікселів (структур).
                    // Це можна зробити кількома способами, але найпростіший - через MemoryMarshal.
                    // Якщо цей метод недоступний, є альтернатива.

                    // Варіант А (сучасний, з System.Memory)
                    var byteSpan = new ReadOnlySpan<byte>(bmpBytes);
                    MemoryMarshal.Cast<byte, ColorRgba32>(byteSpan).CopyTo(pixels);

                    // Варіант Б (класичний, працює скрізь)
                    // Якщо варіант А не працює, використовуйте цей:
                    /*
                    GCHandle handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
                    try
                    {
                        IntPtr destination = handle.AddrOfPinnedObject();
                        // Ми не можемо просто скопіювати bmpBytes, бо bmpData.Stride може бути
                        // більшим за width * 4. Потрібно копіювати рядок за рядком.
                        for (int y = 0; y < bmpData.Height; y++)
                        {
                            IntPtr sourceRow = IntPtr.Add(bmpData.Scan0, y * bmpData.Stride);
                            IntPtr destRow = IntPtr.Add(destination, y * bmpData.Width * 4); // 4 байти на піксель
                            Buffer.MemoryCopy(sourceRow.ToPointer(), destRow.ToPointer(), bmpData.Width * 4, bmpData.Width * 4);
                        }
                    }
                    finally
                    {
                        if (handle.IsAllocated)
                        {
                            handle.Free();
                        }
                    }
                    */
                }
            }
            finally
            {
                // UnlockBits для tempBitmap не потрібен, оскільки він у using-блоці
                // і буде знищений разом з bmpData. Але для чистоти коду,
                // якщо б tempBitmap не був у using, тут був би UnlockBits.
            }

            // Створюємо Memory2D з нашого масиву пікселів.
            // Потрібен NuGet: CommunityToolkit.HighPerformance
            ReadOnlyMemory2D<ColorRgba32> memory2D = pixels.AsMemory().AsMemory2D(texture.Height, texture.Width);


            // --- Етап 2: Кодування та збереження ---

            var encoder = new BcEncoder();

            // Вибираємо формат стиснення.
            // BC3 (DXT5) - для текстур з альфа-каналом.
            // BC1 (DXT1) - для текстур без альфа-каналу (можна з 1-бітним альфа).
           // encoder.OutputOptions.Format = hasAlpha ? CompressionFormat.Bc3 : CompressionFormat.Bc1;
            encoder.OutputOptions.FileFormat = OutputFileFormat.Dds;
            //encoder.OutputOptions.Quality = CompressionQuality.BestQuality;
            encoder.OutputOptions.Format = CompressionFormat.Rgba;
            encoder.OutputOptions.GenerateMipMaps = false; // Для шрифтів міпмапи не потрібні

            // Кодуємо зображення в об'єкт DdsFile
            DdsFile ddsFile = encoder.EncodeToDds(memory2D);

            // Записуємо DdsFile у файл на диску
            using (var fs = new FileStream(outputPath, FileMode.Create))
            {
                ddsFile.Write(fs);
            }
        }
    }
}
