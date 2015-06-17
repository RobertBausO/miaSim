using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

using miaGame;
using miaSim.Foundation;
using miaSim.Tools;


namespace miaSim.Plants
{
	/// <summary>
	/// Configuration of a BrainCell
	/// </summary>
	public class BrainCellDns
	{
		public BrainCellDns()
		{
			MinExtension = 0.01;

			MinGrowPerCycle = 0.00001f;
			MaxGrowPerCylce = 0.0001f;

			GrowPerCylce = SimRandom.NextRandom(MinGrowPerCycle, MaxGrowPerCylce);

		}

		public double MinExtension { get; set; }

		public double MinGrowPerCycle { get; set; }
		public double MaxGrowPerCylce { get; set; }

		public double GrowPerCylce { get; set; }
	}

	/// <summary>
	/// representation of lawn
	/// </summary>
	public class BrainCell : WorldItemBase
	{
		#region ================== Member variables =========================

		private const string COMMAND_TRANSFER_LOAD = "TransferLoad";

		// DNS properties
		private BrainCellDns mDns;
		private bool mIsDocked = false;

		private IList<WorldItemBase> mConnections;

		#endregion

		#region ================== Constructor/Destructor ===================

		public BrainCell(IWorldItemBaseIteraction interaction, Rect position, BrainCellDns dns, int load)
			: base(interaction, "BrainCell", position)
		{
			Load = load;
			mDns = dns;
		}

		#endregion

		#region ================== Properties ===============================

		public int Load { get; set; }

		#endregion

		#region ================== Methods ==================================

		public static WorldItemBase CreateRandomized(IWorldItemBaseIteraction interaction)
		{
			var dns = new BrainCellDns();
			var position = new Rect(new Point(SimRandom.NextRandom(), SimRandom.NextRandom()), new Size(dns.MinExtension, dns.MinExtension));
			return new BrainCell(interaction, position, dns, SimRandom.Next(0,100) < 10 ? 1: 0);
		}

		public override void Update()
		{
			if (!mIsDocked)
			{
				Rect oldPosition = Position;

				var intersects = WorldInteraction.GetIntersectItems(this, null);

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

				if (PositionOk())
				{
					Position = oldPosition;
				}

				if (intersectOnTop && intersectOnLeft && intersectOnRight && intersectOnBottom)
				{
					mIsDocked = true;
					mConnections = intersects;
				}
			}
			else
			{
				if (Load > 0)
				{
					var connectionId = SimRandom.Next(0, mConnections.Count - 1);
					mConnections[connectionId].Tell(new Message(this, COMMAND_TRANSFER_LOAD));
				}
			}
		}

		public override void Tell(Message message)
		{
			if (message.Command == COMMAND_TRANSFER_LOAD)
			{
				var cell = message.SenderItem as BrainCell;

				if (cell != null)
				{
					cell.Load--;
					Load++;
				}
			}
		}

		public override void Draw(PaintContext context)
		{
			var brush = mIsDocked ? Brushes.Blue : Brushes.Green;
			context.DrawRectangle(brush, Position);
			
			if (Load > 0)
			{
				var center = CalcCenter();
				context.DrawEllipse(Brushes.Yellow, center.X, center.Y, Position.Width / 3, Position.Height / 3);
			}

			if (mConnections != null)
			{
				foreach (var connection in mConnections)
				{
					context.DrawLine(Brushes.Black, this.CalcCenter(), connection.CalcCenter());
				}

				context.DrawText(mConnections.Count.ToString(), this.CalcCenter(), TextSize.Small, Brushes.Black);
			}
		}


		#endregion
	}
}
