using System;
namespace MusicRequests.Core.Services
{
	public interface ISecureStorageService
	{
		void SavePasswordForUser (string user, string password);
		string GetPasswordForUser (string user);
		void DeleteUser(string user);
	}
}