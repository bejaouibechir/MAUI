#  **sqlite.md — Application MAUI SQLite complète (version finale)**


#  Application MAUI de gestion des utilisateurs et tâches avec SQLite

---

##  Objectif

- Créer une application .NET MAUI complète qui gère des **utilisateurs** et leurs **tâches**, en utilisant SQLite pour le stockage local.
- Offrir une **expérience utilisateur complète**, avec navigation avant/arrière et rechargement automatique.

---

##  Structure du projet

``` markdown 
TasksManager/
│
├── App.xaml
├── App.xaml.cs
├── AppShell.xaml
├── AppShell.xaml.cs
├── MauiProgram.cs
│
├── Models/
│   ├── User.cs
│   └── UserTask.cs
│
├── Data/
│   ├── UserRepository.cs
│   └── TaskRepository.cs
│
├── Views/
│   ├── UserListPage.xaml
│   ├── UserListPage.xaml.cs
│   ├── AddUserPage.xaml
│   ├── AddUserPage.xaml.cs
│   ├── TaskListPage.xaml
│   ├── TaskListPage.xaml.cs
│   ├── AddTaskPage.xaml
│   └── AddTaskPage.xaml.cs
│
└── Resources/
└── Fonts/

```

---

## Étape 1 : Créer le projet

- Créer un projet MAUI nommé `TasksManager`.
- Installer le package NuGet :

```

sqlite-net-pcl

````

---

## 💬 Étape 2 : Créer les modèles

### **Models/User.cs**

```csharp
using SQLite;

namespace TasksManager.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Post { get; set; }

    public override string ToString() => $"{Name} ({Email})";
}
````

---

### **Models/UserTask.cs**

```csharp
using SQLite;

namespace TasksManager.Models;

public enum TaskStatus
{
    Initial,
    InProgress,
    Done
}

public enum TaskPriority
{
    Low,
    Normal,
    High
}

public class UserTask
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
}
```

---

##  Étape 3 : Créer les repositories

### **Data/UserRepository.cs**

```csharp
using SQLite;
using TasksManager.Models;

namespace TasksManager.Data;

public class UserRepository
{
    private SQLiteConnection db;

    public UserRepository(string dbPath)
    {
        db = new SQLiteConnection(dbPath);
        db.CreateTable<User>();
    }

    public List<User> GetAllUsers() => db.Table<User>().ToList();
    public int SaveUser(User user) => db.Insert(user);
    public int DeleteUser(User user) => db.Delete(user);
}
```

---

### **Data/TaskRepository.cs**

```csharp
using SQLite;
using TasksManager.Models;

namespace TasksManager.Data;

public class TaskRepository
{
    private SQLiteConnection db;

    public TaskRepository(string dbPath)
    {
        db = new SQLiteConnection(dbPath);
        db.CreateTable<UserTask>();
    }

    public List<UserTask> GetTasksByUser(int userId) => db.Table<UserTask>().Where(t => t.UserId == userId).ToList();
    public int SaveTask(UserTask task) => db.Insert(task);
    public int DeleteTask(UserTask task) => db.Delete(task);
}
```

---

##  Étape 4 : Configurer MauiProgram.cs

### **MauiProgram.cs**

```csharp
using TasksManager.Data;

namespace TasksManager;

public static class MauiProgram
{
    public static UserRepository UsersRepository { get; private set; }
    public static TaskRepository TasksRepository { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db3");
        UsersRepository = new UserRepository(dbPath);
        TasksRepository = new TaskRepository(dbPath);

        return builder.Build();
    }
}
```

---

##  Étape 5 : Shell

### **AppShell.xaml**

```xml
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:views="clr-namespace:TasksManager.Views"
       x:Class="TasksManager.AppShell">

    <ShellContent Title="Users" ContentTemplate="{DataTemplate views:UserListPage}" />
</Shell>
```

---

##  Étape 6 : Liste des utilisateurs

### **UserListPage.xaml**

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="TasksManager.Views.UserListPage"
             Title="Users">

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

---

### **UserListPage.xaml.cs**

