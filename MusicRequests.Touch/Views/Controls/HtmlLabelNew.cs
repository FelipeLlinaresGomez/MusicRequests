using System;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;
using MusicRequests.Touch.Styles;
namespace MusicRequests.Touch.Views
{

    /**
     * Esta clase la utilizamos cuando queremos añadir estilos a un html, tanto de fuentes, colores y tamaños,
     * Extiende de UILabel, por lo que no funcionan los href, para eso usamos HTMLTextView
     **/
    [Register("HtmlLabellNew")]
    public class HtmlLabelNew : UILabel
    {
        #region tamaños de textos y fuente
        /// <summary>
        /// The text extra tinier.
        /// </summary>
        public static int textExtraTinier = 9;
        /// <summary>
        /// The text tinier.
        /// </summary>
        public static int textTinier = 10;
        /// <summary>
        /// The text tiny
        /// </summary>
        public static int textTiny = 10;
        /// <summary>
        /// The text normal. same size than TextSize.Small
        /// </summary>
        public static int textNormal = 11;
        /// <summary>
        /// The text large.
        /// </summary>
        public static int textLarge = 12;
        #endregion

        public HtmlLabelNew(IntPtr handle) : base(handle)
        {
        }

        public HtmlLabelNew(CGRect frame) : base(frame)
        {
        }
        #region Properties

