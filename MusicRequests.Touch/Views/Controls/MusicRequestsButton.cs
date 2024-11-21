namespace MusicRequests.Touch.Views.Controls
{
    /// <summary>
    /// Boton que facilita la aplicacion de estilos en funcion de su estado.
    /// </summary>
    [Register("MusicRequestsButton")]
    public class MusicRequestsButton : UIButton
    {
        public CGSize? DesiredMinHitAreaSize { get; set; } = null;

        private Dictionary<UIControlState, UIColor> _backgroundColorForState = new Dictionary<UIControlState, UIColor>();
        private Dictionary<UIControlState, UIColor> _imageTintColorForState = new Dictionary<UIControlState, UIColor>();
        private Dictionary<UIControlState, nfloat> _borderWidthPerState = new Dictionary<UIControlState, nfloat>();
        private Dictionary<UIControlState, UIColor> _borderColorPerState = new Dictionary<UIControlState, UIColor>();

        public string TitleText
        {
            get => Title(UIControlState.Normal);
            set => SetTitle(value, UIControlState.Normal);
        }

        private MusicRequestsButtonStyle _style;
        public MusicRequestsButtonStyle Style
        {
            get => _style;
            set
            {
                _style = value;
                ApplyStyle(applyBaseStyle: true);
            }
        }

        public override bool Enabled
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;
                ApplyStyle();
            }
        }

        private bool _isRounded;
        public bool IsRounded
        {
            get => _isRounded;
            set
            {
                _isRounded = value;
                UpdateCornerRadius();
            }
        }


        public MusicRequestsButton() : base()
        {
        }

        public MusicRequestsButton(CGRect frame) : base(frame)
        {
        }

        private void ApplyStyle(bool applyBaseStyle = false)
        {
            if (applyBaseStyle)
                Style?.BaseStyle?.Invoke(this);

            switch (State)
            {
                case UIControlState.Normal:
                case UIControlState.Highlighted:
                    Style?.NormalStyle?.Invoke(this);
                    break;
                case UIControlState.Disabled:
                    Style?.DisabledStyle?.Invoke(this);
                    break;
            }
        }

        private void UpdateCornerRadius() => Layer.CornerRadius = Bounds.Height / 2;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (IsRounded)
                UpdateCornerRadius();
        }

        public override bool PointInside(CGPoint point, UIEvent uievent)
        {
            if (DesiredMinHitAreaSize is CGSize size)
            {
                var dx = (nfloat)Math.Max(0, (size.Width - Bounds.Width) / 2f);
                var dy = (nfloat)Math.Max(0, (size.Height - Bounds.Height) / 2f);

                return Bounds.Inset(-dx, -dy).Contains(point);
            }
            else
            {
                return base.PointInside(point, uievent);
            }
        }

        public void SetBackgroundColor(UIColor color, UIControlState forState)
        {
            if (color is { })
            {
                _backgroundColorForState[forState] = color;
            }
            else if (_backgroundColorForState.ContainsKey(forState))
            {
                _backgroundColorForState.Remove(forState);
            }
            UpdateStyle();
        }

        public void SetBorderColor(UIColor color, UIControlState forState)
        {
            if (color is { })
            {
                _borderColorPerState[forState] = color;
            }
            else if (_borderColorPerState.ContainsKey(forState))
            {
                _borderColorPerState.Remove(forState);
            }

            UpdateStyle();
        }

        public void SetBorderWidth(nfloat width, UIControlState forState)
        {
            if (width is { })
            {
                _borderWidthPerState[forState] = width;
            }
            else if (_borderWidthPerState.ContainsKey(forState))
            {
                _borderWidthPerState.Remove(forState);
            }

            UpdateStyle();
        }

        public void SetImageTintColor(UIColor color, UIControlState forState)
        {
            if (color is { })
            {
                _imageTintColorForState[forState] = color;

                if (forState == UIControlState.Highlighted)
                    AdjustsImageWhenHighlighted = false;
                else if (forState == UIControlState.Disabled)
                    AdjustsImageWhenDisabled = false;
            }
            else if (_imageTintColorForState.ContainsKey(forState))
            {
                _imageTintColorForState.Remove(forState);
            }

            UpdateStyle();
        }


        private void UpdateStyle()
        {
            if (_backgroundColorForState.TryGetValue(State, out UIColor color))
            {
                BackgroundColor = color;
            }

            if (_borderColorPerState.TryGetValue(State, out UIColor borderColor))
            {
                Layer.BorderColor = borderColor.CGColor;
            }

            if (_borderWidthPerState.TryGetValue(State, out nfloat borderWidth))
            {
                Layer.BorderWidth = borderWidth;
            }

            if (_imageTintColorForState.TryGetValue(State, out UIColor tintColor))
            {
                TintColor = tintColor;
                ImageView.TintColor = tintColor;
            }
        }

    }

    public class MusicRequestsButtonStyle
    {
        public Action<MusicRequestsButton> BaseStyle { get; set; }
        public Action<MusicRequestsButton> NormalStyle { get; set; }
        public Action<MusicRequestsButton> DisabledStyle { get; set; }
    }
}
