using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Webkit;
using MusicRequests.Core.ViewModels;

namespace MusicRequests.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class EmbedWebView : BaseToolbarActivity<EmbedWebViewModel>
    {
        BindableWebView webView;
        View loadingViewContainer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.embed_webview_activity);
            webView = FindViewById<BindableWebView>(Resource.Id.webView);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr2)
            {
                // chromium, enable hardware acceleration
                webView.SetLayerType(LayerType.Hardware, null);
            }
            else
            {
                // older android version, disable hardware acceleration
                webView.SetLayerType(LayerType.Software, null);
            }

            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.CacheMode = CacheModes.NoCache;

            loadingViewContainer = FindViewById(Resource.Id.loadingViewContainer);
            webView.SetWebViewClient(new EmbededWebClient(loadingViewContainer));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override void OnBackPressed()
        {
            if (webView != null && webView.CanGoBack())
            {
                webView.GoBack();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            //if (item.ItemId == Android.Resource.Id.Home)
            //{
            //    OnBackPressed();
            //    return true;
            //}
            return base.OnOptionsItemSelected(item);
        }
    }

    public class EmbededWebClient : WebViewClient
    {

        View mLoadingView;

        public EmbededWebClient(View loadginView)
        {
            mLoadingView = loadginView;
        }

        public override void OnPageStarted(WebView view, string url, Android.Graphics.Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);
        }

        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
        }
    }
}