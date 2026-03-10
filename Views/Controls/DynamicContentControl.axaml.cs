using Avalonia;
using Avalonia.Controls;
using Quibee.Models;

namespace Quibee.Views.Controls;

/// <summary>
/// Control que renderiza dinámicamente el contenido apropiado según el tipo
/// </summary>
public partial class DynamicContentControl : UserControl
{
    public static readonly StyledProperty<LessonContent?> LessonContentProperty =
        AvaloniaProperty.Register<DynamicContentControl, LessonContent?>(nameof(LessonContent));

    public LessonContent? LessonContent
    {
        get => GetValue(LessonContentProperty);
        set => SetValue(LessonContentProperty, value);
    }

    public DynamicContentControl()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == LessonContentProperty && LessonContent != null)
        {
            UpdateContent();
        }
    }

    private void UpdateContent()
    {
        if (LessonContent?.Data == null) return;

        var container = this.FindControl<ContentControl>("ContentContainer");
        if (container == null) return;

        // Seleccionar el control apropiado según el tipo
        Control? contentControl = LessonContent.ContentType switch
        {
            "text" => new TextContentControl { ContentData = LessonContent.Data },
            "heading" => new HeadingContentControl { ContentData = LessonContent.Data },
            "visual_example" => new VisualExampleControl { ContentData = LessonContent.Data, LessonContext = LessonContent },
            "matching_exercise" => new MatchingExerciseControl { ContentData = LessonContent.Data, LessonContext = LessonContent },
            _ => new TextBlock 
            { 
                Text = $"[Tipo no soportado: {LessonContent.ContentType}]",
                Foreground = Avalonia.Media.Brushes.Red
            }
        };

        container.Content = contentControl;
    }
}
