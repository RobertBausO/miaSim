﻿using System;
using System.Collections.Generic;
using System.Threading;
using miaSim.Tools;

namespace miaSim.Foundation
{
	public class World : IWorldItemBaseIteraction
	{
		#region ================== Member variables =========================

		public event Action<World> UpdateDone;

		private readonly List<WorldItemBase> mWorldItems;
		private readonly object mWorldItemLock = new object();
		private Thread mWorker;
		private readonly StopwatchStatistic mLoopStatistic;
		private bool mIsStopped = false;

		private IntersectionMap mMap = new IntersectionMap(20);

		#endregion

		#region ================== Constructor/Destructor ===================

		public World(Szene szene)
		{
			UpdateLock = new object();

			mLoopStatistic = new StopwatchStatistic(25, s =>
			{
				LoopsPerSecond = 1000.0 / mLoopStatistic.AverageMs;
			});

			LoopsPerSecond = 0.0;

			mWorldItems = szene.CreateItems(this);
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
		public List<WorldItemBase> Items { get { return mWorldItems; } }

		/// <summary>
		/// lock for sync with update of the world
		/// </summary>
		public object UpdateLock { get; private set; }

		public void AddItem(WorldItemBase newItem)
		{
			mWorldItems.Add(newItem);
		}

		public void RemoveItem(WorldItemBase item)
		{
			mWorldItems.Remove(item);
		}

		public void Start()
		{
			mIsStopped = false;
			mWorker = new Thread(WorkLoop) { IsBackground = true };
			mWorker.Start();
		}

		public void Stop()
		{
			mIsStopped = true;
			mWorker.Join();
		}

		private bool mUseIntersectionMap = false;

		private void WorkLoop()
		{
			// init intersection map
			if (mUseIntersectionMap)
			{
				lock(UpdateLock)
				{
					Items.ForEach(i => mMap.Add(i));
				}
			}

			do
			{
				mLoopStatistic.MeasurePoint();

				var items = Items;

				lock (UpdateLock)
				{
					int currentIndex = 0;

					while(currentIndex < items.Count)
					{
						var item = items[currentIndex];

						lock (UpdateLock)
						{
							if (mUseIntersectionMap)
								mMap.Remove(item);

							item.Update();

							if (mUseIntersectionMap)
								mMap.Add(item);

							currentIndex++;
						}
					}
				}

				if (UpdateDone != null)
				{
					UpdateDone(this);
				}

			} while (!mIsStopped);
		}


		public IList<WorldItemBase> GetIntersectItems(WorldItemBase worldItem, Type type)
		{
			if (mUseIntersectionMap)
				return GetIntersectItemsByMap(worldItem, type);

			return GetIntersectItemsDirect(worldItem, type);
		}


		public IList<WorldItemBase> GetIntersectItemsByMap(WorldItemBase worldItem, Type type)
		{
			return mMap.GetIntersects(worldItem, type);
		}

		public IList<WorldItemBase> GetIntersectItemsDirect(WorldItemBase worldItem, Type type)
		{
			var worldItemRect = worldItem.Position;
			var list = new List<WorldItemBase>();

			foreach (var item in Items)
			{
				if (type == null || item.GetType().Name == type.Name)
				{
					if (item.Id != worldItem.Id)
					{
						var itemRect = item.Position;

						if (worldItemRect.IntersectsWith(itemRect))
							list.Add(item);
					}
				}
			}

			return list;
		}

		#endregion

	}
}
