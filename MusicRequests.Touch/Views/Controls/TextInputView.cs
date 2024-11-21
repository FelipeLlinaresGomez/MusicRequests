using System.Reflection.Emit;
using CoreAnimation;
using MusicRequests.Touch.Styles;

namespace MusicRequests.Touch.Views.Controls
{
    [Register("TextInputView")]
    public class TextInputView : UITextField
    {
        public UIResponder NextResponderInForm;
        UITextFieldCondition _shouldReturnDelegate;
        private NSLayoutConstraint _placeHolderLeadingConstraint;

        public override UIKeyboardType KeyboardType
        {
            get => base.KeyboardType; set
            {
                base.KeyboardType = value;
                if (value == UIKeyboardType.NumberPad || value == UIKeyboardType.DecimalPad)
                {
                    AddToolbar();
                }
            }
        }

        public TextInputView()
        {
            Initialize();
        }

        public TextInputView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public TextInputView(CGRect frame) : base(frame)
        {
            Initialize();
        }

        protected TextInputView(NSObjectFlag t) : base(t)
        {
            Initialize();
        }

        protected internal TextInputView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        private UIView _underLine;
        private UIView _underLineInActive;
        private UILabel _placeHolderLabel;
        private UILabel _errorLabel;
        private bool floating;
        private UIColor underLineColor = Colors.Primary;
        private bool active;
        private nfloat floatingPlaceHolderFontSize = 8f;
        private const float PLACEHOLDER_MARGIN = 6f;
        private const float ERROR_MARGIN = 4f;

        public static void SetResponderChain(params TextInputView[] responders)
        {
            for (int i = 0; i < responders.Length - 1; i++)
                responders[i].NextResponderInForm = responders[i + 1];
        }

        private void Initialize()
        {
            ReturnKeyType = UIReturnKeyType.Next;
            AutocapitalizationType = UITextAutocapitalizationType.None;
            AutocorrectionType = UITextAutocorrectionType.No;

            _placeHolderLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Lines = 1
            };
            _placeHolderLabel.Layer.MasksToBounds = false;
            _placeHolderLabel.Font = Fonts.MusicRequestsFont.OfSize(14);
            _placeHolderLabel.TextColor = Colors.Gray50;

            _errorLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Lines = 1
            };
            _errorLabel.Layer.MasksToBounds = false;
            _errorLabel.Font = Fonts.MusicRequestsFont.OfSize(12);
            _errorLabel.TextColor = UIColor.Red;

            _underLine = new UIView
            {
                BackgroundColor = UnderLineColor,
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            _underLine.Layer.Opacity = 0f;

            _underLineInActive = new UIView
            {
                BackgroundColor = UnderLineColor,
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            AddSubviews(_placeHolderLabel, _underLineInActive, _underLine, _errorLabel);
            SetupLayout();

            AddTarget(TextChanged, UIControlEvent.EditingChanged);
            _shouldReturnDelegate = (txt) =>
            {
                ResignFirstResponder();
                NextResponderInForm?.BecomeFirstResponder();
                OnDone?.Invoke(this, null);
                return true;
            };
            ShouldReturn += _shouldReturnDelegate;
        }

        private void AddToolbar()
        {
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, DismissKeyBoard);

            var toolbarFrame = new CGRect(0, 0, Dimen.WidthScreen, 44);
            var toolbarItems = new UIBarButtonItem[]
            {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                doneButton
            };
            var toolbar = new UIToolbar(toolbarFrame)
            {
                Items = toolbarItems,
                BackgroundColor = UIColor.White
            };

            InputAccessoryView = toolbar;

            // Desactivamos los botones de undo y redo en iPad
            InputAssistantItem.LeadingBarButtonGroups = new UIBarButtonItemGroup[] { };
            InputAssistantItem.TrailingBarButtonGroups = new UIBarButtonItemGroup[] { };
        }

        private void DismissKeyBoard(object sender, EventArgs e)
        {
            ResignFirstResponder();
            NextResponderInForm?.BecomeFirstResponder();
            OnDone?.Invoke(this, null);
        }

        private void SetupLayout()
        {
            _placeHolderLeadingConstraint = _placeHolderLabel.LeadingAnchor.ConstraintEqualTo(LeadingAnchor);
            _placeHolderLeadingConstraint.Active = true;
            NSLayoutConstraint.ActivateConstraints(new NSLayoutConstraint[]
            {
                _placeHolderLabel.CenterYAnchor.ConstraintEqualTo(CenterYAnchor),
                _placeHolderLabel.WidthAnchor.ConstraintEqualTo(WidthAnchor),
                _placeHolderLabel.HeightAnchor.ConstraintEqualTo(HeightAnchor)
            });
            NSLayoutConstraint.ActivateConstraints(new NSLayoutConstraint[]
            {
                _underLine.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
                _underLine.BottomAnchor.ConstraintEqualTo(BottomAnchor),
                _underLine.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
                _underLine.HeightAnchor.ConstraintEqualTo(2)
            });
            NSLayoutConstraint.ActivateConstraints(new NSLayoutConstraint[]
           {
                _underLineInActive.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
                _underLineInActive.BottomAnchor.ConstraintEqualTo(BottomAnchor),
                _underLineInActive.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
                _underLineInActive.HeightAnchor.ConstraintEqualTo(1)
           });
            NSLayoutConstraint.ActivateConstraints(new NSLayoutConstraint[]
            {
                _errorLabel.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
                _errorLabel.WidthAnchor.ConstraintEqualTo(WidthAnchor),
                _errorLabel.HeightAnchor.ConstraintEqualTo(HeightAnchor,0.55f),
                _errorLabel.TopAnchor.ConstraintEqualTo(_underLine.BottomAnchor,ERROR_MARGIN)
            });
        }

