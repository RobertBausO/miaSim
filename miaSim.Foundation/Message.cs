namespace miaSim.Foundation
{
	public class Message
	{
		public Message(WorldItemBase senderItem, string command)
		{
			SenderItem = senderItem;
			Command = command;
		}

		public WorldItemBase SenderItem { get; set; }
		public string Command { get; set; }
	}
}