```csharp
using TasksManager.Models;

namespace TasksManager.Views;

public partial class UserListPage : ContentPage
{
    public UserListPage()
    {
        InitializeComponent();
        LoadUsers();
    }

    private void LoadUsers()
    {
        UserListView.ItemsSource = MauiProgram.UsersRepository.GetAllUsers();
    }

    private async void OnAddUserClicked(object sender, EventArgs e)
    {
        var page = new AddUserPage();
        page.Disappearing += (s, args) => LoadUsers();
        await Navigation.PushModalAsync(page);
    }

    private async void OnUserTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is User user)
        {
            var page = new TaskListPage(user);
            await Navigation.PushAsync(page);
        }
    }
}
```

---

## Étape 7 : Liste des tâches d’un utilisateur

### **TaskListPage.xaml**

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="TasksManager.Views.TaskListPage"
             Title="Tasks">

    <StackLayout>
        <Label x:Name="UserLabel" FontSize="20" Padding="10"/>
        <ListView x:Name="TaskListView">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <TextCell Text="{Binding Title}" Detail="{Binding Status}" />
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
        <Button Text="Add Task" Clicked="OnAddTaskClicked" />
    </StackLayout>
</ContentPage>
```

---

### **TaskListPage.xaml.cs**

```csharp
using TasksManager.Models;

namespace TasksManager.Views;

public partial class TaskListPage : ContentPage
{
    private User _user;

    public TaskListPage(User user)
    {
        InitializeComponent();
        _user = user;
        Title = $"{user.Name}'s Tasks";
        UserLabel.Text = $"Tasks for {user.Name}";
        LoadTasks();
    }

    private void LoadTasks()
    {
        TaskListView.ItemsSource = MauiProgram.TasksRepository.GetTasksByUser(_user.Id);
    }

    private async void OnAddTaskClicked(object sender, EventArgs e)
    {
        var page = new AddTaskPage(_user);
        page.Disappearing += (s, args) => LoadTasks();
        await Navigation.PushModalAsync(page);
    }
}
```

---

##  Étape 8 : Ajouter un utilisateur

### **AddUserPage.xaml**

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="TasksManager.Views.AddUserPage"
             Title="Add User">

    <StackLayout Padding="10">
        <Entry x:Name="NameEntry" Placeholder="Name" />
        <Entry x:Name="EmailEntry" Placeholder="Email" />
        <Entry x:Name="PostEntry" Placeholder="Post" />
        <Button Text="Save" Clicked="OnSaveClicked" />
    </StackLayout>
</ContentPage>
```

---

### **AddUserPage.xaml.cs**

```csharp
using TasksManager.Models;

namespace TasksManager.Views;

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

        MauiProgram.UsersRepository.SaveUser(user);
        await Navigation.PopModalAsync();
    }
}
```

---

## Étape 9 : Ajouter une tâche

### **AddTaskPage.xaml**

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="TasksManager.Views.AddTaskPage"
             Title="Add Task">

    <StackLayout Padding="10">
        <Entry x:Name="TitleEntry" Placeholder="Title" />
        <Picker x:Name="StatusPicker" Title="Status">
            <Picker.ItemsSource>
                <x:Array Type="{x:Type x:String}">
                    <x:String>Initial</x:String>
                    <x:String>InProgress</x:String>
                    <x:String>Done</x:String>
                </x:Array>
            </Picker.ItemsSource>
        </Picker>
        <Picker x:Name="PriorityPicker" Title="Priority">
            <Picker.ItemsSource>
                <x:Array Type="{x:Type x:String}">
                    <x:String>Low</x:String>
                    <x:String>Normal</x:String>
                    <x:String>High</x:String>
                </x:Array>
            </Picker.ItemsSource>
        </Picker>
        <Button Text="Save" Clicked="OnSaveClicked" />
    </StackLayout>
</ContentPage>
```

---

### **AddTaskPage.xaml.cs**

```csharp
using TasksManager.Models;

namespace TasksManager.Views;

public partial class AddTaskPage : ContentPage
{
    private User _user;

    public AddTaskPage(User user)
    {
        InitializeComponent();
        _user = user;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var task = new UserTask
        {
            UserId = _user.Id,
            Title = TitleEntry.Text,
            Status = (TaskStatus)StatusPicker.SelectedIndex,
            Priority = (TaskPriority)PriorityPicker.SelectedIndex
        };

        MauiProgram.TasksRepository.SaveTask(task);
        await Navigation.PopModalAsync();
    }
}
```

---
