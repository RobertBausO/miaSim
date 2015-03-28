using System;
using miaSim.Foundation;
using System.Windows;

namespace miaSim.Plants
{
	/// <summary>
	/// representation of lawn
	/// </summary>
	public class Lawn : WorldItem
	{
		#region ================== Member variables =========================

		// DNS properties
		private LawnDns mDns;
		private bool mIsDocked = false;

		#endregion

		#region ================== Constructor/Destructor ===================

		public Lawn(IWorldItemIteraction interaction, Rect position, LawnDns dns)
			: base(interaction, "Lawn", position)
		{
			mDns = dns;
		}

		#endregion

		#region ================== Properties ===============================
		#endregion

		#region ================== Methods ==================================

		public static IWorldItem CreateRandomTree(IWorldItemIteraction interaction)
		{
			var dns = new LawnDns();
			var position = new Rect(new Point(Utils.NextRandom(), Utils.NextRandom()), new Size(dns.MinExtension, dns.MinExtension));

			return new Lawn(interaction, position, dns);
		}

		public override void Update(double msSinceLastUpdate)
		{
			//if (mIsDocked)
			{
				var oldPosition = Position;

				var intersects = WorldInteraction.GetIntersectItems(this);

				var intersectOnTop = false;
				var intersectOnBottom = false;
				var intersectOnLeft = false;
				var intersectOnRight = false;

				if (intersects.Count > 0)
					System.Diagnostics.Debug.WriteLine("intersects");

				var left = Position.Left;
				var right = Position.Right;
				var top = Position.Top;
				var bottom = Position.Bottom;

				foreach (var intersect in intersects)
				{
					var intLeft = intersect.Position.Left;
					var intRight = intersect.Position.Right;
					var intTop = intersect.Position.Top;
					var intBottom = intersect.Position.Bottom;

					if (top > intTop && top < intBottom)
						intersectOnTop = true;

					if (bottom > intTop && bottom < intBottom)
						intersectOnBottom = true;

					if (left > intLeft && left < intRight)
						intersectOnLeft = true;

					if (right > intLeft && right < intRight)
						intersectOnRight = true;
				}

				if (!intersectOnTop)
				{
					top -= mDns.MaxGrowPerCylce;
				}

				if (!intersectOnBottom)
				{
					bottom += mDns.MaxGrowPerCylce;
				}

				if (!intersectOnLeft)
				{
					left -= mDns.MaxGrowPerCylce;
				}

				if (!intersectOnRight)
				{
					right += mDns.MaxGrowPerCylce;
				}

				Position = new Rect(new Point(left, top), new Point(right, bottom));

				AdjustPosition();

				if (oldPosition == Position)
				{
					mIsDocked = true;
				}
			}

			//System.Diagnostics.Debug.WriteLine(this.GetDisplayText());
		}

		#endregion
	}
}
