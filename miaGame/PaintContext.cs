using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace miaGame
{
	public class PaintContext
	{
		#region ================== Member variables =========================

		private double mWorld2ScreenXee;
		private double mWorld2ScreenYps;

		#endregion

		#region ================== Constructor/Destructor ===================
		#endregion

		#region ================== Properties ===============================

		public double WorldSizeXee { get; private set; }
		public double WorldSizeYps { get; private set; }

		public double ScreenSizeXee { get; private set; }
		public double ScreenSizeYps { get; private set; }

		public DrawingContext Context { get; private set; }
		public Canvas Canvas { get; private set; }

		#endregion

		#region ================== Methods ==================================

		public PaintContext(double screenWidth, double screenHeight, Canvas canvas, DrawingContext context)
		{
			ScreenSizeXee = screenWidth;
			ScreenSizeYps = screenHeight;

			Context = context;
			Canvas = canvas;

			SetWorldSize(1.0, 1.0);
		}

		public void SetWorldSize(double worldWidth, double worldHeight)
		{
			WorldSizeXee = worldWidth;
			WorldSizeYps = worldHeight;

			mWorld2ScreenXee = ScreenSizeXee / worldWidth;
			mWorld2ScreenYps = ScreenSizeYps / worldHeight;
		}

		public double World2ScreenXee(double worldXee)
		{
			return mWorld2ScreenXee * worldXee;
		}

		public double World2ScreenYps(double worldYps)
		{
			return mWorld2ScreenYps * worldYps;
		}

		public Rect World2Screen(Rect worldRect)
		{
			return new Rect(World2ScreenXee(worldRect.Left),
								World2ScreenYps(worldRect.Top),
								World2ScreenXee(worldRect.Width),
								World2ScreenYps(worldRect.Height));
		}


		public void DrawText(string textToDraw, TextPosition position, TextSize size, Brush brush)
		{
			if (!string.IsNullOrEmpty(textToDraw))
			{
				var sizeInPixel =
					 (size == TextSize.Small) ? ScreenSizeYps / 50 :
					 (size == TextSize.Medium) ? ScreenSizeYps / 25 :
					 ScreenSizeYps / 7;

				var text = new FormattedText(textToDraw,
					 System.Globalization.CultureInfo.CurrentUICulture,
					 FlowDirection.LeftToRight, new Typeface("Arial"),
						 sizeInPixel, brush);

				Point point = new Point(0, 0);

				switch (position)
				{
					case TextPosition.Center:
						text.TextAlignment = TextAlignment.Center;
						point = new Point(ScreenSizeXee / 2, (ScreenSizeYps - text.Height) / 2);
						break;

					case TextPosition.LeftCorner:
						text.TextAlignment = TextAlignment.Left;
						point = new Point(0, 0);
						break;

					case TextPosition.RightCorner:
						text.TextAlignment = TextAlignment.Right;
						point = new Point(ScreenSizeXee, 0);
						break;
				}


				Context.DrawText(text, point);
			}
		}

		public void DrawText(string textToDraw, Point position, TextSize size, Brush brush)
		{
			if (!string.IsNullOrEmpty(textToDraw))
			{
				var sizeInPixel =
					 (size == TextSize.Small) ? ScreenSizeYps / 50 :
					 (size == TextSize.Medium) ? ScreenSizeYps / 25 :
					 ScreenSizeYps / 7;

				var text = new FormattedText(textToDraw,
					 System.Globalization.CultureInfo.CurrentUICulture,
					 FlowDirection.LeftToRight, new Typeface("Arial"),
						 sizeInPixel, brush);

				text.TextAlignment = TextAlignment.Center;

				position.X = World2ScreenXee(position.X);
				position.Y = World2ScreenYps(position.Y);

				Context.DrawText(text, position);
			}
		}


		public void DrawEllipse(Brush brush, double posXee, double posYps, double sizeXee, double sizeYps)
		{
			Context.DrawEllipse(brush, new Pen(),
				new Point(World2ScreenXee(posXee), World2ScreenYps(posYps)),
				World2ScreenXee(sizeXee), World2ScreenYps(sizeYps));
		}

		public void DrawRectangle(Brush brush, Pen pen, Rect worldRect)
		{
			var screenRect = World2Screen(worldRect);
			Context.DrawRectangle(brush, pen, screenRect);
		}

		public void DrawLine(Brush brush, Point a, Point b)
		{
			a.X = World2ScreenXee(a.X);
			a.Y = World2ScreenYps(a.Y);

			b.X = World2ScreenXee(b.X);
			b.Y = World2ScreenYps(b.Y);

			Context.DrawLine(new Pen(brush, World2ScreenXee(0.001f)), a, b);
		}

		#endregion

        



	}
}
