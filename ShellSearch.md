# Implémentation de mécanisme de recherches intégré dans le Shell

Voici un exercice complet pour implémenter un mécanisme de recherche utilisant SearchHandler dans une application .NET MAUI Shell. Cet exercice vous guidera à travers la création d'une interface utilisateur en XAML, l'utilisation de la classe UserSearchHandler et l'ajout de données de test pour rechercher des utilisateurs par leur nom.

## Étape 1 : Classe User
Assurez-vous que la classe User est définie correctement :

``` CSharp
namespace MauiUserSearchApp
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
```
## Étape 2 : Classe UserSearchHandler
Voici une version corrigée de UserSearchHandler. Nous devons utiliser une propriété BindableProperty pour la liste des utilisateurs afin de permettre la liaison de données :

``` CSharp
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

namespace MauiUserSearchApp
{
    public class UserSearchHandler : SearchHandler
    {
        public static readonly BindableProperty UsersProperty =
            BindableProperty.Create(nameof(Users), typeof(IList<User>), typeof(UserSearchHandler), new List<User>());

        public IList<User> Users
        {
            get => (IList<User>)GetValue(UsersProperty);
            set => SetValue(UsersProperty, value);
        }

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                ItemsSource = Users
                    .Where(user => user.Name.ToLower().Contains(newValue.ToLower()))
                    .ToList();
            }
        }
    }
}
```
## Étape 3 : MainPage.xaml
Modifiez MainPage.xaml pour inclure la SearchHandler. Assurez-vous que l'espace de noms pour UserSearchHandler est correct.

```  XML
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiUserSearchApp.MainPage"
             xmlns:local="clr-namespace:MauiUserSearchApp">
    <Shell.SearchHandler>
        <local:UserSearchHandler Placeholder="Search users"
                                 ShowsResults="true"
                                 Users="{Binding Users}" />
    </Shell.SearchHandler>

    <StackLayout Padding="10">
        <Label Text="User List" 
               FontSize="20"
               HorizontalOptions="Center" />
        <CollectionView ItemsSource="{Binding Users}">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <StackLayout>
                        <Label Text="{Binding Name}" 
                               FontSize="18" />
                        <Label Text="{Binding Email}" 
                               FontSize="14" 
                               TextColor="Gray" />
                    </StackLayout>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </StackLayout>
</ContentPage>
```

## Étape 4 : MainPage.xaml.cs
Dans le fichier MainPage.xaml.cs, assurez-vous que la liste des utilisateurs est correctement liée :

``` CSharp
using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace MauiUserSearchApp
{
    public partial class MainPage : ContentPage
    {
        public List<User> Users { get; set; }

        public MainPage()
        {
            InitializeComponent();
            Users = new List<User>
            {
                new User { Name = "Alice", Email = "alice@example.com" },
                new User { Name = "Bob", Email = "bob@example.com" },
                new User { Name = "Charlie", Email = "charlie@example.com" }
                // Ajoutez plus d'utilisateurs si nécessaire
            };
            BindingContext = this;
        }
    }
}
```
## Étape 5 : AppShell.xaml
Vérifiez que la page principale est correctement définie dans AppShell.xaml :

```  XML
<?xml version="1.0" encoding="UTF-8" ?>
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:local="clr-namespace:MauiUserSearchApp"
       x:Class="MauiUserSearchApp.AppShell">

    <FlyoutItem Title="Main Page"
                Icon="dotnet_bot.png">
        <ShellContent
            Title="Main Page"
            ContentTemplate="{DataTemplate local:MainPage}" />
    </FlyoutItem>

</Shell>
```

**Points Clés à Vérifier**
- Propriétés Bindable : Assurez-vous que Users est correctement définie comme BindableProperty dans UserSearchHandler.
- Espaces de Noms : Vérifiez que les espaces de noms sont corrects et que les classes sont accessibles.
- BindingContext : Assurez-vous que le BindingContext de MainPage est correctement défini pour que les propriétés de liaison soient accessibles.

En suivant ces étapes, vous devriez être en mesure de résoudre les erreurs et d'intégrer correctement **SearchHandler** dans votre application MAUI Shell.
