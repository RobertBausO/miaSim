using System.Collections.Generic;

namespace miaSim.Foundation
{
	public interface IWorldItemIteraction
	{
		IList<IWorldItem> GetIntersectItems(IWorldItem worldItem);
	}
}
