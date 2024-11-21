using MusicRequests.Core.Services;
using MusicRequests.Core.ViewModels;
using MvvmCross;
using System.Globalization;

namespace MusicRequests.Touch.Views.Controls
{
    public class MusicRequestsAmountLabel : UILabel
    {
        AmountViewModel _amount;
        public AmountViewModel Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                SetStyles();
            }
        }

        private bool _showAbsoluteValue;
        public bool ShowAbsoluteValue
        {
            get => _showAbsoluteValue;
            set
            {
                _showAbsoluteValue = value;
                SetStyles();
            }
        }

        private string _unknownQuantitySymbol = "-";
        public string UnknownQuantitySymbol
        {
            get => _unknownQuantitySymbol;
            set
            {
                _unknownQuantitySymbol = value;
                SetStyles();
            }
        }

        private UIFont? _decimalPartFont;
        public UIFont? DecimalPartFont
        {
            get => _decimalPartFont;
            set
            {
                _decimalPartFont = value;
                SetStyles();
            }
        }

        private UIColor _defaultColor = UIColor.Black;
        public UIColor DefaultColor
        {
            get => _defaultColor;
            set
            {
                _defaultColor = value;
                SetStyles();
            }
        }

        private UIColor? _negativeAmountColor;
        public UIColor? NegativeAmountColor
        {
            get => _negativeAmountColor;
            set
            {
                _negativeAmountColor = value;
                SetStyles();
            }
        }

        ILocalizationService _localizationService;

        public MusicRequestsAmountLabel()
        {
            _localizationService = Mvx.IoCProvider.Resolve<ILocalizationService>();
        }

        private void SetStyles()
        {
            if (_amount is null)
                return;

            var cultureInfo = new CultureInfo(_localizationService.GetCurrentCulture());

            double? quantity = _amount.Amount;

            if (ShowAbsoluteValue && quantity is { })
                quantity = Math.Abs(quantity.Value);

            string number = quantity is null ? UnknownQuantitySymbol : quantity?.ToString("N2", cultureInfo);

            string text = string.Format(
                "{0} {1}",
                number,
                string.IsNullOrWhiteSpace(_amount.Currency) ? "" : _amount.Currency
            );

            string decimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator;

            string[] parts = text.Split(decimalSeparator.ToCharArray());
            int integralPartLength = parts[0].Length;

            var prettyString = new NSMutableAttributedString(text);

            var color = quantity is null || quantity >= 0 || NegativeAmountColor is null ? DefaultColor : NegativeAmountColor;

            prettyString.SetAttributes(new UIStringAttributes
            {
                Font = Font,
                ForegroundColor = color
            }, new NSRange(0, integralPartLength));

            prettyString.SetAttributes(new UIStringAttributes
            {
                Font = DecimalPartFont ?? Font,
                ForegroundColor = color
            }, new NSRange(integralPartLength, text.Length - integralPartLength));

            AttributedText = prettyString;
        }
    }
}
