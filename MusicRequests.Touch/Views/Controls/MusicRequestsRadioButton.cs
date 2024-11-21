using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MusicRequests.Touch.Helpers;
using MusicRequests.Touch.Styles;
using MvvmCross.Commands;
using UIKit;

namespace MusicRequests.Touch.Views.Controls
{
    public class MusicRequestsRadioButton : UIControl, INotifyPropertyChanged
    {
        UIView _whiteCircle;

        bool _primary = true;
        public bool Primary
        {
            get => _primary;
            set
            {
                _primary = value;
                SetStyles();
            }
        }

        public override bool Selected
        {
            get => base.Selected;
            set
            {
                var oldValue = base.Selected;

                base.Selected = value;
                SetStyles();

                if (value != oldValue)
                {
                    _whiteCircle.Hidden = !value;
                    OnSelectedChanged?.Invoke(value);
                    NotifyPropertyChanged();
                }
            }
        }

        public override bool Enabled
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;
                SetStyles();
            }
        }

        public IMvxCommand Command { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public Action<bool> OnSelectedChanged { get; set; }

        public MusicRequestsRadioButton() => Initialize();

        private void Initialize()
        {
            CreateControls();
            SetupLayout();

            TouchUpInside += OnTouchUpInside;
        }

        private void OnTouchUpInside(object sender, EventArgs e)
        {
            Command?.Execute();
        }

        private void CreateControls()
        {
            _whiteCircle = new UIView
            {
                UserInteractionEnabled = false,
                Hidden = true
            };

            AddSubview(_whiteCircle);
        }

        private void SetupLayout()
        {
            _whiteCircle.HeightConstraint(this, 0.3f);
            _whiteCircle.WidthConstraint(this, 0.3f);
            _whiteCircle.CenterInView(this);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            SetStyles();
        }

        private void SetStyles()
        {

            if (Enabled)
            {
                if (Primary)
                    BackgroundColor = Selected ? Colors.Primary : UIColor.Clear;
                else
                    BackgroundColor = Selected ? Colors.White : UIColor.Clear;

                Layer.BorderWidth = Selected ? 0 : 1;

                if (Primary)
                    Layer.BorderColor = Selected ? UIColor.Clear.CGColor : Colors.Gray55.CGColor;
                else
                    Layer.BorderColor = Colors.White.CGColor;

                if (Primary)
                    _whiteCircle.BackgroundColor = Selected ? Colors.White : UIColor.Clear;
                else
                    _whiteCircle.BackgroundColor = Selected ? UIColor.FromRGB(60, 149, 217) : UIColor.Clear;
            }
            else
            {
                BackgroundColor = Selected ? UIColor.FromRGB(213, 222, 229) : UIColor.White;

                //_whiteCircle.BackgroundColor = BackgroundColor;

                Layer.BorderWidth = Selected ? 0 : 1;
            }

            Layer.CornerRadius = Layer.Bounds.Height / 2f;
            ClipsToBounds = true;

            _whiteCircle.Layer.CornerRadius = _whiteCircle.Bounds.Height / 2f;
            _whiteCircle.ClipsToBounds = true;
        }

        bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                TouchUpInside -= OnTouchUpInside;

                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
