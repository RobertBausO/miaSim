using System.Windows.Controls;
using System.Windows.Media;
using miaSim.Foundation;


namespace miaGame.Painter
{
    public interface IPainter
    {
        void Draw(IWorld game, string debug, double screenWidth, double screenHeight, Canvas canvas, DrawingContext context);
    }
}
