using System;

namespace MusicRequests.Core.Services
{
	public interface ILocalizationService
	{
		string GetCurrent();
		string GetCurrentCulture();
	}
}

