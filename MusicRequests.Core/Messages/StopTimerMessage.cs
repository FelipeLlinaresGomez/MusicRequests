using System;
using MvvmCross.Plugin.Messenger;

namespace MusicRequests.Core
{
	public class StopTimerMessage : MvxMessage
	{
		public bool RestartTimer { get; set; }

		public StopTimerMessage(object sender, bool restartTimer = false):base(sender)
		{
			RestartTimer = restartTimer;
		}
	}
}

