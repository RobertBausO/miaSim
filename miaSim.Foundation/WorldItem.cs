using System;
using System.Collections.Generic;

namespace miaSim.Foundation
{
	public abstract class WorldItem : IWorldItem
	{
		#region ================== Member variables =========================

		private static long mNextId;
		private static readonly object NextIdLock = new object();

		#endregion

		#region ================== Constructor/Destructor ===================

		protected WorldItem(string type, Location location, Extension extension)
		{
			lock (NextIdLock)
			{
				Id = mNextId++;
			}

			Type = type;
			Location = location;
			Extension = extension;

			Connections = new List<Connection>();
		}

		#endregion

		#region ================== Properties ===============================

		public string Type { get; private set; }
		public long Id { get; private set; }

		public Location Location { get; private set; }
		public Extension Extension { get; private set; }

		public IList<Connection> Connections { get; private set; }

		#endregion

		#region ================== Methods ==================================

		public abstract void Update(double msSinceLastUpdate);

		public double CalcDistanceTo(IWorldItem item)
		{
			var xDistance = item.Location.X - Location.X;
			var yDistance = item.Location.Y - Location.Y;

			return Math.Sqrt(xDistance*xDistance + yDistance*yDistance);
		}

		public virtual string GetDisplayText()
		{
			return string.Format("{0}-{3}:{1}:{2}", Id, Location, Extension, Type);
		}

		#endregion

	}
}
