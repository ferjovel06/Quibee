using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Quibee.Models;

/// <summary>
/// Representa los datos deserializados del JSON de un LessonContent
/// </summary>
public class LessonContentData
{
    // Propiedades comunes para texto
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("fontSize")]
    public int? FontSize { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("alignment")]
    public string? Alignment { get; set; } // left, center, right

    [JsonPropertyName("isBulletPoint")]
    public bool? IsBulletPoint { get; set; }

    // Propiedades para headings
    [JsonPropertyName("level")]
    public int? Level { get; set; } // 1, 2, 3 para h1, h2, h3

    // Propiedades para visual_example
    [JsonPropertyName("objects")]
    public List<VisualObject>? Objects { get; set; }

    [JsonPropertyName("layout")]
    public string? Layout { get; set; } // horizontal, vertical, grid

    [JsonPropertyName("spacing")]
    public int? Spacing { get; set; }

    [JsonPropertyName("centerAlign")]
    public bool? CenterAlign { get; set; }

    [JsonPropertyName("answerBox")]
    public AnswerBoxConfig? AnswerBox { get; set; }

    [JsonPropertyName("correctAnswer")]
    public int? CorrectAnswer { get; set; }

    [JsonPropertyName("correctAnswers")]
    public List<int>? CorrectAnswers { get; set; }

    [JsonPropertyName("correctTextAnswer")]
    public string? CorrectTextAnswer { get; set; }

    [JsonPropertyName("correctTextAnswers")]
    public List<string>? CorrectTextAnswers { get; set; }

    // Propiedades para imágenes
    [JsonPropertyName("imageUrl")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("width")]
    public int? Width { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }

    // Propiedades para draggable_numbers
    [JsonPropertyName("numbers")]
    public List<int>? Numbers { get; set; }

    [JsonPropertyName("symbols")]
    public List<string>? Symbols { get; set; }

    [JsonPropertyName("colors")]
    public List<string>? Colors { get; set; }

    // Propiedades para matching_exercise (Desafío)
    [JsonPropertyName("instruction")]
    public string? Instruction { get; set; }

    [JsonPropertyName("rows")]
    public List<MatchingRow>? Rows { get; set; }

    [JsonPropertyName("numberImagePattern")]
    public string? NumberImagePattern { get; set; }

    [JsonPropertyName("numberImageWidth")]
    public int? NumberImageWidth { get; set; }

    [JsonPropertyName("numberImageHeight")]
    public int? NumberImageHeight { get; set; }

    [JsonPropertyName("allowAnyOrder")]
    public bool? AllowAnyOrder { get; set; }

    [JsonPropertyName("showOrderPrompt")]
    public bool? ShowOrderPrompt { get; set; }

    [JsonPropertyName("selectOnly")]
    public bool? SelectOnly { get; set; }
}

/// <summary>
/// Representa un objeto visual (emoji, número, símbolo) en un ejemplo
/// </summary>
public class VisualObject
{
    [JsonPropertyName("imageUrl")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("emoji")]
    public string? Emoji { get; set; }

    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("count")]
    public int? Count { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("fontSize")]
    public int? FontSize { get; set; }

    [JsonPropertyName("answerBox")]
    public bool? AnswerBox { get; set; }

    [JsonPropertyName("width")]
    public int? Width { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }

    [JsonPropertyName("interactive")]
    public bool? Interactive { get; set; }
}

/// <summary>
/// Configuración para una caja de respuesta
/// </summary>
public class AnswerBoxConfig
{
    [JsonPropertyName("position")]
    public string? Position { get; set; } // left, right, below

    [JsonPropertyName("width")]
    public int Width { get; set; } = 80;

    [JsonPropertyName("height")]
    public int Height { get; set; } = 80;

    [JsonPropertyName("interactive")]
    public bool Interactive { get; set; } = true;

    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }
}
