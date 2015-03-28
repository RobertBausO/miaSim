using System;
using miaSim.Foundation;
using System.Windows;

namespace miaSim.Plants
{
	public class WabberTree : WorldItem
	{
		#region ================== Member variables =========================

		private const double MinExtension = 0.01;
		private const double MaxExtension = 0.05;

		private double mWidthGrow = 0.0001f;
		private double mHeightGrow = 0.0001f;

		private readonly double mMaxWidthExtension;
		private readonly double mMaxHeightExtension;

		#endregion

		#region ================== Constructor/Destructor ===================

		public WabberTree(IWorldItemIteraction interaction, Rect position, 
							double maxWidthExtension, double maxHeightExtension)
			: base(interaction, "WabberTree", position)
		{
			mMaxWidthExtension = maxWidthExtension;
			mMaxHeightExtension = maxHeightExtension;
		}

		#endregion

		#region ================== Properties ===============================
		#endregion

		#region ================== Methods ==================================

		public static IWorldItem CreateRandomTree(IWorldItemIteraction interaction)
		{
			var position = new Rect(new Point(Utils.NextRandom(), Utils.NextRandom()), new Size(MinExtension, MinExtension));

			return new WabberTree(interaction, position, Utils.NextRandom(MaxExtension) + MinExtension, Utils.NextRandom(MaxExtension) + MinExtension);
		}

		public override void Update(double msSinceLastUpdate)
		{
			var newWidth = Position.Width + mWidthGrow;
			var newHeight = Position.Height + mHeightGrow;

			if (newWidth > mMaxWidthExtension)
			{
				mWidthGrow = -mWidthGrow;
				newWidth = mMaxWidthExtension;
			}

			if (newWidth < 0)
			{
				mWidthGrow = -mWidthGrow;
				newWidth = 0.0;
			}

			if (newHeight > mMaxHeightExtension)
			{
				mHeightGrow = -mHeightGrow;
				newHeight = mMaxHeightExtension;
			}

			if (newHeight < 0)
			{
				mHeightGrow = -mHeightGrow;
				newHeight = 0.0;
			}

			var oldWidth = Position.Width;
			var oldHeight = Position.Height;

			Position = new Rect(Position.Location, new Size(newWidth, newHeight));


			var intersects = WorldInteraction.GetIntersectItems(this);

			if (intersects.Count > 0)
			{
				Position = new Rect(Position.Location, new Size(oldWidth, oldHeight));
			}
		}

		#endregion
	}
}
