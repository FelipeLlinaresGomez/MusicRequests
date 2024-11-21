using System.Text.RegularExpressions;

namespace MusicRequests.Touch.Views.Controls
{
    [Register(nameof(MusicRequestsHtmlTextView))]
    public class MusicRequestsHtmlTextView : UITextView
    {
        private string _text;
        public override string Text
        {
            get => _text;
            set
            {
                var oldValue = _text;
                _text = value;
                if (!string.IsNullOrEmpty(value) && value != oldValue)
                    UpdateStyles();
            }
        }

        UIFont _font;
        public override UIFont Font
        {
            get => _font;
            set
            {
                var oldValue = value;
                _font = value;

                if (value != oldValue)
                    UpdateStyles();
            }
        }

        UIFont _fontBold;
        public UIFont FontBold
        {
            get => _fontBold;
            set
            {
                var oldValue = value;
                _fontBold = value;

                if (value != oldValue)
                    UpdateStyles();
            }
        }

        UIColor _textColor = UIColor.Black;
        public override UIColor TextColor
        {
            get => _textColor;
            set
            {
                var oldValue = TextColor;
                _textColor = value;

                if (value != oldValue)
                    UpdateStyles();
            }
        }

        public UIColor LinkColor
        {
            get => base.TintColor;
            set => base.TintColor = value;
        }

        public override UITextAlignment TextAlignment
        {
            get => base.TextAlignment;
            set
            {
                var oldValue = TextAlignment;
                base.TextAlignment = value;

                if (value != oldValue)
                    UpdateStyles();
            }
        }

        public override bool TranslatesAutoresizingMaskIntoConstraints
        {
            get => base.TranslatesAutoresizingMaskIntoConstraints;
            set
            {
                base.TranslatesAutoresizingMaskIntoConstraints = value;
                if (!value)
                {
                    ScrollEnabled = false; // necesario si activamos AutoLayout 
                }
            }
        }

        NSAttributedStringDocumentAttributes _attributes;

        public MusicRequestsHtmlTextView() : base()
        {
            Initialize();
        }

        public MusicRequestsHtmlTextView(CGRect frame) : base(frame)
        {
            Initialize();
        }

        public MusicRequestsHtmlTextView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public MusicRequestsHtmlTextView(IntPtr ptr) : base(ptr)
        {
            Initialize();
        }

        private void Initialize()
        {
            Font = UIFont.SystemFontOfSize(12);

            _attributes = new NSAttributedStringDocumentAttributes
            {
                DocumentType = NSDocumentType.HTML,
                StringEncoding = NSStringEncoding.UTF8,
            };

            Editable = false;
            DataDetectorTypes = UIDataDetectorType.Link;

            // Quitamos el padding
            TextContainerInset = UIEdgeInsets.Zero;
            TextContainer.LineFragmentPadding = 0;

            BackgroundColor = UIColor.Clear;
        }

        private void UpdateStyles()
        {
            var tokens = GetTokens();

            NSError error = null;

            var attrString = new NSAttributedString(NSData.FromString(Text ?? ""), _attributes, ref error);

            var alignment = new NSMutableParagraphStyle
            {
                Alignment = TextAlignment
            };
            var att = new NSMutableAttributedString(attrString);
            var range = new NSRange(0, att.Length);
            att.AddAttribute(UIStringAttributeKey.ForegroundColor, TextColor, range);
            att.AddAttribute(UIStringAttributeKey.Font, Font, range);
            att.AddAttribute(UIStringAttributeKey.ParagraphStyle, alignment, range);

            AttributedText = att;

            try
            {
                foreach (var tok in tokens)
                {
                    if (tok.Tag == "b")
                    {
                        att.AddAttribute(UIStringAttributeKey.Font, FontBold, tok.AsRange);
                    }
                    else if (tok.Tag == "font")
                    {
                        att.AddAttribute(UIStringAttributeKey.ForegroundColor, tok.Color, tok.AsRange);
                    }
                }
                AttributedText = att;
            }
            catch
            {
            }
        }

        private List<HtmlToken> GetTokens()
        {
            var ls = new List<HtmlToken>();

            try
            {
                Regex expression = new Regex(@"<([^<>]+)>[^<>]*</([^<>]+)>");

                var results = expression.Matches(Text ?? "");

                int charactersUsedByTags = 0;

                foreach (Match match in results)
                {
                    // Groups[1] = contenido de la etiqueta de apertura (incluye posibles atributos)
                    var lenTags = 2 + match.Groups[1].Value.Length + 3 + match.Groups[2].Value.Length;

                    // Comparamos con la etiqueta de cierre, ya que nunca tendra atributos
                    if (match.Groups[2].Value == "b")
                    {
                        ls.Add(new HtmlToken
                        {
                            Tag = "b",
                            Index = match.Index - charactersUsedByTags,
                            Length = match.Length - lenTags
                        });
                    }
                    else if (match.Groups[2].Value == "font")
                    {
                        var text = match.Groups[1].Value
                                    .Replace("\"", "")
                                    .Replace("#", "")
                                    .Replace("font color=", "");

                        ls.Add(new HtmlToken
                        {
                            Tag = "font",
                            Color = FromHex(text),
                            Index = match.Index - charactersUsedByTags,
                            Length = match.Length - lenTags
                        });
                    }
                    else
                    {
                        ls.Add(new HtmlToken
                        {
                            Tag = match.Groups[2].Value,
                            Index = match.Index - charactersUsedByTags,
                            Length = match.Length - lenTags
                        });

                        charactersUsedByTags -= 2;
                    }

                    charactersUsedByTags += lenTags;
                }
            }
            catch
            {

            }

            return ls;
        }

        private UIColor FromHex(string hex)
        {
            var x = Convert.ToInt32(hex, 16);

            var r = ((x >> 16) & 0xff);
            var g = ((x >> 08) & 0xff);
            var b = ((x >> 00) & 0xff);

            return UIColor.FromRGB(r, g, b);
        }

        public class HtmlToken
        {
            public string Tag { get; set; }

            public UIColor Color { get; set; }

            public int Index { get; set; }

            public int Length { get; set; }

            public NSRange AsRange => new NSRange(Index, Length);
        }
    }
}
