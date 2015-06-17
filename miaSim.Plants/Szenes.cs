using System;
using System.Collections.Generic;
using System.Windows;
using miaSim.Foundation;
using miaSim.Tools;

namespace miaSim.Plants
{
	public static class Szenes
	{
		/// <summary>
		/// in order to define the coordinates in a simples way
		/// all parameter are multiplied by this factor
		/// </summary>
		private const float AdjustFactor = 1000;

		public static List<Szene> GetSzeneList()
		{
			return new List<Szene>
			{
				new Szene((i)=>CreateRandom(100,i), "Random(100)"),
				new Szene((i)=>CreateRandom(500,i), "Random(500)"),
				new Szene((i)=>CreateRandom(1000,i), "Random(1000)"),
				new Szene(CreateSzene001, "four mannas")
			};
		}

		/// <summary>
		/// 4 x Manna grows from small to big
		/// Result: 4 Manna, like a chess board
		/// </summary>
		/// <returns></returns>
		private static List<WorldItemBase> CreateSzene001(IWorldItemBaseIteraction interaction)
		{
			var topManna = new Manna(interaction, Adjust(new Rect(450, 200, 100, 100)), new MannaDns());
			return new List<WorldItemBase> { topManna };
		}

		/// <summary>
		/// create a world (random)
		/// </summary>
		/// <returns></returns>
		private static List<WorldItemBase> CreateRandom(int numberOfItems, IWorldItemBaseIteraction interaction)
		{
			var list = new List<WorldItemBase>();
			var factories = new List<Func<IWorldItemBaseIteraction, WorldItemBase>> { Manna.CreateRandomized, MannaEater.CreateRandomized };

			for (int itemIdx = 0; itemIdx < numberOfItems; itemIdx++)
			{
				var itemTypeIndex = SimRandom.Next(0, factories.Count - 1);
				var item = factories[itemTypeIndex](interaction);

				list.Add(item);
			}

			return list;
		}

		private static Rect Adjust(Rect rect)
		{
			return new Rect(rect.X / AdjustFactor, rect.Y / AdjustFactor, rect.Width / AdjustFactor, rect.Height / AdjustFactor);
		}

	}
}
