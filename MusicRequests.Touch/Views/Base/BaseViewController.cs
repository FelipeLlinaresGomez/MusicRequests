using MusicRequests.Core.ViewModels;
using MvvmCross.Platforms.Ios.Views;
using MusicRequests.Touch.Helpers;
using GameController;

namespace MusicRequests.Touch.Views
{
    public class BaseViewController<TViewModel> : BaseViewController where TViewModel : IBaseViewModel
	{
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
	}

	/// <summary>
	/// A base view controller 
	/// </summary>
	public class BaseViewController : MvxViewController
	{
        public new IBaseViewModel ViewModel
        {
            get { return (IBaseViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

		public BaseViewController()
		{
			this.RestrictRotation (true);
		}


        #region ViewDidLoad

        public override void ViewDidLoad()
		{
			EdgesForExtendedLayout = UIRectEdge.None;
			View.BackgroundColor = UIColor.White;
			UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);

            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                OverrideUserInterfaceStyle = UIUserInterfaceStyle.Light;
            }

            base.ViewDidLoad();

            OverrideBackButtonAndCross();
        }

		#endregion

		#region ViewDidAppear

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			//Only do this if required
			if (HandlesKeyboardNotifications)
			{
				NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
				NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
			}
		}

		#endregion

		#region ViewDidDisappear

		public override void ViewDidDisappear (bool animated)
		{
			
			base.ViewDidDisappear (animated);
			//Only do this if required
			if (HandlesKeyboardNotifications)
			{
				NSNotificationCenter.DefaultCenter.RemoveObserver(UIKeyboard.WillHideNotification);
				NSNotificationCenter.DefaultCenter.RemoveObserver(UIKeyboard.WillShowNotification);
			}
		}

		#endregion

		#region Keyboard display behaviour

		public virtual bool HandlesKeyboardNotifications { get => false; }

		void OnKeyboardNotification (NSNotification notification)
		{
			//Check if the keyboard is becoming visible
			bool visible = notification.Name == UIKeyboard.WillShowNotification;

			//Start an animation, using values from the keyboard
			UIView.BeginAnimations ("AnimateForKeyboard");
			UIView.SetAnimationBeginsFromCurrentState (true);
			UIView.SetAnimationDuration (UIKeyboard.AnimationDurationFromNotification (notification));
			UIView.SetAnimationCurve ((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification (notification));

			//Pass the notification, calculating keyboard height, etc.
			bool landscape = InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || InterfaceOrientation == UIInterfaceOrientation.LandscapeRight;
			if (visible) {
				var keyboardFrame = UIKeyboard.FrameEndFromNotification (notification);
				OnKeyboardChanged (visible, landscape ? keyboardFrame.Width : keyboardFrame.Height);
			} else {
				var keyboardFrame = UIKeyboard.FrameBeginFromNotification (notification);
				OnKeyboardChanged (visible, landscape ? keyboardFrame.Width : keyboardFrame.Height);
			}

			//Commit the animation
			UIView.CommitAnimations ();
		}


		/// <summary>
		/// Override this method to apply custom logic when the keyboard is shown/hidden
		/// </summary>
		/// <param name='visible'>
		/// If the keyboard is visible
		/// </param>
		/// <param name='height'>
		/// Calculated height of the keyboard (width not generally needed here)
		/// </param>
		protected virtual void OnKeyboardChanged (bool visible, nfloat height) { }

		#endregion

		#region Orientation methods

		public override bool ShouldAutorotate() => false;
		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations() => UIInterfaceOrientationMask.Portrait;
		public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation() => UIInterfaceOrientation.Portrait;

		public void RestrictRotation (bool restriction)
		{
			AppDelegate app = (AppDelegate)UIApplication.SharedApplication.Delegate;
			app.RestrictRotation = restriction;
		}
		#endregion

		#region OverrideBackButton

		private void OverrideBackButtonAndCross()
		{
            // Remove back navigation with gesture
            if (NavigationController?.InteractivePopGestureRecognizer != null)
            {
                NavigationController.InteractivePopGestureRecognizer.Enabled = false;
            }

            if (ViewModel is IBaseWizardViewModel baseWizard)
			{
				if (baseWizard.UsarFlechaAtras)
				{
                    UIImage backImg = UIImage.FromBundle(Icons.Common.Arrow_BACK);

                    var leftButton = new UIBarButtonItem(backImg, UIBarButtonItemStyle.Plain,
                    (sender, e) => baseWizard.OnBackNavigation());

                    NavigationItem?.SetLeftBarButtonItem(leftButton, false);
				}
				else
				{
                    NavigationItem.HidesBackButton = true;
                    NavigationItem.LeftBarButtonItem = null;
                }

				if (baseWizard.UsarAspaParaCancelar)
				{
					UIImage AspaImg = UIImage.FromBundle(Icons.Common.ASPA);

					var rightButton = new UIBarButtonItem(AspaImg, UIBarButtonItemStyle.Plain,
					(sender, e) => baseWizard.OnCancelRequested());

					NavigationItem?.SetRightBarButtonItem(rightButton, false);
				}
			}
			else
			{
                NavigationItem.HidesBackButton = true;
                NavigationItem.LeftBarButtonItem = null;
            }

		}
        #endregion
    }
}

