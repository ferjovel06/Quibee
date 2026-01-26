using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Quibee.Converters;

/// <summary>
/// Convertidor que cambia el color del texto según si está seleccionado
/// </summary>
public class BoolToForegroundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isSelected)
        {
            // Si está seleccionado, texto cyan, sino texto oscuro
            return isSelected ? new SolidColorBrush(Color.Parse("#7CDAF3")) : new SolidColorBrush(Color.Parse("#311B42"));
        }
        return new SolidColorBrush(Color.Parse("#311B42"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
