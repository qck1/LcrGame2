using LcrGame.ViewModels;
using System;
using System.Diagnostics;
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
            Debug.Assert(values[1] is IMaximumMinimum, $"The value of values[1] is {values[1].GetType()} not IMaximumMinimum");
            Debug.Assert(values[0] is double, $"The value of values[0] is {values[0].GetType()} not Double");
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
