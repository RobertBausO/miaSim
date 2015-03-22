using miaSim.Foundation;

namespace miaSim.Plants
{
	public class WabberTree : WorldItem
	{
		#region ================== Member variables =========================

		private const double MaxExtension = 0.1f;
		private double mWidthGrow = 0.0001f;
		private double mHeightGrow = 0.0001f;

		private readonly Extension mMaxExtension;

		#endregion

		#region ================== Constructor/Destructor ===================

		public WabberTree(Location location, Extension maxExtension)
			: base("WabberTree", location, new Extension(0.0f, 0.0f))
		{
			mMaxExtension = maxExtension;
		}

		#endregion

		#region ================== Properties ===============================
		#endregion

		#region ================== Methods ==================================

		public static IWorldItem CreateRandomTree()
		{
			var location = new Location(Utils.NextRandom(), Utils.NextRandom());
			var maxExtension = new Extension(Utils.NextRandom(MaxExtension), Utils.NextRandom(MaxExtension));

			return new Tree(location, maxExtension);
		}

		public override void Update(double msSinceLastUpdate)
		{
			Extension.Width += mWidthGrow;
			Extension.Height += mHeightGrow;

			if ((Extension.Width > mMaxExtension.Width) || (Extension.Width < 0))
			{
				mWidthGrow = -mWidthGrow;
			}

			if ((Extension.Height > mMaxExtension.Height) || (Extension.Height < 0))
			{
				mHeightGrow = -mHeightGrow;
			}
		}

		#endregion
	}
}