        private void TextChanged(object sender, EventArgs e)
        {
            if (MaxLength > 0 && !string.IsNullOrEmpty(Text) && Text.Length > MaxLength)
            {
                Text = Text.Substring(0, MaxLength);
            }
        }

        public EventHandler OnDone;
        public EventHandler<bool> OnFocusChanged;


        public override UIFont Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                if (_placeHolderLabel != null)
                {
                    _placeHolderLabel.Font = value;
                }
            }
        }

        public nfloat FloatingPlaceHolderFontSize
        {
            get => floatingPlaceHolderFontSize;
            set
            {
                floatingPlaceHolderFontSize = value;
                InvalidateIntrinsicContentSize();
            }
        }

        private nfloat FPHScale => 0.62f;

        public override string Placeholder
        {
            get => base.Placeholder;
            set => _placeHolderLabel.Text = value;
        }


        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                if (!active)
                {
                    SetNeedsLayout();
                }
            }
        }



        public override NSAttributedString AttributedPlaceholder
        {
            get => _placeHolderLabel.AttributedText;
            set => _placeHolderLabel.AttributedText = value;
        }

        public override CGSize IntrinsicContentSize
        {
            get
            {
                nfloat height = base.IntrinsicContentSize.Height;
                // TextField + placeholder flotante (FPHScale * textfield) + error label(1/2 textfield)
                return new CGSize(NoIntrinsicMetric, height + FPHScale * height + PLACEHOLDER_MARGIN + 0.55f * height + ERROR_MARGIN);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (!active)
            {
                Reset();
                UpdatePlaceHolderPosition(Text);
            }
        }

        private void Reset()
        {
            _placeHolderLabel.Layer.Transform = CATransform3D.Identity;
            Floating = false;
            ConfigPlaceHolderLeftConstraint();
        }


        public bool FloatingPlaceHolder { get; set; } = true;


        public override UIView LeftView
        {
            get => base.LeftView;
            set
            {
                base.LeftView = value;
                ConfigPlaceHolderLeftConstraint();
            }
        }

        private void ConfigPlaceHolderLeftConstraint()
        {
            if (LeftView != null)
            {
                _placeHolderLeadingConstraint.Constant = Floating ? 0 : LeftView.Frame.Width;
            }
            else
            {
                _placeHolderLeadingConstraint.Constant = 0;
            }
        }


        public UIColor UnderLineColor
        {
            get => underLineColor;
            set
            {
                underLineColor = value;
                if (_underLine != null)
                {
                    _underLine.BackgroundColor = value;
                }
                if (_underLineInActive != null)
                {
                    _underLineInActive.BackgroundColor = value;
                }
            }
        }

        public int MaxLength { get; set; } = 0;


        public string ErrorText
        {
            get => _errorLabel.Text;
            set
            {
                _errorLabel.Text = value;
                if (string.IsNullOrEmpty(value))
                {
                    UnderLineColor = Colors.Primary;
                }
                else
                {
                    UnderLineColor = UIColor.Red;
                }
            }
        }

        public UIColor PlaceHolderColor { get => _placeHolderLabel.TextColor; set => _placeHolderLabel.TextColor = value; }
        public bool Floating
        {
            get => floating;
            set
            {
                floating = value;
                ConfigPlaceHolderLeftConstraint();
            }
        }

        private void UpdatePlaceHolderPosition(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                if (FloatingPlaceHolder)
                {
                    if (Floating)
                    {
                        AnimateLabelBack(false);
                        Floating = false;
                    }
                }
                else
                {
                    _placeHolderLabel.Layer.Opacity = 1;
                }
            }
            else
            {
                if (FloatingPlaceHolder)
                {
                    if (!Floating)
                    {
                        FloatLabelToTop(false);
                        Floating = true;
                    }
                }
                else
                {
                    _placeHolderLabel.Layer.Opacity = 0;
                }
            }
        }

        protected bool _hidePlaceholderWhenHasFocus = true;

        public override bool BecomeFirstResponder()
        {
            var flag = base.BecomeFirstResponder();
            if (flag)
            {
                if (FloatingPlaceHolder)
                {
                    if (!Floating || string.IsNullOrEmpty(Text))
                    {
                        FloatLabelToTop();
                        Floating = true;
                    }
                }
                else
                {
                    if (_hidePlaceholderWhenHasFocus)
                    {
                        _placeHolderLabel.Layer.Opacity = 0;
                    }
                }
                ShowActiveBorder();
                OnFocusChanged?.Invoke(this, true);
            }
            active = true;
            return flag;
        }

        public override bool ResignFirstResponder()
        {
            var flag = base.ResignFirstResponder();
            if (flag)
            {
                if (FloatingPlaceHolder)
                {
                    if (Floating && string.IsNullOrEmpty(Text))
                    {
                        AnimateLabelBack();
                        Floating = false;
                    }
                }
                else if (string.IsNullOrEmpty(Text))
                {
                    _placeHolderLabel.Layer.Opacity = 1;
                }
                ShowInactiveBorder();
                OnFocusChanged?.Invoke(this, false);
            }
            active = false;

            return flag;
        }

        void FloatLabelToTop(bool animated = true)
        {
            CATransaction.Begin();
            CATransform3D toTransform = PlaceHolderTransformation();
            if (animated)
            {
                var anim2 = CABasicAnimation.FromKeyPath("transform");
                var fromTransform = CATransform3D.MakeScale(1, 1, 1);
                anim2.From = NSValue.FromCATransform3D(fromTransform);
                anim2.To = NSValue.FromCATransform3D(toTransform);
                anim2.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
                var animGroup = new CAAnimationGroup
                {
                    Animations = new CAAnimation[] { anim2 },
                    Duration = 0.3,
                    FillMode = CAFillMode.Forwards,
                    RemovedOnCompletion = false
                };
                _placeHolderLabel.Layer.AddAnimation(animGroup, "_floatingLabel");
            }
            else
            {
                _placeHolderLabel.Layer.Transform = toTransform;
            }
            ClipsToBounds = false;
            CATransaction.Commit();
        }

        private CATransform3D PlaceHolderTransformation()
        {
            var toTransform = CATransform3D.MakeScale(FPHScale, FPHScale, 1f);
            var centerX = _placeHolderLabel.Frame.Width / 2;
            nfloat phHeight = _placeHolderLabel.Frame.Height;
            nfloat ty = -(phHeight / 2 + PLACEHOLDER_MARGIN + (phHeight * FPHScale) / 2);
            var newWidth = _placeHolderLabel.Frame.Width * FPHScale;
            var newCenterX = newWidth / 2.0f;
            nfloat tx = -(centerX - (centerX - newCenterX));
            toTransform = toTransform.Translate(tx, ty, 0);
            return toTransform;
        }

        void ShowActiveBorder()
        {
            _underLine.Layer.Transform = CATransform3D.MakeScale(0.01f, 1.0f, 1);
            _underLine.Layer.Opacity = 1;
            CATransaction.Begin();
            _underLine.Layer.Transform = CATransform3D.MakeScale(0.01f, 1.0f, 1);
            var anim2 = CABasicAnimation.FromKeyPath("transform");
            var fromTransform = CATransform3D.MakeScale(0.01f, 1.0f, 1);
            var toTransform = CATransform3D.MakeScale(1.0f, 1.0f, 1);
            anim2.From = NSValue.FromCATransform3D(fromTransform);
            anim2.To = NSValue.FromCATransform3D(toTransform);
            anim2.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            anim2.FillMode = CAFillMode.Forwards;
            anim2.RemovedOnCompletion = false;
            _underLine.Layer.AddAnimation(anim2, "_activeBorder");
            CATransaction.Commit();
        }

        void AnimateLabelBack(bool animated = true)
        {
            CATransaction.Begin();
            var toTransform = CATransform3D.MakeScale(1.0f, 1.0f, 1);
            if (animated)
            {
                var anim2 = CABasicAnimation.FromKeyPath("transform");
                var fromTransform = PlaceHolderTransformation();
                anim2.From = NSValue.FromCATransform3D(fromTransform);
                anim2.To = NSValue.FromCATransform3D(toTransform);
                anim2.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
                var animGroup = new CAAnimationGroup
                {
                    Animations = new CAAnimation[] { anim2 },
                    Duration = 0.3,
                    FillMode = CAFillMode.Forwards,
                    RemovedOnCompletion = false
                };
                _placeHolderLabel.Layer.AddAnimation(animGroup, "_animateLabelBack");
            }
            else
            {
                _placeHolderLabel.Layer.Transform = toTransform;
            }

            CATransaction.Commit();
        }


        void ShowInactiveBorder()
        {
            CATransaction.Begin();
            CATransaction.CompletionBlock = () =>
            {
                _underLine.Layer.Opacity = 0f;
            };
            var anim2 = CABasicAnimation.FromKeyPath("transform");
            var fromTransform = CATransform3D.MakeScale(1.0f, 1.0f, 1);
            var toTransform = CATransform3D.MakeScale(0.01f, 1.0f, 1);
            anim2.From = NSValue.FromCATransform3D(fromTransform);
            anim2.To = NSValue.FromCATransform3D(toTransform);
            anim2.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            anim2.FillMode = CAFillMode.Forwards;
            anim2.RemovedOnCompletion = false;
            _underLine.Layer.AddAnimation(anim2, "_activeBorder");
            CATransaction.Commit();
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                ShouldReturn -= _shouldReturnDelegate;
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}