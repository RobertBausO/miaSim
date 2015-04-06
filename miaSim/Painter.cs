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
			mWorld.Items.Sort(CompareItemsByZ);
			foreach (var item in mWorld.Items)
			{
				item.Draw(context);
			}
		}


		private static int CompareItemsByZ(WorldItemBase a, WorldItemBase b)
		{
			return a.ZPosition.CompareTo(b.ZPosition);
		}

	}

}
