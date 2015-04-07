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

		public bool PositionOk()
		{
			var left = Position.Left;
			var right = Position.Right;
			var top = Position.Top;
			var bottom = Position.Bottom;

			var isOK = true;

			Check(left, ref isOK);
			Check(right, ref isOK);
			Check(top, ref isOK);
			Check(bottom, ref isOK);

			return isOK;
		}

		private void Check(double value, ref bool isOk)
		{
			if (value < 0.0 || value > 1.0)
			{
				isOk = false;
			}
		}

		public virtual string GetDisplayText()
		{
			var position = string.Format("{0}/{1}/{2}/{3}", Utils.Double2String(Position.Left), Utils.Double2String(Position.Top), Utils.Double2String(Position.Right), Utils.Double2String(Position.Bottom));
			return string.Format("{0}-{1}:{2}", Id, Type, position);
		}


		public virtual bool ChangeSize(double factor)
		{
			var w = Position.Width * factor;
			var h = Position.Height * factor;

			return ChangeSize(w, h);
		}

		public virtual bool ChangeSize(double widthDiff, double heightDiff)
		{
			var oldPos = Position;

			var w = widthDiff / 2.0;
			var h = heightDiff / 2.0;

			Position = new Rect(new Point(Position.Left - w, Position.Top - h), 
								new Point(Position.Right + w, Position.Bottom + h));

			if (!PositionOk())
			{
				Position = oldPos;
				return false;
			}

			return true;
		}

		public virtual bool SetSize(double width, double height)
		{
			var oldPos = Position;

			var w = width / 2.0;
			var h = height / 2.0;

			var center = CalcCenter();
			
			Position = new Rect(new Point(center.X - w, center.Y - h),
								new Point(center.X + w, center.Y + h));

			if (!PositionOk())
			{
				Position = oldPos;
				return false;
			}

			return true;
		}

		public virtual bool Move(double xee, double yps)
		{
			var oldPos = Position;

			Position = new Rect(new Point(Position.Left + xee, Position.Top + yps),
								new Point(Position.Right + xee, Position.Bottom + yps));

			if (!PositionOk())
			{
				Position = oldPos;
				return false;
			}

			return true;
		}


		#endregion

	}
}
