using System;
using System.Collections.Generic;

namespace miaSim.Foundation
{
	public interface IWorldItemBaseIteraction
	{
		IList<WorldItemBase> GetIntersectItems(WorldItemBase worldItem, Type type);

		void AddItem(WorldItemBase worldItem);
		void RemoveItem(WorldItemBase worldItem);
	}
}
