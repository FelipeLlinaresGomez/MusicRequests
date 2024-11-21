using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.ComponentModel;
using ObjCRuntime;
using CoreGraphics;
using MusicRequests.Touch.Styles;
using MvvmCross.ViewModels;
using System.Windows.Input;

namespace MusicRequests.Touch
{
	[Register("LinkView"), DesignTimeVisible(true)]
	public partial class LinkView : UIView
	{
		public event EventHandler ItemClicked; 

		public LinkView ()
		{
			Initialize ();
		}

		public LinkView (CGRect frame)
		{
			this.Frame = frame;
			Initialize ();
		}

		public LinkView (IntPtr handle) : base (handle)
		{
			Initialize ();
		}


//		public override void AwakeFromNib ()
//		{
//			Initialize ();
//
//		}

		private void Initialize ()
		{
			this.LayoutIfNeeded();
			var width = Dimen.WidthScreen;
			//ImageIcon = new UIImageView (new CGRect (Dimensions.margin_small, (Frame.Height - 44) / 2, 44, 44)); 
			ImageIcon = new UIImageView(new CGRect(Margin.Small,
			                                       Margin.Small + 10,
			                                       44,
			                                       44));
			
			LabelDescription = new UILabel (new CGRect (ImageIcon.Frame.Location.X + 60,
                                                        Margin.Small + 10, 
			                                            width-90, 
			                                            44));
			ImageArrow = new UIImageView(new CGRect(width-20,
                                                        Margin.Small + 23,
														15,
														15));
			ImageArrow.Image = UIImage.FromBundle("Foward");
			ImageArrow.ContentMode = UIViewContentMode.Center;
			this.AddSubviews (ImageIcon, LabelDescription, ImageArrow);

			ItemClicked += LinkView_ItemClicked;
		}

		void LinkView_ItemClicked (object sender, EventArgs e)
		{
			if ((Clicked != null) && (Clicked.CanExecute(null)))
				Clicked.Execute (null);
		}

		public void AddLeftPadding (float padding, float height)
		{
			ImageIcon.Frame = new CGRect (padding, (Frame.Height - height) / 2, height, height); 
			LabelDescription.Frame = new CGRect (2 * padding + height, (Frame.Height - height) / 2, 
				LabelDescription.Frame.Width, height);

			//ImageArrow = Images.LoadImageView ("icons/Forward.png", UIViewContentMode.ScaleAspectFit);
			ImageArrow.Frame = new CGRect (LabelDescription.Frame.X + LabelDescription.Frame.Width + Margin.Tiny,
				(Frame.Height - 15) / 2, 15, 15);
		}


		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			UITouch touch = touches.AnyObject as UITouch;
			if (touch != null)
			{
				// Change the status of the Item
				//this.BackgroundColor = Colors.Gray15;

				//this.BackgroundColor = UIColor.White;
			}
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);
			this.BackgroundColor = UIColor.White;
			if (ItemClicked != null) {
				this.ItemClicked.Invoke (this, null);
			}
		}

//		public static LinkView Create(LinkView link)
//		{
//			var arr = NSBundle.MainBundle.LoadNib ("LinkView", link, null);
//			var v = Runtime.GetNSObject<LinkView> (arr.ValueAt(0));
//			v.Frame = new CoreGraphics.CGRect(0, 0, link.Frame.Width, link.Frame.Height);
//			return v;
//		}

		public UILabel LabelDescription { get; set;	}
		public UIImageView ImageIcon { get; set;}
		public UIImageView ImageArrow { get; set;}


		public ICommand Clicked { get; set; }


	}
}
