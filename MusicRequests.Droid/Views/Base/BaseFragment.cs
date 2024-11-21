using Android.OS;
using Android.Views;
using MusicRequests.Core.ViewModels;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Views.AppCompat;
using MvvmCross;
using MusicRequests.Core.Services;
using AndroidX.AppCompat.Widget;

namespace MusicRequests.Droid.Views
{
    public abstract class BaseFragment : MvxFragment
	{
		private AndroidX.AppCompat.Widget.Toolbar _toolbar;
		private MvxActionBarDrawerToggle _drawerToggle;

		public virtual bool IsConfirmationView() 
		{
			return false;
		}

		public BaseViewModel MyBaseViewModel {
			get{return ViewModel as BaseViewModel;}
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var ignore = base.OnCreateView(inflater, container, savedInstanceState);
			var view = this.BindingInflate(FragmentId, null);

			return view;
		}

		protected abstract int FragmentId { get; }

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
			if (_toolbar != null)
			{
				_drawerToggle.OnConfigurationChanged(newConfig);
			}
		}

		public override void OnActivityCreated(Bundle savedInstanceState)
		{
			base.OnActivityCreated(savedInstanceState);
			if (_toolbar != null)
			{
				_drawerToggle.SyncState();
			}
		}
		public virtual bool IsDocumentView()
		{
			return false;
		}
	}

	public abstract class BaseFragment<TViewModel> : BaseFragment where TViewModel : IBaseViewModel
	{
		protected IKeyboardHandler mKeyboardHandler;

		public new TViewModel ViewModel
		{
			get { return (TViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		#region Resume/Pause/Destroy

		public override void OnResume()
		{
            base.OnResume();
		}

		public override void OnPause()
		{
            base.OnPause();
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
		}

		public override void OnStop()
		{
			base.OnStop();
		}

		#endregion

		#region Attach/Detach

		public override void OnAttach(Android.Content.Context context)
		{
			base.OnAttach(context);
			if (context is IKeyboardHandler)
				mKeyboardHandler = (IKeyboardHandler)context;
		}

		public override void OnDetach()
		{
			base.OnDetach();
			mKeyboardHandler = null;
		}

		#endregion


	}
}

