using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia;
using Avalonia.Media;
using Quibee.Models;
using Quibee.ViewModels;

namespace Quibee.Converters
{
    /// <summary>
    /// Converter que transforma un TemaEdge en una PathGeometry con curva Bezier.
    /// Recibe el Edge y el ViewModel para resolver las posiciones de los nodos.
    /// Genera una curva suave y orgánica entre los centros de los dos nodos.
    /// </summary>
    public class EdgeToPathGeometryConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            // Validar que recibimos los parámetros correctos
            if (values.Count < 2 || values[0] is not TemaEdge edge || values[1] is not LessonsMapViewModel vm)
            {
                return null;
            }

            // Resolver los nodos desde el ViewModel usando el cache O(1)
            var fromNode = vm.GetNodeById(edge.FromId);
            var toNode = vm.GetNodeById(edge.ToId);

            if (fromNode == null || toNode == null)
            {
                return null;
            }

            // Calcular el centro de cada nodo considerando el layout del texto
            // Si TextOnLeft = true, la imagen está a la derecha del botón (offset positivo)
            // Si TextOnRight = true, la imagen está a la izquierda del botón (sin offset)
            const double spacing = 10; // Spacing del StackPanel

            var fromTextWidth = Math.Max(
                MeasureTextWidth(fromNode.Title, 18),
                MeasureTextWidth(fromNode.Description, 14));
            var toTextWidth = Math.Max(
                MeasureTextWidth(toNode.Title, 18),
                MeasureTextWidth(toNode.Description, 14));
            
            double fromImageOffsetX = fromNode.TextOnLeft ? (fromTextWidth + spacing) : 0;
            double toImageOffsetX = toNode.TextOnLeft ? (toTextWidth + spacing) : 0;
            
            var fromCenterX = fromNode.PositionX + fromImageOffsetX + (fromNode.ImageWidth / 2);
            var fromCenterY = fromNode.PositionY + (fromNode.ImageHeight / 2);
            
            var toCenterX = toNode.PositionX + toImageOffsetX + (toNode.ImageWidth / 2);
            var toCenterY = toNode.PositionY + (toNode.ImageHeight / 2);

            // Calcular el ángulo entre los dos nodos
            var dx = toCenterX - fromCenterX;
            var dy = toCenterY - fromCenterY;
            var angle = Math.Atan2(dy, dx);

            // Calcular los puntos de inicio/fin en el borde de las imágenes
            // Asumiendo imágenes circulares o cuadradas, usamos el radio
            var fromRadius = Math.Max(fromNode.ImageWidth, fromNode.ImageHeight) / 2;
            var toRadius = Math.Max(toNode.ImageWidth, toNode.ImageHeight) / 2;

            // Punto de inicio: centro + offset en dirección al destino
            var startX = fromCenterX + Math.Cos(angle) * fromRadius;
            var startY = fromCenterY + Math.Sin(angle) * fromRadius;

            // Punto de fin: centro - offset en dirección desde el origen
            var endX = toCenterX - Math.Cos(angle) * toRadius;
            var endY = toCenterY - Math.Sin(angle) * toRadius;

            // Calcular deltas para curva Bezier orgánica (usando puntos de borde)
            var deltaX = endX - startX;
            var deltaY = endY - startY;

            // Puntos de control para curva suave
            // c1: cerca del nodo origen, con offset vertical
            var c1X = startX + deltaX * 0.35;
            var c1Y = startY + deltaY * 0.25;
            
            // c2: cerca del nodo destino, con offset vertical
            var c2X = endX - deltaX * 0.35;
            var c2Y = endY - deltaY * 0.25;

            // Crear la geometría de la curva Bezier
            var geometry = new PathGeometry();
            var figure = new PathFigure
            {
                StartPoint = new Avalonia.Point(startX, startY),
                IsClosed = false
            };

            // Curva Bezier cúbica con los puntos de control calculados
            var bezierSegment = new BezierSegment
            {
                Point1 = new Avalonia.Point(c1X, c1Y),
                Point2 = new Avalonia.Point(c2X, c2Y),
                Point3 = new Avalonia.Point(endX, endY)
            };

            figure.Segments.Add(bezierSegment);
            geometry.Figures.Add(figure);

            return geometry;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("EdgeToPathGeometryConverter no soporta ConvertBack");
        }

        private static double MeasureTextWidth(string text, double fontSize)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }

            var typeface = new Typeface(new FontFamily("Lilita One"));
            var formatted = new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                fontSize,
                Brushes.White);

            return formatted.Width;
        }
    }
}
