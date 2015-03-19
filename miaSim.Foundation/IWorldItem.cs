using System;
using System.Collections;
using System.Collections.Generic;

namespace miaSim.Foundation
{
	public interface IWorldItem
	{
		string Type { get; }
		long Id { get;  }

		Location Location { get;  }
		Extension Extension { get; }

		void Update(double msSinceLastUpdate);
		string GetDisplayText();

		double CalcDistanceTo(IWorldItem item);

		IList<Connection> Connections { get; }
	}
}
