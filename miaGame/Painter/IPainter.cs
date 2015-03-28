using System.Windows.Controls;
using System.Windows.Media;

namespace miaGame.Painter
{
    public interface IPainter
    {
        void Draw(double screenWidth, double screenHeight, Canvas canvas, DrawingContext context);
    }
}
