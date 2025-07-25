Voici une implémentation **complète et fonctionnelle** d'une `RelayCommand` personnalisée en .NET MAUI, avec support de :

* l'exécution (`Execute`)
* la vérification (`CanExecute`)
* la notification d’état (`CanExecuteChanged`)

---

## 🔧 Étapes de l'implémentation

1. Créer une classe `RelayCommand` (implémentant `ICommand`)
2. Définir :

   * les delegates `Action` et `Func<bool>`
   * l’événement `CanExecuteChanged`
   * les méthodes `CanExecute`, `Execute`, `RaiseCanExecuteChanged`
3. Utiliser cette commande dans un ViewModel et la lier à un bouton

---

###  Classe `RelayCommand.cs`

```csharp
using System;
using System.Windows.Input;

namespace MauiApp.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object? parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
```

---

###  Exemple d'utilisation dans un ViewModel

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MauiApp.Commands;

namespace MauiApp.ViewModels
{
    public class CounterViewModel : INotifyPropertyChanged
    {
        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                if (_count != value)
                {
                    _count = value;
                    OnPropertyChanged();
                    _decrementCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand IncrementCommand { get; }
        private RelayCommand? _decrementCommand;
        public RelayCommand DecrementCommand => _decrementCommand ??= new RelayCommand(
            execute: () => Count--,
            canExecute: () => Count > 0
        );

        public CounterViewModel()
        {
            IncrementCommand = new RelayCommand(() => Count++);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

---

###  Exemple XAML

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:viewModels="clr-namespace:MauiApp.ViewModels"
             x:Class="MauiApp.MainPage">

    <ContentPage.BindingContext>
        <viewModels:CounterViewModel />
    </ContentPage.BindingContext>

    <VerticalStackLayout Spacing="20" Padding="30">

        <Label Text="{Binding Count}" FontSize="32" HorizontalOptions="Center" />

        <Button Text="Incrémenter" Command="{Binding IncrementCommand}" />
        <Button Text="Décrémenter" Command="{Binding DecrementCommand}" />

    </VerticalStackLayout>
</ContentPage>
```

---

##  Résultat attendu

* Le bouton **Incrémenter** augmente le compteur sans condition.
* Le bouton **Décrémenter** diminue le compteur **uniquement si Count > 0**, et se désactive automatiquement quand Count atteint 0.

Voici la **version générique** de `RelayCommand<T>` avec gestion :

* du paramètre de type `T`
* de la vérification `CanExecute`
* de l’événement `CanExecuteChanged`

---

##  Classe `RelayCommand<T>`

```csharp
using System;
using System.Windows.Input;

namespace MauiApp.Commands
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (parameter is T value || parameter == null && typeof(T).IsClass)
                return _canExecute?.Invoke((T?)parameter!) ?? true;

            return false;
        }

        public void Execute(object? parameter)
        {
            if (parameter is T value || parameter == null && typeof(T).IsClass)
                _execute((T?)parameter!);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
```

---

##  Exemple d’utilisation dans un `ViewModel`
```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MauiApp.Commands;

namespace MauiApp.ViewModels
{
    public class MessageViewModel : INotifyPropertyChanged
    {
        private string _message = "";
        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged();
                    SendCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand<string> SendCommand { get; }

        public MessageViewModel()
        {
            SendCommand = new RelayCommand<string>(
                execute: (msg) => Application.Current.MainPage.DisplayAlert("Envoyé", msg, "OK"),
                canExecute: (msg) => !string.IsNullOrWhiteSpace(msg)
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
```

---

##  Exemple XAML

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:viewModels="clr-namespace:MauiApp.ViewModels"
             x:Class="MauiApp.MainPage">

    <ContentPage.BindingContext>
        <viewModels:MessageViewModel />
    </ContentPage.BindingContext>

    <VerticalStackLayout Spacing="20" Padding="30">

        <Entry Placeholder="Message à envoyer" Text="{Binding Message}" />

        <Button Text="Envoyer le message"
                Command="{Binding SendCommand}"
                CommandParameter="{Binding Message}" />

    </VerticalStackLayout>
</ContentPage>
```

---

##  Résultat attendu

* Le bouton **"Envoyer le message"** est **activé uniquement si le champ n'est pas vide**.
* En appuyant, une alerte affiche dynamiquement le message tapé.
* La commande réagit au changement de champ en temps réel (`RaiseCanExecuteChanged()`).



Voici une **implémentation complète et robuste** d'une `AsyncRelayCommand<T>` générique en .NET MAUI avec :

* exécution `async` (via `Task`)
* gestion de `CanExecute`
* détection d’un état d’exécution (`IsExecuting`)
* propagation de `CanExecuteChanged`

---

##  Classe `AsyncRelayCommand<T>`

```csharp
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp.Commands
{
    public class AsyncRelayCommand<T> : ICommand
    {
        private readonly Func<T?, Task> _execute;
        private readonly Func<T?, bool>? _canExecute;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<T?, Task> execute, Func<T?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (_isExecuting)
                return false;

            if (parameter is T value || parameter == null && typeof(T).IsClass)
                return _canExecute?.Invoke((T?)parameter!) ?? true;

            return false;
        }

        public async void Execute(object? parameter)
        {
            _isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                if (parameter is T value || parameter == null && typeof(T).IsClass)
                    await _execute((T?)parameter!);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            });
        }
    }
}
```

---

##  Exemple d’utilisation dans un `ViewModel`

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MauiApp.Commands;

namespace MauiApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username = "";
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                    LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public AsyncRelayCommand<string> LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand<string>(
                async (user) =>
                {
                    await Task.Delay(2000); // Simule un appel réseau
                    await Application.Current.MainPage.DisplayAlert("Bienvenue", $"Bonjour {user}", "OK");
                },
                canExecute: (user) => !string.IsNullOrWhiteSpace(user)
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
```

---

##  Exemple XAML

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:viewModels="clr-namespace:MauiApp.ViewModels"
             x:Class="MauiApp.MainPage">

    <ContentPage.BindingContext>
        <viewModels:LoginViewModel />
    </ContentPage.BindingContext>

    <VerticalStackLayout Padding="30" Spacing="20">

        <Entry Placeholder="Nom d'utilisateur"
               Text="{Binding Username}" />

        <Button Text="Se connecter"
                Command="{Binding LoginCommand}"
                CommandParameter="{Binding Username}" />

    </VerticalStackLayout>
</ContentPage>
```

---

##  Résultat attendu

* Le bouton **"Se connecter"** est désactivé si `Username` est vide.
* Lorsqu’on clique, le bouton est désactivé pendant 2 secondes (simulateur d’appel réseau).
* Une alerte affiche dynamiquement le nom d’utilisateur saisi.
* Le `CanExecute` réagit automatiquement au changement du champ via `RaiseCanExecuteChanged`.



