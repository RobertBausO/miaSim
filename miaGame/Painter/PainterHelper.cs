using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace miaGame.Painter
{
	public enum TextPosition
	{
		LeftCorner,
		RightCorner,
		Center
	}

	public enum TextSize
	{
		Small,
		Medium,
		Big
	}

	public class PainterHelper
	{
		#region ================== Member variables =========================

		private const int TextZIndex = 30000;

		private IBitmapCache mBitmapCache;
		private Dictionary<string, object> mId2Object = new Dictionary<string, object>();

		// draw info
		private int mZIndex;
		private Dictionary<string, object> mId2DrawnObject;


		#endregion

		#region ================== Constructor/Destructor ===================

		public PainterHelper(IBitmapCache bitmapCache)
		{
			mBitmapCache = bitmapCache;
		}

		#endregion

		#region ================== Properties ===============================
		#endregion

		#region ================== Methods ==================================

		public void StartDrawing()
		{
			mZIndex = 0;
			mId2DrawnObject = new Dictionary<string, object>();
		}

		public void EndDrawing(PaintInfo info)
		{
			// remove all objects, which are no longer existing
			RemoveUndrawnObjects(info);
		}

		private void RemoveUndrawnObjects(PaintInfo info)
		{
			var listToRemove = new List<UIElement>();
			foreach (var child in info.Canvas.Children)
			{
				var image = child as Image;
				if (image != null)
				{
					if (!mId2DrawnObject.ContainsKey(image.Tag.ToString()))
					{
						listToRemove.Add(image);
					}
				}
			}

			foreach (var childToRemove in listToRemove)
			{
				info.Canvas.Children.Remove(childToRemove);
			}
		}

		public void DrawImage(string id, double sizeXee, double sizeYps, string name, double angle, double posXee, double posYps, PaintInfo info)
		{
			Image image;

			if (!mId2Object.ContainsKey(id))
			{
				image = new Image();
				image.Tag = id;
				image.Stretch = Stretch.Fill;

				mId2Object.Add(id, image);
				info.Canvas.Children.Add(image);
			}
			else
			{
				image = mId2Object[id] as Image;
			}

			image.Source = mBitmapCache.Get(name);

			if (image.RenderTransform != null || angle != 0.0)
			{
				RotateTransform trans = new RotateTransform(angle,
					 info.World2ScreenXee(sizeXee / 2),
					 info.World2ScreenYps(sizeYps / 2));
				image.RenderTransform = trans;
			}

			image.Width = info.World2ScreenXee(sizeXee);
			image.Height = info.World2ScreenYps(sizeYps);

			Canvas.SetZIndex(image, mZIndex++);
			Canvas.SetLeft(image, info.World2ScreenXee(posXee - (sizeXee / 2)));
			Canvas.SetTop(image, info.World2ScreenYps(posYps - sizeYps / 2));

			// register as drawn
			mId2DrawnObject.Add(id, image);
		}


		public static void DrawText(string text, TextSize size, TextPosition position, Brush brush, PaintInfo info, ref TextBlock element)
		{
			if (element == null)
			{
				element = new TextBlock();
				element.FontFamily = new FontFamily("Arial");
				element.Foreground = brush;
				element.Visibility = System.Windows.Visibility.Hidden;
				element.TextAlignment = TextPosition2Alignment(position);

				info.Canvas.Children.Add(element);
			}

			element.Text = text;

			if (element.ActualWidth > 0)
			{
				element.FontSize = TextSize2FontSize(size, info.Canvas.ActualHeight);

				Canvas.SetZIndex(element, TextZIndex);
				Canvas.SetLeft(element, TextPosition2Left(element.ActualWidth, info.Canvas.ActualWidth, position));
				Canvas.SetTop(element, TextPosition2Top(element.ActualHeight, info.Canvas.ActualHeight, position));

				element.Visibility = System.Windows.Visibility.Visible;
			}
		}

		private static double TextPosition2Left(double elementWidth, double screenWidth, TextPosition pos)
		{
			if (pos == TextPosition.Center) return (screenWidth - elementWidth) / 2;
			if (pos == TextPosition.RightCorner) return screenWidth - elementWidth;

			return 0.0;
		}

		private static double TextPosition2Top(double elementHeight, double screenHeight, TextPosition pos)
		{
			if (pos == TextPosition.Center) return (screenHeight - elementHeight) / 2;

			return 0;
		}

		private static System.Windows.TextAlignment TextPosition2Alignment(TextPosition pos)
		{
			if (pos == TextPosition.LeftCorner) return System.Windows.TextAlignment.Left;
			if (pos == TextPosition.RightCorner) return System.Windows.TextAlignment.Right;

			return System.Windows.TextAlignment.Center;
		}

		private static double TextSize2FontSize(TextSize size, double screenHeight)
		{
			if (size == TextSize.Small) return screenHeight / 50;
			if (size == TextSize.Medium) return screenHeight / 25;

			return screenHeight / 7;
		}

		#endregion

	}
}
