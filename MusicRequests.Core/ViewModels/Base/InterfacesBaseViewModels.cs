using System.Threading.Tasks;
using MusicRequests.Core.Models;
using MvvmCross.ViewModels;

namespace MusicRequests.Core.ViewModels
{
    public interface IBaseLocalizationViewModel : IMvxViewModel
    {
        string GetLocalizedText(string key);
        string GetLocalizedSharedText(string key);
    }

    public interface IBaseViewModel : IBaseLocalizationViewModel
    {
        void Navigate(string Title, TipoAccion type, string url);
        Task OnBackNavigation();
        bool UsarAspaParaCancelar { get; }
        bool UsarFlechaAtras { get; }
    }

    public interface IBaseWizardViewModel : IBaseViewModel
    {
        Task<bool> OnCancelRequested();
    }
}