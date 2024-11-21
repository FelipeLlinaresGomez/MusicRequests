using System;
using MvvmCross.Plugin.Messenger;

namespace MusicRequests.Core.Messages
{
	public class ChangeAvatarMessage : MvxMessage
	{
		public byte[] PhotoBytes { get; set;}

		public ChangeAvatarMessage(object sender, byte[] photo) : base(sender) 
		{
			PhotoBytes = photo;		
		}
	}
}
