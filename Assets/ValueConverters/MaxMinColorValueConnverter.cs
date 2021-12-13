using LcrGame.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace LcrGame.Assets.ValueConverters
{
    public class MaxMinColorValueConnverter : MarkupExtension, IMultiValueConverter
    {
        Brush brushMinimum = new SolidColorBrush(Colors.DarkGreen);
        Brush brushNormal = new SolidColorBrush(Colors.Blue);
        Brush brushMaximum = new SolidColorBrush(Colors.DarkGoldenrod);
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value = System.Convert.ToInt32(values[0]);
            var maxMin = (IMaximumMinimum)values[1];
            if (maxMin.Minimum == maxMin.Maximum) return brushNormal;
            else if (value == maxMin.Minimum) return brushMinimum;
            else if (value == maxMin.Maximum) return brushMaximum;
            return brushNormal;
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
