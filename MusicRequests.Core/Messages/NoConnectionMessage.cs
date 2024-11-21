using MusicRequests.Core.Models;
using MvvmCross.Plugin.Messenger;

namespace MusicRequests.Core.Messages
{
    public class NoConnectionMessage : MvxMessage
	{
		public ConnStatus Type { get; set; }
		public NoConnectionMessage (object sender, ConnStatus type) : base (sender)
		{
			Type = type;
		}
	}
}