        #region HTML con estilos
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
                    SetHtml(value, Colors.Gray50, Fonts.MusicRequestsFont.REGULAR, textTiny, false);
                }
            }
        }
        #endregion

        #region HTML Sin color textExtraTinier centrado
        private string _HtmlExtraTinierCenter;
        public string HtmlExtraTinierCenter
        {
            get
            {
                return HtmlExtraTinierCenter;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlExtraTinierCenter = value;
                    SetHtml(value, null, Fonts.MusicRequestsFont.REGULAR, textExtraTinier, true);
                }
            }
        }
        #endregion

        #region HTML Sin color textTinier centrado
        private string _HtmlTinierCenter;
        public string HtmlTinierCenter
        {
            get
            {
                return HtmlTinierCenter;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlTinierCenter = value;
                    SetHtml(value, null, Fonts.MusicRequestsFont.REGULAR, textTinier, true);
                }
            }
        }
        #endregion

        #region HTML Sin color textTiny centrado
        private string _HtmlTinyCenter;
        public string HtmlTinyCenter
        {
            get
            {
                return HtmlTinyCenter;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlTinyCenter = value;
                    SetHtml(value, null, Fonts.MusicRequestsFont.REGULAR, textTiny, true);
                }
            }
        }
        #endregion

        #region HTML Gris textTiny centrado 
        private string _HtmlTinyCenterGris;
        public string HtmlTinyCenterGris
        {
            get
            {
                return _HtmlTinyCenterGris;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlTinyCenterGris = value;
                    SetHtml(value, Colors.Gray75, Fonts.MusicRequestsFont.REGULAR, textTiny, true);
                }
            }
        }
        #endregion

        #region HTML Sin color textTiny
        private string _HtmlTiny;
        public string HtmlTiny
        {
            get
            {
                return _HtmlTiny;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlTiny = value;
                    SetHtml(value, null, Fonts.MusicRequestsFont.REGULAR, textTiny, false);
                }
            }
        }
        #endregion

        #region HTML MusicRequestsFont azul textTiny
        private string _HtmlMusicRequestsFontTinierLink;
        public string HtmlMusicRequestsFontTinierLink
        {
            get
            {
                return _HtmlMusicRequestsFontTinierLink;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlMusicRequestsFontTinier = value;
                    SetHtml(value, Colors.Primary, Fonts.MusicRequestsFont.REGULAR, textTiny);
                }
            }
        }
        #endregion

        #region HTML MusicRequestsFont sin color textTiny
        private string _HtmlMusicRequestsFontTinier;
        public string HtmlMusicRequestsFontTinier
        {
            get
            {
                return _HtmlMusicRequestsFontTinier;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlMusicRequestsFontTinier = value;
                    SetHtml(value, null, Fonts.MusicRequestsFont.REGULAR, textTiny);
                }
            }
        }
        #endregion

        #region HTML MusicRequestsFont Sin color textExtraTinier centrado
        private string _HtmlMusicRequestsFontExtraTinierCenter;
        public string HtmlMusicRequestsFontExtraTinierCenter
        {
            get
            {
                return _HtmlMusicRequestsFontExtraTinierCenter;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlMusicRequestsFontExtraTinierCenter = value;
                    SetHtml(value, null, Fonts.MusicRequestsFont.REGULAR, textExtraTinier, true);
                }
            }
        }
        #endregion

        #region HTML MusicRequestsFont Sin color textTiny centrado
        private string _HtmlMusicRequestsFontTinierCenter;
        public string HtmlMusicRequestsFontTinierCenter
        {
            get
            {
                return _HtmlMusicRequestsFontTinierCenter;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlMusicRequestsFontTinierCenter = value;
                    SetHtml(value, null, Fonts.MusicRequestsFont.REGULAR, textTiny, true);
                }
            }
        }
        #endregion

        #region HTML MusicRequestsFont Sin color textNormal centrado
        private string _HtmlMusicRequestsFontTinyCenter;
        public string HtmlMusicRequestsFontTinyCenter
        {
            get
            {
                return _HtmlMusicRequestsFontTinyCenter;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlMusicRequestsFontTinyCenter = value;
                    SetHtml(value, null, Fonts.MusicRequestsFont.REGULAR, textNormal, true);
                }
            }
        }
        #endregion

        #region HTML MusicRequestsFont Rojo textNormal centrado
        private string _HtmlMusicRequestsFontTinyCenterRed;
        public string HtmlMusicRequestsFontTinyCenterRed
        {
            get
            {
                return _HtmlMusicRequestsFontTinyCenterRed;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlMusicRequestsFontTinyCenterRed = value;
                    SetHtml(value, Colors.RedFail, Fonts.MusicRequestsFont.REGULAR, textNormal, true);
                }
            }
        }
        #endregion

        #region HTML MusicRequestsFont Sin color textNormal 
        private string _HtmlMusicRequestsFontTiny;
        public string HtmlMusicRequestsFontTiny
        {
            get
            {
                return _HtmlMusicRequestsFontTiny;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlMusicRequestsFontTiny = value;
                    SetHtml(value, null, Fonts.MusicRequestsFont.REGULAR, textNormal);
                }
            }
        }
        #endregion  

        #region HTML MusicRequestsFont Sin color textLarge
        private string _HtmlMusicRequestsFontSmall;
        public string HtmlMusicRequestsFontSmall
        {
            get
            {
                return _HtmlMusicRequestsFontSmall;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlMusicRequestsFontSmall = value;
                    SetHtml(value, null, Fonts.MusicRequestsFont.REGULAR, textLarge);
                }
            }
        }
        #endregion

        #region HTML Sin color textTiny 
        private string _HtmlTinier;
        public string HtmlTinier
        {
            get
            {
                return _HtmlTinier;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlTinier = value;
                    SetHtml(value, null, null, textTiny);
                }
            }
        }
        #endregion

        #region HTML Sin color textLarge
        private string _HtmlSmall;
        public string HtmlSmall
        {
            get
            {
                return _HtmlSmall;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _HtmlSmall = value;
                    SetHtml(value, null, null, textLarge);
                }
            }
        }
        #endregion

        #endregion
        public void SetHtml(string htmlText, UIColor color = null, string font = null, float size = 0f, bool center = false)
        {
            try
            {
                var err = new NSError();
                var atts = new NSAttributedStringDocumentAttributes
                {
                    DocumentType = NSDocumentType.HTML,
                    StringEncoding = NSStringEncoding.UTF8,

                };
                if (font != null && size != 0)
                {
                    htmlText = SetFormat(htmlText, font, size, color, center);
                }
                var attStr = new NSAttributedString(NSData.FromString(htmlText), atts, ref err);
                AttributedText = attStr;
            }catch(Exception e)
            { 
            }
        }

        string SetFormat(string htmlText, string font, float size, UIColor color = null, bool center = false)
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
