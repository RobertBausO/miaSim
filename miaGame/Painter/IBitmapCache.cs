using System.Windows.Media.Imaging;

namespace miaGame.Painter
{
    public interface IBitmapCache
    {
        BitmapImage Get(string name);
    }
}
