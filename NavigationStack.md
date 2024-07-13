# Exemple d'utilisation de navigation Stack 

Cet exemple montre comment utiliser NavigationStack pour rendre l'experience de navigation plus intuitive.

Points Clés de l'Implémentation
Retour à la Page d'Accueil:

- Les boutons dans Page1 et Page2 utilisent Navigation.PopToRootAsync() pour retourner rapidement à la page d'accueil.
  
- Affichage de l'Historique de Navigation:
  - PageHistory affiche la pile de navigation actuelle en utilisant Navigation.NavigationStack.
  - Fonctionnement Global
  - La MainPage permet de naviguer vers Page1 et Page2.
  - Page1 et Page2 permettent de revenir en arrière ou de retourner à la page d'accueil.
  - PageHistory affiche un historique des pages visitées et permet de revenir à la page précédente.


## Étape 1 : Créer le Projet
Créez un nouveau projet MAUI.
Ajoutez trois nouvelles pages ContentPage: MainPage, Page1, Page2, et PageHistory.
## Étape 2 : Configuration de la Navigation
App.xaml.cs

``` CSharp
public App()
{
    InitializeComponent();
    MainPage = new NavigationPage(new MainPage())
    {
        BarBackgroundColor = Colors.Blue,
        BarTextColor = Colors.White
    };
}
```
***MainPage.xaml***

``` XML
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             x:Class="YourNamespace.MainPage">
    <StackLayout>
        <Button Text="Go to Page 1" Clicked="NavigateToPage1"/>
        <Button Text="Go to Page 2" Clicked="NavigateToPage2"/>
        <Button Text="Show Navigation History" Clicked="ShowNavigationHistory"/>
    </StackLayout>
</ContentPage>
```

***MainPage.xaml.cs***

``` CSharp
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void NavigateToPage1(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Page1());
    }

    private async void NavigateToPage2(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Page2());
    }

    private async void ShowNavigationHistory(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PageHistory());
    }
}
```

## Étape 3 : Implémentation des Pages

***Page1.xaml***

``` XML
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             x:Class="YourNamespace.Page1">
    <StackLayout>
        <Button Text="Back" Clicked="NavigateBack"/>
        <Button Text="Return to Home" Clicked="ReturnToHome"/>
    </StackLayout>
</ContentPage>
```

***Page1.xaml.cs***

``` CSharp
public partial class Page1 : ContentPage
{
    public Page1()
    {
        InitializeComponent();
    }

    private async void NavigateBack(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void ReturnToHome(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
```


***Page2.xaml***

``` XML
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             x:Class="YourNamespace.Page2">
    <StackLayout>
        <Button Text="Back" Clicked="NavigateBack"/>
        <Button Text="Return to Home" Clicked="ReturnToHome"/>
    </StackLayout>
</ContentPage>
```

***Page2.xaml.cs***

``` CSharp
public partial class Page2 : ContentPage
{
    public Page2()
    {
        InitializeComponent();
    }

    private async void NavigateBack(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void ReturnToHome(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
```

***PageHistory.xaml.cs***
``` XML
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             x:Class="YourNamespace.PageHistory">
    <StackLayout x:Name="StackLayout">
        <Button Text="Back" Clicked="NavigateBack"/>
    </StackLayout>
</ContentPage>
```

``` CSharp
public partial class PageHistory : ContentPage
{
    public PageHistory()
    {
        InitializeComponent();
        DisplayNavigationStack();
    }

    private void DisplayNavigationStack()
    {
        var navigationStack = Navigation.NavigationStack;
        foreach (var page in navigationStack)
        {
            StackLayout.Children.Add(new Label { Text = page.Title ?? page.GetType().Name });
        }
    }

    private async void NavigateBack(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
```


