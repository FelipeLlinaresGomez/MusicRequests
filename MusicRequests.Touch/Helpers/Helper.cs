using System.Drawing;
using MusicRequests.Touch.Styles;
using MvvmCross.Binding.BindingContext;
using System.Linq.Expressions;
using Ibercaja.Lottie.Ios;

namespace MusicRequests.Touch.Helpers
{

	public static class Helper
	{
		public static UIImage ImageFromUrl (string uri)
		{
			using (var url = new NSUrl (uri)) {
				using (var data = NSData.FromUrl (url)) {
					if (data != null)
						return UIImage.LoadFromData (data);
				}
			}
			// Return an image by default
			return UIImage.FromBundle("MusicRequest_Logo");
		}

		public static byte[] ToByteArray (this NSData data)
		{
			MemoryStream ms = new MemoryStream ();
			data.AsStream ().CopyTo (ms);
			return ms.ToArray ();
		}

		public static UIImage ToImage(this byte[] data, string defaultImage = "Avatar_Default")
		{
			if (data==null) {
				return UIImage.FromBundle(defaultImage);
			}
			UIImage image = null;
			try {
				image = new UIImage(NSData.FromArray(data));
				data = null;
			} catch (Exception ) {
				return null;
			}
			return image;
		}

        /// <summary>
        /// Find the first responder in the <paramref name="view"/>'s subview hierarchy
        /// </summary>
        /// <param name="view">
        /// A <see cref="UIView"/>
        /// </param>
        /// <returns>
        /// A <see cref="UIView"/> that is the first responder or null if there is no first responder
        /// </returns>
        public static UIView FindFirstResponder(this UIView view)
		{
			if (view.IsFirstResponder)
			{
				return view;
			}
			foreach (UIView subView in view.Subviews)
			{
				var firstResponder = subView.FindFirstResponder();
				if (firstResponder != null)
					return firstResponder;

			}
			return null;
		}

		/// <summary>
		/// Find the first Superview of the specified type (or descendant of)
		/// </summary>
		/// <param name="view">
		/// A <see cref="UIView"/>
		/// </param>
		/// <param name="stopAt">
		/// A <see cref="UIView"/> that indicates where to stop looking up the superview hierarchy
		/// </param>
		/// <param name="type">
		/// A <see cref="Type"/> to look for, this should be a UIView or descendant type
		/// </param>
		/// <returns>
		/// A <see cref="UIView"/> if it is found, otherwise null
		/// </returns>
		public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			if (view.Superview != null)
			{
				if (type.IsAssignableFrom(view.Superview.GetType()))
				{
					return view.Superview;
				}

				if (view.Superview != stopAt)
					return view.Superview.FindSuperviewOfType(stopAt, type);
			}

			return null;
		}


		public static double KmToLatitudeDegrees(double km)
		{
			double earthRadius = 6371.0; // in km
			double radiansToDegrees = 180.0/Math.PI;
			return (km/earthRadius) * radiansToDegrees;
		}

		public static double KmToLongitudeDegrees(double km, double atLatitude)
		{
			double earthRadius = 6371.0; // in miles
			double degreesToRadians = Math.PI/180.0;
			double radiansToDegrees = 180.0/Math.PI;
			// derive the earth's radius at that point in latitude
			double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
			return (km / radiusAtLatitude) * radiansToDegrees;
		}

