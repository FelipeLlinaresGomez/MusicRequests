using System;

namespace MusicRequests.Core.Models
{
	public class Cache<TData>
	{
		public TData Data { get; set; }
		public DateTime DateTimeSaved { get; set; }
		public int TimeoutInMinutes { get; set; }
		public string CacheKey { get; set; }

	}
}

