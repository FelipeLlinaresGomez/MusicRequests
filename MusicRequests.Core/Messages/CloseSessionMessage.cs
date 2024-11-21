using System;
using MvvmCross.Plugin.Messenger;

namespace MusicRequests.Core
{
	public class CloseSessionMessage : MvxMessage
	{
		public CloseSessionMessage (object sender):base(sender)
		{
		}
	}
}

