using System;
using MvvmCross.ViewModels;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Content;
using AndroidX.DrawerLayout.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;

namespace MusicRequests.Droid
{
    public interface IDrawerHandler 
	{
		void ShowBurguerButton();
		void HideBurguerButton();
	}

	public abstract class BaseDrawerActivity<TViewModel> : BaseActivity<TViewModel>, IKeyboardHandler, IDrawerHandler where TViewModel : MvxViewModel
	{
        protected AndroidX.AppCompat.Widget.Toolbar mToolbar;
		protected DrawerLayout mDrawerLayout;
		protected ActionBarDrawerToggle mDrawerToggle;

		protected override void OnCreate (Bundle bundle)
		{
 			base.OnCreate (bundle);
		}

		public override void SetContentView (int layoutResId)
		{
			base.SetContentView (layoutResId);
			mToolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar> (Resource.Id.toolbar);
			SetSupportActionBar (mToolbar);
			SupportActionBar.SetDisplayShowTitleEnabled (false);
			SupportActionBar.SetDisplayHomeAsUpEnabled (false);

            SetUpDrawer ();
        }

        private void SetUpDrawer(){
            mDrawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
            mDrawerToggle = new ActionBarDrawerToggle (
                this, mDrawerLayout, mToolbar, Resource.String.drawer_open, Resource.String.drawer_close);
            mDrawerLayout.SetDrawerListener (mDrawerToggle);
			mDrawerToggle.DrawerIndicatorEnabled = false;

            SetCustomBurgerMenuIcon();
			
            mDrawerToggle.SyncState ();
		}

        private void SetCustomBurgerMenuIcon()
        {
            mDrawerToggle.ToolbarNavigationClickListener = new MyClickListener(v => mDrawerLayout.OpenDrawer((int)GravityFlags.Left));
            mDrawerToggle.SetHomeAsUpIndicator(Resource.Drawable.icon_menu_burger);
        }

		protected void OpenDrawer()
		{
			if (mDrawerLayout != null) {
				mDrawerLayout.OpenDrawer((int)GravityFlags.Left);
			}
		}

        protected void CloseDrawer(bool animate = true){
			if (mDrawerLayout != null) {
				mDrawerLayout.CloseDrawer ((int)GravityFlags.Left);
			}
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home) {
				OnBackPressed ();
				return true;
			}
			return base.OnOptionsItemSelected (item);
		}

		private void ShowBackButton()
		{
			//TODO Tell the toggle to set the indicator off
			//this.DrawerToggle.DrawerIndicatorEnabled = false;

			//Block the menu slide gesture
			mDrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
		}

		private void ShowHamburguerMenu()
		{
			//TODO set toggle indicator as enabled 
			//this.DrawerToggle.DrawerIndicatorEnabled = true;

			//Unlock the menu sliding gesture
			mDrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
		}

		public override void OnBackPressed()
		{
			if (mDrawerLayout != null && mDrawerLayout.IsDrawerOpen(GravityCompat.Start))
				mDrawerLayout.CloseDrawers();
			else
				base.OnBackPressed();
		}

		public void CloseSoftKeyboard(bool clearfocus = true)
		{
			try
			{
				InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
				if (CurrentFocus != null)
				{ 
					inputManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
					if(clearfocus)
						CurrentFocus.ClearFocus();
				}	
			}
			catch (Exception ex)
			{
				var msg = ex.Message;
			}
		}

		#region IDrawerHandler implementation
		public void ShowBurguerButton()
		{
			mDrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
			mDrawerToggle.OnDrawerStateChanged(DrawerLayout.LockModeUnlocked);
			mDrawerToggle.DrawerIndicatorEnabled = true;
			mDrawerToggle.SyncState();
			SupportActionBar.SetHomeButtonEnabled(true);
		}

		public void HideBurguerButton()
		{
			mDrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
			mDrawerToggle.OnDrawerStateChanged(DrawerLayout.LockModeLockedClosed);
			mDrawerToggle.DrawerIndicatorEnabled = false;
			mDrawerToggle.SyncState();
			SupportActionBar.SetHomeButtonEnabled(false);
		}
		#endregion

        internal class MyClickListener : Java.Lang.Object, View.IOnClickListener
        {
            private Action<View> _onClick;

            public void OnClick(View v)
            {
                _onClick(v);
            }

            public MyClickListener(Action<View> onClick)
            {
                _onClick = onClick;
            }
        }
	}
}

