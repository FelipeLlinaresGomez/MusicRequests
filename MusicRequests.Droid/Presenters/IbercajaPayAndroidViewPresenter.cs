using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.ViewModels;
using MusicRequests.Core.Presenters;
using MusicRequests.Droid.Views;
using Android.Content;
using System.Threading.Tasks;
using MvvmCross.Platforms.Android.Presenters;
using AndroidX.Fragment.App;

namespace MusicRequests.Droid.Presenters
{
    class MusicRequestAndroidViewPresenter : MvxAndroidViewPresenter
    {
        public MusicRequestAndroidViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }


        public override async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is LogOutPresenterHint)
            {
                HandleLogOutPresenterHint();
                return true;
            }
            else
            {
                return await base.ChangePresentation(hint);
            }
        }

        #region Hint handlers

        /// <summary>
        /// Hace "dismiss" a todos los dialog fragments que puedan haber en la pila, para evitar que se apilen
        /// </summary>
        private void DismissAllUserDialogsFromBackStack()
        {
            if (CurrentFragmentManager.Fragments != null)
            {
                foreach (AndroidX.Fragment.App.Fragment frag in CurrentFragmentManager.Fragments)
                {
                    if (frag is AndroidX.Fragment.App.DialogFragment)
                    {
                        ((AndroidX.Fragment.App.DialogFragment)frag).Dismiss();
                    }
                }
            }
        }

        private void HandleLogOutPresenterHint()
        {
            if (CurrentActivity != null)
            {
                if (CurrentActivity.GetType() != typeof(LoginView) && CurrentActivity.IsTaskRoot)
                {
                    Intent intent = new Intent(CurrentActivity, typeof(LoginView));
                    //intent.AddFlags(ActivityFlags.NewTask);
                    CurrentActivity.StartActivity(intent);
                }
                else
                {
                    Intent intent = new Intent(CurrentActivity, typeof(LoginView));
                    intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
                    CurrentActivity.StartActivity(intent);
                }
            }
        }

        #endregion

        private int CurrentFragmentAmount()
        {
            return CurrentFragmentManager.Fragments != null ? CurrentFragmentManager.Fragments.Count() : 0;
        }

        private AndroidX.Fragment.App.Fragment GetFragmentAtIndex(int index)
        {
            return CurrentFragmentManager.Fragments[index];
        }

    }
}