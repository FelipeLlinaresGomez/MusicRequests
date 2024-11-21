using MusicRequests.Core.ViewModels;
using MvvmCross.ViewModels;

public interface IMvxViewModelCustom<in TParameter, TResult> : IMvxViewModel<TParameter>, IMvxViewModel, IMvxViewModelResultCustom<TResult> where TParameter : notnull where TResult : notnull
{

}