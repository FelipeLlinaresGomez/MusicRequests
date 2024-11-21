using MvvmCross.ViewModels;

namespace MusicRequests.Core.ViewModels
{
    public abstract class MvxViewModelCustom<TParameter, TResult> : MvxViewModelResultCustom<TResult>, IMvxViewModelCustom<TParameter, TResult>, IMvxViewModel<TParameter>, IMvxViewModel, IMvxViewModelResultCustom<TResult> where TParameter : notnull where TResult : notnull
    {
        public abstract void Prepare(TParameter parameter);
    }

}

