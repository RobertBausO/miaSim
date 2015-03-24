using System;
using miaSim.Foundation;

namespace miaSim.Plants
{
	public class WabberTree : WorldItem
	{
		#region ================== Member variables =========================

		private const double MinExtension = 0.01;
		private const double MaxExtension = 0.1;
		private double mWidthGrow = 0.0001f;
		private double mHeightGrow = 0.0001f;

		private readonly Extension mMaxExtension;

		#endregion

		#region ================== Constructor/Destructor ===================

		public WabberTree(IWorldItemIteraction interaction, Location location, Extension maxExtension)
			: base(interaction, "WabberTree", location, new Extension(0.0f, 0.0f))
		{
			mMaxExtension = maxExtension;
		}

		#endregion

		#region ================== Properties ===============================
		#endregion

		#region ================== Methods ==================================

		public static IWorldItem CreateRandomTree(IWorldItemIteraction interaction)
		{
			var location = new Location(Utils.NextRandom(), Utils.NextRandom());
			var maxExtension = new Extension(Utils.NextRandom(MaxExtension) + MinExtension, Utils.NextRandom(MaxExtension) + MinExtension);

			return new WabberTree(interaction, location, maxExtension);
		}

		public override void Update(double msSinceLastUpdate)
		{
			var newWidth = Extension.Width + mWidthGrow;
			var newHeight = Extension.Height + mHeightGrow;

			if (newWidth > mMaxExtension.Width)
			{
				mWidthGrow = -mWidthGrow;
				newWidth = mMaxExtension.Width;
			}

			if (newWidth < 0)
			{
				mWidthGrow = -mWidthGrow;
				newWidth = 0.0;
			}

			if (newHeight > mMaxExtension.Height)
			{
				mHeightGrow = -mHeightGrow;
				newHeight = mMaxExtension.Height;
			}

			if (newHeight < 0)
			{
				mHeightGrow = -mHeightGrow;
				newHeight = 0.0;
			}

			var oldWidth = Extension.Width;
			var oldHeight = Extension.Height;

			Extension.Width = newWidth;
			Extension.Height = newHeight;

			var intersects = WorldInteraction.GetIntersectItems(this);

			if (intersects.Count > 0)
			{
				Extension.Width = oldWidth;
				Extension.Height = oldHeight;
			}
		}

		#endregion
	}
}
