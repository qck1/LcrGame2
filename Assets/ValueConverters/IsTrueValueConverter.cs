using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace LcrGame.Assets.ValueConverters
{
    /// <summary>
    /// Tests if the ToString() of the bound value is true and returns the first value in the ConverterParameter, 
    /// otherwise the second
    /// The ConverterParameter is the return valuse separated by the ":" character.
    ///     The first value is the returned if the result is true
    ///     The second value is the returned if the result is false
    /// 
    /// The true or false (or null) value is converted to the TargetType before being returned, an 
    ///     exception being thrown if the true or false string cannot be converted
    /// </summary>
    public sealed class IsTrueConverter : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        /// <summary>
        /// Binding must be true to return the first or true value, otherwise the second or false value is returned
        /// 
        /// SAMPLE: Content="{Binding IsChecked, RelativeSource={RelativeSource Self}, 
        ///                     Converter={converterHelperSample:IsFalseConverter}, ConverterParameter=Show:Hide}"
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConverterHelper.Result<bool?>(value, i => i, targetType, parameter);
        }

        /// <summary>
        /// This only really works to return the for boolean. Work should be done.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { return ConverterHelper.Result<bool>(value, i => i, targetType, parameter); }

        /// <summary>
        /// All Bindings must be true to return the first or true value, otherwise the second or false value is returned
        /// 
        /// SAMPLE
        /// <RowDefinition.Height>
        ///     <MultiBinding Converter = "{converterHelperSample:IsTrueConverter}"
        ///                 ConverterParameter="*:0">
        ///         <Binding ElementName = "ShowHideButton"
        ///                     Path="IsChecked" />
        ///         <Binding Converter = "{converterHelperSample:IsNullOrEmptyConverter}"
        ///                     ConverterParameter="reverse"
        ///                     ElementName="ReasonTextBox"
        ///                     Path="Text" />
        ///		</MultiBinding>
        ///  </RowDefinition.Height>
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var isTrue = values.All(i => i != null && (i as bool? == true || i.ToString() == "true")); ; //Need to convert to string to ensure no issues
            return ConverterHelper.Result<bool?>(isTrue, i => i, targetType, parameter);
        }

        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetTypes">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows converter to be used directly without XAML declaration
        /// </summary>
        /// <param name="serviceProvider">not used</param>
        /// <returns></returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
