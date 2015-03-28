using System.Windows.Controls;
using System.Windows.Media;
using miaGame.Painter;
using miaSim.Foundation;

namespace miaSim
{
	public class Painter : IPainter
	{
		private DirectPainterHelper mHelper;

		public Painter()
		{
			mHelper = new DirectPainterHelper();
		}

		public void Draw(IWorld iworld, string text, double screenWidth, double screenHeight, Canvas canvas, DrawingContext context)
		{
			var world = iworld as World;
			if (world == null) return;

			lock (world.UpdateLock)
			{
				var info = new PaintInfo(1.0, 1.0, screenWidth, screenHeight, canvas, context);
				DrawWorld(iworld as World, info);
				mHelper.DrawText(text, TextPosition.LeftCorner, TextSize.Medium, Brushes.DarkGreen, info);
			}
		}

		private void DrawWorld(World world, PaintInfo info)
		{
			foreach (var item in world.Items)
			{
				mHelper.DrawRectangle(Brushes.Green, item.Position, info);
			}
		}
	}
}
