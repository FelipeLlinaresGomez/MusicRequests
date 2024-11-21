using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MusicRequests.Core.Services
{
	public static class JsonExtensions
	{
		public static T TryParseJson<T>(this string json) where T : new()
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(json);
			}
			catch (Exception ex)
			{
				return default(T);
			}
		}
	}
}
