using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace miaGame.Painter
{
	public static class DirectPainterHelper
	{
		public static void DrawText(string textToDraw, TextPosition position, TextSize size, Brush brush, PaintInfo info)
		{
			if (!string.IsNullOrEmpty(textToDraw))
			{
				var sizeInPixel =
					 (size == TextSize.Small) ? info.ScreenSizeYps / 50 :
					 (size == TextSize.Medium) ? info.ScreenSizeYps / 25 :
					 info.ScreenSizeYps / 7;

				var text = new FormattedText(textToDraw,
					 System.Globalization.CultureInfo.CurrentUICulture,
					 FlowDirection.LeftToRight, new Typeface("Arial"),
						 sizeInPixel, brush);

				Point point = new Point(0, 0);

				switch (position)
				{
					case TextPosition.Center:
						text.TextAlignment = TextAlignment.Center;
						point = new Point(info.ScreenSizeXee / 2, (info.ScreenSizeYps - text.Height) / 2);
						break;

					case TextPosition.LeftCorner:
						text.TextAlignment = TextAlignment.Left;
						point = new Point(0, 0);
						break;

					case TextPosition.RightCorner:
						text.TextAlignment = TextAlignment.Right;
						point = new Point(info.ScreenSizeXee, 0);
						break;
				}


				info.Context.DrawText(text, point);
			}
		}

		public static void DrawEllipse(Brush brush, double posXee, double posYps, double sizeXee, double sizeYps, PaintInfo info)
		{
			info.Context.DrawEllipse(brush, new Pen(), 
				new Point(info.World2ScreenXee(posXee), info.World2ScreenYps(posYps)), 
				info.World2ScreenXee(sizeXee), info.World2ScreenYps(sizeYps));
		}

		public static void DrawRectangle(Brush brush, Rect worldRect, PaintInfo info)
		{
			var screenRect = info.World2Screen(worldRect);
			info.Context.DrawRectangle(brush, new Pen(), screenRect);
		}
	}
}
