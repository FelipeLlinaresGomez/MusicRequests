using System;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using MusicRequests.Core.ViewModels;

public interface IKeyboardHandler 
{
	void CloseSoftKeyboard(bool clearfocus = true);
}

namespace MusicRequests.Droid.Views
{
    public abstract class BaseToolbarActivity<TViewModel> : BaseActivity, IKeyboardHandler where  TViewModel : IBaseViewModel
	{
		public new TViewModel ViewModel
		{
			get
			{
				return (TViewModel)base.ViewModel;
			}
			set
			{
				base.ViewModel = value;
			}
		}

		protected AndroidX.AppCompat.Widget.Toolbar mToolbar;
		protected int progressBarResourceColor;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			try
			{
				//ViewModel.OnViewAppear();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			try
			{
				//ViewModel.OnViewDissapear();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
			}
		}

		protected String getColoredSpanned(String text, String color)
		{
			String input = "<font color=" + color + ">" + text + "</font>";
			return input;
		}


		public override void SetContentView(int layoutResId)
		{
			base.SetContentView(layoutResId);
			try
			{
				SetUpToolBar();
			}
			catch (Exception ex)
			{
				var msg = ex.Message;
			}
		}



		protected void SetUpToolBar() 
		{
			mToolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
			if (mToolbar != null)
			{
				SetSupportActionBar(mToolbar);
				SupportActionBar.SetDisplayShowTitleEnabled(false);
				SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			}
		}

		protected void OnBackClick(object sender, EventArgs e)
		{
			OnBackPressed();
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home) {
				base.OnBackPressed ();
				return true;
			}
			return base.OnOptionsItemSelected (item);
		}

		public void CloseSoftKeyboard(bool clearfocus = true)
		{
			try
			{
				InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
				var v = CurrentFocus ?? Window?.DecorView;
				if (v != null)
				{ 
					inputManager.HideSoftInputFromWindow(v.WindowToken, HideSoftInputFlags.NotAlways);
					if(clearfocus)
						v.ClearFocus();
				}	
			}
			catch (Exception ex)
			{
				var msg = ex.Message;
			}
		}
	
	}

	public abstract class BaseModalToolbarActivity<TViewModel> : BaseToolbarActivity<TViewModel> where TViewModel : IBaseViewModel
	{
		public override void SetContentView(View view)
		{
			base.SetContentView(view);
			try
			{
				SetUpToolBar();
				SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_action_navigation_close);
				SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			}
			catch (Exception ex)
			{
				var msg = ex.Message;
			}
		}
	}
}

