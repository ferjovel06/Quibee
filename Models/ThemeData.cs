using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Quibee.Models
{
    /// <summary>
    /// Modelo que representa un tema en el mapa de lecciones
    /// </summary>
    public class ThemeData
    {
        private string _imagePath = string.Empty;

        /// <summary>
        /// ID del tema en la base de datos (de la tabla TOPIC)
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Número del tema (1-5)
        /// </summary>
        public int ThemeNumber { get; set; }

        /// <summary>
        /// Título del tema (ej: "Tema 1: Sumas y restas")
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Descripción corta del tema
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Imagen del tema (Bitmap cargado)
        /// </summary>
        public Bitmap? Image { get; private set; }

        /// <summary>
        /// Ruta de la imagen del tema (setter carga el Bitmap automáticamente)
        /// </summary>
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        Image = new Bitmap(AssetLoader.Open(new Uri(value)));
                    }
                    catch (Exception ex)
                    {
                        // Log error silently - image will be null
                        System.Diagnostics.Debug.WriteLine($"Error loading image {value}: {ex.Message}");
                        Image = null;
                    }
                }
            }
        }

        /// <summary>
        /// Ancho de la imagen en píxeles
        /// </summary>
        public double ImageWidth { get; set; } = 100;

        /// <summary>
        /// Alto de la imagen en píxeles
        /// </summary>
        public double ImageHeight { get; set; } = 100;

        /// <summary>
        /// Posición horizontal (Canvas.Left o Canvas.Right)
        /// </summary>
        public double PositionX { get; set; }

        /// <summary>
        /// Posición vertical (Canvas.Top)
        /// </summary>
        public double PositionY { get; set; }

        /// <summary>
        /// Si true, usa Canvas.Right en lugar de Canvas.Left
        /// </summary>
        public bool UseRightAlignment { get; set; } = false;

        /// <summary>
        /// Si true, el texto va a la izquierda de la imagen
        /// </summary>
        public bool TextOnLeft { get; set; } = false;

        /// <summary>
        /// Si true, el texto va a la derecha de la imagen
        /// </summary>
        public bool TextOnRight { get; set; } = false;

        /// <summary>
        /// Ángulo de rotación de la imagen (para la estrella, por ejemplo)
        /// </summary>
        public double RotationAngle { get; set; } = 0;
    }
}