		public static DateTime NSDateToDateTime(this NSDate date)
		{
			DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0);
			DateTime currentDate = reference.AddSeconds(date.SecondsSinceReferenceDate);
			DateTime localDate = currentDate.ToLocalTime();
			return localDate;
		}

		public static NSDate DateTimeToNSDate(this DateTime date) 
		{
			DateTime newDate = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(2001, 1, 1, 0, 0, 0));
			return NSDate.FromTimeIntervalSinceReferenceDate((date - newDate).TotalSeconds);

		}

        public static void HideView(UIView view)
        {
            view.Hidden = true;
        }

		// resize the image to be contained within a maximum width and height, keeping aspect ratio
		public static UIImage MaxResizeImage (this UIImage sourceImage, float maxWidth, float maxHeight)
		{
			var sourceSize = sourceImage.Size;
			var maxResizeFactor = Math.Max (maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
			if (maxResizeFactor > 1) return sourceImage;
			var width = maxResizeFactor * sourceSize.Width;
			var height = maxResizeFactor * sourceSize.Height;
			UIGraphics.BeginImageContext (new CGSize (width, height));
			sourceImage.Draw (new CGRect (0, 0, width, height));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return resultImage;
		}

		// resize the image (without trying to maintain aspect ratio)
		public static UIImage ResizeImage (this UIImage sourceImage, float width, float height)
		{
			UIGraphics.BeginImageContext (new SizeF (width, height));
			sourceImage.Draw (new RectangleF (0, 0, width, height));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return resultImage;
		}

		// crop the image, without resizing
		public static UIImage CropImage (this UIImage sourceImage, int crop_x, int crop_y, int width, int height)
		{
			var imgSize = sourceImage.Size;
			UIGraphics.BeginImageContext (new SizeF (width, height));
			var context = UIGraphics.GetCurrentContext ();
			var clippedRect = new RectangleF (0, 0, width, height);
			context.ClipToRect (clippedRect);
			var drawRect = new CGRect (-crop_x, -crop_y, imgSize.Width, imgSize.Height);
			sourceImage.Draw (drawRect);
			var modifiedImage = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return modifiedImage;
		}

		public static UIImage ToUIImage(this UIView view) 
		{
			UIGraphics.BeginImageContextWithOptions(view.Bounds.Size, true, 0);
			view.Layer.RenderInContext(UIGraphics.GetCurrentContext());
			var img = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return img;
		}


        public static string nameIconSuccess = "icon_success";
        public static string nameIconError = "lottieIB_error";

        /// <summary>
        /// Remplaces the UII mage view for lottie.
        /// </summary>
        /// <param name="imageView">Image view.</param>
        /// <param name="lottieName">Lottie name.</param>
        /// <param name="controller">Controller.</param>
        public static void RemplaceUIImageViewForLottie(UIImageView imageView, string lottieName,
                                                        bool forceCenterToScreen = false, 
                                                        UIViewController controller = null,
                                                        bool addLottieToController = false,
                                                        float animationSpeed = 1.0f)
        {

            if (controller != null)
            {
                controller.View.SetNeedsLayout();
                controller.View.LayoutIfNeeded();
            }

            if (!imageView.Hidden)
            {
                UIView parent = imageView.Superview;   

                if (!string.IsNullOrEmpty(lottieName))
                {
                    CompatibleAnimationView _imgLottie = new CompatibleAnimationView(CompatibleAnimation.Named("Lotties/" + lottieName + ".json"));
                    _imgLottie.Tag = 107713;

                    if (_imgLottie != null)
                    {
                        if (addLottieToController && controller != null)
                        {
                            controller.View.AddSubview(_imgLottie);
                        }
                        else
                        {
                            parent.AddSubview(_imgLottie);
                        }

                        _imgLottie.ContentMode = UIViewContentMode.ScaleAspectFit;

                        var size = new CGSize(imageView.Frame.Height , imageView.Frame.Height);

                        _imgLottie.Frame = new CGRect(CGPoint.Empty ,
                        size);

                        if (!forceCenterToScreen)
                        {
                            _imgLottie.Center = imageView.Center;
                        }                       
                        else
                        {
                            _imgLottie.Center = new CGPoint(Dimen.ScreenBounds.Width / 2, imageView.Center.Y);
                        }
                       
                        imageView.Hidden = true;

                        _imgLottie.AnimationSpeed = animationSpeed;
                        _imgLottie.Play();

                    }
                }
            }
        }

        public static MvxFluentBindingDescription<TTarget, TSource> ToAnd<TTarget, TSource>(this MvxFluentBindingDescription<TTarget, TSource> set, params Expression<Func<TSource, object>>[] properties) where TTarget : class
        {
            return set.ByCombining("And", properties);
        }

        public static MvxFluentBindingDescription<TTarget, TSource> ToOr<TTarget, TSource>(this MvxFluentBindingDescription<TTarget, TSource> set, params Expression<Func<TSource, object>>[] properties) where TTarget : class
        {
            return set.ByCombining("Or", properties);
        }

        public static MvxFluentBindingDescription<TTarget, TSource> ToNot<TTarget, TSource>(this MvxFluentBindingDescription<TTarget, TSource> set, Expression<Func<TSource, object>> property) where TTarget : class
        {
            return set.ByCombining("Not", property);
        }
    }
}

