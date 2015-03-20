using System;
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

		public void Draw(IWorld igame, string debug, double screenWidth, double screenHeight, Canvas canvas, DrawingContext context)
		{
			var info = new PaintInfo(1.0, 1.0, screenWidth, screenHeight, canvas, context);

			DrawWorld(igame as World, info);

			//
			// Add HID
			//
			//mHelper.DrawText("Info!", TextPosition.RightCorner, TextSize.Medium, Brushes.Black, info);
			mHelper.DrawText("Debug info" + Environment.NewLine + debug, TextPosition.LeftCorner, TextSize.Medium, Brushes.Black, info);
		}

		private void DrawWorld(World world, PaintInfo info)
		{
			foreach (var item in world.GetSnapshotOfItems())
			{
				mHelper.DrawEllipse(Brushes.Black, item.Location.X, item.Location.Y, item.Extension.Width, item.Extension.Height, info);
			}
		}
	}
}
