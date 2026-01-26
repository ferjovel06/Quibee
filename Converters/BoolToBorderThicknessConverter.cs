using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Quibee.Converters;

/// <summary>
/// Convertidor que cambia el grosor del borde izquierdo según si está seleccionado
/// </summary>
public class BoolToBorderThicknessConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isSelected)
        {
            // Si está seleccionado, borde derecho de 8px, sino 0
            return isSelected ? new Thickness(0, 0, 8, 0) : new Thickness(0);
        }
        return new Thickness(0);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
