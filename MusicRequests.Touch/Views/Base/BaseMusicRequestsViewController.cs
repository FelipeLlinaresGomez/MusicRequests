using GameController;
using MusicRequests.Core.ViewModels;
using MusicRequests.Touch.Helpers;
using MusicRequests.Touch.Styles;

namespace MusicRequests.Touch.Views.Base
{
    /// <summary>
    /// Base con gestion de teclado y titulo de navegacion
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public abstract class BaseMusicRequestsViewController<TViewModel> : BaseViewController<TViewModel>, IUIGestureRecognizerDelegate where TViewModel : IBaseViewModel
    {
        #region Titulo

        private string _title;
        public override string Title
        {
            get => _title;
            set
            {
                if (!string.IsNullOrEmpty(_title) && string.IsNullOrEmpty(value))
                {
                    // Evitamos que se limpie el titulo al volver de otra pantalla.
                    return;
                }
                _title = value;
                SetNavTitle(this, value);
            }
        }

        public static void SetNavTitle(UIViewController controller, string title, bool bold = true)
        {
            if (controller.NavigationItem != null)
            {
                if (controller.NavigationItem.TitleView is UILabel label)
                {
                    label.Text = title;
                }
                else
                {
                    var labelTitle = new UILabel(new CGRect(0, 0, 200, 44))
                    {
                        TextAlignment = UITextAlignment.Center,
                        Text = title
                    };

                    if (bold)
                    {
                        labelTitle.Font = Fonts.MusicRequestsFont.MediumOfSize(16);
                        labelTitle.TextColor = Colors.White;
                    }
                    else
                    {
                        labelTitle.Font = Fonts.MusicRequestsFont.OfSize(16);
                        labelTitle.TextColor = Colors.White;
                    }
                    controller.NavigationItem.TitleView = labelTitle;
                }
            }
        }

        #endregion

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            AddGestureHideKeyboard();
        }

        #region Control Barra navegacion

        public virtual bool ForceHideNavigationBar { get; set; } = false;
        public virtual bool SetLogoNavigationBar { get; set; } = false;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (ForceHideNavigationBar)
            {
                HideNavigationBar(animated: animated);
            }
            else
            {
                SetNavigationStyle(this, ForceHideNavigationBar);
                ShowNavigationBar(animated: animated);
            }
        }

        public void SetNavigationStyle(UIViewController controller, bool hideNavBar = false)
        {
            if (controller.NavigationController is { })
            {
                controller.NavigationController.NavigationBar.BarStyle = UIBarStyle.Default;
                controller.NavigationController.SetNavigationBarHidden(hideNavBar, false);

                DefaultNavigationBarAppearance(controller?.NavigationController?.NavigationBar);
                if (SetLogoNavigationBar)
                    SetNavigationBarImage();
            }
        }

        public static void DefaultNavigationBarAppearance(UINavigationBar navigationBar)
        {
            var appearance = new UINavigationBarAppearance();

            appearance.ConfigureWithOpaqueBackground();
            appearance.BackgroundColor = Colors.Primary;

            appearance.ShadowColor = UIColor.Clear;

            if (navigationBar is { })
            {
                navigationBar.TintColor = UIColor.White;

                navigationBar.ScrollEdgeAppearance = appearance;
                navigationBar.CompactAppearance = appearance;
                navigationBar.StandardAppearance = appearance;
            }
        }

        public void SetNavigationBarImage()
        {
            if (this?.NavigationItem is UINavigationItem navItem)
            {
                var imgLogo = new UIImageView
                {
                    Image = UIImage.FromBundle(Icons.Common.ASPA),
                    ContentMode = UIViewContentMode.ScaleAspectFit
                };

                var titleView = new UIView(new CGRect(0, 0, 145, 56));
                titleView.AddSubviews(imgLogo);

                imgLogo.WidthConstraint(115);
                imgLogo.HeightConstraint(25);
                imgLogo.CenterInView(titleView);

                navItem.TitleView = titleView;
            }
        }

        public void HideNavigationBar(bool animated)
        {
            NavigationController?.SetNavigationBarHidden(true, animated);
        }

        public void ShowNavigationBar(bool animated)
        {
            NavigationController?.SetNavigationBarHidden(false, animated);
        }

        #endregion

        #region Keyboard

        [Export("gestureRecognizer:shouldReceiveTouch:")]
        public virtual bool ShouldReceiveTouch(UIGestureRecognizer recognizer, UITouch touch)
        {
            return ShouldHideKeyboardOnTap(touch);
        }

        public abstract UIScrollView GetScrollViewForKeyBoard();

        protected virtual void DismissKeyboard()
        {
            View.EndEditing(true);
        }

        private UITapGestureRecognizer _gesture;
        private void AddGestureHideKeyboard()
        {
            _gesture = new UITapGestureRecognizer(DismissKeyboard)
            {
                CancelsTouchesInView = false,
                Delegate = this
            };
            View.AddGestureRecognizer(_gesture);
        }

        public virtual bool ShouldHideKeyboardOnTap(UITouch touch)
        {
            return !(touch.View is UIControl);
        }

        public override bool HandlesKeyboardNotifications => true;

        protected override void OnKeyboardChanged(bool visible, nfloat height)
        {
            var _scrollView = GetScrollViewForKeyBoard();
            if (_scrollView != null)
            {
                if (visible)
                {
                    OnKeyboardWillShow(_scrollView, height);
                }
                else
                {
                    OnKeyboardWillHide(_scrollView);
                }
            }
        }

        protected virtual void OnKeyboardWillShow(UIScrollView scrollView, nfloat height)
        {
            var contentInsets = new UIEdgeInsets(0.0f, 0.0f, height + Margin.Medium - AdditionalSafeAreaInsets.Bottom, 0.0f);
            scrollView.ContentInset = contentInsets;
            scrollView.ScrollIndicatorInsets = contentInsets;
        }

        protected virtual void OnKeyboardWillHide(UIScrollView scrollView)
        {
            UIView.Animate(0.2f, () =>
            {
                scrollView.ContentInset = UIEdgeInsets.Zero;
                scrollView.ScrollIndicatorInsets = UIEdgeInsets.Zero;
            }, null);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_gesture != null)
                {
                    View.RemoveGestureRecognizer(_gesture);
                    _gesture.Dispose();
                    _gesture = null;
                }
            }
            base.Dispose(disposing);
        }

    }
}
