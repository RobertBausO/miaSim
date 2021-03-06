﻿using System;
using System.Collections.Generic;
using System.Windows;

namespace miaSim.Foundation
{
	public class IntersectionMap
	{
		#region ================== Member variables =========================

		private readonly int mCountXandY;
		private Dictionary<long, WorldItemBase>[,] mData;

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
			mData = new Dictionary<long, WorldItemBase>[mCountXandY+1, mCountXandY+1];
		}

		public void Add(WorldItemBase item)
		{
			ForEachMapEntry(item, d => d.Add(item.Id, item), "Add");
		}

		public void Remove(WorldItemBase item)
		{
			//ForEachMapEntry(item, d => d.Remove(item.Id), "Remove");
			//var mapRect = World2Map(item.Position);

			// complete scan, because position could have been changed since last Add
			for (var x = 0; x <= mCountXandY; x++)
			{
				for (var y = 0; y <= mCountXandY; y++)
				{
					if (mData[x,y] != null && mData[x,y].ContainsKey(item.Id))
					{
						mData[x, y].Remove(item.Id);
					}
				}
			}
		}

		public IList<WorldItemBase> GetIntersects(WorldItemBase item, Type type)
		{
			var list = new List<WorldItemBase>();
			var worldRect = item.Position;
			var mapRect = World2Map(worldRect);

			// find intersections
			var candidates = GetCandidatesByFullScan(mapRect);

			foreach (var candidate in candidates.Values)
			{
				if (type == null || candidate.GetType().Name == type.Name)
				{
					if (candidate.Id != item.Id)
					{
						var candidateRect = candidate.Position;

						if (candidateRect.IntersectsWith(worldRect))
							list.Add(candidate);
					}
				}
			}

			return list;
		}

		private Dictionary<long, WorldItemBase> GetCandidatesByFullScan(Rect mapRect)
		{
			var candidates = new Dictionary<long, WorldItemBase>();

			var outboundRect = new Rect(new Point(Adjust((int)mapRect.Left - 1), Adjust((int)mapRect.Top - 1)),
				new Point(Adjust((int)mapRect.Right + 1), Adjust((int)mapRect.Bottom + 1)));

			// scan the outbound line of the rect
			var action = new Action<Dictionary<long, WorldItemBase>>(dict => MergeDictionaries(candidates, dict));

			// full scan
			ForEachMapEntry(outboundRect, action);

			return candidates;
		}


		private static void MergeDictionaries(Dictionary<long, WorldItemBase> baseDict, Dictionary<long, WorldItemBase> toBeAdded)
		{
			foreach (var key in toBeAdded.Keys)
			{
				if (!baseDict.ContainsKey(key))
				{
					baseDict.Add(key, toBeAdded[key]);
				}
			}
		}


		private void ForEachMapEntry(WorldItemBase item, Action<Dictionary<long, WorldItemBase>> toDo, string description)
		{
			var worldRect = item.Position;
			var mapRect = World2Map(worldRect);

			//System.Diagnostics.Debug.WriteLine(string.Format("MapEntry;Action={0};ID={1};Rect={2}", description, item.Id, mapRect));

			ForEachMapEntry(mapRect, toDo);
		}

		private void ForEachMapEntry(Rect mapRect, Action<Dictionary<long, WorldItemBase>> toDo)
		{
			for (var x = (int)mapRect.Left; x <= (int)mapRect.Right; x++)
			{
				for (var y = (int)mapRect.Top; y <= (int)mapRect.Bottom; y++)
				{
					if (mData[x, y] == null)
					{
						mData[x, y] = new Dictionary<long, WorldItemBase>();
					}

					toDo(mData[x, y]);
				}
			}
		}

		private Rect World2Map(Rect worldRect)
		{
			var p1 = new Point(World2MapLeftTop(worldRect.Left), World2MapLeftTop(worldRect.Top));
			var p2 = new Point(World2MapRightBottom(worldRect.Right), World2MapRightBottom(worldRect.Bottom));
			return new Rect(p1, p2);
		}

		private int World2MapLeftTop(double world)
		{
			return Adjust((int)(Math.Floor(mCountXandY * world)));
		}

		private int World2MapRightBottom(double world)
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
