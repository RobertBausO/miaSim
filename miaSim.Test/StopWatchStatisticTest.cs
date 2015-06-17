using System.Collections.Generic;
using System.Threading;
using miaSim.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace miaSim.Test
{
	[TestClass]
	public class StopWatchStatisticTest
	{
		[TestMethod]
		public void Normal()
		{
			const int waitInMs = 25;
			var averageList = new List<double>();
			var watch = new StopwatchStatistic(10, s => averageList.Add(s.AverageMs));

			for (int loop = 0; loop <= 100; loop++)
			{
				watch.MeasurePoint();
				Thread.Sleep(waitInMs);
			}

			Assert.AreEqual(10, averageList.Count);

			var number = 0;
			foreach (var avg in averageList)
			{
				var text = string.Format("{0}: Deviation: {1} / {2}", number++, waitInMs, avg);
				System.Diagnostics.Debug.WriteLine(text);
				//Assert.IsTrue(Helper.Equals(waitInMs, avg), text);
			}
		}


		[TestMethod]
		public void ConstanceOnFast()
		{
			var averageList = new List<double>();
			var watch = new StopwatchStatistic(111, s => averageList.Add(s.AverageTicks));

			var stopWatch = Stopwatch.StartNew();

			while (stopWatch.ElapsedMilliseconds < 5000)
			{
				watch.MeasurePoint();

				var results = new List<double>();
				for(int loopIndex = 0; loopIndex< 100; loopIndex++)
				{
					results.Add(Math.Sqrt(loopIndex+1));
				}
			}

			averageList.Sort();

			System.Diagnostics.Debug.WriteLine(string.Format("Min={0}; Max={1}", averageList[0], averageList[averageList.Count-111]));
		}


	}
}
