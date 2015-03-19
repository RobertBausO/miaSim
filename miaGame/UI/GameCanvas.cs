using System;
using System.Windows.Threading;
using miaGame.Painter;
using miaGame.Tools;
using System.Windows.Controls;
using miaSim.Foundation;
using Timer = System.Timers.Timer;

namespace miaGame
{
	public class GameCanvas : Canvas
	{

		#region ================== Member variables =========================

		private IWorld mWorld;
		private IPainter mPainter;

		private readonly StopwatchStatistic mDrawGameStatistic;
		private double mFps = double.MaxValue;

		private long mCounter = 0;

		#endregion

		#region ================== Constructor/Destructor ===================

		public GameCanvas()
		{
			mDrawGameStatistic = new StopwatchStatistic(10, s =>
			{
				mFps = 1000.0 / mDrawGameStatistic.Average.TotalMilliseconds;
			});

			mFps = 0.0;

		}

		#endregion

		#region ================== Properties ===============================

		public double Fps { get { return mFps; } }

		#endregion

		#region ================== Methods ==================================

		public void Init(IWorld world, IPainter painter)
		{
			mWorld = world;
			mPainter = painter;
		}

		public void Update()
		{
			mDrawGameStatistic.MeasurePoint();
			Dispatcher.BeginInvoke(new Action(InvalidateVisual), DispatcherPriority.Input, null);
		}

		protected override void OnRender(System.Windows.Media.DrawingContext dc)
		{
			if (mWorld != null && mPainter != null)
			{
				var text = "FPS=" + mFps.ToString("0.00");
				text += ";" + mCounter++;

				mPainter.Draw(mWorld, text, ActualWidth, ActualHeight, this, dc);
			}
		}

		#endregion

	}
}
