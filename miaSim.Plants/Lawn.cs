using System;
using miaSim.Foundation;
using System.Windows;
using miaGame.Painter;
using System.Windows.Media;
using System.Collections.Generic;

namespace miaSim.Plants
{
	/// <summary>
	/// representation of lawn
	/// </summary>
	public class Lawn : WorldItemBase
	{
		#region ================== Member variables =========================

		private const string COMMAND_TRANSFER_LOAD = "TransferLoad";

		// DNS properties
		private LawnDns mDns;
		private bool mIsDocked = false;

		private IList<IWorldItem> mConnections;

		#endregion

		#region ================== Constructor/Destructor ===================

		public Lawn(IWorldItemIteraction interaction, Rect position, LawnDns dns, int load)
			: base(interaction, "Lawn", position)
		{
			Load = load;
			mDns = dns;
		}

		#endregion

		#region ================== Properties ===============================

		public int Load { get; set; }

		#endregion

		#region ================== Methods ==================================

		public static IWorldItem CreateRandomTree(IWorldItemIteraction interaction)
		{
			var dns = new LawnDns();
			var position = new Rect(new Point(Utils.NextRandom(), Utils.NextRandom()), new Size(dns.MinExtension, dns.MinExtension));

			return new Lawn(interaction, position, dns, Utils.Next(0,100) < 10 ? 1: 0);
		}

		public override void Update()
		{
			if (!mIsDocked)
			{
				Rect oldPosition = Position;

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
					mConnections = intersects;
				}
			}
			else
			{
				if (Load > 0)
				{
					var connectionId = Utils.Next(0, mConnections.Count - 1);
					mConnections[connectionId].Tell(new Message(this, COMMAND_TRANSFER_LOAD));
				}
			}

			//System.Diagnostics.Debug.WriteLine(this.GetDisplayText());
		}

		public override void Tell(Message message)
		{
			if (message.Command == COMMAND_TRANSFER_LOAD)
			{
				var lawn = message.SenderItem as Lawn;

				if (lawn != null)
				{
					lawn.Load--;
					Load++;
				}
			}
		}

		public override void Draw(PaintInfo paintInfo)
		{
			var brush = mIsDocked ? Brushes.Blue : Brushes.Green;
			DirectPainterHelper.DrawRectangle(brush, Position, paintInfo);
			
			if (Load > 0)
			{
				var center = Center();
				DirectPainterHelper.DrawEllipse(Brushes.Yellow, center.X, center.Y, Position.Width / 3, Position.Height / 3, paintInfo);
			}

			if (mConnections != null)
			{
				foreach (var connection in mConnections)
				{
					DirectPainterHelper.DrawLine(Brushes.Black, this.Center(), connection.Center(), paintInfo);
				}

				DirectPainterHelper.DrawText(mConnections.Count.ToString(), this.Center(), TextSize.Small, Brushes.Black, paintInfo);
			}
		}


		#endregion
	}
}
