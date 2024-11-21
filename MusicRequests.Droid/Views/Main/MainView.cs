using System;
using Android.App;
using Android.OS;
using Android.Views;
using MusicRequests.Core.ViewModels;
using Android.Content.PM;
using MusicRequests.Core;
using Android.Graphics;
using MvvmCross.Plugin.Messenger;
using MvvmCross;
using AndroidX.Fragment.App;
using AndroidX.Core.View;

namespace MusicRequests.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask)]
    public class MainView : BaseDrawerActivity<MainViewModel>,
               Android.Views.ViewTreeObserver.IOnGlobalLayoutListener
    {
        View activityRootView;

        public static readonly string KeyDestino = "destino";
        public static readonly string KeyDestinoId = "destinoId";
        public static readonly string KeyData = "data";

        MvxSubscriptionToken _token;

        IMvxMessenger _messenger;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main_activity);
            activityRootView = FindViewById(Android.Resource.Id.Content);
            OverridePendingTransition(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();

            _token = _messenger.SubscribeOnMainThread<CloseSessionMessage>((message) =>
            {
                this.CloseDrawer();
                //ViewModel.Logout();
            });
        }

        protected override void OnResume()
        {

            base.OnResume();

            activityRootView.ViewTreeObserver.AddOnGlobalLayoutListener(this);

            if (GetCurrentFragment() == null)
            {
                ViewModel.ShowMenu();
            }
            else
            {
                ViewModel.NavigateToViewModelRequest();
            }
            CloseDrawer();
        }

        protected override void OnPause()
        {
            base.OnPause();
            activityRootView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
        }


        public override async void OnBackPressed()
        {
            if (mDrawerLayout != null && mDrawerLayout.IsDrawerOpen(GravityCompat.Start))
            {
                mDrawerLayout.CloseDrawers();
            }
            else if (GetCurrentFragment()?.GetType() == typeof(HomeView))
            {
                //await ViewModel.ShowBackButtonLogoutDilog();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        bool StackedViews()
        {
            return SupportFragmentManager.BackStackEntryCount > 2;
        }

        AndroidX.Fragment.App.Fragment GetCurrentFragment()
        {
            return SupportFragmentManager.FindFragmentById(Resource.Id.fragment_container);
        }

        Type GetCurrentFragmentType()
        {
            var currentFragment = GetCurrentFragment();
            if (currentFragment == null) return null;
            return currentFragment.GetType();
        }

        void TryRemoveAllPreviousConfirmationFragmentsFromStack()
        {
            //Fragments.Count = 1(menu lareral) + resto fragmentos
            for (int i = SupportFragmentManager.Fragments.Count - 1; i > 0; i--)
            {
                var frag = (BaseFragment)GetCurrentFragment();
                if (frag.IsConfirmationView())
                {
                    SupportFragmentManager.PopBackStackImmediate();
                }
            }
        }

        void TryRemoveAllPreviousFragmentsFromStack()
        {
            if (SupportFragmentManager.Fragments != null)
            {
                for (int i = SupportFragmentManager.Fragments.Count - 1; i > 0; i--)
                {
                    if (SupportFragmentManager.Fragments[i] != null)
                    {
                        SupportFragmentManager.PopBackStackImmediate();
                    }
                }
            }
        }

        #region IMenuViewHandler implementation

        public void OnItemSelected()
        {
            CloseDrawer();
        }

        #endregion

        #region ViewTreeObserver.IOnGlobalLayoutListener implementation
        public void OnGlobalLayout()
        {
            var currentFragment = GetCurrentFragment();
            if (currentFragment != null && currentFragment is ISoftKeyboardAwareFragment)
            {

                Rect r = new Rect();
                activityRootView.RootView.GetWindowVisibleDisplayFrame(r);


                var screenHeight = activityRootView.RootView.Height;
                var keypadHeight = screenHeight - r.Bottom;


                var keyboardAwareFragment = (ISoftKeyboardAwareFragment)currentFragment;
                if (keypadHeight > screenHeight * 0.15)
                {
                    // 99% of the time the height diff will be due to a keyboard.
                    keyboardAwareFragment.OnKeyboardShown();
                }
                else
                {
                    keyboardAwareFragment.OnKeyboardHidden();
                }
            }
        }
        #endregion
    }
}

