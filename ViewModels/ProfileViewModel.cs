using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Quibee.Models;

namespace Quibee.ViewModels;

public class ProfileViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private readonly int _studentId;
    private readonly int _levelNumber;

    private string _studentFullName = string.Empty;
    private string _birthDateText = "No disponible";
    private string _genderText = "No disponible";

    public ProfileViewModel(MainWindowViewModel? mainWindowViewModel, int studentId, int levelNumber)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _studentId = studentId;
        _levelNumber = levelNumber;

        CompletedLessons = new ObservableCollection<ProfileLessonSummary>();

        InicioCommand = new RelayCommand(OnInicio);
        LeccionesCommand = new RelayCommand(OnLecciones);
        ManualCommand = new RelayCommand(OnManual);

        _ = LoadProfileAsync();
    }

    public ObservableCollection<ProfileLessonSummary> CompletedLessons { get; }

    public string StudentFullName
    {
        get => _studentFullName;
        private set
        {
            if (_studentFullName != value)
            {
                _studentFullName = value;
                OnPropertyChanged();
            }
        }
    }

    public string BirthDateText
    {
        get => _birthDateText;
        private set
        {
            if (_birthDateText != value)
            {
                _birthDateText = value;
                OnPropertyChanged();
            }
        }
    }

    public string GenderText
    {
        get => _genderText;
        private set
        {
            if (_genderText != value)
            {
                _genderText = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand InicioCommand { get; }
    public ICommand LeccionesCommand { get; }
    public ICommand ManualCommand { get; }

    private async Task LoadProfileAsync()
    {
        try
        {
            var context = ServiceLocator.GetDbContext();

            var student = await context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.IdStudent == _studentId);

            if (student != null)
            {
                StudentFullName = $"{student.FirstName} {student.LastName}".Trim();
                BirthDateText = student.DateOfBirth.ToString("dd/MM/yyyy");
                GenderText = MapGender(student.Gender);
            }

            var completedLessons = await (
                from progress in context.StudentLessonProgress.AsNoTracking()
                join lesson in context.Lessons.AsNoTracking() on progress.IdLesson equals lesson.IdLesson
                where progress.IdStudent == _studentId && progress.IsCompleted && lesson.IsActive
                orderby progress.UpdatedAt descending, progress.LastAccessed descending, lesson.OrderIndex
                select lesson.Title)
                .Distinct()
                .Take(4)
                .ToListAsync();

            CompletedLessons.Clear();

            if (completedLessons.Count == 0)
            {
                CompletedLessons.Add(new ProfileLessonSummary
                {
                    DisplayNumber = string.Empty,
                    Title = "No completed lessons yet"
                });
                return;
            }

            for (var i = 0; i < completedLessons.Count; i++)
            {
                CompletedLessons.Add(new ProfileLessonSummary
                {
                    DisplayNumber = (i + 1).ToString(),
                    Title = completedLessons[i]
                });
            }
        }
        catch
        {
            CompletedLessons.Clear();
            CompletedLessons.Add(new ProfileLessonSummary
            {
                DisplayNumber = string.Empty,
                Title = "Unable to load lessons"
            });
        }
    }

    private void OnInicio()
    {
        _mainWindowViewModel?.NavigateToWelcome();
    }

    private void OnLecciones()
    {
        _mainWindowViewModel?.NavigateToLessonsMap(_studentId, _levelNumber);
    }

    private void OnManual()
    {
    }

    private static string MapGender(string? gender)
    {
        return gender?.Trim().ToLowerInvariant() switch
        {
            "male" => "Masculino",
            "female" => "Femenino",
            "masculino" => "Masculino",
            "femenino" => "Femenino",
            _ => "No especificado"
        };
    }
}