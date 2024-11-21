using System;

namespace MusicRequests.Core.Models
{
	public enum GPSError
	{
		PositionUnavailable,
		Unauthorized,
		Disabled
	}

	public class GPSMusicRequestException : Exception
	{
		public GPSError ErrorType { get; set; }

		public GPSMusicRequestException (GPSError type)
		{
			ErrorType = type;
		}
	}
}

