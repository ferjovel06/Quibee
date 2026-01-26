using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Quibee.Converters;

/// <summary>
/// Convertidor que cambia el color de fondo según si está seleccionado
/// </summary>
public class BoolToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isSelected)
        {
            // Si está seleccionado, fondo morado, sino transparente
            return isSelected ? new SolidColorBrush(Color.Parse("#311B42")) : Brushes.Transparent;
        }
        return Brushes.Transparent;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
