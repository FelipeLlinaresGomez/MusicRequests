using Foundation;
using System;
using UIKit;
using System.ComponentModel;
using CoreGraphics;
using MusicRequests.Touch.Styles;
using System.Windows.Input;

namespace MusicRequests.Touch
{
	[Register("LinkViewDesc"), DesignTimeVisible(true)]
	public partial class LinkViewDesc : UIView
	{

		public UILabel LabelTitle { get; set; }
		public UILabel LabelDescription { get; set; }
		public UIImageView ImageIcon { get; set; }
		public UIImageView ImageArrow { get; set; }

		public event EventHandler ItemClicked; 

		public LinkViewDesc ()
		{
			Initialize ();
		}

		public LinkViewDesc (CGRect frame)
		{
			this.Frame = frame;
			Initialize ();
		}

		public LinkViewDesc (IntPtr handle) : base (handle)
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
			var width = this.Frame.Width;

			ImageIcon = new UIImageView (new CGRect (Margin.Small, (Frame.Height - 44) / 2, 44, 44));

			LabelTitle = new UILabel (new CGRect (2 * Margin.Small + 44, 
			                                      Margin.Small,
			                                      width - ImageIcon.Frame.Width - ImageIcon.Frame.X - 2*Margin.Small - 30,
												 20));

			LabelDescription = new UILabel (new CGRect (
				2 * Margin.Small + 44, 
				Margin.Small + 20, 
				LabelTitle.Frame.Width, 
				this.Frame.Height - LabelTitle.Frame.Height - Margin.Small - 20));
			LabelDescription.LineBreakMode = UILineBreakMode.WordWrap;
			LabelDescription.Lines = 0;

			ImageArrow = new UIImageView (new CGRect (LabelTitle.Frame.X + LabelTitle.Frame.Width,
				(Frame.Height - 20) / 2, 30, 30));
			ImageArrow.Image = UIImage.FromBundle ("Forward");
			ImageArrow.ContentMode = UIViewContentMode.ScaleToFill;
			this.AddSubviews (ImageIcon, LabelTitle, LabelDescription, ImageArrow);

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
				this.BackgroundColor = Colors.Gray15;

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




		public ICommand Clicked { get; set; }


	}
}
