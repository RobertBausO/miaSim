using System;
using System.Diagnostics;

namespace miaSim.Tools
{
	/// <summary>
	/// Only for time measurement
	/// </summary>
	public class StopwatchStatistic
	{

		#region ================== Member variables =========================

		private Stopwatch mStopwatch;

		private readonly int mReportAfterMeasures;
		private readonly Action<StopwatchStatistic> mReportAction;

		#endregion

		#region ================== Constructor/Destructor ===================

		public StopwatchStatistic(int reportAfterMeasures, Action<StopwatchStatistic> reportAction)
		{
			mReportAfterMeasures = reportAfterMeasures;
			mReportAction = reportAction;

			Count = 0;
			mStopwatch = Stopwatch.StartNew();
		}

		#endregion

		#region ================== Properties ===============================

		public double AverageTicks { get; private set; }
		public double AverageMs { get; private set; }

		public long Count { get; private set; }

		#endregion

		#region ================== Methods ==================================

		public void MeasurePoint()
		{
			Count++;

			if (Count == mReportAfterMeasures)
			{
				// stop
				mStopwatch.Stop();

				AverageTicks = (double)mStopwatch.Elapsed.Ticks / (double)Count;
				AverageMs = AverageTicks / (double)TimeSpan.TicksPerMillisecond;

				// do action
				mReportAction(this);

				// reset
				Count = 0;
				mStopwatch.Reset();
				mStopwatch.Start();
			}
		}

		public override string ToString()
		{
			return string.Format("Average={0}", AverageMs);
		}

		#endregion

	}
}
