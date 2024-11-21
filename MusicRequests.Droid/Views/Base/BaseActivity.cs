using Android.Content.PM;
using Android.OS;
using Android.Views;
using MusicRequests.Core.Services;
using Microsoft.Maui.ApplicationModel;
using MvvmCross;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using Plugin.CurrentActivity;

namespace MusicRequests.Droid
{
    public class BaseActivity : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        } 
        protected override void OnResume()
        {
            CrossCurrentActivity.Current.Activity = this;
            base.OnResume();
        }

        protected override void OnPause()
        {            
            base.OnPause();
        }

        protected override void OnStart()
        {
            CrossCurrentActivity.Current.Activity = this;
            base.OnStart();
        }

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            CrossCurrentActivity.Current.Activity = this;
            base.OnCreate(savedInstanceState, persistentState);

            this.Window.SetSoftInputMode(SoftInput.AdjustPan);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Com.Indigitall.Xamarin.Android.Utils.PermissionUtils.OnRequestPermissionResult(this, requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public abstract class BaseActivity<TViewModel> :
                                        BaseActivity where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
