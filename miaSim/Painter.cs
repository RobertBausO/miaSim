using System.Windows.Controls;
using System.Windows.Media;
using miaGame;
using miaSim.Foundation;

namespace miaSim
{
	public class Painter : IPainter
	{
		private World mWorld;

		public Painter(World world)
		{
			mWorld = world;
		}

		public void Draw(PaintContext context)
		{
			DrawWorldItems(context);
			context.DrawText(mWorld.Info, TextPosition.LeftCorner, TextSize.Medium, Brushes.DarkGreen);
		}

		private void DrawWorldItems(PaintContext context)
		{
			foreach (var item in mWorld.Items)
			{
				item.Draw(context);
			}
		}
	}
}
