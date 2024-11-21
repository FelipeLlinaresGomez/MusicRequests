
using MvvmCross.ViewModels;

namespace MusicRequests.Core.Presenters
{
    /// <summary>
    /// PresentationHint que elimina de la pila de navegación las vistas 
    /// con ViewModel que implementen IBaseWizardViewModel
    /// </summary>
    public class CloseWizardPresentationHint : MvxPresentationHint
    {
        public CloseWizardPresentationHint(bool animated)
        {
            Animated = animated;
        }


        public bool Animated { get; private set; }


        public string DestinationViewModelTypeName { get; set; }
        public string DestinationViewModelArgsTypeName { get; set; }
        public string DestinationViewModelArgsJson { get; set; }
    }
}
