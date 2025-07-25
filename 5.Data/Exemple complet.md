## 🧱 Étape 1 : CRUD SQLite Classique (Sans Repository, Sans MVVM)

### 📁 Structure initiale du projet

```
MyCrudApp/
│
├── Models/
│   └── Person.cs
│
├── Data/
│   └── PersonDatabase.cs
│
├── MainPage.xaml
├── MainPage.xaml.cs
├── App.xaml.cs
├── MauiProgram.cs
```

### 📦 Packages NuGet à installer

```bash
dotnet add package sqlite-net-pcl
```

### 📄 Models/Person.cs

```csharp
using SQLite;

namespace MyCrudApp.Models;

public class Person
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Age { get; set; }
}
```

### 📄 Data/PersonDatabase.cs

```csharp
using SQLite;
using MyCrudApp.Models;

namespace MyCrudApp.Data;

public class PersonDatabase
{
    readonly SQLiteAsyncConnection _database;

    public PersonDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Person>().Wait();
    }

    public Task<List<Person>> GetPeopleAsync() => _database.Table<Person>().ToListAsync();
    public Task<int> SavePersonAsync(Person person) => person.Id == 0 ? _database.InsertAsync(person) : _database.UpdateAsync(person);
    public Task<int> DeletePersonAsync(Person person) => _database.DeleteAsync(person);
}
```

### 📄 MauiProgram.cs

```csharp
using MyCrudApp.Data;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "people.db3");
        builder.Services.AddSingleton(new PersonDatabase(dbPath));

        return builder.Build();
    }
}
```

### 📄 MainPage.xaml

```xml
<VerticalStackLayout Padding="20">
    <Entry x:Name="NameEntry" Placeholder="Nom" />
    <Entry x:Name="AgeEntry" Placeholder="Âge" Keyboard="Numeric" />
    <Button Text="Ajouter" Clicked="OnAddClicked"/>
    <CollectionView x:Name="PeopleList">
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <SwipeView>
                    <SwipeView.RightItems>
                        <SwipeItems>
                            <SwipeItem Text="Supprimer" Invoked="OnDelete" />
                        </SwipeItems>
                    </SwipeView.RightItems>
                    <Grid Padding="10">
                        <Label Text="{Binding Name}" FontAttributes="Bold" />
                        <Label Text="{Binding Age}" />
                    </Grid>
                </SwipeView>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</VerticalStackLayout>
```

### 📄 MainPage.xaml.cs

```csharp
using MyCrudApp.Models;
using MyCrudApp.Data;

namespace MyCrudApp;

public partial class MainPage : ContentPage
{
    PersonDatabase _database;

    public MainPage(PersonDatabase database)
    {
        InitializeComponent();
        _database = database;
        LoadPeople();
    }

    async void LoadPeople()
    {
        PeopleList.ItemsSource = await _database.GetPeopleAsync();
    }

    async void OnAddClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(NameEntry.Text) && int.TryParse(AgeEntry.Text, out int age))
        {
            await _database.SavePersonAsync(new Person { Name = NameEntry.Text, Age = age });
            NameEntry.Text = "";
            AgeEntry.Text = "";
            LoadPeople();
        }
    }

    async void OnDelete(object sender, EventArgs e)
    {
        var person = (sender as SwipeItem).BindingContext as Person;
        if (person != null)
        {
            await _database.DeletePersonAsync(person);
            LoadPeople();
        }
    }
}
```

---

## 🔁 Étape 2 : Refactor en Repository Pattern

### 📁 Structure

```
MyCrudApp/
│
├── Models/
├── Data/
│   └── IPersonRepository.cs
│   └── PersonRepository.cs
│   └── PersonDatabase.cs
```

### 📄 Data/IPersonRepository.cs

```csharp
using MyCrudApp.Models;

namespace MyCrudApp.Data;

public interface IPersonRepository
{
    Task<List<Person>> GetAllAsync();
    Task SaveAsync(Person person);
    Task DeleteAsync(Person person);
}
```

### 📄 Data/PersonRepository.cs

