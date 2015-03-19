using System;

namespace miaSim.Test
{
	static class Helper
	{
		public static bool Equals(double value1, double value2)
		{
			const double epsilon = 0.0000001;
			var diff = Math.Abs(value1 - value2);
			return diff <= epsilon;

		}
	}
}
