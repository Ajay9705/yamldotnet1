﻿using System;
using System.ComponentModel;
using System.Globalization;

namespace YamlDotNet.RepresentationModel.Serialization
{
	/// <summary>
	/// Performs type conversions.
	/// </summary>
	public static class ObjectConverter
	{
		/// <summary>
		/// Attempts to convert the specified value to another type using various approaches.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		/// <remarks>
		/// The conversion is first attempted by ditect casting.
		/// If it fails, the it attempts to use the <see cref="IConvertible"/> interface.
		/// If it still fails, it tries to use the <see cref="TypeConverter"/> of the value's type, and then the <see cref="TypeConverter"/> of the <typeparamref name="TTo"/> type.
		/// </remarks>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
		/// <exception cref="InvalidCastException">The value cannot be converted to the specified type.</exception>
		public static TTo Convert<TFrom, TTo>(TFrom value)
		{
			return (TTo)Convert(value, typeof(TTo));
		}

		/// <summary>
		/// Attempts to convert the specified value to another type using various approaches.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="to">To.</param>
		/// <returns></returns>
		/// <remarks>
		/// The conversion is first attempted by ditect casting.
		/// If it fails, the it attempts to use the <see cref="IConvertible"/> interface.
		/// If it still fails, it tries to use the <see cref="TypeConverter" /> of the value's type, and then the <see cref="TypeConverter" /> of the <paramref name="to" /> type.
		/// </remarks>
		/// <exception cref="ArgumentNullException">Either <paramref name="value"/> or <paramref name="to"/> are null.</exception>
		/// <exception cref="InvalidCastException">The value cannot be converted to the specified type.</exception>
		public static object Convert(object value, Type to)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			if (to == null)
			{
				throw new ArgumentNullException("to");
			}

			Type resultType = value.GetType();
			if (resultType == to || to.IsAssignableFrom(resultType))
			{
				return value;
			}

			if (typeof(IConvertible).IsAssignableFrom(resultType))
			{
				return System.Convert.ChangeType(value, to, CultureInfo.InvariantCulture);
			}

			TypeConverter resultTypeConverter = TypeDescriptor.GetConverter(resultType);
			if (resultTypeConverter != null && resultTypeConverter.CanConvertTo(to))
			{
				return resultTypeConverter.ConvertTo(value, to);
			}

			TypeConverter expectedTypeConverter = TypeDescriptor.GetConverter(to);
			if (expectedTypeConverter != null && expectedTypeConverter.CanConvertFrom(resultType))
			{
				return expectedTypeConverter.ConvertFrom(value);
			}

			throw new InvalidCastException(string.Format(CultureInfo.InvariantCulture, "Cannot convert from type '{0}' to type '{1}'.", resultType.FullName, to.FullName));
		}
	}
}