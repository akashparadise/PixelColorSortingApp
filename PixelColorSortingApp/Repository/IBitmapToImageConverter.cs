using System.Drawing;
using System.Windows.Media.Imaging;

namespace ColorSortingUtility.Repository
{
    public interface IBitmapToImageConverter
    {
        BitmapImage BitmapToImageSource(Bitmap bitmap);
    }
}
