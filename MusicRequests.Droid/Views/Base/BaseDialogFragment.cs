
using Android.App;
using Android.OS;
using Android.Views;
using MusicRequests.Core.ViewModels;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MusicRequests.Droid.Views
{
    public abstract class BaseDialogFragment : MvxDialogFragment
    {
        protected abstract int FragmentId { get; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            return this.BindingInflate(FragmentId, null);
        }
    }

    public abstract class BaseDialogFragment<TViewModel> : BaseDialogFragment where TViewModel : IBaseViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
