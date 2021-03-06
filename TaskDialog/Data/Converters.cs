﻿using System;
using System.Collections;
using System.Windows;
using System.Windows.Data;

namespace TaskDialogInterop.Data
{
	/// <summary>
	/// Converts a null check into a negated visibility value.
	/// </summary>
	internal class NotNullToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (targetType != typeof(Visibility))
			{
				throw new InvalidOperationException();
			}
			return ((value != null) ? Visibility.Visible : Visibility.Collapsed);
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
	/// <summary>
	/// Converts a boolean into a negated visibility value.
	/// </summary>
	internal class NotBooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return DependencyProperty.UnsetValue;
			if (value.GetType() != typeof(bool))
				throw new InvalidOperationException();

			return ((!(bool)value) ? Visibility.Visible : Visibility.Collapsed);
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return DependencyProperty.UnsetValue;
			if (value.GetType() != typeof(Visibility))
				throw new InvalidOperationException();

			return (((Visibility)value) != Visibility.Visible);
		}
	}
	/// <summary>
	/// Converts a collection's count, specifically whether it is empty or not, into a visibility value.
	/// </summary>
	internal class CollectionNotEmptyToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return DependencyProperty.UnsetValue;
			if (!(value is ICollection))
				throw new InvalidOperationException();

			return ((ICollection)value).Count != 0 ? Visibility.Visible : Visibility.Collapsed;
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
	/// <summary>
	/// Chops up multiline command link text appropriately.
	/// </summary>
	internal class CommandLinkTextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return null;

			if (value.GetType() != typeof(string))
			{
				throw new InvalidOperationException();
			}
			if (targetType != typeof(string))
			{
				throw new InvalidOperationException();
			}

			if (parameter == null || parameter.ToString() == "1")
            {
                if (value.ToString().Contains("\n"))
				{
					return value.ToString().Substring(0, value.ToString().IndexOf("\n"));
				}

                return value;
            }

            if (parameter.ToString() == "2")
            {
                if (value.ToString().Contains("\n"))
                {
                    return value.ToString().Substring(value.ToString().IndexOf("\n") + 1);
                }

                return null;
            }

            return value;
        }
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
	/// <summary>
	/// Determines visibility for command link extra text.
	/// </summary>
	internal class CommandLinkExtraTextVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return Visibility.Collapsed;

			if (value.GetType() != typeof(String))
			{
				throw new InvalidOperationException();
			}
			if (targetType != typeof(Visibility))
			{
				throw new InvalidOperationException();
			}

			return (String.IsNullOrEmpty(value.ToString()) || !value.ToString().Contains("\n")) ? Visibility.Collapsed : Visibility.Visible;
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
