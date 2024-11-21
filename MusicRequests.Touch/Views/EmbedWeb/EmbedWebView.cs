using MvvmCross.Binding.BindingContext;
using WebKit;
using MusicRequests.Core.ViewModels;
using MusicRequests.Touch.Views;
using MusicRequests.Touch.Styles;

namespace MusicRequests.Touch.Views
{
    public class EmbedWebView : BaseViewController<EmbedWebViewModel>, IWKUIDelegate
    {
        WKWebView _webView;
        UIToolbar _toolbar;

        public override void LoadView()
        {
            base.LoadView();
            CreateControls();
            SetConstraints();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Initialize();
            MvvmCrossBinding();

        }

        private void Initialize()
        {
            this.SetNavigationStyleWithText(ViewModel.Title);

            var leftbutton = new UIBarButtonItem()
            {
                Title = ViewModel.BackText
            };

            leftbutton.Clicked += (object sender, EventArgs e) =>
            {
                NavigationController.PopViewController(true);
                _webView.Reload();
            };

            NavigationItem.SetLeftBarButtonItem(leftbutton, false);
        }


        void CreateControls()
        {
            var configuration = new WKWebViewConfiguration();
            configuration.Preferences.JavaScriptEnabled = true;
            configuration.Preferences.JavaScriptCanOpenWindowsAutomatically = true;
            _webView = new WKWebView(base.View.Bounds, configuration)
            {
                ContentMode = UIViewContentMode.ScaleToFill,
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            _webView.UIDelegate = this;

            _toolbar = new UIToolbar
            {
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            var backNavigation = new UIBarButtonItem(
                UIImage.FromBundle("Back").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate),
                UIBarButtonItemStyle.Plain, OnBackNavigation);
            backNavigation.TintColor = Colors.Primary;
            var fowardNavigation = new UIBarButtonItem(
                UIImage.FromBundle("ForwardWebView").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate),
                UIBarButtonItemStyle.Plain, OnFowardNavigation);
            fowardNavigation.TintColor = Colors.Primary;

            _toolbar.SetItems(new UIBarButtonItem[] { backNavigation, new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), fowardNavigation }, false);

            View.AddSubviews(_webView, _toolbar);
        }

        private void SetConstraints()
        {
            // Webview pegado arriba y a los laterales de la vista
            // Webview bottom al top de toolbar
            // Toolbar laterales y abajo de la vista
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _webView.TopAnchor.ConstraintEqualTo(View.TopAnchor),
                _webView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor),
                _webView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor),
                _webView.BottomAnchor.ConstraintEqualTo(_toolbar.TopAnchor),
                _toolbar.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor),
                _toolbar.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor),
                _toolbar.BottomAnchor.ConstraintEqualTo(View.BottomAnchor)
            });
        }

        private void MvvmCrossBinding()
        {
            var set = this.CreateBindingSet<EmbedWebView, EmbedWebViewModel>();
            set.Bind(this).For(v => v.UrlBase).To(vm => vm.UrlBase);
            set.Apply();
        }

        private void OnFowardNavigation(object sender, EventArgs e)
        {
            if (_webView.CanGoForward)
            {
                _webView.GoForward();
            }
        }

        private void OnBackNavigation(object sender, EventArgs e)
        {
            if (_webView.CanGoBack)
            {
                _webView.GoBack();
            }
        }

        /// <summary>
        ///  Force all popup windows to remain in the current WKWebView.
        ///  By default, WKWebView is blocking new windows from being created
        ///  ex<a href="link" target="_blank"> text</a>.
        ///  This code catches those popup windows and displays them in the current WKWebView.
        /// </summary>
        [Export("webView:createWebViewWithConfiguration:forNavigationAction:windowFeatures:")]
        public WKWebView CreateWebView(WKWebView webView, WKWebViewConfiguration configuration, WKNavigationAction navigationAction, WKWindowFeatures windowFeatures)
        {
            // open in current view
            webView.LoadRequest(navigationAction.Request);

            // don't return a new view to build a popup into (the default behavior).
            return null;
        }

        private string _urlBase;
        public string UrlBase
        {
            get => _urlBase;
            set
            {
                _urlBase = value;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        NSUrl url = new NSUrl(value);
                        var scheme = url.Scheme;
                        if (string.IsNullOrEmpty(scheme))
                        {
                            url = new NSUrl(value, false);
                        }

                        _webView.LoadRequest(new NSUrlRequest(url));
                    }
                    catch (Exception)
                    {
                    }
                }

            }
        }

    }
}