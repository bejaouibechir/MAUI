🔥 💯 Merci pour ta précision ! On va tout reprendre **de A à Z**, étape par étape, sans raccourci.
Voici **une version finale, complète, intégrale**, prête à être directement utilisée comme tutoriel ou README pour ton repo GitHub.

---

# 💥 ✅ **Solution complète WebAPI + MAUI (TaskManager)**

---

# 🌐 **PARTIE 1 — WebAPI (.NET 7 ou 8)**

---

## ✅ 1️⃣ Ajout des bibliothèques

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Swashbuckle.AspNetCore
```

---

## ✅ 2️⃣ Ajout des modèles

### **Models/User.cs**

```csharp
namespace MauApp2.WebApi.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Post { get; set; }
}
```

---

### **Models/UserTask.cs**

```csharp
namespace MauApp2.WebApi.Models;

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
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
}
```

---

## ✅ 3️⃣ Ajout du DbContext

### **Data/AppDbContext.cs**

```csharp
using Microsoft.EntityFrameworkCore;
using MauApp2.WebApi.Models;

namespace MauApp2.WebApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();
    public DbSet<UserTask> Tasks => Set<UserTask>();
}
```

---

## ✅ 4️⃣ Ajout des contrôleurs

### **Controllers/UsersController.cs**

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MauApp2.WebApi.Data;
using MauApp2.WebApi.Models;

namespace MauApp2.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> Get() => await _context.Users.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return user ?? NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<User>> Post(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
```

---

### **Controllers/TasksController.cs**

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MauApp2.WebApi.Data;
using MauApp2.WebApi.Models;

namespace MauApp2.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("user/{userId}")]
    public async Task<IEnumerable<UserTask>> GetByUser(int userId)
        => await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();

    [HttpPost]
    public async Task<ActionResult<UserTask>> Post(UserTask task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Post), new { id = task.Id }, task);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
```

---

## ✅ 5️⃣ Collection Postman (JSON)

Voici une **version finale** de la collection, directement intégrable et prête à importer dans Postman.

```json
{
	"info": {
		"_postman_id": "3588155a-91fd-47d1-9b55-2ad749c7bd4e",
		"name": "TaskManagerCollection",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
	},
	"item": [
		{
			"name": "Users",
			"item": [
				{
					"name": "Get all users",
					"request": {
						"method": "GET",
						"header": [],
						"url": {
							"raw": "http://localhost:5163/api/users",
							"protocol": "http",
							"host": ["localhost"],
							"port": "5163",
							"path": ["api", "users"]
						}
					}
				},
				{
					"name": "Get user by ID",
					"request": {
						"method": "GET",
						"header": [],
						"url": {
							"raw": "http://localhost:5163/api/users/1",
							"protocol": "http",
							"host": ["localhost"],
							"port": "5163",
							"path": ["api", "users", "1"]
						}
					}
				},
				{
					"name": "Add user",
					"request": {
						"method": "POST",
						"header": [
							{ "key": "Content-Type", "value": "application/json" }
						],
						"body": {
							"mode": "raw",
							"raw": "{\n  \"name\": \"Sophie Dubois\",\n  \"email\": \"sophie.dubois@example.com\",\n  \"post\": \"Product Owner\"\n}",
							"options": { "raw": { "language": "json" } }
						},
						"url": {
							"raw": "http://localhost:5163/api/users",
							"protocol": "http",
							"host": ["localhost"],
							"port": "5163",
							"path": ["api", "users"]
						}
					}
				},
				{
					"name": "Delete user",
					"request": {
						"method": "DELETE",
						"header": [],
						"url": {
							"raw": "http://localhost:5163/api/users/1",
							"protocol": "http",
							"host": ["localhost"],
							"port": "5163",
							"path": ["api", "users", "1"]
						}
					}
				}
			]
		},
		{
			"name": "Tasks",
			"item": [
				{
					"name": "Get tasks by user ID",
					"request": {
						"method": "GET",
						"header": [],
						"url": {
							"raw": "http://localhost:5163/api/tasks/user/1",
							"protocol": "http",
							"host": ["localhost"],
							"port": "5163",
							"path": ["api", "tasks", "user", "1"]
						}
					}
				},
				{
					"name": "Add task",
					"request": {
						"method": "POST",
						"header": [
							{ "key": "Content-Type", "value": "application/json" }
						],
						"body": {
							"mode": "raw",
							"raw": "{\n  \"userId\": 1,\n  \"title\": \"Préparer le rapport trimestriel\",\n  \"status\": 0,\n  \"priority\": 2\n}",
							"options": { "raw": { "language": "json" } }
						},
						"url": {
							"raw": "http://localhost:5163/api/tasks",
							"protocol": "http",
							"host": ["localhost"],
							"port": "5163",
							"path": ["api", "tasks"]
						}
					}
				},
				{
					"name": "Delete task",
					"request": {
						"method": "DELETE",
						"header": [],
						"url": {
							"raw": "http://localhost:5163/api/tasks/1",
							"protocol": "http",
							"host": ["localhost"],
							"port": "5163",
							"path": ["api", "tasks", "1"]
						}
					}
				}
			]
		}
	]
}
```

---

## 💬 **Comment l'utiliser ?**

✅ Copie tout le contenu du JSON ci-dessus.
✅ Va dans **Postman → Import → Raw text → Colle → Importer**.
✅ Les requêtes sont prêtes !

---

### 🚀 **Si tu veux, je peux aussi te générer directement un fichier `.json` à télécharger (par exemple `TaskManagerCollection.postman_collection.json`). Tu veux ? 💪🔥**


---

# 📱 **PARTIE 2 — Application MAUI**

---

## ✅ 1️⃣ DTO

### **DTO/UserDto.cs**

```csharp
namespace MauiApp2.DTO;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Post { get; set; }
}
```

---

### **DTO/UserTaskDto.cs**

```csharp
namespace MauiApp2.DTO;

