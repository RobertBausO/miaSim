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
				new Szene(CreateSzene001, "four mannas"),
				new Szene(CreateSzene002, "one mannas + one eater"),
				new Szene(CreateSzene003, "four mannas + one eater"),
				new Szene((i)=>CreateRandom(100, true, false, i), "Manna-Random(100)"),
				new Szene((i)=>CreateRandom(1000, true, false, i), "Manna-Random(1000)"),
				new Szene((i)=>CreateRandom(2000, true, false, i), "Manna-Random(2000)"),
				new Szene((i)=>CreateRandom(100, true, true, i), "M+E-Random(100)"),
				new Szene((i)=>CreateRandom(500, true, true, i), "M+E-Random(500)"),
				new Szene((i)=>CreateRandom(1000, true, true, i), "M+E-Random(1000)"),
			};
		}

		/// <summary>
		/// 4 x Manna grows from small to big
		/// Result: 4 Manna, like a chess board
		/// </summary>
		/// <returns></returns>
		private static List<WorldItemBase> CreateSzene001(IWorldItemBaseIteraction interaction)
		{
			var leftManna = new Manna(interaction, Adjust(new Rect(200, 450, 100, 100)), new MannaDns());
			var topManna = new Manna(interaction, Adjust(new Rect(450, 200, 100, 100)), new MannaDns());
			var rightManna = new Manna(interaction, Adjust(new Rect(700, 450, 100, 100)), new MannaDns());
			var bottomManna = new Manna(interaction, Adjust(new Rect(450, 700, 100, 100)), new MannaDns());
			return new List<WorldItemBase> { leftManna, topManna, rightManna, bottomManna };
		}

		/// <summary>
		/// 1 x Manna grows from small to big
		/// </summary>
		/// <returns></returns>
		private static List<WorldItemBase> CreateSzene002(IWorldItemBaseIteraction interaction)
		{
			var dns = new MannaEaterDns();

			return new List<WorldItemBase>
			{
				new Manna(interaction, Adjust(new Rect(450, 450, 100, 100)), new MannaDns()),
				new MannaEater(interaction, new Rect(0.5, 0.5, dns.MinExtension * 2.0, dns.MinExtension * 2.0), new Vector(), dns)
			};
		}

		/// <summary>
		/// 4 x Manna grows from small to big
		/// Result: 4 Manna, like a chess board
		/// </summary>
		/// <returns></returns>
		private static List<WorldItemBase> CreateSzene003(IWorldItemBaseIteraction interaction)
		{
			var list = CreateSzene001(interaction);

			var dns = new MannaEaterDns();
			var eater = new MannaEater(interaction, new Rect(0.500, 0.500, dns.MaxExtension / 2.0, dns.MaxExtension / 2.0), new Vector(-dns.MaxMovement, -dns.MaxMovement), dns);
			list.Add(eater);

			return list;
		}

		/// <summary>
		/// create a world (random)
		/// </summary>
		/// <returns></returns>
		private static List<WorldItemBase> CreateRandom(int numberOfItems, bool manna, bool mannaEater, IWorldItemBaseIteraction interaction)
		{
			var list = new List<WorldItemBase>();
			var factories = new List<Func<IWorldItemBaseIteraction, WorldItemBase>>();

			if (manna) factories.Add(Manna.CreateRandomized);
			if (mannaEater) factories.Add(MannaEater.CreateRandomized);

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
