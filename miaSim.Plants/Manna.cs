using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

using miaGame;
using miaSim.Foundation;

namespace miaSim.Plants
{
	/// <summary>
	/// Configuration of a Manna
	/// </summary>
	public class MannaDns
	{
		public MannaDns()
		{
			MinExtension = 0.01;

			MinPercentageGrowPerCycle = 2.5 / 100.0;
			MaxPercentageGrowPerCylce = 5.0 / 100.0;

			PercentageGrowPerCylce = Utils.NextRandom(MinPercentageGrowPerCycle, MaxPercentageGrowPerCylce);
		}

		// fix definitions
		public double MinExtension { get; set; }

		public double MinPercentageGrowPerCycle { get; set; }
		public double MaxPercentageGrowPerCylce { get; set; }

		// individual settings
		public double PercentageGrowPerCylce { get; set; }
	}

	/// <summary>
	/// representation of Manna
	/// </summary>
	public class Manna : WorldItemBase
	{
		#region ================== Member variables =========================

		// DNS properties
		private MannaDns mDns;

		#endregion

		#region ================== Constructor/Destructor ===================

		public Manna(WorldItemBaseIteraction interaction, Rect position, MannaDns dns)
			: base(interaction, "Manna", position)
		{
			mDns = dns;
		}

		#endregion

		#region ================== Properties ===============================
		#endregion

		#region ================== Methods ==================================

		public static WorldItemBase CreateRandomized(WorldItemBaseIteraction interaction)
		{
			var dns = new MannaDns();
			var position = new Rect(new Point(Utils.NextRandom(), Utils.NextRandom()), new Size(dns.MinExtension, dns.MinExtension));
			return new Manna(interaction, position, dns);
		}

		public override void Update()
		{
			Rect oldPosition = Position;

			var intersectOnTop = false;
			var intersectOnBottom = false;
			var intersectOnLeft = false;
			var intersectOnRight = false;

			var intersects = WorldInteraction.GetIntersectItems(this, typeof(Manna));

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

			double widthDiff = Position.Width * mDns.PercentageGrowPerCylce;
			double heightDiff = Position.Height * mDns.PercentageGrowPerCylce;

			if (!intersectOnTop)
			{
				top -= heightDiff / 2;
			}

			if (!intersectOnBottom)
			{
				bottom += heightDiff / 2;
			}

			if (!intersectOnLeft)
			{
				left -= widthDiff / 2;
			}

			if (!intersectOnRight)
			{
				right += widthDiff / 2;
			}

			Position = new Rect(new Point(left, top), new Point(right, bottom));

			if (!PositionOk())
			{
				Position = oldPosition;
			}

			// die when smaller than min area
			if (Area() < mDns.MinExtension*mDns.MinExtension)
			{
				WorldInteraction.RemoveItem(this);
			}
		}

		public override void Tell(Message message)
		{
		}

		public override void Draw(PaintContext context)
		{
			var brush = Brushes.DarkGreen;
			context.DrawRectangle(brush, Position);
		}

		public override bool ChangeSize(double widthDiff, double heightDiff)
		{
			var oldPosition = Position;

			if (base.ChangeSize(widthDiff, heightDiff))
			{
				if (Position.Width < mDns.MinExtension || Position.Height < mDns.MinExtension)
				{
					Position = oldPosition;
					return false;
				}
			}

			return true;
		}

		#endregion
	}
}
