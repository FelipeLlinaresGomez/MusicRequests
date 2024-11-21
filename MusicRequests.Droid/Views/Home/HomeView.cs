using Android.Runtime;
using Android.Views;
using MusicRequests.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace MusicRequests.Droid.Views
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragment_container, true)]
    [Register("MusicRequests.Droid.views.HomeView")]
    public class HomeView : BaseFragment<HomeViewModel>
    {
        protected override int FragmentId => Resource.Layout.home_fragment;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            MvvmCrossBinding();

            view.ClearAnimation();

            return view;
        }

        void MvvmCrossBinding()
        {
            var set = this.CreateBindingSet<HomeView, HomeViewModel>();
            //set.Bind(this).For(v => v.DisplayTerms).To(vm => vm.Bienvenida);

            set.Apply();
        }
        public override void OnResume()
        {
            base.OnResume();
        }

        public override void OnPause()
        {
            base.OnPause();
        }

        public override void OnStop()
        {

            base.OnStop();
        }
    }
}