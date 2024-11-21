using System;
using MvvmCross.Plugin.Messenger;

namespace MusicRequests.Core.Messages
{
	public class DismissMenuMessage : MvxMessage
	{
		public DismissMenuMessage (object sender) : base (sender)
		{
		}
	}
}

