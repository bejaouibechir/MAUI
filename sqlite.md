# Application gestion des tâches qui consomme SQLite

## Étape 1 : Configuration du projet MAUI
 - Créez un nouveau projet MAUI :
 - Ouvrez Visual Studio 2022.
 - Sélectionnez "Créer un nouveau projet".
 - Choisissez "Application .NET MAUI" et cliquez sur "Suivant".
 - Nommez le projet TasksManger et cliquez sur "Créer".

**Note**: Il faut installer le package sqlite-net-pcl 
Copier cette ligne dans le fichier **csproj** et compilez

``` XML
<PackageReference Include="sqlite-net-pcl" Version="1.9.172" />
```

## Étape 2 : Configuration des modèles de données
### Définir les modèles User et Task :

Dans le dossier Models, créez deux fichiers User.cs et Task.cs

**User.cs:**

``` CSharp
namespace MauiApp1;

// User.cs
using SQLite;
    public class User
    {
        [PrimaryKey, AutoIncrement,Column("Id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Post { get; set; }

    public override string ToString()
    {
        return $"Name: {Name} Email: {Email} Post: {Post}";
    }
}
```

**Task.cs:**

``` CSharp
// Task.cs
using SQLite;

namespace MauiApp1.Models
{
    public enum TaskStatus
    {
        Initial,
        InProgress,
        Done
    }

    public class Task
    {
        [PrimaryKey, AutoIncrement,Column("Id")]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public TaskStatus Status { get; set; }
    }
}
```

## Étape 3 : Configuration des repositories
### Créer les repositories :

Créez deux fichiers **UserRepository.cs** et **TaskRepository.cs**

**UserRepository.cs:**

``` CSharp
// UserRepository.cs
using SQLite;
using MauiApp1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MauiApp1
{
    public class UserRepository
    {
        private readonly SQLiteConnection _database;

        public UserRepository(string dbPath)
        {
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<User>();
        }

        public List<User> GetAllUsers()
        {
            return _database.Table<User>().ToList();
        }

        public User GetUser(int id)
        {
            return _database.Table<User>().FirstOrDefault(u => u.Id == id);
        }

        public int SaveUser(User user)
        {
            if (user.Id != 0)
            {
                return _database.Update(user);
            }
            else
            {
                return _database.Insert(user);
            }
        }

        public int DeleteUser(int id)
        {
            return _database.Delete<User>(id);
        }
    }
}
```

**TaskRepository.cs:**

``` CSharp
// TaskRepository.cs
using SQLite;
using MauiApp1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MauiApp1.Data
{
    public class TaskRepository
    {
        private readonly SQLiteConnection _database;

        public TaskRepository(string dbPath)
        {
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<Task>();
        }

        public List<Task> GetAllTasks()
        {
            return _database.Table<Task>().ToList();
        }

        public List<Task> GetTasksByUser(int userId)
        {
            return _database.Table<Task>().Where(t => t.UserId == userId).ToList();
        }

        public Task GetTask(int id)
        {
            return _database.Table<Task>().FirstOrDefault(t => t.Id == id);
        }

        public int SaveTask(Task task)
        {
            if (task.Id != 0)
            {
                return _database.Update(task);
            }
            else
            {
                return _database.Insert(task);
            }
        }

        public int DeleteTask(int id)
        {
            return _database.Delete<Task>(id);
        }
    }
}

```

## Étape 4 : Configuration de l'injection de dépendances

Configurer les services dans **MauiProgram.cs** :

``` CSharp

using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MauiApp1.Data;
using System.IO;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "MauiApp1.db3");

            builder.Services.AddSingleton<UserRepository>(s => ActivatorUtilities.CreateInstance<UserRepository>(s, dbPath));
            builder.Services.AddSingleton<TaskRepository>(s => ActivatorUtilities.CreateInstance<TaskRepository>(s, dbPath));

            return builder.Build();
        }
    }
}
```