public enum TaskStatus { Initial, InProgress, Done }
public enum TaskPriority { Low, Normal, High }

public class UserTaskDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
}
```

---

## ✅ 2️⃣ Services

### **Services/UserService.cs**

```csharp
using MauiApp2.DTO;
using System.Net.Http.Json;

namespace MauiApp2.Services;

public class UserService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:5163/api/users";

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserDto>> GetAllAsync()
        => await _httpClient.GetFromJsonAsync<List<UserDto>>(BaseUrl) ?? new List<UserDto>();

    public async Task<UserDto?> GetByIdAsync(int id)
        => await _httpClient.GetFromJsonAsync<UserDto>($"{BaseUrl}/{id}");

    public async Task AddAsync(UserDto user)
        => await _httpClient.PostAsJsonAsync(BaseUrl, user);

    public async Task DeleteAsync(int id)
        => await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
}
```

---

### **Services/TaskService.cs**

```csharp
using MauiApp2.DTO;
using System.Net.Http.Json;

namespace MauiApp2.Services;

public class TaskService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:5163/api/tasks";

    public TaskService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserTaskDto>> GetTasksByUserAsync(int userId)
        => await _httpClient.GetFromJsonAsync<List<UserTaskDto>>($"{BaseUrl}/user/{userId}") ?? new List<UserTaskDto>();

    public async Task AddAsync(UserTaskDto task)
        => await _httpClient.PostAsJsonAsync(BaseUrl, task);

    public async Task DeleteAsync(int id)
        => await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
}
```

---

## ✅ 3️⃣ Injections

### **MauiProgram.cs**

```csharp
builder.Services.AddHttpClient<UserService>();
builder.Services.AddHttpClient<TaskService>();

builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<TaskRepository>();
```

---

## ✅ 4️⃣ Repositories

### **Repositories/UserRepository.cs**

```csharp
using MauiApp2.DTO;
using MauiApp2.Services;

namespace MauiApp2.Repositories;

public class UserRepository
{
    private readonly UserService _service;

    public UserRepository(UserService service)
    {
        _service = service;
    }

    public Task<List<UserDto>> GetAll() => _service.GetAllAsync();
    public Task<UserDto?> GetById(int id) => _service.GetByIdAsync(id);
    public Task Add(UserDto user) => _service.AddAsync(user);
    public Task Delete(int id) => _service.DeleteAsync(id);
}
```

---

### **Repositories/TaskRepository.cs**

```csharp
using MauiApp2.DTO;
using MauiApp2.Services;

namespace MauiApp2.Repositories;

public class TaskRepository
{
    private readonly TaskService _service;

    public TaskRepository(TaskService service)
    {
        _service = service;
    }

    public Task<List<UserTaskDto>> GetTasksByUser(int userId) => _service.GetTasksByUserAsync(userId);
    public Task Add(UserTaskDto task) => _service.AddAsync(task);
    public Task Delete(int id) => _service.DeleteAsync(id);
}
```

---

## ✅ 5️⃣ Intégration dans les ContentPages (via injection)

### **Exemple UserListPage.xaml.cs**

```csharp
public partial class UserListPage : ContentPage
{
    private readonly UserRepository _repo;

    public UserListPage(UserRepository repo)
    {
        InitializeComponent();
        _repo = repo;
        LoadUsers();
    }

    private async void LoadUsers()
    {
        var users = await _repo.GetAll();
        UserListView.ItemsSource = users;
    }

    private async void OnAddUserClicked(object sender, EventArgs e)
    {
        var newUser = new UserDto
        {
            Name = "Nouveau user",
            Email = "new@example.com",
            Post = "Developer"
        };
        await _repo.Add(newUser);
        await LoadUsers();
    }
}
```

---

## ✅ **Pas besoin de modifications XAML ou AppShell.xaml**

* Navigation et structure restent identiques.
* L’injection se fait automatiquement.



