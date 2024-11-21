using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Com.Indigitall.Xamarin.Android.Utils;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace MusicRequests.Core.Services
{
    public class PermissionsService : IPermissionsService
    {
        private Activity CurrentActivity
        {
            get
            {
                return Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            }
        }

        #region PermissionLocation

        readonly string[] PermissionsLocation = {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };

        public static readonly int RequestLocationId = 123;

        public bool HasLocationPermission()
        {
            if (ContextCompat.CheckSelfPermission(CurrentActivity,
                Manifest.Permission.AccessFineLocation) != Permission.Granted &&
                ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.AccessCoarseLocation) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(CurrentActivity, PermissionsLocation, RequestLocationId);
                return false;
            }
            return true;
        }

        #endregion

        #region WriteExternalLocation

        readonly string[] PermissionsWrite = {
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.ReadContacts
        };

        public static readonly int WriteLocationId = 124;

        public event EventHandler OnWritePermissionGrantedEvent;

        public bool CheckPermissions()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                if (ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.ReadMediaAudio) != Permission.Granted ||
                    ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.ReadMediaVideo) != Permission.Granted ||
                    ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.ReadMediaImages) != Permission.Granted ||
                    ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.ReadContacts) != Permission.Granted ||
                    ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.PostNotifications) != Permission.Granted)
                {

                    string permisosParaActivar = "";
                    if (ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.ReadMediaAudio) != Permission.Granted)
                        permisosParaActivar += "* Audio\n";
                    if (ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.ReadMediaVideo) != Permission.Granted)
                        permisosParaActivar += "* Video\n";
                    if (ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.ReadMediaImages) != Permission.Granted)
                        permisosParaActivar += "* Images\n";
                    if (ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.ReadContacts) != Permission.Granted)
                        permisosParaActivar += "* Contactos\n";
                    if (ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.PostNotifications) != Permission.Granted)
                        permisosParaActivar += "* Notificaciones\n";

                    var result = Task.Run(async () =>
                    {
                        IUserDialogs userDialogs = Mvx.IoCProvider.Resolve<IUserDialogs>();
                        await userDialogs.AlertAsync($"Para el funcionamiento correcto de Bizum debe activar los siguientes permisos:\n\n{permisosParaActivar}\n\nPor seguridad, te advertimos que vamos a recopilar información de tu dispositivo y de las aplicaciones que tienes instaladas, para verificar que no existe riesgo con ninguna de ellas y garantizar un correcto funcionamiento de esta aplicación.", "Información", "OK");

                        string[] permisos =
                        {
                            Manifest.Permission.ReadMediaImages,
                            Manifest.Permission.ReadMediaVideo,
                            Manifest.Permission.ReadMediaAudio,
                            Manifest.Permission.ReadContacts,
                            Manifest.Permission.PostNotifications
                        };
                        ActivityCompat.RequestPermissions(CurrentActivity, permisos, WriteLocationId);

                    });

                    return false;
                }

            }
            else if (ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.WriteExternalStorage) != Permission.Granted ||
                ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.ReadExternalStorage) != Permission.Granted ||
                ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.ReadContacts) != Permission.Granted)
            {
                var result = Task.Run(async () =>
                {
                    string permisosParaActivar = "* Almacenamiento\n* Contactos\n";

                    IUserDialogs userDialogs = Mvx.IoCProvider.Resolve<IUserDialogs>();
                    await userDialogs.AlertAsync($"Para el funcionamiento correcto de Bizum debe activar los siguientes permisos:\n\n{permisosParaActivar}\nPor seguridad, te advertimos que vamos a recopilar información de tu dispositivo y de las aplicaciones que tienes instaladas, para verificar que no existe riesgo con ninguna de ellas y garantizar un correcto funcionamiento de esta aplicación.", "Información", "OK");

                    string[] permisos =
                    {
                            Manifest.Permission.WriteExternalStorage,
                            Manifest.Permission.ReadExternalStorage,
                            Manifest.Permission.ReadContacts,
                        };
                    ActivityCompat.RequestPermissions(CurrentActivity, permisos, 1);

                });



                return false;
            }
            return true;
        }

        public void OnWritePermissionGranted()
        {
            if (OnWritePermissionGrantedEvent != null)
            {
                OnWritePermissionGrantedEvent.Invoke(null, null);
            }
        }

        #endregion

        #region Imei

        readonly string[] PermissionsIMEI = {
            Manifest.Permission.ReadPhoneState
        };

        public static readonly int ImeiId = 126;

        //public event EventHandler OnReadPhoneStatePermissionGrantedEvent;

        public bool HasReadPhoneStatePermission()
        {
            if (ContextCompat.CheckSelfPermission(CurrentActivity, Manifest.Permission.ReadPhoneState) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(CurrentActivity, PermissionsIMEI, ImeiId);
                return false;
            }
            return true;
        }

        //public void OnReadPhoneStatePermissionGranted()
        //{
        //	if (OnReadPhoneStatePermissionGrantedEvent != null)
        //	{
        //		OnReadPhoneStatePermissionGrantedEvent.Invoke(null, null);
        //	}
        //}

        #endregion

    }
}
