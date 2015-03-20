using System;
using System.Collections.Generic;
using System.Threading;
using miaGame.Tools;

namespace miaSim.Foundation
{
    public class World : IWorld
    {
		 #region ================== Member variables =========================

	    public event Action<World> UpdateDone;

	    private readonly IList<IWorldItem> mWorldItems;
		 private readonly object mWorldItemLock = new object();
	    private Thread mWorker;
		 private readonly StopwatchStatistic mLoopStatistic;

		 #endregion

		 #region ================== Constructor/Destructor ===================

	    private World()
	    {
			 mWorldItems = new List<IWorldItem>();

			 mLoopStatistic = new StopwatchStatistic(10, s =>
			 {
				 LoopsPerSecond = 1000.0 / mLoopStatistic.Average.TotalMilliseconds;
			 });

			 LoopsPerSecond = 0.0;
	    }

		 #endregion

		 #region ================== Properties ===============================

		 public double LoopsPerSecond { get; private set; }

		 #endregion

		 #region ================== Methods ==================================

	    public IList<IWorldItem> GetSnapshotOfItems()
	    {
		    IList<IWorldItem> newList;

		    lock (mWorldItemLock)
		    {
			    newList = new List<IWorldItem>(mWorldItems);
		    }

		    return newList;
	    }

		 /// <summary>
		 /// create a world (random)
		 /// </summary>
		 /// <returns></returns>
	    public static World Create(int numberOfItems, IList<Func<IWorldItem>> factories)
	    {
		    var newWorld = new World();

		    for (int itemIdx = 0; itemIdx < numberOfItems; itemIdx++)
		    {
			    var itemTypeIndex = Utils.Next(0, factories.Count - 1);
			    var item = factories[itemTypeIndex]();

				 newWorld.AddItem(item);
		    }

			 return newWorld;
	    }

	    public void AddItem(IWorldItem newItem)
	    {
		    foreach (var item in mWorldItems)
		    {
			    var distance = item.CalcDistanceTo(newItem);

				 newItem.Connections.Add(new Connection(item, distance));
				 item.Connections.Add(new Connection(newItem, distance));
		    }

			 mWorldItems.Add(newItem);
	    }

	    public void Start()
	    {
			 mWorker = new Thread(WorkLoop) {IsBackground = true};
		    mWorker.Start();
	    }

	    private void WorkLoop()
	    {
		    var dictLastUpdate = new Dictionary<long, DateTime>();

		    do
		    {
				 mLoopStatistic.MeasurePoint();

				 var items = GetSnapshotOfItems();

			    foreach (var item in items)
			    {
					 var now = DateTime.Now;

					 if (dictLastUpdate.ContainsKey(item.Id))
				    {
					    var lastUpdate = dictLastUpdate[item.Id];
					    var diff = now.Subtract(lastUpdate);

						 item.Update(diff.TotalMilliseconds);
				    }
					 else
					 {
						 item.Update(0.0);
					 }

					 dictLastUpdate[item.Id] = now;
			    }

			    if (UpdateDone != null)
			    {
				    UpdateDone(this);
			    }

		    } while (true);
	    }

		 #endregion
    }
}
