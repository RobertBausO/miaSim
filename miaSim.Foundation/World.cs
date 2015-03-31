using System;
using System.Collections.Generic;
using System.Threading;
using miaGame.Tools;

namespace miaSim.Foundation
{
	public class World : IWorldItemIteraction
	{
		#region ================== Member variables =========================

		public event Action<World> UpdateDone;

		private readonly List<IWorldItem> mWorldItems;
		private readonly object mWorldItemLock = new object();
		private Thread mWorker;
		private readonly StopwatchStatistic mLoopStatistic;

		private IntersectionMap mMap = new IntersectionMap(20);

		#endregion

		#region ================== Constructor/Destructor ===================

		private World()
		{
			UpdateLock = new object();

			mWorldItems = new List<IWorldItem>();

			mLoopStatistic = new StopwatchStatistic(25, s =>
			{
				LoopsPerSecond = 1000.0 / mLoopStatistic.AverageMs;
			});

			LoopsPerSecond = 0.0;
		}

		#endregion

		#region ================== Properties ===============================

		public double LoopsPerSecond { get; private set; }

		public string Info { get; set; }

		#endregion

		#region ================== Methods ==================================

		/// <summary>
		/// all items of the world
		/// </summary>
		public List<IWorldItem> Items { get { return mWorldItems; } }

		/// <summary>
		/// lock for sync with update of the world
		/// </summary>
		public object UpdateLock { get; private set; }

		/// <summary>
		/// create a world (random)
		/// </summary>
		/// <returns></returns>
		public static World Create(int numberOfItems, IList<Func<IWorldItemIteraction, IWorldItem>> factories)
		{
			var newWorld = new World();

			for (int itemIdx = 0; itemIdx < numberOfItems; itemIdx++)
			{
				var itemTypeIndex = Utils.Next(0, factories.Count - 1);
				var item = factories[itemTypeIndex](newWorld);

				newWorld.AddItem(item);
			}

			return newWorld;
		}

		public void AddItem(IWorldItem newItem)
		{
			//foreach (var item in mWorldItems)
			//{
			//	var distance = item.CalcDistanceTo(newItem);

			//	newItem.Connections.Add(new Connection(item, distance));
			//	item.Connections.Add(new Connection(newItem, distance));
			//}

			mWorldItems.Add(newItem);
		}

		public void Start()
		{
			mWorker = new Thread(WorkLoop) { IsBackground = true };
			mWorker.Start();
		}

		private void WorkLoop()
		{
			// init intersection map
			Items.ForEach(i => mMap.Add(i));

			do
			{
				mLoopStatistic.MeasurePoint();

				var items = Items;

				lock (UpdateLock)
				{
					foreach (var item in items)
					{
						mMap.Remove(item);
						{
							item.Update();
						}
						mMap.Add(item);
					}
				}

				if (UpdateDone != null)
				{
					UpdateDone(this);
				}

			} while (true);
		}


		public IList<IWorldItem> GetIntersectItems(IWorldItem worldItem)
		{
			return GetIntersectItemsByMap(worldItem);
			//return GetIntersectItemsDirect(worldItem);
		}


		public IList<IWorldItem> GetIntersectItemsByMap(IWorldItem worldItem)
		{
			return mMap.GetIntersects(worldItem);
		}

		public IList<IWorldItem> GetIntersectItemsDirect(IWorldItem worldItem)
		{
			var worldItemRect = worldItem.Position;
			var list = new List<IWorldItem>();

			foreach (var item in Items)
			{
				if (item.Id != worldItem.Id)
				{
					var itemRect = item.Position;

					if (worldItemRect.IntersectsWith(itemRect))
						list.Add(item);
				}
			}

			return list;
		}

		#endregion

	}
}
