using System.Threading.Tasks;
using MusicRequests.Core.Presenters;

namespace MusicRequests.Core.ViewModels
{
    public class BaseWizardViewModel : BaseWizardViewModel<string, bool>
    {
    }

    public class BaseWizardViewModel<TParameter> : BaseWizardViewModel<TParameter, bool>
    {
    }

    public class BaseWizardViewModel<TParameter, TResult> : BaseViewModel<TParameter, TResult>, IBaseWizardViewModel
    {
        public override void ViewAppearing()
        {
            base.ViewAppearing();
        }

        public virtual async Task<bool> OnCancelRequested()
        {
            bool result = await _navigationService.ShowConfirmationDialogAsync(
                GetLocalizedSharedText("WizardCancelMessage"),
                GetLocalizedSharedText("WizardCancelTitle"),
                GetLocalizedSharedText("WizardCancelYes"),
                GetLocalizedSharedText("WizardCancelNo"));

            if (result)
            {
                return await _navigationService.ChangePresentation(new CloseWizardPresentationHint(false));
            }
            return false;
        }

        public override async Task OnBackNavigation()
        {
            if (UsarFlechaAtras)
            {
                await _navigationService.Close(this);
            }
            else
            {
                await OnCancelRequested();
            }
        }

        protected virtual async Task<bool> RequestCancelConfirmationFromUser(string message, string title, string okText, string cancelText)
        {
            return await _navigationService.ShowConfirmationCustomDialogAsync(message, title, okText, cancelText);
        }
    }
}

