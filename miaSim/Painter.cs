using System.Windows.Controls;
using System.Windows.Media;
using miaGame;
using miaSim.Foundation;
using System.Collections.Generic;

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
			context.DrawText(mWorld.Info, TextPosition.LeftCorner, TextSize.Medium, Brushes.Black);
		}

		private void DrawWorldItems(PaintContext context)
		{
			lock(mWorld.UpdateLock)
			{
				var list = new List<WorldItemBase>(mWorld.Items);
				list.Sort(CompareItemsByZ);
				foreach (var item in mWorld.Items)
				{
					item.Draw(context);
				}
			}
		}

		private static int CompareItemsByZ(WorldItemBase a, WorldItemBase b)
		{
			return a.ZPosition.CompareTo(b.ZPosition);
		}

	}

}
