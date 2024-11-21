using System;
namespace MusicRequests.Core.Models
{
	public enum ConnStatus
	{
		NoConnection,
		NoAPIResponse,
		DownloadFileFailed
	}

	public class ConnMusicRequestException : Exception
	{
		public ConnStatus Code { get; set; }
		public ConnMusicRequestException (ConnStatus code)
		{
			Code = code;
		}
	}
}

