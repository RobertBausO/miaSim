using System;
using System.Diagnostics;

namespace miaGame.Tools
{
    /// <summary>
    /// Only for time measurement
    /// </summary>
    public class StopwatchStatistic
    {

        #region ================== Member variables =========================

        private Stopwatch mStopwatch;
        private TimeSpan mSum;
        private bool mIsRunning;

        private readonly int mReportAfterMeasures;
		  private readonly Action<StopwatchStatistic> mReportAction;

        #endregion

        #region ================== Constructor/Destructor ===================

		  public StopwatchStatistic(int reportAfterMeasures, Action<StopwatchStatistic> reportAction)
        {
            mReportAfterMeasures = reportAfterMeasures;
            mReportAction = reportAction;

            Reset();
        }

        #endregion

        #region ================== Properties ===============================

        public TimeSpan Min { get; private set; }
        public TimeSpan Max { get; private set; }

        public TimeSpan Average { get { return TimeSpan.FromMilliseconds(mSum.TotalMilliseconds / Count); } }

        public long Count { get; private set; }

        public bool IsRunning { get { return mIsRunning; } }

        #endregion

        #region ================== Methods ==================================

        public void MeasurePoint()
        {
            if (IsRunning)
            {
                Stop();

                if (Count == mReportAfterMeasures)
                {
                    mReportAction(this);
                    Reset();
                }
            }

            Start();
        }

        public void Start()
        {
            mIsRunning = true;
            mStopwatch.Start();
        }

        public void Stop()
        {
            mIsRunning = false;
            mStopwatch.Stop();
            Count++;

            mSum = mSum.Add(mStopwatch.Elapsed);

            if (mStopwatch.Elapsed < Min)
                Min = mStopwatch.Elapsed;

            if (mStopwatch.Elapsed > Max)
                Max = mStopwatch.Elapsed;

            mStopwatch.Reset();
        }

        public void Reset()
        {
            mIsRunning = false;
            mStopwatch = new Stopwatch();
            Count = 0;
            mSum = TimeSpan.FromMilliseconds(0);

            Min = TimeSpan.MaxValue;
            Max = TimeSpan.MinValue;
        }

        public override string ToString()
        {
            return string.Format("Min={0}; Max={1}; Average={2}", (int)Min.TotalMilliseconds, (int)Max.TotalMilliseconds, (int)Average.TotalMilliseconds);
        }

        #endregion

    }
}
