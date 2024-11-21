using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MusicRequests.Core.Services
{
	public static class TaskExtensions
	{
		public static Task<TOutput> LogExceptions<TOutput> (this Task<TOutput> task)
		{
			bool error = false;
			task.ContinueWith (t => {
				var aggException = t.Exception.Flatten ();
				foreach (var exception in aggException.InnerExceptions) {
					Debug.WriteLine (exception.Message); // throw only first, search for solution
					error = true;
					break;
				}
			}, TaskContinuationOptions.OnlyOnFaulted); // not valid for multi task continuations

			if (error)
				return null;
			return task;
		}

		public static Task LogExceptions (this Task task)
		{
			bool error = false;
			task.ContinueWith (t => {
				var aggException = t.Exception.Flatten ();
				foreach (var exception in aggException.InnerExceptions) {
					Debug.WriteLine (exception.Message); // throw only first, search for solution
					error = true;
					break;
				}
			}, TaskContinuationOptions.OnlyOnFaulted); // not valid for multi task continuations

			if (error)
				return null;
			return task;
		}
	}
}

