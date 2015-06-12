using System;
using miaGame;
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

		/// <summary>
		/// calculates the center of the item
		/// </summary>
		/// <returns></returns>
		public Point CalcCenter()
		{
			return new Point(Position.Left + Position.Width / 2.0, Position.Top + Position.Height / 2.0);
		}

		/// <summary>
		/// checks whether the position is ok or not
		/// </summary>
		/// <returns></returns>
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


		public virtual bool ChangeArea(double factor)
		{
			var sqrtFactor = Math.Sqrt(factor);
			var w = Position.Width * sqrtFactor;
			var h = Position.Height * sqrtFactor;

			return SetSize(w, h);
		}

		/// <summary>
		/// change size relative
		/// </summary>
		/// <param name="factor"></param>
		/// <returns></returns>
		public virtual bool ChangeSize(double factor)
		{
			var w = Position.Width * factor;
			var h = Position.Height * factor;

			return ChangeSize(w, h);
		}

		/// <summary>
		/// change size absolute
		/// </summary>
		/// <param name="widthDiff"></param>
		/// <param name="heightDiff"></param>
		/// <returns></returns>
		public virtual bool ChangeSize(double widthDiff, double heightDiff)
		{
			var newWidth = Position.Width + widthDiff;
			var newHeight = Position.Height + heightDiff;

			return SetSize(newWidth, newHeight);
		}

		/// <summary>
		/// Set new size relative to the center of the item
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Robber eats a part of the victim
		/// </summary>
		/// <param name="victim"></param>
		/// <param name="robber"></param>
		/// <param name="taken"></param>
		/// <param name="received"></param>
		/// <returns></returns>
		public static bool MoveArea(WorldItemBase victim, WorldItemBase robber, double taken)
		{
			Rect orgVictimRect = victim.Position;
			double orgVictimArea = victim.Area();

			if (victim.ChangeArea(1 - taken))
			{
				double newVictimArea = victim.Area();

				double takenArea = orgVictimArea - newVictimArea;

				double receivedArea = takenArea;

				// h' = SQRT((h*adiff + w*h*h)w)
				double newHeight = Math.Sqrt(((receivedArea + robber.Position.Width * robber.Position.Height) * robber.Position.Height) / robber.Position.Width);

				// w' = (w/h) * h'
				double newWidth = (robber.Position.Width / robber.Position.Height) * newHeight;

				if (robber.SetSize(newWidth, newHeight))
				{
					return true;
				}

				victim.Position = orgVictimRect;
				return false;
			}

			return false;
		}

		public double Area()
		{
			return Position.Width*Position.Height;
		}


		/// <summary>
		/// move when possible
		/// </summary>
		/// <param name="xee"></param>
		/// <param name="yps"></param>
		/// <returns></returns>
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