## Étape 5 : Utilisation des repositories dans l'application

Injecter les repositories dans **.xaml.cs** :

``` CSharp
using MauiApp1.Data;

namespace MauiApp1
{
    public partial class App : Application
    {
        public static UserRepository UsersRepository { get; private set; }
        public static TaskRepository TasksRepository { get; private set; }

        public App(UserRepository userRepository, TaskRepository taskRepository)
        {
            InitializeComponent();

            MainPage = new AppShell();

            UsersRepository = userRepository;
            TasksRepository = taskRepository;
        }
    }
}
```

## Étape 6 : Création des interfaces utilisateur


Créer la page pour lister les utilisateurs :

Créez un fichier UserListPage.xaml dans le dossier Views.
```  XML
<!-- UserListPage.xaml -->
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiApp1.Views.UserListPage">
    <StackLayout>
        <ListView x:Name="UserListView" />
        <Button Text="Add User" Clicked="OnAddUserClicked" />
    </StackLayout>
</ContentPage>
```

Implémentez le code-behind UserListPage.xaml.cs.

``` CSharp
// UserListPage.xaml.cs
using Microsoft.Maui.Controls;
using MauiApp1.Models;
using System.Threading.Tasks;

namespace MauiApp1.Views
{
    public partial class UserListPage : ContentPage
    {
        public UserListPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            UserListView.ItemsSource = App.UsersRepository.GetAllUsers();
        }

        private async void OnAddUserClicked(object sender, EventArgs e)
        {
            var addUserPage = new AddUserPage();
            addUserPage.Disappearing += (s, args) => LoadUsers();
            await Navigation.PushModalAsync(addUserPage);
        }
    }
}
```
Créer la page pour ajouter un utilisateur :

Créez un fichier AddUserPage.xaml dans le dossier Views.
```  XML
<!-- AddUserPage.xaml -->
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiApp1.Views.AddUserPage">
    <StackLayout>
        <Entry x:Name="NameEntry" Placeholder="Name" />
        <Entry x:Name="EmailEntry" Placeholder="Email" />
        <Entry x:Name="PostEntry" Placeholder="Post" />
        <Button Text="Save" Clicked="OnSaveClicked" />
    </StackLayout>
</ContentPage>
```
Implémentez le code-behind AddUserPage.xaml.cs.

``` CSharp
// AddUserPage.xaml.cs
using Microsoft.Maui.Controls;
using MauiApp1.Models;

namespace MauiApp1.Views
{
    public partial class AddUserPage : ContentPage
    {
        public AddUserPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var user = new User
            {
                Name = NameEntry.Text,
                Email = EmailEntry.Text,
                Post = PostEntry.Text
            };

            App.UsersRepository.SaveUser(user);
            await Navigation.PopModalAsync();
        }
    }
}
```
## Étape 7 : Création des pages pour les tâches
Créer la page pour lister les tâches :

Créez un fichier TaskListPage.xaml dans le dossier Views.
```  XML
<!-- TaskListPage.xaml -->
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiApp1.Views.TaskListPage">
    <StackLayout>
        <ListView x:Name="TaskListView" />
        <Button Text="Add Task" Clicked="OnAddTaskClicked" />
    </StackLayout>
</ContentPage>
```

Implémentez le code-behind TaskListPage.xaml.cs.
``` CSharp
// TaskListPage.xaml.cs
using Microsoft.Maui.Controls;
using MauiApp1.Models;

namespace MauiApp1.Views
{
    public partial class TaskListPage : ContentPage
    {
        public TaskListPage()
        {
            InitializeComponent();
            LoadTasks();
        }

        private void LoadTasks()
        {
            TaskListView.ItemsSource = App.TasksRepository.GetAllTasks();
        }

        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            var addTaskPage = new AddTaskPage();
            addTaskPage.Disappearing += (s, args) => LoadTasks();
            await Navigation.PushModalAsync(addTaskPage);
        }
    }
}
```
Créer la page pour ajouter une tâche :

