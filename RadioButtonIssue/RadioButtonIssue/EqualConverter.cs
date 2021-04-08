using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

#nullable enable

namespace RadioButtonIssue
{
    /// <summary>
    /// Checks whether the incoming value equals the provided parameter.
    /// </summary>
    public class EqualConverter : ValueConverterExtension, IValueConverter
    {
        /// <summary>
        /// Checks whether the incoming value doesn't equal the provided parameter.
        /// </summary>
        /// <param name="value">The first object to compare.</param>
        /// <param name="targetType">The type of the binding target property. This is not implemented.</param>
        /// <param name="parameter">The second object to compare.</param>
        /// <param name="culture">The culture to use in the converter. This is not implemented.</param>
        /// <returns>True if <paramref name="value"/> and <paramref name="parameter"/> are equal, False if they are not equal.</returns>
        public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture) => ConvertInternal(value, parameter);

        internal static bool ConvertInternal(object? value, object? parameter) =>
            (value != null && value.Equals(parameter)) || (value == null && parameter == null);

        /// <summary>
        /// This method is not implemented and will throw a <see cref="NotImplementedException"/>.
        /// </summary>
        /// <param name="value">N/A</param>
        /// <param name="targetType">N/A</param>
        /// <param name="parameter">N/A</param>
        /// <param name="culture">N/A</param>
        /// <returns>N/A</returns>
        public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Checks whether the incoming value equals the provided parameter.
    /// </summary>
    public class EqualConverter<T> : ValueConverterExtension, IValueConverter
    {
        private IEqualityComparer<T>? _comparer;

        public EqualConverter() : this(EqualityComparer<T>.Default) { }
        public EqualConverter(IEqualityComparer<T> comparer) => Comparer = comparer;

        public bool ThrowOnBadType { get; set; }
        public IEqualityComparer<T> Comparer { get => _comparer ??= EqualityComparer<T>.Default; set => _comparer = value; }

        /// <summary>
        /// Checks whether the incoming value doesn't equal the provided parameter.
        /// </summary>
        /// <param name="value">The first object to compare.</param>
        /// <param name="targetType">The type of the binding target property. This is not implemented.</param>
        /// <param name="parameter">The second object to compare.</param>
        /// <param name="culture">The culture to use in the converter. This is not implemented.</param>
        /// <returns>True if <paramref name="value"/> and <paramref name="parameter"/> are equal, False if they are not equal.</returns>
        public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (value is null && parameter is null)
                return true;
            if ((value is null) != (parameter is null))
                return false;
            if (value is not T v)
                return ThrowOnBadType ? new ArgumentException($"{nameof(value)} is not of type {typeof(T)}", nameof(value)) : false;
            if (parameter is not T p)
                return ThrowOnBadType ? new ArgumentException($"{nameof(parameter)} is not of type {typeof(T)}", nameof(parameter)) : false;
            return Comparer.Equals(v, p);
        }

        /// <summary>
        /// This method is not implemented and will throw a <see cref="NotImplementedException"/>.
        /// </summary>
        /// <param name="value">N/A</param>
        /// <param name="targetType">N/A</param>
        /// <param name="parameter">N/A</param>
        /// <param name="culture">N/A</param>
        /// <returns>N/A</returns>
        public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
            => throw new NotImplementedException();
    }

    public class StringEqualConverter : EqualConverter<string>
    {
        // from http://stackoverflow.com/a/32764112/548304
        private static readonly Dictionary<StringComparison, Func<StringComparer>> ComparsionToComparer =
           new()
           {
               [StringComparison.CurrentCulture] = () => StringComparer.CurrentCulture,
               [StringComparison.CurrentCultureIgnoreCase] = () => StringComparer.CurrentCultureIgnoreCase,
               [StringComparison.InvariantCulture] = () => StringComparer.InvariantCulture,
               [StringComparison.InvariantCultureIgnoreCase] = () => StringComparer.InvariantCultureIgnoreCase,
               [StringComparison.Ordinal] = () => StringComparer.Ordinal,
               [StringComparison.OrdinalIgnoreCase] = () => StringComparer.OrdinalIgnoreCase
           };

        public StringEqualConverter(StringComparison stringComparison) : base(ComparsionToComparer[stringComparison]()) { }
        public StringEqualConverter() : base() { }
        public StringEqualConverter(StringComparer stringComparer) : base(stringComparer) { }
    }
}
