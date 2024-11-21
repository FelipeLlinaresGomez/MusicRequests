using MvvmCross.Binding.BindingContext;
using MusicRequests.Core.ViewModels;
using MusicRequests.Touch.Styles;
using MusicRequests.Touch.Views.Login.Controls;
using MusicRequests.Touch.Helpers;
using MusicRequests.Touch.Views.Controls;
using MusicRequests.Touch.Views.Base;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;

namespace MusicRequests.Touch.Views
{
    public partial class LoginView : BaseMusicRequestsViewController<LoginViewModel>
    {
        ActivityView _activityView;
        SplashBackgroundView _imgFondo;

        UIScrollView _scrollView;
        UIView _contentView;

        UIStackView _vstackContent;

        //Logo
        UIImageView _imgLogo;

        FormInputView _inputUsuario;
        FormInputView _inputContraseña;
        FieldRecordarUsuarioView _recordarUsuario;

        MusicRequestsButton _btnAcceder;

        public override void LoadView()
        {
            base.LoadView();
            HideNavigationBar(false);
            CreateControls();
            SetupLayout();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            MvvmCrossBinding();
        }

        public override void ViewWillAppear(bool animated)
        {
            ForceHideNavigationBar = true;
            base.ViewWillAppear(animated);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetStyles();
        }

        #region Create controls

        private void CreateControls()
        {
            _activityView = new ActivityView(Dimen.ScreenBounds);

            CreateContent();

            View.AddSubview(_activityView);
        }

        private void CreateContent()
        {
            _imgFondo = new SplashBackgroundView();
            _scrollView = new UIScrollView();
            _contentView = new UIView();

            _imgLogo = new UIImageView();
            _imgLogo.Image = UIImage.FromBundle(Icons.Pagos.APPLE_PAY).ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

            _inputUsuario = new FormInputView();
            _inputContraseña = new FormInputView() { SecureEntry = true };
            _recordarUsuario = new FieldRecordarUsuarioView();

            _btnAcceder = new MusicRequestsButton();

            _vstackContent = new UIStackView(new UIView[]{
                _imgLogo,
                _inputUsuario,
                _inputContraseña,
                _recordarUsuario,
                Templates.VSpacer(),
                _btnAcceder
            })
            {
                Axis = UILayoutConstraintAxis.Vertical,
                Spacing = Margin.MediumLarge,
                LayoutMarginsRelativeArrangement = true,
                LayoutMargins = new AutoLayoutHelper.Margins(
                    top: 125,
                    left: Margin.MediumLarge,
                    right: Margin.MediumLarge,
                    bottom: Margin.ExtraLarge
                ).Insets
            };

            _contentView.AddSubviews(
                _vstackContent
            );

            _scrollView.AddSubview(_contentView);
            View.AddSubviews(_imgFondo, _scrollView);
        }

        #endregion

        private void SetupLayout()
        {
            _scrollView.SetupScroll(_contentView);
            _contentView.MinHeightConstraint(View, 0.9f);

            _vstackContent.BindToView();

            _imgLogo.WidthConstraint(300);
            _imgLogo.HeightConstraint(135);

            _inputUsuario.ToAutoLayout();
            _inputContraseña.ToAutoLayout();
            _recordarUsuario.ToAutoLayout();

            _btnAcceder.HeightConstraint(Dimen.ButtonHeight);

            _imgFondo.BindToView();
        }

