using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace MusicRequests.Core.ViewModels
{
    public interface IMvxNavigationServiceCustom : IMvxNavigationService
    {
        /// <summary>
        /// Navigate to a ViewModel determined by its type, which returns a result.
        /// </summary>
        /// <param name="presentationBundle">(optional) presentation bundle</param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <typeparam name="TViewModel">Type of <see cref="T:MvvmCross.ViewModels.IMvxViewModel" /></typeparam>
        /// <typeparam name="TResult">Result from the ViewModel</typeparam>
        /// <returns>Returns a <see cref="T:System.Threading.Tasks.Task`1" /> with <see cref="!:TResult" /></returns>
        Task<TResult?> Navigate<TViewModel, TResult>(IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModelResultCustom<TResult> where TResult : class;

        /// <summary>
        /// Navigate to a ViewModel determined by its type, with parameter and which returns a result.
        /// </summary>
        /// <param name="param">ViewModel parameter</param>
        /// <param name="presentationBundle">(optional) presentation bundle</param>
        /// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
        /// <typeparam name="TViewModel">Type of <see cref="T:MvvmCross.ViewModels.IMvxViewModel`2" /></typeparam>
        /// <typeparam name="TParameter">Parameter passed to ViewModel</typeparam>
        /// <typeparam name="TResult">Result from the ViewModel</typeparam>
        /// <returns>Returns a <see cref="T:System.Threading.Tasks.Task`1" /> with <see cref="!:TResult" /></returns>
        Task<TResult?> Navigate<TViewModel, TParameter, TResult>(TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModelCustom<TParameter, TResult> where TParameter : notnull where TResult : class;

        /// <summary>
        /// Closes the View attached to the ViewModel and returns a result to the underlying ViewModel
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="result"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> Close<TResult>(IMvxViewModelResultCustom<TResult> viewModel, TResult result, CancellationToken cancellationToken = default(CancellationToken)) where TResult : class;

    }
}

