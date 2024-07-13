# Exercice: Projet de Navigation avec MAUI
## Objectif: Créer une application MAUI démontrant l'utilisation des NavigationPage, la navigation entre les pages, et le passage de données.
Cet exercice couvre les principaux aspects de la navigation avec NavigationPage dans une application MAUI, offrant aux étudiants une expérience pratique et complète.

### Étape 1 : Créer le Projet
- Créez un nouveau projet MAUI.
- Ajoutez trois nouvelles pages ContentPage: MainPage, Page1, Page2.
### Étape 2 : Configuration de la Navigation**
  ***App.xaml.cs***

``` CSharp

public App()
{
    InitializeComponent();
    MainPage = new NavigationPage(new MainPage());
}
```
***MainPage.xaml***

``` XML
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             x:Class="YourNamespace.MainPage">
    <StackLayout>
        <Button Text="Go to Page 1" Clicked="NavigateToPage1"/>
        <Button Text="Go to Page 2" Clicked="NavigateToPage2"/>
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
}
```


### Étape 3 : Utilisation de Push, Pop, InsertPageBefore, RemovePage**
***Page1.xaml***
``` XML
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             x:Class="YourNamespace.Page1">
    <StackLayout>
        <Button Text="Back" Clicked="NavigateBack"/>
        <Button Text="Insert Page 2 and Navigate" Clicked="InsertAndNavigate"/>
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

    private async void InsertAndNavigate(object sender, EventArgs e)
    {
        var page2 = new Page2();
        Navigation.InsertPageBefore(page2, this);
        await Navigation.PopAsync();
    }
}
```

### Étape 4 : Passage de Données
***Page2.xaml.cs***
``` CSharp
public partial class Page2 : ContentPage
{
    public string Data { get; set; }
    
    public Page2(string data)
    {
        InitializeComponent();
        Data = data;
        BindingContext = this;
    }

    public Page2()
    {
        InitializeComponent();
    }
}
```
***MainPage.xaml.cs (Mise à jour)***
``` CSharp
private async void NavigateToPage2(object sender, EventArgs e)
{
    await Navigation.PushAsync(new Page2("Data from MainPage"));
}
```
### Étape 5 : Événements de Navigation
***Page2.xaml.cs (Mise à jour)***
``` CSharp
public partial class Page2 : ContentPage
{
    public string Data { get; set; }

    public Page2(string data)
    {
        InitializeComponent();
        Data = data;
        BindingContext = this;
    }

    public Page2()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        DisplayAlert("Navigated To", "Welcome to Page2! Data: " + Data, "OK");
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        DisplayAlert("Navigated From", "Leaving Page2", "OK");
    }
}
```
### Étape 6 : Propriétés de NavigationPage
***App.xaml.cs (Mise à jour)***
``` CSharp
public App()
{
    InitializeComponent();
    var navigationPage = new NavigationPage(new MainPage())
    {
        BarBackgroundColor = Colors.Blue,
        BarTextColor = Colors.White,
        TitleIcon = "icon.png"
    };
    MainPage = navigationPage;
}
```
Utilisation de CurrentPage et NavigationStack
``` CSharp
var currentPage = Navigation.NavigationStack.Last();
var navigationStack = Navigation.NavigationStack;
```
### Étape 7 : Navigation Modale
***MainPage.xaml.cs (Mise à jour)***
``` CSharp
private async void NavigateToModalPage(object sender, EventArgs e)
{
    await Navigation.PushModalAsync(new ModalPage());
}

private async void CloseModalPage(object sender, EventArgs e)
{
    await Navigation.PopModalAsync();
}
```
***MainPage.xaml (Mise à jour)***
``` XML
<Button Text="Go to Modal Page" Clicked="NavigateToModalPage"/>
```
***ModalPage.xaml.cs***
``` CSharp
public partial class ModalPage : ContentPage
{
    public ModalPage()
    {
        InitializeComponent();
    }

    private async void CloseModalPage(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
```
***ModalPage.xaml***
``` XML
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             x:Class="YourNamespace.ModalPage">
    <StackLayout>
        <Button Text="Close Modal" Clicked="CloseModalPage"/>
    </StackLayout>
</ContentPage>
```
