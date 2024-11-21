using System;
using System.Collections.Generic;
using MusicRequests.Core.Models;
using MvvmCross.Plugin.Messenger;

namespace MusicRequests.Core
{
	public class ShowViewModelMessage : MvxMessage
	{
		public Type ViewModelType { get; set; }
		public Dictionary<string, string> Parameters { get; set; }

		public ShowViewModelMessage (object sender, Type viewModelType, Dictionary<string, string> parameters) : base (sender)
		{
			ViewModelType = viewModelType;
			Parameters = parameters;
		}
	}
}
