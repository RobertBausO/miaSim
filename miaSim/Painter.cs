using System;
using System.Windows.Controls;
using System.Windows.Media;
using miaGame.Painter;
using miaSim.Foundation;


namespace miaSim
{
	public class Painter : IPainter
	{
		private PainterHelper mHelper = new PainterHelper(null);

		public void Draw(IWorld game, string debug, double screenWidth, double screenHeight, Canvas canvas, DrawingContext context)
		{
			var info = new PaintInfo(1.0, 1.0, screenWidth, screenHeight, canvas, context);

			mHelper.StartDrawing(info);

			PainterHelper.DrawText("Here we are", TextSize.Medium, TextPosition.RightCorner, Brushes.LightGreen, info);
			PainterHelper.DrawText("DebugInfo" + Environment.NewLine + debug, TextSize.Medium, TextPosition.LeftCorner, Brushes.Black, info);

			mHelper.EndDrawing(info);
		}
	}
}
