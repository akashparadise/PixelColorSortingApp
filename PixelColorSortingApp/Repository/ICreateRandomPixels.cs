using System.Windows.Media.Imaging;

namespace ColorSortingUtility.Repository
{
    public interface ICreateRandomPixels
    {
        BitmapSource CreateRandomBitmapSource(int width, int height);
    }
}
