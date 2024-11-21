using System;

namespace MusicRequests.Core.Models
{
	public enum ApiStatusCode
	{
		Unauthorized = 401,
		NotFound = 404,
		InternalServerError = 500,
		InvalidToken = 498,
		NoContent = 304,
		BadRequest = 400,
        RootedDevice = 800

	}

	public class HttpMusicRequestException : Exception
	{
		public ApiStatusCode StatusCode { get; set; }
		
		public HttpMusicRequestException (int statusCode, string message) : base (message)
		{
			try
			{
				StatusCode = (ApiStatusCode)statusCode;
			}
			catch 
			{
				StatusCode = ApiStatusCode.InternalServerError;
			}
		}
	}
}