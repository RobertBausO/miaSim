using System;
using System.Threading;

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

		public static float NextRandom()
		{
			return (float) RandomInstance.Value.NextDouble();
		}

		public static float NextRandom(float max)
		{
			return NextRandom() * max;
		}

		public static int Next(int min, int max)
		{
			return RandomInstance.Value.Next(min, max);
		}

		#endregion

        



	}
}
