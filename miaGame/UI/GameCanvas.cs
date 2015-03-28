using System;
using System.Windows.Threading;
using miaGame.Painter;
using System.Windows.Controls;

namespace miaGame
{
	public class GameCanvas : Canvas
	{

		#region ================== Member variables =========================

		private IPainter mPainter;

		#endregion

		#region ================== Constructor/Destructor ===================

		public GameCanvas()
		{
		}

		#endregion

		#region ================== Properties ===============================
		#endregion

		#region ================== Methods ==================================

		public void Init(IPainter painter)
		{
			mPainter = painter;
		}

		public void Update()
		{
			Dispatcher.BeginInvoke(new Action(InvalidateVisual), DispatcherPriority.Input, null);
		}

		protected override void OnRender(System.Windows.Media.DrawingContext dc)
		{
			if (mPainter != null)
			{
				mPainter.Draw(ActualWidth, ActualHeight, this, dc);
			}
		}

		#endregion

	}
}
