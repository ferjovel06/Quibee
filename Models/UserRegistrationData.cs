using System;

namespace Quibee.Models
{
    /// <summary>
    /// Modelo que contiene los datos del usuario durante el proceso de registro
    /// </summary>
    public class UserRegistrationData
    {
        public string Nombres { get; set; } = "";
        public string Apellidos { get; set; } = "";
        public DateTime? FechaNacimiento { get; set; }
        public string Genero { get; set; } = "";
        public string Grado { get; set; } = "";
        public string ClaveAcceso { get; set; } = "";

        public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();
    }
}
