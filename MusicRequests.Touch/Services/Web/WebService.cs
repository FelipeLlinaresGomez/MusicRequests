using System;
using MusicRequests.Core.Services;
using UIKit;
using Foundation;

namespace MusicRequests.Touch.Services
{
	public class WebService : IWebService
	{
		public void OpenWeb (string url)
		{
			var urlEncoded = new Uri (url);
			UIApplication.SharedApplication.OpenUrl(new NSUrl(urlEncoded.AbsoluteUri));
		}
	}
}

