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

		public Location(float x, float y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// x-position from 0 to 1
		/// </summary>
		public float X { get; set; }

		/// <summary>
		/// y-position from 0 to 1
		/// </summary>
		public float Y { get; set; }

		public override string ToString()
		{
			return string.Format("L({0} {1})", Utils.Float2String(X), Utils.Float2String(Y));
		}
	}
}