```csharp
using MyCrudApp.Models;

namespace MyCrudApp.Data;

public class PersonRepository : IPersonRepository
{
    private readonly PersonDatabase _db;

    public PersonRepository(PersonDatabase db)
    {
        _db = db;
    }

    public Task<List<Person>> GetAllAsync() => _db.GetPeopleAsync();
    public Task SaveAsync(Person person) => _db.SavePersonAsync(person);
    public Task DeleteAsync(Person person) => _db.DeletePersonAsync(person);
}
```

### 📄 MauiProgram.cs (ajout)

```csharp
builder.Services.AddSingleton<IPersonRepository, PersonRepository>();
```

### 📄 MainPage.xaml.cs (modif)

```csharp
private readonly IPersonRepository _repo;

public MainPage(IPersonRepository repo)
{
    InitializeComponent();
    _repo = repo;
    LoadPeople();
}

async void LoadPeople()
{
    PeopleList.ItemsSource = await _repo.GetAllAsync();
}

async void OnAddClicked(object sender, EventArgs e)
{
    if (!string.IsNullOrWhiteSpace(NameEntry.Text) && int.TryParse(AgeEntry.Text, out int age))
    {
        await _repo.SaveAsync(new Person { Name = NameEntry.Text, Age = age });
        NameEntry.Text = "";
        AgeEntry.Text = "";
        LoadPeople();
    }
}

async void OnDelete(object sender, EventArgs e)
{
    var person = (sender as SwipeItem).BindingContext as Person;
    if (person != null)
    {
        await _repo.DeleteAsync(person);
        LoadPeople();
    }
}
```

---

## 🔄 Étape 3 : Passage MVVM

### 📁 Structure

```
MyCrudApp/
│
├── ViewModels/
│   └── MainViewModel.cs
│
├── Views/
│   └── MainPage.xaml
│   └── MainPage.xaml.cs
```

### 📄 ViewModels/MainViewModel.cs

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyCrudApp.Models;
using MyCrudApp.Data;
using System.Collections.ObjectModel;

namespace MyCrudApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IPersonRepository _repo;

    [ObservableProperty] string name;
    [ObservableProperty] string age;
    [ObservableProperty] ObservableCollection<Person> people;

    public MainViewModel(IPersonRepository repo)
    {
        _repo = repo;
        LoadPeople();
    }

    [RelayCommand]
    async Task LoadPeople()
    {
        var list = await _repo.GetAllAsync();
        People = new ObservableCollection<Person>(list);
    }

    [RelayCommand]
    async Task AddPerson()
    {
        if (!string.IsNullOrWhiteSpace(Name) && int.TryParse(Age, out int ageValue))
        {
            var person = new Person { Name = Name, Age = ageValue };
            await _repo.SaveAsync(person);
            await LoadPeople();
            Name = Age = "";
        }
    }

    [RelayCommand]
    async Task DeletePerson(Person person)
    {
        await _repo.DeleteAsync(person);
        await LoadPeople();
    }
}
```

### 📄 Views/MainPage.xaml (modifié)

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:vm="clr-namespace:MyCrudApp.ViewModels"
             x:Class="MyCrudApp.Views.MainPage">

    <ContentPage.BindingContext>
        <vm:MainViewModel />
    </ContentPage.BindingContext>

    <VerticalStackLayout Padding="20">
        <Entry Placeholder="Nom" Text="{Binding Name}" />
        <Entry Placeholder="Âge" Text="{Binding Age}" Keyboard="Numeric"/>
        <Button Text="Ajouter" Command="{Binding AddPersonCommand}" />
        <CollectionView ItemsSource="{Binding People}">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <SwipeView>
                        <SwipeView.RightItems>
                            <SwipeItems>
                                <SwipeItem Text="Supprimer" 
                                           Command="{Binding BindingContext.DeletePersonCommand, Source={x:Reference Name=MainPageRef}}"
                                           CommandParameter="{Binding .}" />
                            </SwipeItems>
                        </SwipeView.RightItems>
                        <Grid Padding="10">
                            <Label Text="{Binding Name}" FontAttributes="Bold"/>
                            <Label Text="{Binding Age}"/>
                        </Grid>
                    </SwipeView>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </VerticalStackLayout>
</ContentPage>
```

### 📄 MauiProgram.cs (final pour DI)

