using System;
using System.Windows;
using System.Windows.Media;

using miaGame;
using miaSim.Foundation;
using miaSim.Tools;

namespace miaSim.Plants
{
	/// <summary>
	/// Configuration of a Manna
	/// </summary>
	public class MannaDns
	{
		public MannaDns()
		{
			StartExtension = 0.001;
			PercentageGrowPerCylce = 0.5 / 100.0;
		}

		// fix definitions
		public double StartExtension { get; set; }

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

		public Manna(IWorldItemBaseIteraction interaction, Rect position, MannaDns dns)
			: base(interaction, "Manna", position)
		{
			mDns = dns;
		}

		#endregion

		#region ================== Properties ===============================
		#endregion

		#region ================== Methods ==================================

		public static WorldItemBase CreateRandomized(IWorldItemBaseIteraction interaction)
		{
			var dns = new MannaDns();
			var position = new Rect(
				new Point(SimRandom.NextRandom(0.0, 1 - dns.StartExtension), SimRandom.NextRandom(0.0, 1 - dns.StartExtension)), 
				new Size(dns.StartExtension, dns.StartExtension));
			return new Manna(interaction, position, dns);
		}

		public override void Update()
		{
			if (WillDie())
			{
				Die();
				return;
			}

			Grow();

			System.Diagnostics.Debug.WriteLine("MannaArea: " + Area());
		}

		private bool WillDie()
		{
			return Area() < mDns.StartExtension*mDns.StartExtension;
		}

		private void Die()
		{
			WorldInteraction.RemoveItem(this);
		}

		private void CanGrow(out bool canLeft, out bool canTop, out bool canRight, out bool canBottom)
		{
			var intersectOnTop = false;
			var intersectOnBottom = false;
			var intersectOnLeft = false;
			var intersectOnRight = false;

			foreach (var intersect in WorldInteraction.GetIntersectItems(this, typeof(Manna)))
			{
				var intLeft = intersect.Position.Left;
				var intRight = intersect.Position.Right;
				var intTop = intersect.Position.Top;
				var intBottom = intersect.Position.Bottom;

				if (Position.Top > intTop && Position.Top < intBottom)
					intersectOnTop = true;

				if (Position.Bottom > intTop && Position.Bottom < intBottom)
					intersectOnBottom = true;

				if (Position.Left > intLeft && Position.Left < intRight)
					intersectOnLeft = true;

				if (Position.Right > intLeft && Position.Right < intRight)
					intersectOnRight = true;
			}

			canLeft = !intersectOnLeft;
			canTop = !intersectOnTop;
			canRight = !intersectOnRight;
			canBottom = !intersectOnBottom;
		}

		private void Grow()
		{
			bool canLeft, canTop, canRight, canBottom;
			CanGrow(out canLeft, out canTop, out canRight, out canBottom);

			double widthDiff = Position.Width * Math.Sqrt(mDns.PercentageGrowPerCylce);
			double heightDiff = Position.Height * Math.Sqrt(mDns.PercentageGrowPerCylce);

			var left = Position.Left;
			var top = Position.Top;
			var right = Position.Right;
			var bottom = Position.Bottom;

			if (canTop)
			{
				top = Math.Max(top - heightDiff / 2, 0.0);
			}

			if (canBottom)
			{
				bottom = Math.Min(bottom + heightDiff / 2, 1.0);
			}

			if (canLeft)
			{
				left = Math.Max(left - widthDiff / 2, 0.0);
			}

			if (canRight)
			{
				right = Math.Min(right + widthDiff / 2, 1.0);
			}

			Position = new Rect(new Point(left, top), new Point(right, bottom));
		}

		public override void Tell(Message message)
		{
		}

		public override void Draw(PaintContext context)
		{
			var brush = Brushes.DarkGreen;
			context.DrawRectangle(brush, new Pen(), Position);
		}

		#endregion
	}
}
