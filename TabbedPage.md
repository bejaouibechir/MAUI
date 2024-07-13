# Le tour des propriétés, les fonctions et les évènements de TabbedPage 

Dans le contexte d'une TabbedPage d'une application MAUI, les principales propriétés, fonctions et événements fréquemment utilisés sont :

## Propriétés
- **Children :** Collection de pages affichées sous forme d'onglets.
- **CurrentPage :** La page actuellement affichée.
- **BarBackgroundColor :** Couleur de fond de la barre d'onglets.
- **BarTextColor :** Couleur du texte des onglets.
- **SelectedTabColor :** Couleur de l'onglet sélectionné.
- **UnselectedTabColor :** Couleur des onglets non sélectionnés.

## Fonctions
**AddChild(Page page) :** Ajouter une page à la collection d'onglets.
**RemoveChild(Page page) :** Supprimer une page de la collection d'onglets.

## Événements
**CurrentPageChanged :** Déclenché lorsque la page actuelle change.
**Appearing :** Déclenché lorsque la page apparaît.
**Disappearing :** Déclenché lorsque la page disparaît.

## Objectif
Créer une application MAUI qui utilise une TabbedPage pour gérer différentes catégories de tâches. L'application comprendra trois onglets : "Toutes les tâches", "Tâches en cours" et "Tâches terminées". Vous utiliserez les propriétés, fonctions et événements principaux de TabbedPage.


## Étape 1 : Création du Projet MAUI
Créez un nouveau projet MAUI dans Visual Studio.

``` XML
<?xml version="1.0" encoding="utf-8" ?>
<TabbedPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
            xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
            x:Class="TaskManagerApp.MainPage"
            BarBackgroundColor="LightGray"
            BarTextColor="Black"
            SelectedTabColor="Green"
            UnselectedTabColor="Gray">
    <TabbedPage.Children>
        <ContentPage Title="All Tasks">
            <StackLayout>
                <Label Text="All Tasks" 
                       VerticalOptions="CenterAndExpand" 
                       HorizontalOptions="CenterAndExpand" />
            </StackLayout>
        </ContentPage>
        <ContentPage Title="In Progress">
            <StackLayout>
                <Label Text="In Progress Tasks" 
                       VerticalOptions="CenterAndExpand" 
                       HorizontalOptions="CenterAndExpand" />
            </StackLayout>
        </ContentPage>
        <ContentPage Title="Completed">
            <StackLayout>
                <Label Text="Completed Tasks" 
                       VerticalOptions="CenterAndExpand" 
                       HorizontalOptions="CenterAndExpand" />
            </StackLayout>
        </ContentPage>
    </TabbedPage.Children>
</TabbedPage>


```
***MainPage.xaml.cs***

``` CSharp
namespace TaskManagerApp;

public partial class MainPage : TabbedPage
{
    public MainPage()
    {
        InitializeComponent();
        this.CurrentPageChanged += OnCurrentPageChanged;
    }

    private void OnCurrentPageChanged(object sender, EventArgs e)
    {
        DisplayAlert("Page Changed", $"You are on {this.CurrentPage.Title}", "OK");
    }
}

```

**Explication des Scripts**
**Propriétés**

***BarBackgroundColor***, ***BarTextColor***, ***SelectedTabColor***, ***UnselectedTabColor*** : Définissent respectivement la couleur de fond de la barre d'onglets, la couleur du texte des onglets, la couleur de l'onglet sélectionné et la couleur des onglets non sélectionnés.

**Children**

Ajout de trois pages enfant **(ContentPage)** à la **TabbedPage** avec des titres différents : "All Tasks", "In Progress" et "Completed".

**CurrentPageChanged (Événement)**

Abonnement à l'événement **CurrentPageChanged** pour afficher une alerte chaque fois que l'utilisateur change de page.


## Étape 3 : Ajouter et Supprimer des Pages Dynamiquement

Ajoutez un bouton pour ajouter une nouvelle page de tâches et un autre pour supprimer la page actuelle.

**MainPage.xaml**

``` XML
<?xml version="1.0" encoding="utf-8" ?>
<TabbedPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
            xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
            x:Class="TaskManagerApp.MainPage"
            BarBackgroundColor="LightGray"
            BarTextColor="Black"
            SelectedTabColor="Green"
            UnselectedTabColor="Gray">
    <StackLayout>
        <Button Text="Add New Task Page" Clicked="OnAddPageClicked" />
        <Button Text="Remove Current Page" Clicked="OnRemovePageClicked" />
    </StackLayout>
    <TabbedPage.Children>
        <ContentPage Title="All Tasks">
            <StackLayout>
                <Label Text="All Tasks" 
                       VerticalOptions="CenterAndExpand" 
                       HorizontalOptions="CenterAndExpand" />
            </StackLayout>
        </ContentPage>
        <ContentPage Title="In Progress">
            <StackLayout>
                <Label Text="In Progress Tasks" 
                       VerticalOptions="CenterAndExpand" 
                       HorizontalOptions="CenterAndExpand" />
            </StackLayout>
        </ContentPage>
        <ContentPage Title="Completed">
            <StackLayout>
                <Label Text="Completed Tasks" 
                       VerticalOptions="CenterAndExpand" 
                       HorizontalOptions="CenterAndExpand" />
            </StackLayout>
        </ContentPage>
    </TabbedPage.Children>
</TabbedPage>

```
***MainPage.xaml.cs***

``` CSharp
namespace TaskManagerApp;

public partial class MainPage : TabbedPage
{
    public MainPage()
    {
        InitializeComponent();
        this.CurrentPageChanged += OnCurrentPageChanged;
    }

    private void OnCurrentPageChanged(object sender, EventArgs e)
    {
        DisplayAlert("Page Changed", $"You are on {this.CurrentPage.Title}", "OK");
    }

    private void OnAddPageClicked(object sender, EventArgs e)
    {
        var newPage = new ContentPage
        {
            Title = $"Task Page {Children.Count + 1}",
            Content = new StackLayout
            {
                Children = 
                {
                    new Label 
                    {
                        Text = $"This is Task Page {Children.Count + 1}",
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    }
                }
            }
        };
        this.Children.Add(newPage);
    }

    private void OnRemovePageClicked(object sender, EventArgs e)
    {
        if (this.CurrentPage != null && this.Children.Count > 1)
        {
            this.Children.Remove(this.CurrentPage);
        }
    }
}

```

***Explication des Scripts***
-  **Fonctions**

    - **OnAddPageClicked :** Ajoute une nouvelle page à la TabbedPage dynamiquement avec un titre et du contenu.
    - **OnRemovePageClicked :** Supprime la page actuellement sélectionnée, si elle n'est pas la seule page restante.

- **Événements**

  - **Clicked :** Gère les événements de clic des boutons pour ajouter ou supprimer des pages.
