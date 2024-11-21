using System;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;

namespace MusicRequests.Droid
{
    [Activity(
		  MainLauncher = true
		, Theme = "@style/SplashTheme"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    [MetaData("android.app.shortcuts", Resource = "@xml/shortcuts")]
    public class SplashScreen : MvxStartActivity
    {
        public SplashScreen()
			: base(Resource.Layout.splash_activity)
        {
			
        }

        ImageView parentView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Setup.IsInitialized = true;

            parentView = FindViewById<ImageView>(Resource.Id.parentView);
            SetProperDrawableForAspectRatio();

            if (Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                /*Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
					SystemUiFlags.LayoutStable
					| SystemUiFlags.HideNavigation
					| SystemUiFlags.LayoutFullscreen
					| SystemUiFlags.LayoutHideNavigation
					| SystemUiFlags.Fullscreen
					| SystemUiFlags.Immersive);*/
            }
            else
            {
                this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            }

            UserDialogs.Init(this);
        }


        void SetProperDrawableForAspectRatio()
        {
            try
            {
                parentView.SetImageDrawable(GetDrawableForAspectRatio());
            }
            catch (System.Exception ex)
            {

            }
        }

        Drawable GetDrawableForAspectRatio()
        {
            var display = WindowManager.DefaultDisplay;
            Point point = new Point();
            display.GetSize(point);

            double factorVideo3_4 = 2048d / 2732d;
            double factorVideo9_16 = 1080d / 1920d;

            var factorDispositivo = (double)point.X / point.Y;
            bool es4_3 = Math.Abs(factorDispositivo - factorVideo3_4) < Math.Abs(factorDispositivo - factorVideo9_16);

            int res;
            if (es4_3)
            {
                res = Resource.Drawable.fondo_login;
            }
            else
            {
                res = Resource.Drawable.fondo_login_16_9;
            }

            var drawable = Resources.GetDrawable(res);
            return drawable;
        }
    }
}
