﻿using System;
using System.Collections.Generic;
using System.Windows;

namespace miaSim.Foundation
{
	public abstract class WorldItem : IWorldItem
	{
		#region ================== Member variables =========================

		private static long mNextId;
		private static readonly object NextIdLock = new object();

		#endregion

		#region ================== Constructor/Destructor ===================

		protected WorldItem(IWorldItemIteraction worldInteraction, string type, Rect position)
		{
			lock (NextIdLock)
			{
				Id = mNextId++;
			}

			WorldInteraction = worldInteraction;

			Type = type;
			Position = position;

			Connections = new List<Connection>();
		}

		#endregion

		#region ================== Properties ===============================

		public IWorldItemIteraction WorldInteraction { get; private set; }

		public string Type { get; private set; }
		public long Id { get; private set; }

		public Rect Position { get; set; }

		public IList<Connection> Connections { get; private set; }

		#endregion

		#region ================== Methods ==================================

		public abstract void Update(double msSinceLastUpdate);

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

		#endregion

	}
}
