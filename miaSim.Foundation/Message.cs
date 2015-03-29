using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miaSim.Foundation
{
	public class Message
	{
		public Message(IWorldItem senderItem, string command)
		{
			SenderItem = senderItem;
			Command = command;
		}

		public IWorldItem SenderItem { get; set; }
		public string Command { get; set; }
	}
}
