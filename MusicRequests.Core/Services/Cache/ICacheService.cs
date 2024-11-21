using System;
using System.Threading.Tasks;
using MusicRequests.Core.Models;

namespace MusicRequests.Core.Services
{
	public interface ICacheService
	{
		Task<TData> GetData<TData> (string fileKey, int timeoutInminutes, Func<Task<TData>> actionExecution, string password = "");
		Task<TData> GetDataNoCache<TData>(Func<Task<TData>> actionExecution);
		Task GetDataNoCache (Func<Task> actionExecution);
		//Cache<TData> ReadFile<TData> (string fileKey, string password);
		//Task<DateTime> GetLastModificationDateOfFile<TData> (string fileKey, string password);
		DateTime GetLastModificationDateOfFile<TData> (string fileKey, string password);
		Task SaveData<TData> (TData input, string fileKey, int timeoutInminutes, string password);
		Task<Cache<TData>> ReadFile<TData> (string fileKey, string password);
		Cache<TData> ReadFileSync<TData> (string fileKey, string password);

		/// <summary>
		/// Borra la cache del fichero que indiquemos
		/// </summary>
		/// <param name="filename">Filename.</param>
		/// <param name="isprivate">If set to <c>true</c> isprivate.</param>
		void DeleteCache(string filename, bool isprivate);

		/// <summary>
		/// Gestiona la cache de datos sin expiración
		/// </summary>
		/// <returns>The data eternal cache.</returns>
		/// <param name="fileKey">File key.</param>
		/// <param name="actionExecution">Action execution.</param>
		/// <typeparam name="TData">The 1st type parameter.</typeparam>
		Task<TData> GetDataEternalCache<TData>(string fileKey, Func<Task<TData>> actionExecution, string password);

	}
}

