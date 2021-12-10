using System;
using System.ComponentModel;

namespace LcrGame.Assets.ValueConverters
{
    public static class ConverterHelper
    {
        public static object Result(bool? comparedResult, Type targetType, object parameter, object nullValue = null, object trueValue = null, object falseValue = null)
        {
            var parameterString = parameter is string ? parameter.ToString() : string.Empty;
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
            var returnValue = (comparedResult.HasValue ? comparedResult.Value ? trueValue : falseValue : nullValue).ToString();
            try
            {
                if (targetType == typeof(object)) return returnValue;
                TypeConverter converter = TypeDescriptor.GetConverter(targetType);
                return converter.ConvertFrom(returnValue);
            }
            catch
            {
                var errorValue = returnValue;
                throw new Exception(
                    $"Failed in attempt to convert {errorValue} to type {targetType.Name} using TypeConverter in class TrueFalseValues");
            }
        }

        public static object Result<T>(object value, Func<T, bool?> comparer, Type targetType, object parameter, object nullValue = null, object trueValue = null, object falseValue = null)
        {
            var parameterString = parameter?.ToString() ?? string.Empty;
            if (parameterString.Contains("?"))
            {
                var delimaterLocation = parameterString.IndexOf("?", StringComparison.Ordinal);
                var compareValue = parameterString.Substring(0, delimaterLocation);
                parameter = parameterString.Substring(delimaterLocation + 1);
                return Result(value?.ToString() == compareValue, targetType, parameter, nullValue, trueValue, falseValue);
            }
            else
            {
                var comparedResult = value is T ? comparer((T)value) : null;
                return Result(comparedResult, targetType, parameter, nullValue, trueValue, falseValue);
            }
        }

        public static object ResultWithParameterValue(Func<string, bool?> comparer, Type targetType, object parameter, object nullValue = null, object trueValue = null, object falseValue = null)
        {
            var parameterString = parameter.ToString();
            var questionIndex = parameterString.IndexOf('?');
            if (questionIndex > 0)
            {
                var compareValue = parameterString.Substring(0, questionIndex);
                var returnValues = parameterString.Substring(questionIndex + 1);
                return Result(comparer(compareValue), targetType, returnValues, nullValue, trueValue, falseValue);
            }
            else
            {
                return comparer(parameterString);
            }
        }
    }
}

