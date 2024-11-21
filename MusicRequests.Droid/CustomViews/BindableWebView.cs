using System;
using Android.Content;
using Android.Util;
using Android.Webkit;

namespace MusicRequests.Droid
{
    public class BindableWebView : WebView
    {
        public BindableWebView(Context context) :
            base(context)
        {
            Initialize();
        }

        public BindableWebView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public BindableWebView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        void Initialize()
        {
            this.SetWebViewClient(new MyWebViewClient());
        }


        #region BaseUrl
        private string _baseUrl;
        public string BaseUrl
        {
            get { return _baseUrl; }
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                _baseUrl = value;

                LoadUrl(_baseUrl);
                UpdatedHtmlContent();
            }
        }
        #endregion

        public event EventHandler HtmlContentChanged;

        private void UpdatedHtmlContent()
        {
            var handler = HtmlContentChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }

    public class MyWebViewClient : WebViewClient
    {
        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            view.LoadUrl(url);
            return true;
        }
    }
}
