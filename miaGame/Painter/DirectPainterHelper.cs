using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using miaSim.Foundation;

namespace miaGame.Painter
{
	public class DirectPainterHelper
	{
		public void DrawText(string textToDraw, TextPosition position, TextSize size, Brush brush, PaintInfo info)
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


		public void DrawImage(BitmapImage image, double xee, double yps, double size, PaintInfo info)
		{
			var point = new System.Windows.Point(info.World2ScreenXee(xee - size / 2), info.World2ScreenYps(yps - size / 2));
			var dimension = new System.Windows.Size(info.World2ScreenXee(size), info.World2ScreenYps(size));

			info.Context.DrawImage(image, new System.Windows.Rect(point, dimension));
		}

		public void DrawImage(BitmapImage image, double xee, double yps, double size, double angle, PaintInfo info)
		{
			Image imageControl = new Image();
			imageControl.Source = image;
			imageControl.Stretch = Stretch.Fill;

			imageControl.Width = info.World2ScreenXee(size);
			imageControl.Height = info.World2ScreenYps(size);

			Canvas.SetLeft(imageControl, info.World2ScreenXee(size) / 2);
			Canvas.SetTop(imageControl, info.World2ScreenYps(size) / 2);

			RotateTransform trans = new RotateTransform(angle, info.World2ScreenXee(size / 2), info.World2ScreenYps(size / 2));
			imageControl.RenderTransform = trans;

			Canvas container = new Canvas();
			container.Children.Add(imageControl);
			container.Arrange(new Rect(0, 0, info.ScreenSizeXee, info.ScreenSizeYps));

			// render the result to a new bitmap. 
			RenderTargetBitmap target = new RenderTargetBitmap(
				 (int)info.World2ScreenXee(size) * 2,
				 (int)info.World2ScreenYps(size) * 2,
				 image.DpiX,
				 image.DpiY, PixelFormats.Default);
			target.Render(container);

			//var point = new System.Windows.Point(info.World2ScreenXee(xee - size / 2), info.World2ScreenYps(yps - size / 2));
			//var dimension = new System.Windows.Size(info.World2ScreenXee(size), info.World2ScreenYps(size));

			var point = new System.Windows.Point(info.World2ScreenXee(xee) - target.Width / 2, info.World2ScreenYps(yps) - target.Height / 2);
			var dimension = new System.Windows.Size(target.Width, target.Height);

			info.Context.DrawImage(target, new System.Windows.Rect(point, dimension));
		}

		public void DrawEllipse(Brush brush, double posXee, double posYps, double sizeXee, double sizeYps, PaintInfo info)
		{
			info.Context.DrawEllipse(brush, new Pen(), 
				new Point(info.World2ScreenXee(posXee), info.World2ScreenYps(posYps)), 
				info.World2ScreenXee(sizeXee), info.World2ScreenYps(sizeYps));
		}

		public void DrawRectangle(Brush brush, Location location, Extension extension, PaintInfo info)
		{
			var screenRect = info.World2Screen(Utils.LocationExtension2Rect(location, extension));
			info.Context.DrawRectangle(brush, new Pen(), screenRect);
		}
	}
}
