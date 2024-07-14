# Partie I Navigation vers des pages définies et non définies

**Objectif :**
Apprendre à naviguer entre des pages définies et non définies dans l'hiérarchie visuelle de Shell et observer la création de la pile de navigation.

## Étape 1 : Créer le projet MAUI avec Shell

Créez deux pages, MainPage.xaml et DetailsPage.xaml.

**MainPage.xaml:**
``` XML
<?xml version="1.0" encoding="UTF-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="YourNamespace.MainPage">

    <StackLayout>
        <Label Text="Main Page" HorizontalOptions="Center" VerticalOptions="CenterAndExpand" />
        <Button Text="Go to Details Page" Clicked="OnDetailsButtonClicked"/>
        <Button Text="Go to New Page" Clicked="OnNewPageButtonClicked"/>
    </StackLayout>

</ContentPage>

```
**MainPage.xaml.cs:**

``` CSharp
using Microsoft.Maui.Controls;

namespace YourNamespace
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnDetailsButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//detailsPage");
        }

        private async void OnNewPageButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//newPage");
        }
    }
}

```

***Details page:***

``` XML
<?xml version="1.0" encoding="UTF-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="YourNamespace.DetailsPage">

     <StackLayout>
     <Label Text="Details Page" HorizontalOptions="Center" VerticalOptions="CenterAndExpand" />
     <Button Text="Go back to Main page" Clicked="Button_Clicked"/>
 </StackLayout>

</ContentPage>

```
``` CSharp 
public partial class DetailsPage : ContentPage
{
	public DetailsPage()
	{
		InitializeComponent();
	}

    async private void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//mainPage");
    }
}
```
## Étape 2 : Observer la création de la pile de navigation

- Lancez l'application et cliquez sur le bouton "Go to Details Page" pour naviguer vers une page définie.
- Cliquez ensuite sur le bouton "Go to New Page" pour naviguer vers une page non définie.
- Utilisez le débogueur pour observer la pile de navigation après chaque navigation.



Modifier le fichier AppShell.xaml pour définir la structure de l'application.

``` XML

<?xml version="1.0" encoding="UTF-8" ?>
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       x:Class="YourNamespace.AppShell">

    <!-- Définir les routes -->
    <ShellContent Title="MainPage" ContentTemplate="{DataTemplate local:MainPage}" Route="mainPage"/>
    <ShellContent Title="DetailsPage" ContentTemplate="{DataTemplate local:DetailsPage}" Route="detailsPage"/>

</Shell>

```

# Partie 2 : Gestion des navigations vers des pages non définies
**Objectif :**
Apprendre à gérer les exceptions lors de la navigation vers des routes inexistantes.

# Étape 1 : Ajouter la gestion des exceptions dans MainPage.xaml.cs

``` CSharp
private async void OnNewPageButtonClicked(object sender, EventArgs e)
{
    try
    {
        await Shell.Current.GoToAsync("//newPage");
    }
    catch (ArgumentException ex)
    {
        await DisplayAlert("Error", "The route does not exist.", "OK");
    }
}
```
# Étape 2 : Créer une page pour afficher les erreurs

Créez une nouvelle page **ErrorPage.xaml**

**ErrorPage.xaml**

<?xml version="1.0" encoding="UTF-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="YourNamespace.ErrorPage">

    <StackLayout>
        <Label Text="Error Page" HorizontalOptions="Center" VerticalOptions="CenterAndExpand" />
        <Button Text="Go Back" Clicked="OnGoBackButtonClicked"/>
    </StackLayout>

</ContentPage>

**ErrorPage.xaml.cs**

``` CSharp 
using Microsoft.Maui.Controls;

namespace YourNamespace
{
    public partial class ErrorPage : ContentPage
    {
        public ErrorPage()
        {
            InitializeComponent();
        }

        private async void OnGoBackButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//mainPage");
        }
    }
}

```

## Étape 3 : Modifier la gestion des exceptions pour naviguer vers ErrorPage

``` CSharp
private async void OnNewPageButtonClicked(object sender, EventArgs e)
{
    try
    {
        await Shell.Current.GoToAsync("//newPage");
    }
    catch (ArgumentException ex)
    {
        await Shell.Current.GoToAsync("//errorPage");
        Console.WriteLine(ex.Message);
    }
}

```
## Étape 4 : Ajouter la route pour ErrorPage dans AppShell.xaml

``` XML
<ShellContent Title="ErrorPage" ContentTemplate="{DataTemplate local:ErrorPage}" Route="errorPage"/>

```
Résumé :

- Dans la première partie, vous avez appris à naviguer vers des pages définies et non définies, observant la création de la pile de navigation.
- Dans la deuxième partie, vous avez appris à gérer les exceptions lors de la navigation vers des routes inexistantes et à rediriger l'utilisateur vers une page d'erreur.



