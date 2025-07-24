## 🌐 1. Backend REST API (.NET 8 Web API)

### 📁 Structure :

```
TaskApi/
├── Controllers/
│   └── TasksController.cs
├── Models/
│   └── TaskItem.cs
├── Data/
│   └── AppDbContext.cs
├── Program.cs
├── appsettings.json
```

### 📦 Packages NuGet à installer :

```bash
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

### 📄 TaskItem.cs

```csharp
namespace TaskApi.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public bool IsDone { get; set; }
}
```

### 📄 AppDbContext.cs

```csharp
using Microsoft.EntityFrameworkCore;
using TaskApi.Models;

namespace TaskApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
}
```

### 📄 TasksController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using TaskApi.Data;
using TaskApi.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;
    public TasksController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll() => await _context.Tasks.ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Create(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = task.Id }, task);
    }
}
```

### 📄 Program.cs

```csharp
using Microsoft.EntityFrameworkCore;
using TaskApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TasksDb"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

---

## 📱 2. MAUI App – Sans MVVM

### 📁 Structure :

```
MauiApp/
├── Models/TaskItem.cs
├── Services/ITaskRepository.cs
├── Services/TaskRepository.cs
├── MainPage.xaml / .cs
├── App.xaml.cs
├── MauiProgram.cs
```

### 📦 Packages NuGet :

```bash
dotnet add package sqlite-net-pcl
dotnet add package Microsoft.Extensions.Http
```

### 📄 Models/TaskItem.cs

```csharp
using SQLite;

namespace MauiApp.Models;

public class TaskItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsDone { get; set; }
}
```

### 📄 Services/ITaskRepository.cs

```csharp
using MauiApp.Models;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllAsync();
    Task AddAsync(TaskItem task);
}
```

### 📄 Services/TaskRepository.cs

```csharp
using SQLite;
using MauiApp.Models;
using System.Net.Http.Json;

public class TaskRepository : ITaskRepository
{
    private readonly SQLiteAsyncConnection _db;
    private readonly HttpClient _http;
    private readonly string _apiUrl = "http://10.0.2.2:5000/api/tasks"; // Android localhost

    public TaskRepository()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "tasks.db");
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<TaskItem>().Wait();

        _http = new HttpClient();
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        try { return await _http.GetFromJsonAsync<List<TaskItem>>(_apiUrl); }
        catch { return await _db.Table<TaskItem>().ToListAsync(); }
    }

    public async Task AddAsync(TaskItem task)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(_apiUrl, task);
            if (!response.IsSuccessStatusCode)
                await _db.InsertAsync(task);
        }
        catch
        {
            await _db.InsertAsync(task);
        }
    }
}
```

### 📄 MainPage.xaml

```xml
<VerticalStackLayout Padding="30">
    <Entry x:Name="titleEntry" Placeholder="Titre de la tâche"/>
    <Button Text="Ajouter" Clicked="OnAddClicked"/>
    <CollectionView x:Name="taskList">
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <Label Text="{Binding Title}" />
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</VerticalStackLayout>
```

### 📄 MainPage.xaml.cs

```csharp
using MauiApp.Models;
using MauiApp.Services;

namespace MauiApp;

public partial class MainPage : ContentPage
{
    private readonly ITaskRepository _repo;

    public MainPage(ITaskRepository repo)
    {
        InitializeComponent();
        _repo = repo;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        taskList.ItemsSource = await _repo.GetAllAsync();
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        var task = new TaskItem { Title = titleEntry.Text };
        await _repo.AddAsync(task);
        titleEntry.Text = "";
        taskList.ItemsSource = await _repo.GetAllAsync();
    }
}
```

### 📄 MauiProgram.cs

```csharp
using MauiApp.Services;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();
        builder.Services.AddSingleton<ITaskRepository, TaskRepository>();
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
    }
}
```

---

## 📱 3. MAUI App – Avec MVVM

Ajoute :

### 📁 Structure additionnelle :

```
MauiApp/
├── ViewModels/TaskViewModel.cs
├── Views/MainPage.xaml / .cs
```

### 📄 ViewModels/TaskViewModel.cs

```csharp
using System.Collections.ObjectModel;
using System.Windows.Input;
using MauiApp.Models;
using MauiApp.Services;

public class TaskViewModel : BindableObject
{
    private readonly ITaskRepository _repo;

    public ObservableCollection<TaskItem> Tasks { get; } = new();
    public string NewTitle { get; set; } = "";
    public ICommand AddCommand { get; }

    public TaskViewModel(ITaskRepository repo)
    {
        _repo = repo;
        AddCommand = new Command(async () => await AddTask());
        LoadTasks();
    }

    private async Task AddTask()
    {
        var task = new TaskItem { Title = NewTitle };
        await _repo.AddAsync(task);
        NewTitle = "";
        Tasks.Clear();
        foreach (var t in await _repo.GetAllAsync()) Tasks.Add(t);
        OnPropertyChanged(nameof(NewTitle));
    }

    private async void LoadTasks()
    {
        var list = await _repo.GetAllAsync();
        Tasks.Clear();
        foreach (var t in list) Tasks.Add(t);
    }
}
```

### 📄 MainPage.xaml (MVVM)

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             x:Class="MauiApp.Views.MainPage"
             xmlns:vm="clr-namespace:MauiApp.ViewModels">
    <ContentPage.BindingContext>
        <vm:TaskViewModel />
    </ContentPage.BindingContext>

    <VerticalStackLayout Padding="30">
        <Entry Text="{Binding NewTitle}" Placeholder="Titre"/>
        <Button Text="Ajouter" Command="{Binding AddCommand}"/>
        <CollectionView ItemsSource="{Binding Tasks}">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <Label Text="{Binding Title}" />
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </VerticalStackLayout>
</ContentPage>
```

### 📄 MauiProgram.cs (version MVVM)

```csharp
builder.Services.AddSingleton<ITaskRepository, TaskRepository>();
builder.Services.AddSingleton<TaskViewModel>();
builder.Services.AddSingleton<MainPage>();
```


