# Partie I

# Exploration des propriétés et méthodes de FlyoutPage dans MAUI 

**Objectif :** Explorer et expérimenter avec les principales propriétés et méthodes de la classe ***FlyoutPage*** dans MAUI, en utilisant à la fois le code C# et le XAML

## Etape 1. Création d'une nouvelle application MAUI :

Créez une nouvelle application MAUI avec un modèle de projet qui inclut un FlyoutPage.

## Etape 2. Utilisation du XAML pour définir une FlyoutPage :

a. Création de la FlyoutPage en XAML :

Créez un fichier XAML pour définir la structure de votre FlyoutPage

``` XML
<!-- MainPage.xaml -->
<FlyoutPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
            xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
            x:Class="YourNamespace.MainPage">
    
    <FlyoutPage.Flyout>
     <NavigationPage>
         <x:Arguments>
             <ContentPage Title="Menu">
                 <StackLayout BackgroundColor="LightBlue" Padding="10">
                     <Label Text="Menu" FontSize="Large" HorizontalOptions="Center" VerticalOptions="CenterAndExpand" />
                 </StackLayout>
             </ContentPage>
         </x:Arguments>
     </NavigationPage>
 </FlyoutPage.Flyout>

 <FlyoutPage.FlyoutLayoutBehavior>
     <OnPlatform x:TypeArguments="FlyoutLayoutBehavior">
         <On Platform="iOS" Value="Split" />
         <On Platform="Android" Value="Flyout" />
     </OnPlatform>
 </FlyoutPage.FlyoutLayoutBehavior>

 <FlyoutPage.Detail>
     <NavigationPage>
         <x:Arguments>
             <ContentPage Title="Page de contenu">
                 <StackLayout>
                     <Label Text="Contenu de la page" FontSize="Large" HorizontalOptions="Center" VerticalOptions="CenterAndExpand" />
                 </StackLayout>
             </ContentPage>
         </x:Arguments>
     </NavigationPage>
 </FlyoutPage.Detail>

</FlyoutPage>

```
# Etape 3. Utilisation des méthodes dans le code-behind :
## 3.1 Gestion des événements OnAppearing et OnDisappearing :

``` CSharp

// MainPage.xaml.cs
using Microsoft.Maui.Controls;

namespace YourNamespace
{
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            System.Console.WriteLine("FlyoutPage est apparu !");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            System.Console.WriteLine("FlyoutPage a disparu !");
        }
    }
}


```

## 3.2 Utilisation de ToggleFlyout() avec un bouton :

``` xml
<!-- MainPage.xaml -->
<FlyoutPage ...>
    <!-- Autres éléments de la page comme précédemment -->

    <!-- Ajout d'un bouton pour basculer le menu déroulant -->
    <FlyoutPage.Flyout>
        <NavigationPage>
            <x:Arguments>
                <ContentPage Title="Menu">
                    <StackLayout>
                        <Button Text="Toggle Menu" Clicked="ToggleMenuClicked" />
                    </StackLayout>
                </ContentPage>
            </x:Arguments>
        </NavigationPage>
    </FlyoutPage.Flyout>
</FlyoutPage>

```
***Le code Bhind de la page***

``` CSharp
// MainPage.xaml.cs
namespace YourNamespace
{
    public partial class MainPage : FlyoutPage
    {
        // Constructeur et autres méthodes comme précédemment

        private void ToggleMenuClicked(object sender, EventArgs e)
        {
            IsPresented = !IsPresented;
        }
    }
}


```

# Etape 4. Test et observation :

Compilez et exécutez l'application. Testez chaque fonctionnalité ajoutée et observez comment elles affectent le comportement de la FlyoutPage.


# Partie II

Ces exemples montrent comment utiliser différentes propriétés et méthodes avancées de FlyoutPage pour personnaliser et contrôler le comportement du menu déroulant dans une application MAUI.



# Etape 5  Utilisation de IsPresented pour contrôler la visibilité du menu déroulant :

``` CSharp
// Toggle menu déroulant avec un bouton
Button toggleButton = new Button { Text = "Toggle Menu" };
toggleButton.Clicked += (sender, e) =>
{
    flyoutPage.IsPresented = !flyoutPage.IsPresented;
};

```
# Etape 6 Définition de l'arrière-plan (FlyoutBackground) et d'une icône (FlyoutIcon) pour le menu déroulant :

``` CSharp
// Définition de l'arrière-plan et de l'icône du menu déroulant
flyoutPage.FlyoutBackground = Color.LightGray;
flyoutPage.FlyoutIcon = "menu_icon.png"; // Fichier image dans les ressources de l'application

```

# Etape 7 Définition du titre du menu déroulant (FlyoutTitle) :

``` CSharp 

// Définition du titre du menu déroulant
flyoutPage.FlyoutTitle = "Menu principal";

```

# Etape 8 Contrôle manuel de l'ouverture et de la fermeture du menu déroulant avec Open() et Close() :

``` CSharp 

// Ouverture et fermeture manuelles du menu déroulant
flyoutPage.Open(); // Ouvre le menu déroulant
flyoutPage.Close(); // Ferme le menu déroulant

```

# Etape 9 Définition dynamique de la largeur du menu déroulant avec SetFlyoutWidth() :


``` CSharp 
// Définition dynamique de la largeur du menu déroulant
flyoutPage.SetFlyoutWidth(DeviceInfo.Platform == DevicePlatform.iOS ? 300 : 400); // Exemple de largeur différente selon la plateforme

```

# Etape 10 Utilisation de PushAsync() et PopAsync() pour la navigation à partir du menu déroulant :

``` CSharp 
// Exemple de navigation à partir du menu déroulant
var contentPage = new ContentPage { Title = "Page de contenu" };
flyoutPage.Detail = new NavigationPage(contentPage);
await flyoutPage.Navigation.PushAsync(new AutrePage()); // Navigation vers une autre page depuis le menu déroulant
await flyoutPage.Navigation.PopAsync(); // Retour à la page précédente


```

# Etape 11 Utilisation de SetIsPresentedFromAction() pour définir une action personnalisée pour contrôler la visibilité du menu déroulant :

``` CSharp 

// Définition d'une action personnalisée pour contrôler la visibilité du menu déroulant
flyoutPage.SetIsPresentedFromAction((bool isPresented) =>
{
    if (isPresented)
    {
        // Actions à exécuter lorsque le menu déroulant est ouvert
        Console.WriteLine("Menu déroulant ouvert !");
    }
    else
    {
        // Actions à exécuter lorsque le menu déroulant est fermé
        Console.WriteLine("Menu déroulant fermé !");
    }
});

```




