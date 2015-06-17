using System;
using System.Collections.Generic;

namespace miaSim.Foundation
{
	public class Szene
	{
		#region ================== Member variables =========================

		private readonly Func<IWorldItemBaseIteraction, List<WorldItemBase>> mCreateWorldFunc;

		#endregion

		#region ================== Constructor/Destructor ===================

		public Szene(Func<IWorldItemBaseIteraction, List<WorldItemBase>> createWorld, string name)
		{
			mCreateWorldFunc = createWorld;
			Name = name;
		}

		#endregion

		#region ================== Properties ===============================

		public string Name { get; private set; }

		#endregion

		#region ================== Methods ==================================

		public List<WorldItemBase> CreateItems(IWorldItemBaseIteraction interaction)
		{
			return mCreateWorldFunc(interaction);
		}

		#endregion
	}
}
