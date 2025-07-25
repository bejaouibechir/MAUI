###  Exercice 1 — Création d’un service REST avec Swagger et annotations

**Objectif** : Définir un Web API .NET 8 documenté avec Swagger (annotations `SwaggerOperation`) pour une entité `Product`.

#### Solution :

```csharp
// Controller
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Récupère tous les produits")]
    public IEnumerable<Product> GetAll() => _productList;

    [HttpPost]
    [SwaggerOperation(Summary = "Ajoute un produit")]
    public IActionResult Post(Product p) => Ok(p);
}
```

Ajoutez dans `Program.cs` :

```csharp
builder.Services.AddSwaggerGen(opt => {
    opt.EnableAnnotations();
});
```

---

###  Exercice 2 — Génération du client REST avec NSwagStudio

**Objectif** : Générer un client REST typé à partir de Swagger (OpenAPI) avec NSwagStudio.

#### Étapes :

1. Lancer NSwagStudio.
2. Coller l’URL Swagger (ex. `https://localhost:7065/swagger/v1/swagger.json`).
3. Cliquer sur **"Generate Outputs"**.
4. Inclure le fichier généré dans le projet MAUI.

---

###  Exercice 3 — Appel d’une API REST dans MAUI sans MVVM

**Objectif** : Utiliser `HttpClient` dans le code-behind pour récupérer des utilisateurs (MockApi).

#### Solution :

```csharp
string baseUrl = "https://62e1401cfa99731d75d247cc.mockapi.io";
HttpClient client = new();

private async void LoadUsers()
{
    var response = await client.GetAsync($"{baseUrl}/users");
    var stream = await response.Content.ReadAsStreamAsync();
    var users = await JsonSerializer.DeserializeAsync<List<User>>(stream);
    userListView.ItemsSource = users;
}
```

---

###  Exercice 4 — Implémenter un Repository REST avec injection

**Objectif** : Abstraire `HttpClient` via un `IUserRepository`.

#### Interfaces et DI :

```csharp
public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();
}
public class UserRepository : IUserRepository
{
    private readonly HttpClient _client;
    public UserRepository(HttpClient client) => _client = client;

    public async Task<List<User>> GetAllUsersAsync()
    {
        var res = await _client.GetAsync("https://.../users");
        var stream = await res.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<List<User>>(stream);
    }
}
```

Et dans `MauiProgram.cs` :

```csharp
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton(new HttpClient());
```

---

###  Exercice 5 — Consommation de service REST avec DataTemplate et CollectionView

**Objectif** : Afficher dynamiquement les résultats du service dans une `CollectionView`.

#### Solution :

```xml
<CollectionView ItemsSource="{Binding Users}">
    <CollectionView.ItemTemplate>
        <DataTemplate>
            <StackLayout>
                <Label Text="{Binding name}" />
                <Image Source="{Binding avatar}" />
            </StackLayout>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

---

###  Exercice 6 — Ajouter une entité via POST

**Objectif** : Implémenter un formulaire d’ajout d’utilisateur avec `HttpClient.PostAsync`.

#### Code :

```csharp
var user = new User { name = "Bechir", createdAt = DateTime.Now };
string json = JsonSerializer.Serialize(user);
var content = new StringContent(json, Encoding.UTF8, "application/json");
await client.PostAsync($"{baseUrl}/users", content);
```

---

###  Exercice 7 — Mise à jour d’une entité via PUT

**Objectif** : Modifier un utilisateur existant (id = 1).

```csharp
var user = Users.First(x => x.id == "1");
user.name = "Nouveau nom";
string json = JsonSerializer.Serialize(user);
var content = new StringContent(json, Encoding.UTF8, "application/json");
await client.PutAsync($"{baseUrl}/users/1", content);
```

---

###  Exercice 8 — Suppression d’une entité avec DELETE

**Objectif** : Supprimer un utilisateur sélectionné dans une `CollectionView`.

```csharp
var selectedUser = (User)collectionView.SelectedItem;
await client.DeleteAsync($"{baseUrl}/users/{selectedUser.id}");
```

---

###  Exercice 9 — Gestion des erreurs REST (try/catch + affichage)

**Objectif** : Capturer les erreurs HTTP et afficher un message utilisateur.

```csharp
try
{
    var response = await client.GetAsync($"{baseUrl}/users");
    if (!response.IsSuccessStatusCode)
        throw new Exception($"Erreur {response.StatusCode}");
}
catch (Exception ex)
{
    await App.Current.MainPage.DisplayAlert("Erreur", ex.Message, "OK");
}
```

---

###  Exercice 10 — Intégration REST + SQLite locale (cache offline)

**Objectif** : Récupérer les données via REST, les stocker en SQLite pour consultation hors-ligne.

#### Étapes :

1. Récupérer via `HttpClient`.
2. Stocker dans SQLite avec `sqlite-net-pcl`.
3. Vérifier connectivité (`Connectivity.Current.NetworkAccess`).


