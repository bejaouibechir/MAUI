# ViewModel avec ObservableObject, RelayCommand, et ObservablePropertyAttribute

D'abord, assurez-vous d'installer le package NuGet CommunityToolkit.Mvvm.

**dotnet add package CommunityToolkit.Mvvm**

Ensuite, voici comment utiliser ces éléments dans un ViewModel :

**ViewModel :**

``` CSharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MyApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        // Propriété observable
        [ObservableProperty]
        private string _message;

        // Collection observable
        public ObservableCollection<string> Items { get; } = new ObservableCollection<string>();

        // Commande relay
        public ICommand AddItemCommand { get; }
        public ICommand ClearItemsCommand { get; }

        public MainViewModel()
        {
            // Initialisation des commandes
            AddItemCommand = new RelayCommand(AddItem);
            ClearItemsCommand = new RelayCommand(ClearItems);

            // Valeur par défaut pour la propriété Message
            Message = "Hello, MAUI!";
        }

        private void AddItem()
        {
            Items.Add($"Item {Items.Count + 1}");
        }

        private void ClearItems()
        {
            Items.Clear();
        }
    }
}
```

## Utilisation du ViewModel dans une Vue
### Voici comment utiliser ce ViewModel dans une vue .NET MAUI (XAML) :

MainPage.xaml :

```  XML
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MyApp"
             x:Class="MyApp.MainPage">

    <ContentPage.BindingContext>
        <local:MainViewModel />
    </ContentPage.BindingContext>

    <StackLayout Padding="10">
        <Label Text="{Binding Message}" 
               FontSize="24" 
               HorizontalOptions="Center" />

        <Button Text="Add Item" 
                Command="{Binding AddItemCommand}" 
                HorizontalOptions="Center" 
                VerticalOptions="Center" 
                Margin="10" />

        <Button Text="Clear Items" 
                Command="{Binding ClearItemsCommand}" 
                HorizontalOptions="Center" 
                VerticalOptions="Center" 
                Margin="10" />

        <CollectionView ItemsSource="{Binding Items}">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <TextCell Text="{Binding .}" />
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </StackLayout>
</ContentPage>
```

### Explication:
- **ObservableObject** :
La classe MainViewModel hérite de ObservableObject, ce qui fournit l'implémentation de INotifyPropertyChanged.
- **ObservablePropertyAttribute :**
Utilisé pour la propriété Message. Cet attribut génère automatiquement le code nécessaire pour notifier la vue des modifications de cette propriété.
- **RelayCommand :**
Utilisé pour créer les commandes AddItemCommand et ClearItemsCommand, qui lient les actions à des méthodes (AddItem et ClearItems).
Ce ViewModel gère une collection d'éléments (Items) et deux commandes pour ajouter et effacer des éléments de cette collection. Il montre également comment utiliser une propriété observable (Message) pour afficher un texte dans la vue.

En utilisant ces éléments de CommunityToolkit.Mvvm, vous pouvez simplifier la gestion de l'état et des interactions dans votre application .NET MAUI.
