## 🔧 Pré-requis

* Visual Studio 2022 (avec workload MAUI installé)
* NuGet packages à installer :

  * `CommunityToolkit.Mvvm`

```bash
dotnet add package CommunityToolkit.Mvvm
```

---

## 📁 Structure du projet

```
MyMauiApp/
│
├── Models/
│   └── TaskItem.cs
│
├── ViewModels/
│   └── TaskViewModel.cs
│
├── Views/
│   └── TaskPage.xaml
│   └── TaskPage.xaml.cs
│
├── App.xaml
├── AppShell.xaml
└── MauiProgram.cs
```

---

## ✅ Étape 1 : Le modèle `TaskItem.cs`

```csharp
namespace MyMauiApp.Models;

public class TaskItem
{
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}
```

---

## ✅ Étape 2 : ViewModel `TaskViewModel.cs`

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyMauiApp.Models;
using System.Collections.ObjectModel;

namespace MyMauiApp.ViewModels;

public partial class TaskViewModel : ObservableObject
{
    [ObservableProperty]
    private string newTaskTitle;

    [ObservableProperty]
    private ObservableCollection<TaskItem> tasks = new();

    [RelayCommand]
    private void AddTask()
    {
        if (!string.IsNullOrWhiteSpace(NewTaskTitle))
        {
            Tasks.Add(new TaskItem { Title = NewTaskTitle, IsCompleted = false });
            NewTaskTitle = string.Empty;
        }
    }

    [RelayCommand]
    private void ToggleCompleted(TaskItem task)
    {
        if (task != null)
        {
            task.IsCompleted = !task.IsCompleted;
            OnPropertyChanged(nameof(Tasks)); // Refresh UI if needed
        }
    }

    [RelayCommand]
    private void DeleteTask(TaskItem task)
    {
        if (task != null && Tasks.Contains(task))
        {
            Tasks.Remove(task);
        }
    }
}
```

---

## ✅ Étape 3 : Page XAML `TaskPage.xaml`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:viewmodels="clr-namespace:MyMauiApp.ViewModels"
             x:Class="MyMauiApp.Views.TaskPage">

    <ContentPage.BindingContext>
        <viewmodels:TaskViewModel />
    </ContentPage.BindingContext>

    <VerticalStackLayout Padding="20" Spacing="15">

        <Entry Placeholder="Nouvelle tâche"
               Text="{Binding NewTaskTitle}" />

        <Button Text="Ajouter"
                Command="{Binding AddTaskCommand}" />

        <CollectionView ItemsSource="{Binding Tasks}">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <SwipeView>
                        <SwipeView.RightItems>
                            <SwipeItems>
                                <SwipeItem Text="Supprimer"
                                           BackgroundColor="Red"
                                           Command="{Binding BindingContext.DeleteTaskCommand, Source={x:Reference Name=TaskPage}}"
                                           CommandParameter="{Binding .}" />
                            </SwipeItems>
                        </SwipeView.RightItems>
                        <Grid Padding="10">
                            <Label Text="{Binding Title}" 
                                   TextDecorations="{Binding IsCompleted, Converter={StaticResource BoolToTextDecorationConverter}}" />
                            <CheckBox IsChecked="{Binding IsCompleted}" 
                                      CheckedChanged="OnCheckChanged"/>
                        </Grid>
                    </SwipeView>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>

    </VerticalStackLayout>
</ContentPage>
```

> 💡 Il vous faudra déclarer une `BoolToTextDecorationConverter` (ou autre effet visuel) si vous voulez barrer le texte quand `IsCompleted` est `true`.

---

## ✅ Étape 4 : MauiProgram.cs – Injection

```csharp
using CommunityToolkit.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        return builder.Build();
    }
}
```

---

## ✅ Étape 5 : Navigation depuis AppShell

**AppShell.xaml**

```xml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:views="clr-namespace:MyMauiApp.Views"
       x:Class="MyMauiApp.AppShell">

    <ShellContent Title="Tâches" ContentTemplate="{DataTemplate views:TaskPage}" />
</Shell>
```

---

## ✅ Étape 6 : Ajout du converter (optionnel)

```csharp
public class BoolToTextDecorationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => (bool)value ? TextDecorations.Strikethrough : TextDecorations.None;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

Et déclarez-le dans vos ressources XAML.

