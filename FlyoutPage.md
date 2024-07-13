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
     <ContentPage Title="Menu">
         <StackLayout BackgroundColor="LightBlue" Padding="10">
             <Label Text="Menu" FontSize="Large" HorizontalOptions="Center" VerticalOptions="CenterAndExpand" />
         </StackLayout>
     </ContentPage>
 </FlyoutPage.Flyout>
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

public partial class FlyoutPageDemo : FlyoutPage
{
	public FlyoutPageDemo()
	{
		InitializeComponent();
        IsPresentedChanged += FlyoutPageDemo_IsPresentedChanged;
        
	
	}

    private void FlyoutPageDemo_IsPresentedChanged(object sender, EventArgs e)
    {
        FlyoutPage flyoutPage = sender as FlyoutPage;
        if (flyoutPage.IsPresented==true) 
        {
            ;
        }
        else
        {
            ;
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

# Partie II Création dynamique de FlyoutPage

**Objectif :** Créer une page simple utilisant FlayoutPage pour afficher des éléments d'interface utilisateur.

Dans le projet, ajoutez une nouvelle page **DynamicFlyoutPageDemo** qui hérite de **FlyoutPage**

``` CSharp
public class DynamicFlyoutPageDemo : FlyoutPage
{
	public DynamicFlyoutPageDemo()
	{
        // Création de la page Flyout
        var flyout = new ContentPage
        {
            Title = "Menu"
        };

        var button1 = new Button
        {
            Text = "Afficher Contenu 1"
        };

        var button2 = new Button
        {
            Text = "Afficher Contenu 2"
        };

        var stackLayout = new StackLayout
        {
            Children = { button1, button2 }
        };

        flyout.Content = stackLayout;

        // Création des pages Detail
        var detailPage1 = new ContentPage
        {
            Content = new Label { Text = "Ceci est le contenu 1" }
        };

        var detailPage2 = new ContentPage
        {
            Content = new Label { Text = "Ceci est le contenu 2" }
        };

        // Gestion des événements des boutons
        button1.Clicked += (sender, e) =>
        {
             Detail = new NavigationPage(detailPage1);
             IsPresented = false; // Masquer le menu après sélection
        };

        button2.Clicked += (sender, e) =>
        {
             Detail = new NavigationPage(detailPage2);
             IsPresented = false; // Masquer le menu après sélection
        };

        // Configuration de Flyout et Detail
         Flyout = flyout;
         Detail = new NavigationPage(new ContentPage
        {
            Content = new Label { Text = "Sélectionnez une option dans le menu" }
        });

    }
 
}

```

***Configuration de l'application :***

``` CSharp
public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MyFlyoutPage();
    }
}

```

**Test de la page :**

Assurez-vous que votre projet MAUI cible l'émulateur ou le dispositif de votre choix.
Compilez et exécutez l'application pour tester la FlyoutPage créée.

**Validation :**

Vérifiez que le menu de la page Flyout contient les deux boutons.
Cliquez sur chaque bouton et assurez-vous que le contenu correct s'affiche dans la section Detail.
