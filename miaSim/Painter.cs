using System;
using System.Windows.Controls;
using System.Windows.Media;
using miaGame.Painter;
using miaSim.Foundation;

namespace miaSim
{
	public class Painter : IPainter
	{
		private TextBlock mGameInfoText;
		private TextBlock mGameMessageText;
		private TextBlock mDebugText;

		private PainterHelper mHelper;

		public Painter()
		{
			mHelper = new PainterHelper(null);
		}

		public void Draw(IWorld igame, string debug, double screenWidth, double screenHeight, Canvas canvas, DrawingContext context)
		{
			var info = new PaintInfo(1.0, 1.0, screenWidth, screenHeight, canvas, context);

			info.Canvas.BeginInit();

			//var game = igame as World;
			//mWorldPainter.Draw(game.World, info);

			//
			// Add HID
			//
			var infoText = "Info!";

			PainterHelper.DrawText(infoText, TextSize.Medium, TextPosition.RightCorner, Brushes.Black, info, ref mGameInfoText);
			PainterHelper.DrawText("Debug info" + Environment.NewLine + debug, TextSize.Small, TextPosition.LeftCorner, Brushes.Black, info, ref mDebugText);

			info.Canvas.EndInit();
		}

		private void DrawWorld(World world, PaintInfo info)
		{
			foreach (var item in world.GetSnapshotOfItems())
			{
				//mHelper.DrawImage(item.Id.ToString(),  );
			}
			
		}
	}
}
