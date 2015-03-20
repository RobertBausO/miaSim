namespace miaSim.Foundation
{
	/// <summary>
	/// currently a two dimensional position
	/// </summary>
	public class Location
	{
		public Location()
		{
		}

		public Location(double x, double y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// x-position from 0 to 1
		/// </summary>
		public double X { get; set; }

		/// <summary>
		/// y-position from 0 to 1
		/// </summary>
		public double Y { get; set; }

		public override string ToString()
		{
			return string.Format("L({0} {1})", Utils.Double2String(X), Utils.Double2String(Y));
		}
	}
}
