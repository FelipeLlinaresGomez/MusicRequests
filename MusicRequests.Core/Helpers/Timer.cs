using System;
using System.Threading;
using System.Threading.Tasks;
using MusicRequests.Core.Services;

namespace MusicRequests.Core.Helpers
{
	public delegate void TimerCallback (object state);

	public sealed class Timer : CancellationTokenSource, IDisposable
	{
		public Timer (TimerCallback callback, object state, int dueTime, int period)
		{
			Task.Delay (dueTime, Token).ContinueWith (async (t, s) => {
					var tuple = (Tuple<TimerCallback, object>)s;

					while (true) {
						if (IsCancellationRequested)
							break;
						Task.Factory.StartNew (() => tuple.Item1 (tuple.Item2));
						await Task.Delay (period);
					}

				}, 
          Tuple.Create (callback, state), 
          CancellationToken.None,
          TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
          TaskScheduler.Default);
		}

		public new void Dispose () { base.Cancel (); }
	}


	public sealed class TimerPeriod : CancellationTokenSource, IDisposable
	{
		public TimerPeriod(TimerCallback callback, object state, int period)
		{
			Task.Delay(period, Token).ContinueWith(async (t, s) =>
			{
				var tuple = (Tuple<TimerCallback, object>)s;

				while (true)
				{
					if (IsCancellationRequested)
						break;
					Task.Factory.StartNew(() => tuple.Item1(tuple.Item2));
					await Task.Delay(period);
				}

			},
		  Tuple.Create(callback, state),
		  CancellationToken.None,
		  TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
		  TaskScheduler.Default);
		}

		public new void Dispose() { base.Cancel(); }
	}
}