Créez un fichier AddTaskPage.xaml dans le dossier Views.
```  XML
<!-- AddTaskPage.xaml -->
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiApp1.Views.AddTaskPage">
    <StackLayout>
        <Picker x:Name="UserPicker" Title="Select User" />
        <Entry x:Name="TitleEntry" Placeholder="Title" />
        <Picker x:Name="StatusPicker" Title="Select Status">
            <Picker.ItemsSource>
                <x:Array Type="{x:Type x:String}">
                    <x:String>Initial</x:String>
                    <x:String>InProgress</x:String>
                    <x:String>Done</x:String>
                </x:Array>
            </Picker.ItemsSource>
        </Picker>
        <Picker x:Name="PriorityPicker" Title="Select Priority">
            <Picker.ItemsSource>
                <x:Array Type="{x:Type x:String}">
                    <x:String>High</x:String>
                    <x:String>Low</x:String>
                    <x:String>Undetermined</x:String>
                </x:Array>
            </Picker.ItemsSource>
        </Picker>
        <Button Text="Save" Clicked="OnSaveClicked" />
    </StackLayout>
</ContentPage>
```
Implémentez le code-behind AddTaskPage.xaml.cs.

``` CSharp
// AddTaskPage.xaml.cs
using Microsoft.Maui.Controls;
using MauiApp1.Models;
using System.Linq;

namespace MauiApp1.Views
{
    public partial class AddTaskPage : ContentPage
    {
        public AddTaskPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            var users = App.UsersRepository.GetAllUsers();
            UserPicker.ItemsSource = users;
            UserPicker.ItemDisplayBinding = new Binding("Name");
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var selectedUser = UserPicker.SelectedItem as User;

            var task = new Task
            {
                UserId = selectedUser.Id,
                Title = TitleEntry.Text,
                Status = (TaskStatus)StatusPicker.SelectedIndex,
                Priority = (TaskPriority)PriorityPicker.SelectedIndex
            };

            App.TasksRepository.SaveTask(task);
            await Navigation.PopModalAsync();
        }
    }
}
```
## Étape 8 : Configuration de AppShell.xaml
Configurer AppShell.xaml pour naviguer entre les pages :
```  XML
<!-- AppShell.xaml -->
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:local="clr-namespace:MauiApp1.Views"
       x:Class="MauiApp1.AppShell">

    <ShellContent Title="Users" ContentTemplate="{DataTemplate local:UserListPage}" />
    <ShellContent Title="Tasks" ContentTemplate="{DataTemplate local:TaskListPage}" />

</Shell>
```
En suivant ces étapes, vous aurez une application MAUI complète avec une interface utilisateur pratique pour ajouter et lister des utilisateurs et des tâches, avec les fonctionnalités CRUD et des données par défaut insérées lors du démarrage de l'application.


# Mise àjour des interfaces

Pour ajouter un gestionnaire d'événements à la page UserListPage afin d'afficher les tâches relatives à l'utilisateur sélectionné, nous allons mettre à jour le fichier UserListPage.xaml et son code-behind. Nous allons également ajuster TaskListPage pour accepter un paramètre d'utilisateur et afficher les tâches correspondantes.

## Étape 9 : Mise à jour de UserListPage.xaml
Modifier UserListPage.xaml pour ajouter un gestionnaire d'événements ItemTapped :
```  XML
<!-- UserListPage.xaml -->
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiApp1.Views.UserListPage">
    <StackLayout>
        <ListView x:Name="UserListView" ItemTapped="OnUserTapped">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <TextCell Text="{Binding Name}" Detail="{Binding Email}" />
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
        <Button Text="Add User" Clicked="OnAddUserClicked" />
    </StackLayout>
</ContentPage>
```
## Étape 10 : Mise à jour du code-behind UserListPage.xaml.cs
Modifier le code-behind pour gérer l'événement ItemTapped et naviguer vers TaskListPage avec l'utilisateur sélectionné :

