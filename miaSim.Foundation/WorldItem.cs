using System;
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

		public virtual string GetDisplayText()
		{
			var position = string.Format("{0},{1},{2},{3}", Position.Left, Position.Top, Position.Right, Position.Bottom);
			return string.Format("{0}-{1}:{2}", Id, Type, position);
		}

		#endregion

	}
}
