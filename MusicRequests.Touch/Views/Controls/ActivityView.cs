using Ibercaja.Lottie.Ios;
using MusicRequests.Touch.Styles;

namespace MusicRequests.Touch.Views
{
    public class ActivityView : UIView
    {
        public UILabel labelTitle;
        CompatibleAnimationView _imgLoading;
        public UIView _backGroundView;

        private readonly nfloat SPINNER_SIZE = 120f;

        public ActivityView(CGRect frame) : base(frame)
        {
            CreateControls();
        }

        void CreateControls()
        {
            labelTitle = new UILabel();
            labelTitle.Font = Fonts.MusicRequestsFont.MediumOfSize(18);
            labelTitle.TextColor = Colors.Primary;
            labelTitle.TextAlignment = UITextAlignment.Center;
            labelTitle.LineBreakMode = UILineBreakMode.WordWrap;
            labelTitle.Lines = 0;
            labelTitle.SizeToFit();

            _backGroundView = new UIView(new CGRect(
                   0,
                   0,
                   Dimen.ScreenBounds.Width,
                   Dimen.ScreenBounds.Height
               ))
            {
                BackgroundColor = Colors.White
            };

            _imgLoading = new CompatibleAnimationView(CompatibleAnimation.Named("Lotties/ib_spinner_azul-bancadigital.json"));
            _imgLoading.LoopAnimationCount = -1;
            _imgLoading.AnimationSpeed = 1.7f;

            _backGroundView.AddSubviews(_imgLoading, labelTitle);
            this.AddSubviews(_backGroundView);

            BackgroundColor = UIColor.White;
            Layer.BorderWidth = 0.75f;
            Layer.BorderColor = Colors.Gray15.CGColor;
        }

        public UIColor Color { get; set; }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (Bounds.Height > 200)
            {
                labelTitle.Frame = new CGRect(
                    x: 0,
                    y: 0,
                    width: this.Bounds.Width * 0.90f,
                    height: 0);

                if (!string.IsNullOrEmpty(labelTitle.Text))
                {
                    labelTitle.SizeToFit();
                }

                _imgLoading.Frame = new CGRect(
                    x: Bounds.Width / 2 - SPINNER_SIZE / 2,
                    y: Bounds.Height / 2 - SPINNER_SIZE / 2 - Dimen.IsoContainerMarginTop * 4 - labelTitle.Bounds.Height,
                    width: SPINNER_SIZE,
                    height: SPINNER_SIZE
                );

                labelTitle.Frame = new CGRect(
                x: (Bounds.Width - labelTitle.Bounds.Width) / 2f,
                y: _imgLoading.Frame.Bottom,
                width: labelTitle.Bounds.Width,
                height: labelTitle.Bounds.Height);
            }
            else
            {
                UpdateFrames(Bounds);
            }
        }

        public void UpdateFrames(CGRect frame)
        {
            _backGroundView.Frame = new CGRect(
                0,
                0,
                frame.Width,
                frame.Height
            );

            labelTitle.Frame = new CGRect(
                    x: 0,
                    y: 0,
                    width: frame.Width * 0.90f,
                    height: 0);

            if (!string.IsNullOrEmpty(labelTitle.Text))
            {
                labelTitle.SizeToFit();
            }

            _imgLoading.Frame = new CGRect(
                x: _backGroundView.Bounds.Width / 2 - SPINNER_SIZE / 2,
                y: _backGroundView.Bounds.Height / 2 - SPINNER_SIZE / 2,
                width: SPINNER_SIZE,
                height: SPINNER_SIZE
            );

            labelTitle.Frame = new CGRect(
                x: (_backGroundView.Bounds.Width - labelTitle.Bounds.Width) / 2f,
                y: _imgLoading.Frame.Bottom,
                width: labelTitle.Bounds.Width,
                height: labelTitle.Bounds.Height);
        }

        public bool IsLoading
        {
            get
            {
                return !Hidden;
            }
            set
            {
                if (value)
                {
                    Hidden = false;
                    _imgLoading.Play();
                }
                else
                {
                    _imgLoading.Stop();
                    Hidden = true;
                }
            }
        }

        public string Text
        {
            get { return labelTitle.Text; }
            set
            {
                labelTitle.Text = value;
                SetNeedsLayout();
            }
        }
    }
}