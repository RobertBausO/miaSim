using System.Collections.Generic;
using System.Windows;

namespace miaSim.Foundation
{
	public interface IWorldItem
	{
		string Type { get; }
		long Id { get;  }

		Rect Position { get;  }

		void Update(double msSinceLastUpdate);
		string GetDisplayText();

		IList<Connection> Connections { get; }
	}
}
