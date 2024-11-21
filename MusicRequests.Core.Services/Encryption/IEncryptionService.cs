using System;

namespace MusicRequests.Core.Services
{
	public interface IEncryptionService
	{
		string Encrypt (string value, string password);
		string Decrypt (string value, string password);
		bool VerifyDeepLinkHash(string hash, string timestamp);
		string CalculateMd5Hash(string value);
	}
}

