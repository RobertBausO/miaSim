﻿using System;
using System.Collections.Generic;
using System.Windows;

namespace miaSim.Foundation
{
	public class IntersectionMap
	{
		#region ================== Member variables =========================

		private readonly int mCountXandY;
		private Dictionary<long, IWorldItem>[,] mData;

		#endregion

		#region ================== Constructor/Destructor ===================

		public IntersectionMap(int countXandY)
		{
			mCountXandY = countXandY;
			Reset();
		}

		#endregion

		#region ================== Properties ===============================
		#endregion

		#region ================== Methods ==================================

		public void Reset()
		{
			mData = new Dictionary<long, IWorldItem>[mCountXandY+1, mCountXandY+1];
		}

		public void Add(IWorldItem item)
		{
			ForEachMapEntry(item, d => d.Add(item.Id, item));
		}

		public void Remove(IWorldItem item)
		{
			ForEachMapEntry(item, d => d.Remove(item.Id));
		}

		public IList<IWorldItem> GetIntersects(IWorldItem item)
		{
			var list = new List<IWorldItem>();
			var worldRect = Utils.LocationExtension2Rect(item.Location, item.Extension);
			var mapRect = World2Map(worldRect);

			// find intersections
			//var candidates = GetCandidatesByOutbound(mapRect);
			var candidates = GetCandidatesByFullScan(mapRect);

			foreach (var candidate in candidates.Values)
			{
				if (candidate.Id != item.Id)
				{
					var candidateRect = Utils.LocationExtension2Rect(candidate.Location, candidate.Extension);

					if (candidateRect.IntersectsWith(worldRect))
						list.Add(item);
				}
			}

			return list;
		}

		private Dictionary<long, IWorldItem> GetCandidatesByOutbound(Rect mapRect)
		{
			var candidates = new Dictionary<long, IWorldItem>();

			var outboundRect = new Rect(new Point(Adjust((int)mapRect.Left - 1), Adjust((int)mapRect.Top + 1)),
				new Point(Adjust((int)mapRect.Right + 1), Adjust((int)mapRect.Bottom - 1)));

			// scan the outbound line of the rect
			var action = new Action<Dictionary<long, IWorldItem>>(dict => MergeDictionaries(candidates, dict));

			// top
			ForEachMapEntry(new Rect(new Point(outboundRect.Left, outboundRect.Top), new Point(outboundRect.Right, outboundRect.Top)), action);

			// left
			ForEachMapEntry(new Rect(new Point(outboundRect.Left, outboundRect.Top), new Point(outboundRect.Left, outboundRect.Bottom)), action);

			// right
			ForEachMapEntry(new Rect(new Point(outboundRect.Right, outboundRect.Top), new Point(outboundRect.Right, outboundRect.Bottom)), action);

			// bottom
			ForEachMapEntry(new Rect(new Point(outboundRect.Left, outboundRect.Bottom), new Point(outboundRect.Right, outboundRect.Bottom)), action);

			return candidates;
		}

		private Dictionary<long, IWorldItem> GetCandidatesByFullScan(Rect mapRect)
		{
			var candidates = new Dictionary<long, IWorldItem>();

			var outboundRect = new Rect(new Point(Adjust((int)mapRect.Left - 1), Adjust((int)mapRect.Top - 1)),
				new Point(Adjust((int)mapRect.Right + 1), Adjust((int)mapRect.Bottom + 1)));

			// scan the outbound line of the rect
			var action = new Action<Dictionary<long, IWorldItem>>(dict => MergeDictionaries(candidates, dict));

			// full scan
			ForEachMapEntry(outboundRect, action);

			return candidates;
		}


		private static void MergeDictionaries(Dictionary<long, IWorldItem> baseDict, Dictionary<long, IWorldItem> toBeAdded)
		{
			foreach (var key in toBeAdded.Keys)
			{
				if (!baseDict.ContainsKey(key))
				{
					baseDict.Add(key, toBeAdded[key]);
				}
			}
		}


		private void ForEachMapEntry(IWorldItem item, Action<Dictionary<long, IWorldItem>> toDo)
		{
			var worldRect = Utils.LocationExtension2Rect(item.Location, item.Extension);
			var mapRect = World2Map(worldRect);

			ForEachMapEntry(mapRect, toDo);
		}

		private void ForEachMapEntry(Rect mapRect, Action<Dictionary<long, IWorldItem>> toDo)
		{



			for (var x = (int)mapRect.Left; x <= (int)mapRect.Right; x++)
			{
				for (var y = (int)mapRect.Top; y <= (int)mapRect.Bottom; y++)
				{
					if (mData[x, y] == null)
					{
						mData[x, y] = new Dictionary<long, IWorldItem>();
					}

					toDo(mData[x, y]);
				}
			}
		}

		private Rect World2Map(Rect worldRect)
		{
			var p1 = new Point(World2MapLeftBottom(worldRect.Left), World2MapRightTop(worldRect.Top));
			var p2 = new Point(World2MapRightTop(worldRect.Right), World2MapLeftBottom(worldRect.Bottom));
			return new Rect(p1, p2);
		}

		private int World2MapLeftBottom(double world)
		{
			return Adjust((int)(Math.Floor(mCountXandY * world)));
		}

		private int World2MapRightTop(double world)
		{
			return Adjust((int)(Math.Ceiling(mCountXandY * world)));
		}

		private int Adjust(int value)
		{
			if (value < 0)
				value = 0;

			if (value > mCountXandY)
				value = mCountXandY;

			return value;
		}


		#endregion

	}
}
