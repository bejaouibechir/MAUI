# Utilisation de la navigation personnalisée

En fournissant une implémentation personnalisée de INavigation, vous pouvez contrôler et personnaliser entièrement le comportement de navigation de votre application, répondant ainsi à des besoins spécifiques et créant des expériences utilisateur uniques.
Pour utiliser cette implémentation personnalisée, vous devez l'assigner à une propriété INavigation dans votre application.

**Raisons d'utilisation:**

***Points Clés***
**Navigation Spécifique:**

- Implémenter des comportements spécifiques à une plateforme ou un contexte particulier.

**Scénarios Complexes:**

- Gérer des scénarios de navigation complexes qui ne peuvent pas être exprimés facilement avec les mécanismes standard.

**Tests et Simulations:**

- Utiliser une implémentation personnalisée pour des tests unitaires ou des simulations.

**Intégration Tiers:**

- Intégrer des systèmes de navigation tiers nécessitant une interface spécifique.


``` CSharp 
public partial class App : Application
{
    public static INavigation CustomNavigation { get; private set; }

    public App()
    {
        InitializeComponent();

        var mainPage = new MainPage();
        MainPage = mainPage;

        CustomNavigation = new CustomNavigation();
    }
}

```

Ensuite, vous pouvez utiliser App.CustomNavigation pour naviguer dans votre application.

``` CSharp

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void NavigateToPage1(object sender, EventArgs e)
    {
        await App.CustomNavigation.PushAsync(new Page1());
    }

    private async void NavigateToPage2(object sender, EventArgs e)
    {
        await App.CustomNavigation.PushAsync(new Page2("Data from MainPage"));
    }
}
```


