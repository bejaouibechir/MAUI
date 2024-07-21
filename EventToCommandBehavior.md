# Utiliser d'autres types EventToCommandBehavior pour lier un évenement à une commande 

## Définir la commande dans le ViewModel 

### Etape1: Commencez par définir la classe dubehaviour principale :

``` CSharp

using System;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public class EventToCommandBehavior : Behavior<VisualElement>
    {
        public static readonly BindableProperty EventNameProperty = BindableProperty.Create(
            nameof(EventName), typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command), typeof(ICommand), typeof(EventToCommandBehavior));

        public string EventName
        {
            get => (string)GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        private Delegate _eventHandler;
        private VisualElement _associatedObject;

        public VisualElement AssociatedObject
        {
            get => _associatedObject;
            private set => _associatedObject = value;
        }

        protected override void OnAttachedTo(VisualElement bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
            AttachEvent(bindable);
            bindable.BindingContextChanged += OnBindingContextChanged;
            BindingContext = bindable.BindingContext;
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            DetachEvent(bindable);
            AssociatedObject = null;
            bindable.BindingContextChanged -= OnBindingContextChanged;
            base.OnDetachingFrom(bindable);
        }

        private async void OnBindingContextChanged(object sender, EventArgs e)
        {
            await Task.Delay(100); // Ajoutez un léger délai pour la synchronisation
            BindingContext = ((BindableObject)sender).BindingContext;
        }

        private void AttachEvent(VisualElement bindable)
        {
            if (string.IsNullOrWhiteSpace(EventName))
                return;

            var eventInfo = bindable.GetType().GetEvent(EventName);
            if (eventInfo == null)
            {
                throw new ArgumentException($"No event named '{EventName}' found on type '{bindable.GetType().Name}'");
            }

            var methodInfo = typeof(EventToCommandBehavior).GetMethod(nameof(OnEventFired), BindingFlags.Instance | BindingFlags.NonPublic);
            _eventHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, methodInfo);
            eventInfo.AddEventHandler(bindable, _eventHandler);
        }

        private void DetachEvent(VisualElement bindable)
        {
            if (_eventHandler == null)
                return;

            var eventInfo = bindable.GetType().GetEvent(EventName);
            if (eventInfo != null)
            {
                eventInfo.RemoveEventHandler(bindable, _eventHandler);
            }
            _eventHandler = null;
        }

        private void OnEventFired(object sender, EventArgs eventArgs)
        {
            if (Command == null)
                return;

            if (Command.CanExecute(eventArgs))
            {
                Command.Execute(eventArgs);
            }
        }

        private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventToCommandBehavior)bindable;
            if (behavior.AssociatedObject == null)
                return;

            behavior.DetachEvent(behavior.AssociatedObject);
            behavior.AttachEvent(behavior.AssociatedObject);
        }
    }
}


```
### Etape2: Créer le View Model:

Ajoutez la classe  **MainViewModel** au projet.

``` CSharp
using System.Windows.Input;

namespace MauiApp1
{
    public class MainViewModel : BindableObject
    {
        public ICommand UnfocusedCommand { get; }

        public MainViewModel()
        {
            UnfocusedCommand = new Command(OnUnfocused);
        }

        private void OnUnfocused(object obj)
        {
            App.Current.MainPage.DisplayAlert("Test de behavior", "Behavior testé avec succès", "OK");
        }
    }
}

```

### Etape3: Définition de l'interface XAML:

``` XML
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MauiApp1"
             x:Class="MauiApp1.MainPage">

    <ContentPage.BindingContext>
        <local:MainViewModel />
    </ContentPage.BindingContext>

    <StackLayout>
        <Entry Placeholder="Tapez ici...">
            <Entry.Behaviors>
                <local:EventToCommandBehavior EventName="Unfocused"
                                              Command="{Binding UnfocusedCommand}" />
            </Entry.Behaviors>
        </Entry>
        <Entry Placeholder="Et puis là ..." />
    </StackLayout>
</ContentPage>

```
**Remarque**:

Le léger délai dans la méthode OnBindingContextChanged peut aider à résoudre les problèmes de synchronisation liés aux liaisons de données. Cela donne au système le temps de mettre à jour les liaisons avant que le comportement ne tente d'exécuter la commande. Si le problème persiste, il peut être utile d'examiner d'autres parties de l'application pour des problèmes potentiels de performance ou de synchronisation.
