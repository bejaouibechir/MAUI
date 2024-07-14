# Gestion Complète de la Navigation dans MAUI Shell via les évènements OnNavigating,OnAppearing et OnNavigated

En suivant cette approche, vous pouvez gérer de manière robuste la navigation dans votre application MAUI tout en intégrant des actions spécifiques à chaque étape du cycle de vie de la navigation.

## Points à Considérer

 - **Validation :** Utilisez ***args.Cancel*** dans **OnNavigating** pour annuler la navigation si les données ne sont pas valides.
 
- **Sauvegarde :** Implémentez **SaveData** dans **OnNavigated** pour sauvegarder des données avant de quitter la page.

- **Chargement Initial :** Utilisez **LoadData** et **UpdateUI** dans **OnAppearing** pour charger des données initiales après que la nouvelle page a été affichée.


## Implémentations des Méthodes

Utilisez **UpdateUI** pour mettre à jour l'interface utilisateur après la navigation.

``` CSharp
private void UpdateUI()
{
    // Exemple de mise à jour de l'interface utilisateur après la navigation
    Debug.WriteLine("UI updated after navigation.");
}
```


Utilisez **SaveData** pour sauvegarder des données avant de quitter la page actuelle.

``` CSharp

private void SaveData()
{
    // Exemple de sauvegarde de données dans un MemoryStream
    _memoryStream.SetLength(0); // Clear the stream
    using (StreamWriter writer = new StreamWriter(_memoryStream, Encoding.UTF8, leaveOpen: true))
    {
        string login = "user1";
        string password = "pass123";

        writer.WriteLine(login);
        writer.WriteLine(password);
        writer.Flush();
    }

    Debug.WriteLine("Data saved to memory successfully.");
}

```

Utilisez **LoadData** pour charger des données initiales après que la nouvelle page a été affichée

``` CSharp
// Exemple de chargement de données depuis un MemoryStream
_memoryStream.Seek(0, SeekOrigin.Begin);
using (StreamReader reader = new StreamReader(_memoryStream, Encoding.UTF8, leaveOpen: true))
{
    string login = reader.ReadLine();
    string password = reader.ReadLine();

    Debug.WriteLine($"Loaded data from memory: Login = {login}, Password = {password}");
}

```

Implémentez les trois évènements dans Shell 

``` CSharp
 protected override void OnAppearing()
 {
     base.OnAppearing();
     // Exemple de chargement de données initiales après avoir affiché la nouvelle page
     LoadData();
     // Exemple de mise à jour de l'interface utilisateur après la navigation
     UpdateUI();
 }

 
 protected override void OnNavigating(ShellNavigatingEventArgs args)
 {
     
    
     base.OnNavigating(args);
     Debug.WriteLine($"Navigating from {args.Source}: {args.Target}");

     // Exemple d'annulation de la navigation basée sur une condition
     if (args.Target.Location.OriginalString == "//detailsPage")
     {
         // Exemple de nettoyage ou de sauvegarde de données avant de quitter la page
         if (!ValidateData())
         {
             Debug.WriteLine("Navigation to detailsPage cancelled.");
             args.Cancel();
         }
         SaveData();
     }
 }


 protected override void OnNavigated(ShellNavigatedEventArgs args)
 {
     base.OnNavigated(args);
     Debug.WriteLine($"Navigated from {args.Previous?.Location}: {args.Current.Location}");
 }


```

Voici le code complet:

``` CSharp 
public partial class AppShell : Shell
{
    private MemoryStream _memoryStream = new MemoryStream();
    public AppShell()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Exemple de chargement de données initiales après avoir affiché la nouvelle page
        LoadData();
        // Exemple de mise à jour de l'interface utilisateur après la navigation
        UpdateUI();
    }

    
    protected override void OnNavigating(ShellNavigatingEventArgs args)
    {
        
       
        base.OnNavigating(args);
        Debug.WriteLine($"Navigating from {args.Source}: {args.Target}");

        // Exemple d'annulation de la navigation basée sur une condition
        if (args.Target.Location.OriginalString == "//detailsPage")
        {
            // Exemple de nettoyage ou de sauvegarde de données avant de quitter la page
            if (!ValidateData())
            {
                Debug.WriteLine("Navigation to detailsPage cancelled.");
                args.Cancel();
            }
            SaveData();
        }
    }

  

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {

        base.OnNavigated(args);
        Debug.WriteLine($"Navigated from {args.Previous?.Location}: {args.Current.Location}");
    }

    private void LoadData()
    {
        // Exemple de chargement de données depuis un MemoryStream
        _memoryStream.Seek(0, SeekOrigin.Begin);
        using (StreamReader reader = new StreamReader(_memoryStream, Encoding.UTF8, leaveOpen: true))
        {
            string login = reader.ReadLine();
            string password = reader.ReadLine();

            Debug.WriteLine($"Loaded data from memory: Login = {login}, Password = {password}");
        }
    }

    private void SaveData()
    {
        // Exemple de sauvegarde de données dans un MemoryStream
        _memoryStream.SetLength(0); // Clear the stream
        using (StreamWriter writer = new StreamWriter(_memoryStream, Encoding.UTF8, leaveOpen: true))
        {
            string login = "user1";
            string password = "pass123";

            writer.WriteLine(login);
            writer.WriteLine(password);
            writer.Flush();
        }

        Debug.WriteLine("Data saved to memory successfully.");
    }

    private void UpdateUI()
    {
        // Exemple de mise à jour de l'interface utilisateur après la navigation
        Debug.WriteLine("UI updated after navigation.");
    }

    private bool ValidateData()
    {
        // Exemple de validation des données avant de quitter la page
        string login = "user1";
        string password = "pass123";

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            Debug.WriteLine("Login or password cannot be empty.");
            return false;
        }

        // Autres logiques de validation peuvent être ajoutées ici selon les besoins

        return true;
    }
}

```


