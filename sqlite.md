#  **sqlite.md — Application MAUI SQLite complète**


##  Étape 1 : Créer le projet

- Créer un projet MAUI nommé `TasksManager`.
- Installer le package NuGet :

```

sqlite-net-pcl

````

---

##  Étape 2 : Créer les modèles

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

### **Models/Task.cs**

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

public class Task
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
    public User GetUser(int id) => db.Table<User>().FirstOrDefault(u => u.Id == id);
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
        db.CreateTable<Task>();
    }

    public List<Task> GetTasksByUser(int userId) => db.Table<Task>().Where(t => t.UserId == userId).ToList();
    public Task GetTask(int id) => db.Table<Task>().FirstOrDefault(t => t.Id == id);
    public int SaveTask(Task task) => db.Insert(task);
    public int DeleteTask(Task task) => db.Delete(task);
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

##  Étape 5 : Définir le Shell

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

### **Views/UserListPage.xaml**

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

### **Views/UserListPage.xaml.cs**

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

##  Étape 7 : Liste des tâches d’un utilisateur

### **Views/TaskListPage.xaml**

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="TasksManager.Views.TaskListPage">

    <StackLayout>
        <Label x:Name="UserLabel" FontSize="20" Padding="10"/>
        <ListView x:Name="TaskListView" />
        <Button Text="Add Task" Clicked="OnAddTaskClicked" />
    </StackLayout>
</ContentPage>
```

---

### **Views/TaskListPage.xaml.cs**

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

##  Étape 8 : Ajout d’un utilisateur

### **Views/AddUserPage.xaml**

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

### **Views/AddUserPage.xaml.cs**

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

##  Étape 9 : Ajout d’une tâche

### **Views/AddTaskPage.xaml**

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

### **Views/AddTaskPage.xaml.cs**

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
        var task = new Task
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
