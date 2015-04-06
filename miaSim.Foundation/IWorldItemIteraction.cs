using System.Collections.Generic;

namespace miaSim.Foundation
{
	public interface WorldItemBaseIteraction
	{
		IList<WorldItemBase> GetIntersectItems(WorldItemBase worldItem);

		void AddItem(WorldItemBase worldItem);
		void RemoveItem(WorldItemBase worldItem);
	}
}
