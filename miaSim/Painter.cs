using System.Windows.Controls;
using System.Windows.Media;
using miaGame.Painter;
using miaSim.Foundation;

namespace miaSim
{
	public class Painter : IPainter
	{
		private DirectPainterHelper mHelper;
		private World mWorld;

		public Painter(World world)
		{
			mWorld = world;
			mHelper = new DirectPainterHelper();
		}

		public void Draw(double screenWidth, double screenHeight, Canvas canvas, DrawingContext context)
		{
			var info = new PaintInfo(1.0, 1.0, screenWidth, screenHeight, canvas, context);

			DrawWorldItems(info);

			mHelper.DrawText(mWorld.Info, TextPosition.LeftCorner, TextSize.Medium, Brushes.DarkGreen, info);
		}

		private void DrawWorldItems(PaintInfo info)
		{
			foreach (var item in mWorld.Items)
			{
				mHelper.DrawRectangle(Brushes.Green, item.Position, info);
			}
		}
	}
}
