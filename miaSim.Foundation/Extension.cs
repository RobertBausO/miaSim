namespace miaSim.Foundation
{
	/// <summary>
	/// extension always as ellipse
	/// </summary>
	public class Extension
	{
		public Extension()
		{
		}

		public Extension(double width, double height)
		{
			Width = width;
			Height = height;
		}

		/// <summary>
		/// x-position from 0 to 1
		/// </summary>
		public double Width { get; set; }

		/// <summary>
		/// y-position from 0 to 1
		/// </summary>
		public double Height { get; set; }

		public override string ToString()
		{
			return string.Format("E({0} {1})", Utils.Double2String(Width), Utils.Double2String(Height));
		}

	}
}
