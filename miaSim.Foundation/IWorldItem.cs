using miaGame.Painter;
using System.Collections.Generic;
using System.Windows;

namespace miaSim.Foundation
{
	public interface IWorldItem
	{
		string Type { get; }
		long Id { get;  }

		Rect Position { get;  }

		void Update();
		void Draw(PaintInfo paintInfo);
		
		string GetDisplayText();

		IList<Connection> Connections { get; }
	}
}
