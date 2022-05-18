using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace ColorSortingUtility.Repository
{
    class BitmapToImageConverter : IBitmapToImageConverter
    {
        private readonly ILogger<BitmapToImageConverter> _logger;

        public BitmapToImageConverter(ILogger<BitmapToImageConverter> logger)
        {
            _logger = logger;
        }

        public BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            string methodName = "BitmapToImageSource";
            _logger.LogInformation($"{ methodName }: adding pixels from bitmap to a list");

            List<Color> list = new List<Color>();

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color pixelHUE = bitmap.GetPixel(i, j);
                    list.Add(pixelHUE);
                }
            }

            var orderedListByHUE = list.OrderBy(color => color.GetHue()).ToList();

            _logger.LogInformation($"{ methodName }: sorting pixels based on HUE values");

            int pos = 0;
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    bitmap.SetPixel(x, y, orderedListByHUE[pos++]);
                }
            }

            _logger.LogInformation($"{ methodName }: Creating an image with sorted pixels");

            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                _logger.LogInformation($"{ methodName }: An image with sorted pixels created");
                return bitmapimage;
            }
        }
    }
}
