using System;
using System.Threading;
using System.Windows;

namespace miaSim.Foundation
{
	public static class Utils
	{
		#region ================== Member variables =========================

		// declare as thread static is 
		private static readonly ThreadLocal<Random> RandomInstance = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));

		#endregion

		#region ================== Constructor/Destructor ===================
		#endregion

		#region ================== Properties ===============================
		#endregion

		#region ================== Methods ==================================

		public static string Double2String(double value)
		{
			return value.ToString("0.000");
		}

		public static double NextRandom()
		{
			return RandomInstance.Value.NextDouble();
		}

		public static double NextRandom(double max)
		{
			return NextRandom() * max;
		}

		public static double NextRandom(double min, double max)
		{
			return NextRandom(max - min) + min;
		}

		public static int Next(int min, int max)
		{
			return RandomInstance.Value.Next(min, max+1); // including max
		}

		#endregion

        



	}
}
