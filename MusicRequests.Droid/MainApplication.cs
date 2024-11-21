using System;
using System.Net;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using AndroidX.Lifecycle;
using MusicRequests.Core;
using MusicRequests.Core.Services;
using Java.Interop;
using Microsoft.Maui.ApplicationModel;
using MvvmCross.Platforms.Android.Views;
using Plugin.CurrentActivity;
using AndroidX.AppCompat.App;

namespace MusicRequests.Droid
{
    //You can specify additional application information in this attribute
    [Application (UsesCleartextTraffic = true)]
    public class MainApplication : MvxAndroidApplication<Setup, App>, Application.IActivityLifecycleCallbacks, ILifecycleObserver
    {
        static readonly string TAG = "MusicRequest";

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();

            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

            ProcessLifecycleOwner.Get().Lifecycle.AddObserver(this);

            Platform.Init(this);
            CrossCurrentActivity.Current.Init(this);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            RegisterActivityLifecycleCallbacks(this);

        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        #region Lifecycle

        [Lifecycle.Event.OnStop]
        [Export]
        public void Stopped()
        {
            Log.Debug(TAG, "App entered background state.");
            App.AppIsInBackground = true;
            App.RefreshTombstoningTimer();
        }

        [Lifecycle.Event.OnStart]
        [Export]
        public void Started()
        {
            Log.Debug(TAG, "App entered foreground state.");
            App.AppIsInBackground = false;

        }
        #endregion

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }

    

        void HandleAndroidException(object sender, RaiseThrowableEventArgs e)
        {
            Android.Util.Log.Error("MusicRequest", "Error general MusicRequests pay");
            Android.Util.Log.Error("MusicRequest", e.Exception.Message);
            Android.Util.Log.Error("MusicRequest", e.Exception.StackTrace);
        }
    }
}
