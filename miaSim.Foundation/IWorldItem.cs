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

		Point Center();

		void Update();
		void Draw(PaintInfo paintInfo);

		void Tell(Message message);
		
		string GetDisplayText();
	}
}