``` CSharp
// UserListPage.xaml.cs
using Microsoft.Maui.Controls;
using MauiApp1.Models;
using System;
using System.Threading.Tasks;

namespace MauiApp1.Views
{
    public partial class UserListPage : ContentPage
    {
        public UserListPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            UserListView.ItemsSource = App.UsersRepository.GetAllUsers();
        }

        private async void OnAddUserClicked(object sender, EventArgs e)
        {
            var addUserPage = new AddUserPage();
            addUserPage.Disappearing += (s, args) => LoadUsers();
            await Navigation.PushModalAsync(addUserPage);
        }

        private async void OnUserTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is User selectedUser)
            {
                var taskListPage = new TaskListPage(selectedUser);
                await Navigation.PushAsync(taskListPage);
            }
        }
    }
}
```

## Étape 11 : Mise à jour de TaskListPage.xaml.cs

Modifier le code-behind de TaskListPage pour accepter un utilisateur et afficher les tâches correspondantes :

``` CSharp
// TaskListPage.xaml.cs
using Microsoft.Maui.Controls;
using MauiApp1.Models;
using System.Collections.Generic;

namespace MauiApp1.Views
{
    public partial class TaskListPage : ContentPage
    {
        private User _selectedUser;

        public TaskListPage(User selectedUser)
        {
            InitializeComponent();
            _selectedUser = selectedUser;
            Title = $"{selectedUser.Name}'s Tasks";
            LoadTasks();
        }

        private void LoadTasks()
        {
            TaskListView.ItemsSource = App.TasksRepository.GetTasksByUser(_selectedUser.Id);
        }

        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            var addTaskPage = new AddTaskPage(_selectedUser);
            addTaskPage.Disappearing += (s, args) => LoadTasks();
            await Navigation.PushModalAsync(addTaskPage);
        }
    }
}
```
## Étape 12: Mise à jour de AddTaskPage.xaml.cs
Modifier le code-behind de AddTaskPage pour accepter un utilisateur et associer les tâches à l'utilisateur :

``` CSharp
// AddTaskPage.xaml.cs
using Microsoft.Maui.Controls;
using MauiApp1.Models;
using System.Linq;

namespace MauiApp1.Views
{
    public partial class AddTaskPage : ContentPage
    {
        private User _selectedUser;

        public AddTaskPage(User selectedUser)
        {
            InitializeComponent();
            _selectedUser = selectedUser;
            LoadUsers();
        }

        private void LoadUsers()
        {
            var users = App.UsersRepository.GetAllUsers();
            UserPicker.ItemsSource = users;
            UserPicker.ItemDisplayBinding = new Binding("Name");
            UserPicker.SelectedItem = _selectedUser; // Set the selected user
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var task = new Task
            {
                UserId = _selectedUser.Id,
                Title = TitleEntry.Text,
                Status = (TaskStatus)StatusPicker.SelectedIndex,
                Priority = (TaskPriority)PriorityPicker.SelectedIndex
            };

            App.TasksRepository.SaveTask(task);
            await Navigation.PopModalAsync();
        }
    }
}
```

## Étape 13 : Mise à jour de AppShell.xaml
Vérifiez que AppShell.xaml est correctement configuré pour la navigation entre les pages :
```  XML
<!-- AppShell.xaml -->
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:local="clr-namespace:MauiApp1.Views"
       x:Class="MauiApp1.AppShell">

    <ShellContent Title="Users" ContentTemplate="{DataTemplate local:UserListPage}" />
    <ShellContent Title="Tasks" ContentTemplate="{DataTemplate local:TaskListPage}" />

</Shell>
```
Avec ces modifications, vous pouvez maintenant sélectionner un utilisateur dans UserListPage pour afficher les tâches relatives à cet utilisateur dans TaskListPage. Les nouvelles tâches ajoutées seront associées à l'utilisateur sélectionné.



