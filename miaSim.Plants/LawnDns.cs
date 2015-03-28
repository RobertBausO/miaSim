using miaSim.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miaSim.Plants
{
	public class LawnDns
	{
		public LawnDns()
		{
			MinExtension = 0.01;

			MinGrowPerCycle = 0.00001f;
			MaxGrowPerCylce = 0.0001f;

			GrowPerCylce = Utils.NextRandom(MinGrowPerCycle, MaxGrowPerCylce);

		}

		public double MinExtension { get; set; }

		public double MinGrowPerCycle { get; set; }
		public double MaxGrowPerCylce { get; set; }

		public double GrowPerCylce { get; set; }
	}
}
