using System.Collections.Generic;
using System.Threading;
using miaGame.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace miaSim.Test
{
	[TestClass]
	public class StopWatchStatisticTest
	{
		[TestMethod]
		public void Normal()
		{
			const int waitInMs = 15;
			var averageList = new List<double>();
			var watch = new StopwatchStatistic(10, s => averageList.Add(s.Average.TotalMilliseconds));

			for (int loop = 0; loop <= 100; loop++)
			{
				watch.MeasurePoint();
				Thread.Sleep(waitInMs);
			}

			Assert.AreEqual(10, averageList.Count);

			foreach (var avg in averageList)
			{
				Assert.IsTrue(Helper.Equals(waitInMs, avg));
			}
		}

	}
}
