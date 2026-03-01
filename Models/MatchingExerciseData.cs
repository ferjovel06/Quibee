using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Quibee.Models;

/// <summary>
/// Datos para un ejercicio de emparejamiento (drag-and-drop) en la sección Desafío.
/// Cada fila muestra objetos repetidos y una casilla donde el alumno
/// debe colocar (arrastrar) el número que representa la cantidad.
/// </summary>
public class MatchingExerciseData
{
    /// <summary>
    /// Instrucción que se muestra arriba del ejercicio.
    /// Ej. "Arrastra el número correcto a cada grupo"
    /// </summary>
    [JsonPropertyName("instruction")]
    public string? Instruction { get; set; }

    /// <summary>
    /// Lista de filas del ejercicio. Cada fila tiene objetos + respuesta correcta.
    /// </summary>
    [JsonPropertyName("rows")]
    public List<MatchingRow>? Rows { get; set; }

    /// <summary>
    /// Patrón base para las imágenes de números.
    /// Ej. "avares://Quibee/Assets/Images/{0}.png" donde {0} se reemplaza con el número.
    /// </summary>
    [JsonPropertyName("numberImagePattern")]
    public string? NumberImagePattern { get; set; }

    /// <summary>
    /// Ancho de las imágenes de números arrastrables (opcional, default 55)
    /// </summary>
    [JsonPropertyName("numberImageWidth")]
    public int? NumberImageWidth { get; set; }

    /// <summary>
    /// Alto de las imágenes de números arrastrables (opcional, default 55)
    /// </summary>
    [JsonPropertyName("numberImageHeight")]
    public int? NumberImageHeight { get; set; }
}

/// <summary>
/// Una fila del ejercicio: grupo de imágenes/emojis + la respuesta (número correcto).
/// </summary>
public class MatchingRow
{
    /// <summary>
    /// URL de la imagen (avares://...) que se repite
    /// </summary>
    [JsonPropertyName("imageUrl")]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Emoji alternativo si no se usa imagen
    /// </summary>
    [JsonPropertyName("emoji")]
    public string? Emoji { get; set; }

    /// <summary>
    /// Cantidad de objetos que se muestran en la fila
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; set; }

    /// <summary>
    /// Número correcto que debe arrastrar el alumno
    /// </summary>
    [JsonPropertyName("correctAnswer")]
    public int CorrectAnswer { get; set; }

    /// <summary>
    /// Ancho de cada imagen en la fila (opcional)
    /// </summary>
    [JsonPropertyName("imageWidth")]
    public int? ImageWidth { get; set; }

    /// <summary>
    /// Alto de cada imagen en la fila (opcional)
    /// </summary>
    [JsonPropertyName("imageHeight")]
    public int? ImageHeight { get; set; }

    /// <summary>
    /// Color de fondo de la fila (opcional, hex)
    /// </summary>
    [JsonPropertyName("rowColor")]
    public string? RowColor { get; set; }
}
