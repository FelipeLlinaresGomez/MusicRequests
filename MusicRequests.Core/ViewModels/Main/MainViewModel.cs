using MvvmCross.Plugin.Messenger;

namespace MusicRequests.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        IMvxMessenger _messenger;
        MvxSubscriptionToken _navToken;
        ShowViewModelMessage _ShowViewModelMessage;

        public MainViewModel(IMvxMessenger messenger)
        {
            _messenger = messenger;


            _navToken = _messenger.SubscribeOnMainThread<ShowViewModelMessage>((message) =>
            {
                _ShowViewModelMessage = message;
            });
        }

        public void NavigateToViewModelRequest()
        {

            if (_ShowViewModelMessage != null)
            {
                _navigationService.Navigate(_ShowViewModelMessage.ViewModelType, _ShowViewModelMessage.Parameters);
            }
            _ShowViewModelMessage = null;
        }


        public void ShowMenu()
        {
            _navigationService.Navigate<HomeViewModel>();
        }
    }
}