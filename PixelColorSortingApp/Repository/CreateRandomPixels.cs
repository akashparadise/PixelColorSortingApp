using Microsoft.Extensions.Logging;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColorSortingUtility.Repository
{
    public class CreateRandomPixels : ICreateRandomPixels
    {
        private readonly ILogger<CreateRandomPixels> _logger;

        public CreateRandomPixels(ILogger<CreateRandomPixels> logger)
        {
            _logger = logger;
        }

        public BitmapSource CreateRandomBitmapSource(int width, int height)
        {
            string methodName = "CreateRandomBitmapSource";
            _logger.LogInformation($"{ methodName }: Creating an array and filling it with random pixels");

            #region
            /*Bitmap bmp = new Bitmap(width, height);
            Random random = new Random();

            //create random pixels
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //generate random ARGB value
                    int a = random.Next(256);
                    int r = random.Next(256);
                    int g = random.Next(256);
                    int b = random.Next(256);

                    //set ARGB value
                    bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(a, r, g, b));
                }
            }*/
            #endregion

            var randomPixels = new byte[3 * height * width];
            new Random().NextBytes(randomPixels);

            _logger.LogInformation($"{ methodName }: Creating a bitmap source of random pixels");
            return BitmapSource.Create(width, height, 96d, 96d, PixelFormats.Bgr24, null, randomPixels, width * 3);
        }
    }
}
