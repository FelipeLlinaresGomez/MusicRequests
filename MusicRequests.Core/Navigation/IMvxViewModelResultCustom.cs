using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace MusicRequests.Core.ViewModels
{
    public interface IMvxViewModelResultCustom<TResult> : IMvxViewModel where TResult : notnull
    {
        TaskCompletionSource<object?>? CloseCompletionSource { get; set; }
    }
}

