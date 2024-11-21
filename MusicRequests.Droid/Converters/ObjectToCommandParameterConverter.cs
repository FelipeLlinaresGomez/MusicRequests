using System.Windows.Input;
using MvvmCross.Binding.ValueConverters;
using MvvmCross.Converters;

namespace MusicRequests.Droid
{
    public class ObjectToCommandParameterConverter : MvxValueConverter<ICommand, ICommand>
    {
        protected override ICommand Convert(ICommand value, System.Type targetType, object parameter,
                                            System.Globalization.CultureInfo culture)
        {
            return new MvxWrappingCommand(value, parameter);
        }
    }
}
