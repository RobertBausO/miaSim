namespace miaSim.Foundation
{
	public class Connection
	{
		public Connection(IWorldItem item, double distance)
		{
			Item = item;
			Distance = distance;
		}

		public IWorldItem Item { get; private set; }
		public double Distance { get; private set; }
	}
}
