using System;
using System.Collections.Generic;
using miaSim.Foundation;

namespace miaSim.Test.Stubs
{
	internal class WorldItemInteractionMock : WorldItemBaseIteraction
	{
		public IList<WorldItemBase> GetIntersectItems(WorldItemBase worldItem, Type type)
		{
			return new List<WorldItemBase>();
		}

		public void AddItem(WorldItemBase worldItem)
		{
		}

		public void RemoveItem(WorldItemBase worldItem)
		{
		}
	}
}
