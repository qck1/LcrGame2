using System;
using System.ComponentModel;
using System.Linq;

namespace LcrGame.Assets.ValueConverters
{
    public static class ConverterHelper
    {
        /// <summary>
        /// Used with IValueConverter and IMultiValueConverter to provide more flexibility using the ConverterParameter.
        /// The ConverterParamer, which is assumed to be a string
        ///  A) If tha string is "reverse" then comparedResult is inverted.
        ///  B) Else
        ///     1) is stripped of any text before a "?"
        ///     2) spearated into pieces that are separated by the delimeter ":"
        ///     3) the parts are used to specify the true, false, and null values to replace
        ///         the specified default values
        /// 
        /// if value returned is determined by the parameter string and the compared result:
        ///     1) first value from parameter.ToString() is returned if comparedResult is true
        ///     2) second value from parameter.ToString() is returned if comparedResult is false
        ///     3) third value from parameter.ToString() is returned if comparedResult is null
        /// This value will be converter to the type specified by the targetType, creating an excpetion 
        ///         is there is a conversion error
        /// 
        /// if there is no parameter value then boolean valuse are returned
        /// 
        /// SAMPLE: return ConverterHelper.Result(value == null, targetType, parameter);
        /// </summary>
        /// <param name="comparedResult">nullable boolean value that is used to determin returned value</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">string that specifies what valuses to assign to true, false, and null values </param>
        /// <param name="nullValue">Default value to use if compareResult is null</param>
        /// <param name="trueValue">Default value to use if compareResult is true</param>
        /// <param name="falseValue">Default value to use if compareResult is false</param>
        /// <returns>converted value as targetType</returns>
		public static object Result(bool? comparedResult, Type targetType, object parameter, object nullValue = null,
            object trueValue = null, object falseValue = null)
        {
            var parameterString = parameter is string ? parameter.ToString() : string.Empty;
            if (parameterString.Contains("?")) parameterString = parameterString.Substring(parameterString.IndexOf("?") + 1);
            if (parameterString == string.Empty) return comparedResult;
            falseValue = falseValue ?? "false";
            trueValue = trueValue ?? "true";
            if (parameterString.ToLower() == "reverse")
            {
                var temp = falseValue;
                falseValue = trueValue;
                trueValue = temp;
            }
            else if (parameterString.Contains(":"))
            {
                var values = parameterString.Split(':');
                trueValue = values[0];
                falseValue = values[1];
                nullValue = values.Length > 2 ? values[2] : nullValue;
            }

            if (nullValue == null && comparedResult == null) return null;
            var returnValue = (comparedResult.HasValue ? (comparedResult.Value ? trueValue : falseValue) : nullValue).ToString();

            return ConvertToType(returnValue, targetType);
        }

        /// <summary>
        /// Used with IValueConverter and IMultiValueConverter to provide more flexibility using the ConverterParameter.
        /// The ConverterParamer, which is assumed to be a string
        ///  A) If tha string is "reverse" then comparedResult is inverted.
        ///  B) Else
        ///     1) is stripped of any text before a "?"
        ///     2) spearated into pieces that are separated by the delimeter ":"
        ///     3) the parts are used to specify the true, false, and null values to replace
        ///         the specified default values
        /// 
        /// if value returned is determined by the parameter string and the comparer value:
        ///     1) first value from parameter.ToString() is returned if comparer value is true
        ///     2) second value from parameter.ToString() is returned if comparer value is false
        ///     3) third value from parameter.ToString() is returned if comparer value is null
        /// This value will be converter to the type specified by the targetType, creating an excpetion is there 
        ///         is a conversion error
        /// 
        /// if there is no parameter value then boolean valuse are returned
        /// 
        /// SAMPLE: return ConverterHelper.Result&lt;bool?>(value, i => !i, targetType, parameter);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The variable to use with the comparer</param>
        /// <param name="comparer">function that translates value argument to true, false, or null</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">string that specifies what valuses to assign to true, false, and null values </param>
        /// <param name="nullValue">Default value to use if comparer value is null</param>
        /// <param name="trueValue">Default value to use if comparer value is true</param>
        /// <param name="falseValue">Default value to use if comparer value is false</param>
        /// <returns>converted value as targetType</returns>
        public static object Result<T>(object value, Func<T, bool?> comparer, Type targetType, object parameter,
            object nullValue = null, object trueValue = null, object falseValue = null)
        {
            var comparedResult = value is T ? comparer((T)value) : null;
            return Result(comparedResult, targetType, parameter, nullValue, trueValue, falseValue);
        }

        /// <summary>
        /// Used with IValueConverter and IMultiValueConverter to provide more flexibility using the ConverterParameter.
        /// 
        /// The paremeter string is a string that contains strings separated by "?" and ":" 
        ///     1) The parmeter argument is separated into strings delimited by the ":"
        ///     2) The value before each "?" character specifies the string to be used with the comparer argument.
        ///         The compare will determine if this is a match, and if so, the value after the last "?"
        ///         in the ":" delimited string is returned. 
        /// 
        /// SAMPLE: return ConverterHelper.ResultWithParameterValue(
        ///         p => String.Equals(value.ToString(), p, StringComparison.CurrentCultureIgnoreCase), targetType, parameter);
        /// </summary>
        /// <param name="comparer">function that to determine which values to use in the parameter argument</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">string that specifies what valuses to assign to true, false, and null values </param>
        /// <param name="nullValue">Default value to use if compareResult is null</param>
        /// <param name="trueValue">Default value to use if compareResult is true</param>
        /// <param name="falseValue">Default value to use if compareResult is false</param>
        /// <returns>converted value as targetType</returns>
        public static object ResultWithParameterValue(Func<string, bool?> comparer, Type targetType,
            object parameter, object nullValue = null, object trueValue = null, object falseValue = null)
        {
            var parameterString = parameter.ToString();
            var compareItems = parameterString.Split(':').Select(i => i.Split('?').ToArray()).ToArray();
            if (compareItems.Length > 1)
            {
                for (int i = 0; i < compareItems.Length - 1; i++)
                {
                    for (int j = 0; j < compareItems[i].Length - 1; j++)
                    {
                        // ":" used as false value because it cannot be in the result string
                        var returnValue = Result(comparer(compareItems[i][j]), typeof(string), compareItems[i][j],
                            null, compareItems[i][compareItems[i].Length - 1], ":");
                        if (returnValue.ToString().ToLower() != ":") return ConvertToType(returnValue, targetType);
                    }
                }
                return ConvertToType(compareItems.Last().First(), targetType);
            }
            return ConvertToType(parameterString, targetType);
        }

        private static object ConvertToType(object value, Type targetType)
        {
            try
            {
                if (targetType == typeof(object)) return value;
                TypeConverter converter = TypeDescriptor.GetConverter(targetType);
                return converter.ConvertFrom(value);
            }
            catch
            {
                var errorValue = value;
                throw new Exception(
                    $"Failed in attempt to convert {errorValue} to type {targetType.Name} using TypeConverter in class TrueFalseValues");
            }
        }
    }
}
