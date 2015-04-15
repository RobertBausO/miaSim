using System.Windows.Controls;
using System.Windows.Media;

namespace miaGame
{
    public interface IPainter
    {
        void Draw(PaintContext context);
    }
}
