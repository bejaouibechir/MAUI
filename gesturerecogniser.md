# Utiliser d'autres types de GestureRecognizers

## Définir la commande dans le ViewModel 

### Etape1: Commencez par définir une commande dans votre ViewModel. Par exemple, pour une commande qui sera exécutée lors d'un tapotement :

``` CSharp
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MauiApp.ViewModels
{
    public class MainViewModel : BindableObject
    {
        public ICommand TapCommand { get; }

        public MainViewModel()
        {
            TapCommand = new Command(OnTapped);
        }

        private void OnTapped()
        {
            // Logique à exécuter lors du tapotement
        }
    }
}

```
### Etape2: Lier la commande à un GestureRecognizer dans XAML :

Ajoutez le **GestureRecognizer** à l'élément visuel souhaité dans le fichier XAML et liez-le à la commande dans le ViewModel.

``` XML
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiApp.Views.MainPage">

    <ContentPage.BindingContext>
        <local:MainViewModel />
    </ContentPage.BindingContext>

    <StackLayout>
        <Label Text="Tap me!"
               HorizontalOptions="Center"
               VerticalOptions="CenterAndExpand">
            <Label.GestureRecognizers>
                <TapGestureRecognizer Command="{Binding TapCommand}" />
            </Label.GestureRecognizers>
        </Label>
    </StackLayout>
</ContentPage>

```
Dans cet exemple, nous avons un Label avec le texte "Tap me!" et un TapGestureRecognizer attaché. Lorsque l'utilisateur tapote sur le label, la commande TapCommand dans le ViewModel est exécutée.




