using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MusicRequests.Touch
{
	[Register("HtmlTextView")]
	public partial class HtmlTextView : UITextView
	{
		public HtmlTextView(IntPtr handle) : base(handle)
		{
			Initialize();
		}

		public HtmlTextView(CGRect frame) : base(frame)
		{
			Initialize();
		}

		public HtmlTextView() : base()
		{
			Initialize();
		}

		private void Initialize()
        {
			Editable = false;
			DataDetectorTypes = UIDataDetectorType.Link;
		}

		private string _Html;
		public string Html
		{
			get
			{
				return _Html;
			}
			set 
			{
				if (!string.IsNullOrEmpty(value))
				{
					_Html = value;
					SetHtml(value);
				}
			}
		}

		public void SetHtml(string htmlText)
		{
			var err = new NSError();
			var atts = new NSAttributedStringDocumentAttributes
			{
				DocumentType = NSDocumentType.HTML,
				StringEncoding = NSStringEncoding.UTF8
			};
			var attStr = new NSAttributedString(NSData.FromString(htmlText), atts, ref err);
			AttributedText = attStr;
		}
	}
}
