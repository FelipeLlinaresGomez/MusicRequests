using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MusicRequests.Core.ViewModels
{
    public class MvxViewModelResultCustom<TResult> : MvxViewModel, IMvxViewModelResultCustom<TResult>, IMvxViewModel where TResult : notnull
    {
        public TaskCompletionSource<object?>? CloseCompletionSource { get; set; }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource!.Task.IsCompleted && !CloseCompletionSource!.Task.IsFaulted)
            {
                CloseCompletionSource!.TrySetCanceled();
            }
            base.ViewDestroy(viewFinishing);
        }
    }
}

