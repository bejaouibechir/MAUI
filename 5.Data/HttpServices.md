

## Classe complète : `HttpPersonService.cs`

```csharp
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MyCrudApp.Models;

namespace MyCrudApp.Services;

public class HttpPersonService : IHttpPersonService
{
    private readonly HttpClient _http;
    private const string BaseUrl = "https://localhost:7098/api/person"; // ⚠️ à adapter selon l'URL de votre API

    public HttpPersonService(HttpClient httpClient)
    {
        _http = httpClient;
    }

    public async Task<List<Person>> GetAllAsync()
    {
        try
        {
            var response = await _http.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();

            var people = await response.Content.ReadFromJsonAsync<List<Person>>();
            return people ?? new List<Person>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HttpPersonService] GET ERROR: {ex.Message}");
            return new List<Person>();
        }
    }

    public async Task<Person?> AddAsync(Person person)
    {
        try
        {
            var json = JsonSerializer.Serialize(person);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(BaseUrl, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Person>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HttpPersonService] POST ERROR: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> UpdateAsync(Person person)
    {
        try
        {
            var json = JsonSerializer.Serialize(person);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PutAsync($"{BaseUrl}/{person.Id}", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HttpPersonService] PUT ERROR: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HttpPersonService] DELETE ERROR: {ex.Message}");
            return false;
        }
    }
}
```

---

##  Rappel : Interface associée `IHttpPersonService.cs`

```csharp
using MyCrudApp.Models;

namespace MyCrudApp.Services;

public interface IHttpPersonService
{
    Task<List<Person>> GetAllAsync();
    Task<Person?> AddAsync(Person person);
    Task<bool> UpdateAsync(Person person);
    Task<bool> DeleteAsync(int id);
}
```

---

##  Injection dans `MauiProgram.cs`

```csharp
builder.Services.AddHttpClient<IHttpPersonService, HttpPersonService>();
```

---

Voici la version complète de la classe `HttpTaskService` pour gérer un modèle `UserTask` via REST, avec toutes les opérations CRUD et une interface associée.

---

##  Exemple de modèle `UserTask`

```csharp
namespace MyCrudApp.Models;

public class UserTask
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public bool IsCompleted { get; set; }
}
```

---

##  Interface `IHttpTaskService.cs`

```csharp
using MyCrudApp.Models;

namespace MyCrudApp.Services;

public interface IHttpTaskService
{
    Task<List<UserTask>> GetAllAsync();
    Task<UserTask?> GetByIdAsync(int id);
    Task<UserTask?> AddAsync(UserTask task);
    Task<bool> UpdateAsync(UserTask task);
    Task<bool> DeleteAsync(int id);
}
```

---

##  Classe complète `HttpTaskService.cs`

```csharp
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MyCrudApp.Models;

namespace MyCrudApp.Services;

public class HttpTaskService : IHttpTaskService
{
    private readonly HttpClient _http;
    private const string BaseUrl = "https://localhost:7098/api/usertask"; // à adapter

    public HttpTaskService(HttpClient httpClient)
    {
        _http = httpClient;
    }

    public async Task<List<UserTask>> GetAllAsync()
    {
        try
        {
            var response = await _http.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();

            var tasks = await response.Content.ReadFromJsonAsync<List<UserTask>>();
            return tasks ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HttpTaskService] GET ALL ERROR: {ex.Message}");
            return new();
        }
    }

    public async Task<UserTask?> GetByIdAsync(int id)
    {
        try
        {
            var response = await _http.GetAsync($"{BaseUrl}/{id}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<UserTask>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HttpTaskService] GET ID ERROR: {ex.Message}");
            return null;
        }
    }

    public async Task<UserTask?> AddAsync(UserTask task)
    {
        try
        {
            var json = JsonSerializer.Serialize(task);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(BaseUrl, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserTask>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HttpTaskService] POST ERROR: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> UpdateAsync(UserTask task)
    {
        try
        {
            var json = JsonSerializer.Serialize(task);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PutAsync($"{BaseUrl}/{task.Id}", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HttpTaskService] PUT ERROR: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HttpTaskService] DELETE ERROR: {ex.Message}");
            return false;
        }
    }
}
```

---

## 🧩 Injection dans `MauiProgram.cs`

```csharp
builder.Services.AddHttpClient<IHttpTaskService, HttpTaskService>();
```
---


 

 


