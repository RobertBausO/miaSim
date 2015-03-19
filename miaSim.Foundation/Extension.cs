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

		public Extension(float width, float height)
		{
			Width = width;
			Height = height;
		}

		/// <summary>
		/// x-position from 0 to 1
		/// </summary>
		public float Width { get; set; }

		/// <summary>
		/// y-position from 0 to 1
		/// </summary>
		public float Height { get; set; }

		public override string ToString()
		{
			return string.Format("E({0} {1})", Utils.Float2String(Width), Utils.Float2String(Height));
		}

	}
}
