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

			MinGrowPerCycle = 0.00001f;
			MaxGrowPerCylce = 0.0001f;

			GrowPerCylce = Utils.NextRandom(MinGrowPerCycle, MaxGrowPerCylce);
		}

		// fix definitions
		public double MinExtension { get; set; }

		public double MinGrowPerCycle { get; set; }
		public double MaxGrowPerCylce { get; set; }

		// individual settings
		public double GrowPerCylce { get; set; }
	}

	/// <summary>
	/// representation of Manna
	/// </summary>
	public class Manna : WorldItemBase
	{
		#region ================== Member variables =========================

		// DNS properties
		private MannaDns mDns;
		private IList<WorldItemBase> mConnections;

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

			var intersects = WorldInteraction.GetIntersectItems(this, typeof(Manna));

			var intersectOnTop = false;
			var intersectOnBottom = false;
			var intersectOnLeft = false;
			var intersectOnRight = false;

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

			if (!PositionOk())
			{
				Position = oldPosition;
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
