using System.Windows.Controls;
using System.Windows.Media;
using miaSim.Foundation;

namespace miaGame.Painter
{
    public interface IPainter
    {
        void Draw(IWorld iworld, string text, double screenWidth, double screenHeight, Canvas canvas, DrawingContext context);
    }
}
