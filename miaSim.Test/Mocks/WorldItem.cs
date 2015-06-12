using System.Windows;
using miaGame;
using miaSim.Foundation;

namespace miaSim.Test.Mocks
{
	internal class WorldItem : WorldItemBase
	{
		public WorldItem(WorldItemBaseIteraction worldInteraction, string type, Rect position) : base(worldInteraction, type, position)
		{
		}

		public override void Update()
		{
		}

		public override void Tell(Message message)
		{
		}

		public override void Draw(PaintContext paintInfo)
		{
		}
	}
}
