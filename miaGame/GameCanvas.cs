using System;
using System.Windows.Threading;
using System.Windows.Controls;

namespace miaGame
{
	public class GameCanvas : Canvas
	{

		#region ================== Member variables =========================

		private IPainter mPainter;

		#endregion

		#region ================== Constructor/Destructor ===================
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
			Dispatcher.BeginInvoke(new Action(InvalidateVisual), DispatcherPriority.ApplicationIdle, null);
		}

		protected override void OnRender(System.Windows.Media.DrawingContext dc)
		{
			if (mPainter != null)
			{
				var worker = new PaintContext(ActualWidth, ActualHeight, this, dc);
				mPainter.Draw(worker);
			}
		}

		#endregion

	}
}
