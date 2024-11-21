using Android.Hardware.Fingerprints;
using Android.OS;
using Android.Widget;
using AndroidX.Core.Hardware.Fingerprint;

namespace MusicRequests.Droid.Views
{
    public class FingerprintUiHelper : FingerprintManagerCompat.AuthenticationCallback
    {
        public interface ICallback
        {
            void OnAuthenticated();
            void OnError();
        }

        static readonly long ERROR_TIMEOUT_MILLIS = 2000;
        static readonly long SUCCESS_DELAY_MILLIS = 800;

        readonly FingerprintManagerCompat mFingerprintManager;
        readonly ImageView mIcon;
        readonly TextView mErrorTextView;
        readonly ICallback mCallback;
        AndroidX.Core.OS.CancellationSignal mCancellationSignal;

        bool mSelfCancelled;

        public class FingerprintUiHelperBuilder
        {
            FingerprintManagerCompat mFingerPrintManager;

            public FingerprintUiHelperBuilder(FingerprintManagerCompat fingerprintManager)
            {
                mFingerPrintManager = fingerprintManager;
            }

            public FingerprintUiHelper Build(ImageView icon, TextView errorTextView, ICallback callback)
            {
                return new FingerprintUiHelper(mFingerPrintManager, icon, errorTextView, callback);
            }
        }

        /// <summary>
        /// Constructor for {@link FingerprintUiHelper}. This method is expected to be called from
        /// only the {@link FingerprintUiHelperBuilder} class.
        /// </summary>
        /// <param name="fingerprintManager">Fingerprint manager.</param>
        /// <param name="icon">Icon.</param>
        /// <param name="errorTextView">Error text view.</param>
        /// <param name="callback">Callback.</param>
        FingerprintUiHelper(FingerprintManagerCompat fingerprintManager,
            ImageView icon, TextView errorTextView, ICallback callback)
        {
            mFingerprintManager = fingerprintManager;
            mIcon = icon;
            mErrorTextView = errorTextView;
            mCallback = callback;
        }

        public bool IsFingerprintAuthAvailable
        {
            get
            {
                return mFingerprintManager != null &&
                    mFingerprintManager.IsHardwareDetected &&
                    mFingerprintManager.HasEnrolledFingerprints;
            }
        }

        public void StartListening(FingerprintManagerCompat.CryptoObject cryptoObject)
        {
            if (!IsFingerprintAuthAvailable)
                return;

            mCancellationSignal = new AndroidX.Core.OS.CancellationSignal();
            mSelfCancelled = false;
            mFingerprintManager.Authenticate(cryptoObject, 0, mCancellationSignal, this, null);
            mIcon?.SetImageResource(Resource.Drawable.fingerprint);
        }

        public void StopListening()
        {
            try
            {
                if (mCancellationSignal != null)
                {
                    mSelfCancelled = true;
                    mCancellationSignal.Cancel();
                    mCancellationSignal = null;
                }
            }
            catch
            {
                //error cancelando
            }
        }

        public override void OnAuthenticationError(int errMsgId, Java.Lang.ICharSequence errString)
        {
            try
            {

                if (!mSelfCancelled)
                {
                    ShowError(errString.ToString());
                    mIcon?.PostDelayed(() =>
                    {
                        mCallback.OnError();
                    }, ERROR_TIMEOUT_MILLIS);
                }
            }
            catch (Java.Lang.Exception)
            {
                ShowError("Se ha producido un error con la Huella");
            }
            catch (System.Exception)
            {
                ShowError("Se ha producido un error con la Huella");
            }
        }

        public override void OnAuthenticationHelp(int helpMsgId, Java.Lang.ICharSequence helpString)
        {
            ShowError(helpString.ToString());
        }

        public override void OnAuthenticationFailed()
        {
            ShowError("Huella no reconocida");
        }

        public override void OnAuthenticationSucceeded(FingerprintManagerCompat.AuthenticationResult result)
        {
            mErrorTextView.RemoveCallbacks(ResetErrorTextRunnable);
            mIcon.SetImageResource(Resource.Drawable.ic_fingerprint_success);
            mErrorTextView.SetTextColor(mErrorTextView.Resources.GetColor(Resource.Color.ic_blue, null));
            mErrorTextView.Text = "Autenticación correcta";


            mIcon.PostDelayed(() =>
            {
                mCallback.OnAuthenticated();
            }, SUCCESS_DELAY_MILLIS);
        }


        void ShowError(string error)
        {
            mIcon.SetImageResource(Resource.Drawable.ic_fingerprint_error);
            mErrorTextView.Text = error;
            mErrorTextView.SetTextColor(mErrorTextView.Resources.GetColor(Resource.Color.ic_red, null));
            mErrorTextView.RemoveCallbacks(ResetErrorTextRunnable);
            mErrorTextView.PostDelayed(ResetErrorTextRunnable, ERROR_TIMEOUT_MILLIS);
        }

        void ResetErrorTextRunnable()
        {
            mErrorTextView.SetTextColor(
            mErrorTextView.Resources.GetColor(Resource.Color.ic_gris90, null));
            mErrorTextView.Text = "Se produjo un error, vuelve a intentarlo";
            mIcon.SetImageResource(Resource.Drawable.fingerprint);
        }
    }
}
