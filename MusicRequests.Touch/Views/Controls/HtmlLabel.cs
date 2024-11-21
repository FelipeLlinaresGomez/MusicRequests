using System;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MusicRequests.Touch
{
	[Register("HtmlLabel")]
	public partial class HtmlLabel : UILabel
	{
		private string _font;
		private float _size;
		private UIColor _color;

		public HtmlLabel(IntPtr handle) : base(handle)
		{
		}

		public HtmlLabel(CGRect frame, string font, float size, UIColor color) : base(frame)
		{
			_font = font;
			_size = size;
			_color = color;
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
				StringEncoding = NSStringEncoding.UTF8,

			};
			if (_font != null && _size != 0)
			{
				htmlText = SetFormat(htmlText, _font, _size, _color);
			}
			var attStr = new NSAttributedString(NSData.FromString(htmlText), atts, ref err);
			AttributedText = attStr;
		}

		private string SetFormat(string htmlText, string font, float size, UIColor color = null, bool center = false)
		{
			nfloat red = 0, green = 0, blue = 0, alpha = 0;
			if (color != null)
			{
				color.GetRGBA(out red, out green, out blue, out alpha);
				red *= 255 / alpha;
				green *= 255 / alpha;
				blue *= 255 / alpha;
			}
			StringBuilder sb = new StringBuilder();
			sb.Append("<span style = \"font-family:");
			sb.Append(font);
			sb.Append("; font-size:");
			sb.Append(size);
			sb.Append("pt;");
			if (color != null)
			{
				sb.Append(" color:");
				sb.Append(string.Format("RGB({0},{1},{2})", Int64.Parse(Math.Truncate(red).ToString()), Int64.Parse(Math.Truncate(green).ToString()), Int64.Parse(Math.Truncate(blue).ToString())));
				sb.Append(";");
			}
			sb.Append(" \">");
			sb.Append(htmlText);
			sb.Append("</span>");

			if (center)
			{
				string html = sb.ToString();
				sb = new StringBuilder();
				sb.Append("<center>");
				sb.Append(html);
				sb.Append("</center>");
			}
			return sb.ToString();
		}
	}
}
