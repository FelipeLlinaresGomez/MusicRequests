using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views.InputMethods;
using Com.Airbnb.Lottie;
using MusicRequests.Core;
using MusicRequests.Core.Managers;
using MusicRequests.Core.Services;
using MusicRequests.Core.ViewModels;
using MusicRequests.Droid.CustomViews;
using MusicRequests.Droid.Helpers;
using Javax.Crypto;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using AndroidX.Core.Hardware.Fingerprint;

namespace MusicRequests.Droid.Views
{
    [Register("MusicRequests.Droid.views.LoginView")]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, WindowSoftInputMode = Android.Views.SoftInput.AdjustPan)]
    public class LoginView : BaseToolbarActivity<LoginViewModel>
    {      
        private CheckBox swRememberUser;

        private LottieAnimationView ltVamos;

        // FingerPrint variables
        private ITouchIdService _touchService;
        private ISecureStorageService _secureStorageService;
        FingerprintManagerCompat mFingerprintManager
        {
            get { return FingerprintManagerCompat.From(this); }
        }
        ImageView ivFondo;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login_activity);
            OverridePendingTransition(0, Resource.Animation.abc_fade_out);

            swRememberUser = FindViewById<CheckBox>(Resource.Id.swRememberUser);
            ivFondo = FindViewById<ImageView>(Resource.Id.ivFondo);
            ltVamos = FindViewById<LottieAnimationView>(Resource.Id.ltVamos);

            ltVamos.Speed = 0.8f;

            //SetProperDrawableForAspectRatio();                  

            MvvmCrossBinding();

            HandleShortcutItem();

            SetRevelock();
        }

        public void HandleShortcutItem()
        {
            // Take action based on the shortcut type
            if (!string.IsNullOrEmpty(Intent?.Data?.LastPathSegment))
            {
                Type viewModel = typeof(HomeViewModel);
                switch (Intent.Data.LastPathSegment)
                {
                    case "BizumEnviarDineroView":
                        //viewModel = typeof(BizumEnviarDineroViewModel);
                        break;
                    case "BizumSolicitarDineroView":
                        //viewModel = typeof(BizumSolicitarDineroViewModel);
                        break;
                    case "UltimosMovimientosView":
                        //viewModel = typeof(UltimosMovimientosViewModel);
                        break;
                }

                var managerShortcutDeepLinking = Mvx.IoCProvider.Resolve<IShortcutDeeplinkingManager>();
                managerShortcutDeepLinking.CrearTransaccionShortcutDeeplink(viewModel);
            }
        }

        private void SetRevelock()
        {
            var userNameField = FindViewById<TextView>(Resource.Id.etUsuarioParticular);
            var passwordField = FindViewById<TextView>(Resource.Id.inputPassword);
        }

        #region MvvmCrossBinding
        void MvvmCrossBinding()
        {
            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            //set.Bind(this).For(v => v.DisplayTerms).To(vm => vm.Terms);
            //set.Bind(this).For(v => v.BienvenidoMessage).To(vm => vm.BienvenidoAppMessage);
            set.Apply();
        }
        #endregion


        void BtForgetUser_Click(object sender, EventArgs e)
        {
            ForgetUser();
        }

        void SwRememberUser_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (!e.IsChecked && App.RememberUser == null)
            {
                ForgetUser();
            }
        }

        private void ForgetUser()
        {
            CloseSoftKeyboard();
        }

        protected override void OnResume()
        {
            base.OnResume();
            swRememberUser.CheckedChange += SwRememberUser_CheckedChange;
        }

        protected override void OnPause()
        {
            base.OnPause();
            swRememberUser.CheckedChange -= SwRememberUser_CheckedChange;
        }

        public override void OnBackPressed()
        {
            this.FinishAffinity();
        }
        
        #region Background image

        void SetProperDrawableForAspectRatio()
        {
            try
            {
                ivFondo.SetImageDrawable(GetDrawableForAspectRatio());
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

        #endregion
    }
}

