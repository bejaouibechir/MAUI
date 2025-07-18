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
            
            
            async private void OnTapped(object param)
            {
                if (param is Label label)
                {
                    string text = label.Text ?? "";
                    await Application.Current.MainPage.DisplayAlert("Texte du label", text, "OK");
                }
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
    <Label Text="Tap me!" x:Name="mylabel"
           HorizontalOptions="Center"
           VerticalOptions="CenterAndExpand">
        <Label.GestureRecognizers>
            <TapGestureRecognizer Command="{Binding TapCommand}"
                                  CommandParameter="{Binding Source={x:Reference mylabel}}"
                                  />
        </Label.GestureRecognizers>
    </Label>
</StackLayout>

</ContentPage>

```
Dans cet exemple, nous avons un Label avec le texte "Tap me!" et un TapGestureRecognizer attaché. Lorsque l'utilisateur tapote sur le label, la commande TapCommand dans le ViewModel est exécutée.




