using MusicRequests.Core.ViewModels;
using MusicRequests.Touch.Styles;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MusicRequests.Touch.Views.Controls
{
    public class MusicRequestsToolbar : UIToolbar
    {
        UIBarButtonItem _btnLeft;
        UIBarButtonItem _btnRight;

        IMvxViewModel _viewModel;
        public IMvxViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                UpdateBar();
            }
        }

        bool _forceCross;
        public bool ForceCross
        {
            get => _forceCross;
            set
            {
                _forceCross = value;
                UpdateBar();
            }
        }

        bool _forceBack = true;
        public bool ForceBack
        {
            get => _forceBack;
            set
            {
                _forceBack = value;
                UpdateBar();
            }
        }


        UIImage _customLeftBtnIcon = null;
        public UIImage CustomLeftBtnIcon
        {
            get => _customLeftBtnIcon;
            set
            {
                _customLeftBtnIcon = value;
                UpdateBar();
            }
        }

        public IMvxCommand ForceCrossCommand { get; set; }

        public override UIColor TintColor
        {
            get => base.TintColor;
            set
            {
                base.TintColor = value;

                if (_btnLeft is { })
                    _btnLeft.TintColor = value;

                if (_btnRight is { })
                    _btnRight.TintColor = value;
            }
        }

        public MusicRequestsToolbar() : base()
        {
            BackgroundColor = UIColor.Clear;
            Opaque = false;
            Translucent = true;
            TintColor = Colors.Black;
        }

        public override void DrawRect(CGRect area, UIViewPrintFormatter formatter)
        {
        }

        public override void Draw(CGRect rect)
        {
        }

        private void UpdateBar()
        {
            _btnLeft = null;
            _btnRight = null;

            var vm = ViewModel as IBaseViewModel;

            if (ForceCross)
            {
                _btnRight = new UIBarButtonItem(UIImage.FromBundle(Helpers.Icons.Common.ASPA), UIBarButtonItemStyle.Plain, (_, __) => ForceCrossCommand?.Execute());  
            }

            if (ForceBack)
                _btnLeft = new UIBarButtonItem(UIImage.FromFile("icons/back_navigationbar.png"), UIBarButtonItemStyle.Plain, (_, __) => vm.OnBackNavigation());

            var ls = new List<UIBarButtonItem>();

            if (_btnLeft is { })
            {
                _btnLeft.TintColor = TintColor;
                ls.Add(_btnLeft);
            }

            ls.Add(new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace));

            if (_btnRight is { })
            {
                _btnRight.TintColor = TintColor;
                ls.Add(_btnRight);
            }

            SetItems(ls.ToArray(), animated: false);
        }
    }
}
