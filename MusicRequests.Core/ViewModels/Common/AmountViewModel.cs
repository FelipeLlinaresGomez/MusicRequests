using System;

namespace MusicRequests.Core.ViewModels
{
    public class AmountViewModel
    {
        public AmountViewModel()
        {
        }

        public AmountViewModel(double? amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public double? Amount { get; set; }
        public string Currency { get; set; }
        public bool NoDisponible { get; set; }

        public double AmountValue
        {
            get
            {
                return Amount ?? 0;
            }
        }

        public override string ToString()
        {
            return $"{AmountValue.ToString("N2")} {Currency}";
        }
    }
}