```csharp
builder.Services.AddSingleton<IPersonRepository, PersonRepository>();
builder.Services.AddSingleton<MainViewModel>();
builder.Services.AddSingleton<MainPage>();
```

# Etape 2 Ajout de API Rest (Deuxième source des données)
---

##  Objectif

Ajouter un service `HttpPersonService` pour consommer une API REST (`GET`, `POST`, `PUT`, `DELETE`) en parallèle de SQLite. L’objectif est de montrer comment injecter et utiliser ce service dans un projet MAUI existant.

---

##  1. Préparation de l’API REST (si besoin)

Si vous n'avez pas encore d'API, voici une suggestion rapide pour la structure du contrôleur ASP.NET Core Web API :

```csharp
[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private static List<Person> _data = new();

    [HttpGet]
    public ActionResult<IEnumerable<Person>> Get() => _data;

    [HttpPost]
    public ActionResult<Person> Create(Person person)
    {
        person.Id = _data.Count + 1;
        _data.Add(person);
        return person;
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Person person)
    {
        var existing = _data.FirstOrDefault(p => p.Id == id);
        if (existing == null) return NotFound();
        existing.Name = person.Name;
        existing.Age = person.Age;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var person = _data.FirstOrDefault(p => p.Id == id);
        if (person == null) return NotFound();
        _data.Remove(person);
        return NoContent();
    }
}
```

---

##  2. Packages à installer côté MAUI

```bash
dotnet add package Microsoft.Extensions.Http
```

---

##  3. Structure mise à jour

```
MyCrudApp/
│
├── Services/
│   └── IHttpPersonService.cs
│   └── HttpPersonService.cs
```

---

##  Services/IHttpPersonService.cs

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

##  Services/HttpPersonService.cs

```csharp
using System.Net.Http.Json;
using MyCrudApp.Models;

namespace MyCrudApp.Services;

public class HttpPersonService : IHttpPersonService
{
    private readonly HttpClient _http;
    private const string BaseUrl = "https://localhost:7098/api/person"; // à adapter

    public HttpPersonService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Person>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<Person>>(BaseUrl) ?? new();
    }

    public async Task<Person?> AddAsync(Person person)
    {
        var res = await _http.PostAsJsonAsync(BaseUrl, person);
        return res.IsSuccessStatusCode ? await res.Content.ReadFromJsonAsync<Person>() : null;
    }

    public async Task<bool> UpdateAsync(Person person)
    {
        var res = await _http.PutAsJsonAsync($"{BaseUrl}/{person.Id}", person);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var res = await _http.DeleteAsync($"{BaseUrl}/{id}");
        return res.IsSuccessStatusCode;
    }
}
```

---

## 🧩 4. Injection dans `MauiProgram.cs`

```csharp
builder.Services.AddHttpClient<IHttpPersonService, HttpPersonService>();
```

---

## 🧠 5. Utilisation dans le `MainViewModel.cs`

Vous pouvez combiner `_repo` (local SQLite) et `_http` (distant REST) dans le `MainViewModel` :

```csharp
private readonly IPersonRepository _repo;
private readonly IHttpPersonService _http;

public MainViewModel(IPersonRepository repo, IHttpPersonService http)
{
    _repo = repo;
    _http = http;
    LoadPeople();
}

[RelayCommand]
async Task LoadPeople()
{
    // Exemple : charger depuis l'API distante
    var list = await _http.GetAllAsync();
    People = new ObservableCollection<Person>(list);
}

[RelayCommand]
async Task AddPerson()
{
    if (!string.IsNullOrWhiteSpace(Name) && int.TryParse(Age, out int ageValue))
    {
        var newPerson = new Person { Name = Name, Age = ageValue };

        var result = await _http.AddAsync(newPerson);
        if (result is not null)
        {
            await LoadPeople();
            Name = Age = "";
        }
    }
}

[RelayCommand]
async Task DeletePerson(Person person)
{
    if (await _http.DeleteAsync(person.Id))
        await LoadPeople();
}
```

> Vous pouvez facilement basculer entre `_repo` (offline) et `_http` (online) selon un booléen `IsOfflineMode` par exemple.

---






