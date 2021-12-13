using LcrGame.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace LcrGame.Assets.ValueConverters
{
    public class MaxMinAssociatedTextValueConnverter : MarkupExtension, IMultiValueConverter
    {
        Brush brushGreen = new SolidColorBrush(Colors.Green);
        Brush brushBlue = new SolidColorBrush(Colors.Blue);
        Brush brushYellow = new SolidColorBrush(Colors.Yellow);
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value = System.Convert.ToInt32(values[0]);
            var maxMin = (IMaximumMinimum)values[1];
            if (maxMin.Minimum == maxMin.Maximum) return null;
            else if (value == maxMin.Minimum) return $"Shortest ({value})";
            else if (value == maxMin.Maximum) return $"Longest ({value})";
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
