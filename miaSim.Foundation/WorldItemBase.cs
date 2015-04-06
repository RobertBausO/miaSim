using miaGame;
using System;
using System.Collections.Generic;
using System.Windows;

namespace miaSim.Foundation
{
	public abstract class WorldItemBase
	{
		#region ================== Member variables =========================

		private static long mNextId;
		private static readonly object NextIdLock = new object();

		#endregion

		#region ================== Constructor/Destructor ===================

		protected WorldItemBase(WorldItemBaseIteraction worldInteraction, string type, Rect position)
		{
			lock (NextIdLock)
			{
				Id = mNextId++;
			}

			WorldInteraction = worldInteraction;

			Type = type;
			Position = position;
		}

		#endregion

		#region ================== Properties ===============================

		public WorldItemBaseIteraction WorldInteraction { get; private set; }

		public string Type { get; private set; }
		public long Id { get; private set; }

		public Rect Position { get; set; }

		public virtual int ZPosition { get { return 0; } }

		#endregion

		#region ================== Methods ==================================

		public abstract void Update();
		public abstract void Draw(PaintContext paintInfo);
		public abstract void Tell(Message message);

		public Point CalcCenter()
		{
			return new Point(Position.Left + Position.Width / 2.0, Position.Top + Position.Height / 2.0);
		}

		public void AdjustPosition()
		{
			var left = Position.Left;
			var right = Position.Right;
			var top = Position.Top;
			var bottom = Position.Bottom;

			var changed = false;

			Adjust(ref left, ref changed);
			Adjust(ref right, ref changed);
			Adjust(ref top, ref changed);
			Adjust(ref bottom, ref changed);

			if (changed)
			{
				Position = new Rect(new Point(left, top), new Point(right, bottom));
			}
		}

		private void Adjust(ref double value, ref bool changed)
		{
			if (value < 0.0)
			{
				value = 0.0;
				changed = true;
			}

			if (value > 1.0)
			{
				value = 1.0;
				changed = true;
			}
		}

		public virtual string GetDisplayText()
		{
			var position = string.Format("{0}/{1}/{2}/{3}", Utils.Double2String(Position.Left), Utils.Double2String(Position.Top), Utils.Double2String(Position.Right), Utils.Double2String(Position.Bottom));
			return string.Format("{0}-{1}:{2}", Id, Type, position);
		}

		public static void TransferSize(WorldItemBase from, WorldItemBase to, double part)
		{
			var widthTransfer = from.Position.Width * part;
			var heightTransfer = from.Position.Height * part;

			from.ChangeSize(-widthTransfer, -heightTransfer);
			to.ChangeSize(widthTransfer, heightTransfer);
		}

		public void ChangeSize(double widthDiff, double heightDiff)
		{
			var w = widthDiff / 2.0;
			var h = heightDiff / 2.0;

			Position = new Rect(new Point(Position.Left - w, Position.Top - h), 
										new Point(Position.Right + w, Position.Bottom + h));

			AdjustPosition();
		}

		public void SetSize(double width, double height)
		{
			var w = width / 2.0;
			var h = height / 2.0;

			var center = CalcCenter();
			
			Position = new Rect(new Point(center.X - w, center.Y - h),
										new Point(center.X + w, center.Y + h));
			AdjustPosition();
		}

		public void Move(double xee, double yps)
		{
			Position = new Rect(new Point(Position.Left + xee, Position.Top + yps),
										new Point(Position.Right + xee, Position.Bottom + yps));
		}


		#endregion

	}
}
