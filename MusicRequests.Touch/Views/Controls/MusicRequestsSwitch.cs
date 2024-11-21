using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Commands;
using System.Reflection;

namespace MusicRequests.Touch.Views.Controls
{
    public class MusicRequestsSwitch : UIControl
    {
        UIView _thumbView;
        CGPoint _onPoint;
        CGPoint _offPoint;
        bool _isAnimating;
        private bool on = false;

        public bool UpdateStateOnSwipe { get; set; }

        public bool On
        {
            get => on;
            set
            {
                if (value != on)
                {
                    on = value;
                    if (didLayoutView)
                    {
                        Animate();
                    }
                    else
                    {
                        SendActionForControlEvents(UIControlEvent.ValueChanged);
                    }
                    OnStateChanged?.Invoke(value);
                }
            }
        }

        public IMvxAsyncCommand Command { get; set; }

        public Action<bool> OnBeginTracking { get; set; }
        public Action<bool> OnStateChanged { get; set; }

        public UIColor OnTintColor { get; set; } = UIColor.FromRGB(113, 180, 225);
        public UIColor OffTintColor { get; set; } = UIColor.FromRGB(191, 189, 189);

        public nfloat CornerRadius { get; set; } = 0.5f;

        public UIColor ThumbOnTintColor { get; set; } = UIColor.FromRGB(0, 111, 198);
        public UIColor ThumbOffTintColor { get; set; } = UIColor.FromRGB(239, 239, 239);

        public UIColor ThumbShadowColor { get; set; } = UIColor.Black;
        public nfloat ThumbShadowRadius { get; set; } = 1.5f;
        public nfloat ThumbShadowOpacity { get; set; } = 0.4f;
        public CGSize ThumbShadowOffset { get; set; } = new CGSize(0.75, 0.75);

        public nfloat ThumbCornerRadius { get; set; } = 0.5f;
        public CGSize ThumbSize { get; set; } = new CGSize(20, 20);

        public nfloat Padding { get; set; } = -3;


        public nfloat AnimationDuration { get; set; } = 0.2f;

        public override CGSize IntrinsicContentSize => new CGSize(34, 14);

        public MusicRequestsSwitch(NSCoder decoder) : base(decoder)
        {
            SetupView();
        }

        public MusicRequestsSwitch() : base(new CGRect(location: CGPoint.Empty, size: new CGSize(34, 14)))
        {
            SetupView();
        }

        private void SetupView()
        {
            _thumbView = new UIView(CGRect.Empty)
            {
                UserInteractionEnabled = false
            };
            _thumbView.Layer.ShadowColor = ThumbShadowColor.CGColor;
            _thumbView.Layer.ShadowRadius = ThumbShadowRadius;
            _thumbView.Layer.ShadowOpacity = (float)ThumbShadowOpacity;
            _thumbView.Layer.ShadowOffset = ThumbShadowOffset;

            AddSubview(_thumbView);
        }

        bool didLayoutView;
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            Layer.CornerRadius = Bounds.Size.Height * CornerRadius;
            CGSize thumbSize = ThumbSize != CGSize.Empty ? ThumbSize : new CGSize(Bounds.Height - 2, Bounds.Height - 2);
            nfloat yPos = (Bounds.Height - thumbSize.Height) / 2f;
            _onPoint = new CGPoint(Bounds.Width - thumbSize.Width - Padding, yPos);
            _offPoint = new CGPoint(Padding, yPos);
            if (!_isAnimating)
            {
                BackgroundColor = On ? OnTintColor : OffTintColor;
                _thumbView.BackgroundColor = On ? ThumbOnTintColor : ThumbOffTintColor;
                _thumbView.Frame = new CGRect(location: On ? _onPoint : _offPoint, size: thumbSize);
            }
            else
            {
                _thumbView.Frame = new CGRect(_thumbView.Frame.Location, size: thumbSize);
            }
            _thumbView.Layer.CornerRadius = thumbSize.Height * ThumbCornerRadius;
            didLayoutView = true;
        }

        private void Animate()
        {
            if (_isAnimating)
            {
                AnimationsEnabled = false;
                AnimationsEnabled = true;
            }
            _isAnimating = true;
            Animate(
                duration: AnimationDuration,
                delay: 0,
                options: UIViewAnimationOptions.CurveEaseOut | UIViewAnimationOptions.BeginFromCurrentState,
                animation: () =>
                {
                    nfloat xLocation = On ? _onPoint.X : _offPoint.X;
                    _thumbView.Frame = new CGRect(location: new CGPoint(xLocation, _thumbView.Frame.Y),
                                                  size: _thumbView.Frame.Size);
                    BackgroundColor = On ? OnTintColor : OffTintColor;
                    _thumbView.BackgroundColor = On ? ThumbOnTintColor : ThumbOffTintColor;
                },
                completion: () =>
                {
                    _isAnimating = false;
                    SendActionForControlEvents(UIControlEvent.ValueChanged);
                }
             );
        }

        public override bool BeginTracking(UITouch uitouch, UIEvent uievent)
        {
            base.BeginTracking(uitouch, uievent);
            if (Command is { })
            {
                Command.Execute();
            }
            else if (UpdateStateOnSwipe)
            {
                On = !On;
                OnBeginTracking?.Invoke(On);
            }
            return true;
        }
    }

    public class MusicRequestsSwitchPropertyTargetBinding
    : MvxPropertyInfoTargetBinding<MusicRequestsSwitch>
    {
        // used to figure out whether a subscription to MyPropertyChanged
        // has been made
        private bool _subscribed;

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public MusicRequestsSwitchPropertyTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        // describes how to set MyProperty on MyView
        protected override void SetValueImpl(object target, object value)
        {
            if (!(target is MusicRequestsSwitch view)) return;

            view.On = (bool)value;
        }

        // is called when we are ready to listen for change events
        public override void SubscribeToEvents()
        {
            var myView = View;
            if (myView == null)
            {
                var logger = Mvx.IoCProvider.Resolve<ILogger<MusicRequestsSwitch>>();
                logger.LogError("Error - MusicRequestsSwitch is null in MyViewMyPropertyTargetBinding");
                return;
            }

            _subscribed = true;
            myView.ValueChanged += HandleMyPropertyChanged;
        }

        private void HandleMyPropertyChanged(object sender, EventArgs e)
        {
            var myView = View;
            if (myView == null) return;

            FireValueChanged(myView.On);
        }

        // clean up
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (isDisposing)
            {
                var myView = View;
                if (myView != null && _subscribed)
                {
                    myView.ValueChanged -= HandleMyPropertyChanged;
                    _subscribed = false;
                }
            }
        }
    }
}
