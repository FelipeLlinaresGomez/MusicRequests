using System;
using CoreGraphics;
using Foundation;
using MusicRequests.Touch.Styles;
using ObjCRuntime;
using UIKit;

namespace MusicRequests.Touch
{
	public static class Animations
	{
		public static void FadeIn(NSObject sender, 
                                  string name, 
                                  UIView viewToAnimate, 
                                  double duration, 
                                  double delay,
                                  Action callback = null)
		{
			viewToAnimate.Alpha = 0;
			
			//UIView.BeginAnimations(name);
			//UIView.SetAnimationDuration(duration);
			//UIView.SetAnimationDelay(delay);
			//UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
			//UIView.SetAnimationDelegate(sender);
			//UIView.SetAnimationDidStopSelector(new Selector(name));

			//viewToAnimate.Alpha = 1;

			//UIView.CommitAnimations();


            UIView.Animate(duration, 
                           delay, 
                           UIViewAnimationOptions.CurveEaseInOut,
                           () => viewToAnimate.Alpha = 1, 
                           () => {
                               callback?.Invoke();
            });


		}

		public static void Hide(NSObject sender, string name, UIView viewToAnimate, double duration, double delay)
		{
			viewToAnimate.Hidden = false;

			UIView.BeginAnimations(name);
			UIView.SetAnimationDuration(duration);
			UIView.SetAnimationDelay(delay);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
			UIView.SetAnimationDelegate(sender);
			UIView.SetAnimationDidStopSelector(new Selector(name));

			viewToAnimate.Hidden = true;

			UIView.CommitAnimations();
		}

		public static void FadeInTranslationFromBottom(NSObject sender, string name, UIView viewToAnimate, double duration, double delay)
		{
			var frameView = viewToAnimate.Frame;
			viewToAnimate.Frame = new CGRect(frameView.X, Dimen.HeightScreen, frameView.Width, frameView.Height);
			viewToAnimate.Alpha = 0;

			UIView.BeginAnimations(name);
			UIView.SetAnimationDuration(duration);
			UIView.SetAnimationDelay(delay);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
			UIView.SetAnimationDelegate(sender);
			UIView.SetAnimationDidStopSelector(new Selector(name));

			viewToAnimate.Alpha = 1;
			viewToAnimate.Frame = frameView;

			UIView.CommitAnimations();
		}
	}
}