        private void MvvmCrossBinding()
        {
            var set = this.CreateBindingSet<LoginView, LoginViewModel>();

            set.Bind(_activityView).For(v => v.IsLoading).To(vm => vm.Cargando);
            set.Bind(_activityView).For(v => v.Text).To(vm => vm.CargandoStr);

            set.Bind(this).For(v => v.Title).To(vm => vm.Title);
            //set.Bind(_imgFondo).For(v => v.ImageBytes).To(vm => vm.SplashScreen);

            this.BindLanguage(_inputUsuario, nameof(FormInputView.Titulo), "Usuario");
            this.BindLanguage(_inputUsuario.Input, nameof(TextInputView.Placeholder), "UsuarioHint");
            set.Bind(_inputUsuario.Input).To(vm => vm.Usuario);

            this.BindLanguage(_inputContraseña, nameof(FormInputView.Titulo), "Contraseña");
            this.BindLanguage(_inputContraseña.Input, nameof(TextInputView.Placeholder), "ContraseñaHint");
            set.Bind(_inputContraseña.Input).To(vm => vm.Contraseña);

            set.Bind(_recordarUsuario).For(v => v.Checked).To(vm => vm.UsuarioRecordado);
            this.BindLanguage(_recordarUsuario, nameof(FieldRecordarUsuarioView.RecordarUsuario), "RecordarUsuario");
            set.Bind(_recordarUsuario.Tap()).For(v => v.Command).To(vm => vm.RecordarUsuarioCommand);

            this.BindLanguage(_btnAcceder, nameof(UIButton.Title), "IniciarSesion");
            set.Bind(_btnAcceder).To(vm => vm.IniciarSesionCommand);

            set.Apply();
        }

        private void SetStyles()
        {
            Theme.PrimaryRoundedButton(_btnAcceder);
        }

        #region DisplayTouchId

        private void DisplayTouchIdPopupIfPossible()
        {
            //if (IsAccessingMessageVisible)
            //{
            //    var deviceService = DeviceInfo.Current;

            //    if (deviceService.Version.Major > 7)
            //    {
            //        LocalAuthentication.LAContext context = new LocalAuthentication.LAContext();
            //        //Lets double check the device supports Touch ID
            //        if (context.CanEvaluatePolicy(LocalAuthentication.LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out error))
            //        {
            //            var replyHandler = new LocalAuthentication.LAContextReplyHandler((success, error) =>
            //            {
            //                InvokeOnMainThread(async () =>
            //                {
            //                    if (success)
            //                    {
            //                        // On Success
            //                        var secureService = Mvx.IoCProvider.Resolve<ISecureStorageService>();
            //                        //await ViewModel.LoginUserThroughTouchId(secureService.GetPasswordForUser(ViewModel.UserName));
            //                    }
            //                    else
            //                    {
            //                        _viewEntrarConHuella.Hidden = true;
            //                        if ((error != null) && (error.Code != -2))
            //                        {
            //                            UIAlertController alert;
            //                            // On error
            //                            if (Dimensions.FaceIdIsSupported())
            //                            {
            //                                alert = UIAlertController.Create("Error", ViewModel.GetLocalizedText("FaceIdErrorMessage"), UIAlertControllerStyle.Alert);
            //                                alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
            //                            }
            //                            else
            //                            {
            //                                alert = UIAlertController.Create("Error", ViewModel.GetLocalizedText("TouchIdErrorMessaje"), UIAlertControllerStyle.Alert);
            //                                alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
            //                            }
            //                            PresentViewController(alert, true, null);
            //                        }
            //                    }
            //                });
            //            });

            //            if (Dimensions.FaceIdIsSupported())
            //            {
            //                context.EvaluatePolicy(LocalAuthentication.LAPolicy.DeviceOwnerAuthenticationWithBiometrics, ViewModel.GetLocalizedText("FaceIdMessageTitle"), replyHandler);
            //            }
            //            else
            //            {
            //                context.EvaluatePolicy(LocalAuthentication.LAPolicy.DeviceOwnerAuthenticationWithBiometrics, ViewModel.GetLocalizedText("TouchIdMessageTitle"), replyHandler);
            //            }
            //        }
            //        else
            //        {
            //            _viewEntrarConHuella.Hidden = true;
            //        }
            //    }
            //}
        }

        public override UIScrollView GetScrollViewForKeyBoard() => _scrollView;

        #endregion
    }
}