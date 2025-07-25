### **Exercice 1 – Commande de validation de formulaire avec DelegateCommand**

**Énoncé :**
Créez un formulaire d’inscription avec deux champs (nom, email) et un bouton "S'inscrire". Le bouton est désactivé si l’un des champs est vide. Utilisez `DelegateCommand`.

**Solution :**

```csharp
public class InscriptionViewModel
{
    public string Nom { get; set; } = "";
    public string Email { get; set; } = "";

    public DelegateCommand ValiderCommande { get; }

    public InscriptionViewModel()
    {
        ValiderCommande = new DelegateCommand(Executer, PeutExecuter);
    }

    private bool PeutExecuter(object arg) => !string.IsNullOrWhiteSpace(Nom) && !string.IsNullOrWhiteSpace(Email);
    private void Executer(object obj) => Application.Current.MainPage.DisplayAlert("OK", "Inscription validée", "Fermer");
}
```

```xml
<Entry Text="{Binding Nom}" />
<Entry Text="{Binding Email}" />
<Button Text="S'inscrire" Command="{Binding ValiderCommande}" />
```

---

### **Exercice 2 – Commande avec paramètre dynamique**

**Énoncé :**
Créez une liste de boutons générés dynamiquement dans un `CollectionView`, chacun doit afficher un message personnalisé selon la donnée liée via `CommandParameter`.

**Solution (XAML partielle) :**

```xml
<CollectionView ItemsSource="{Binding Messages}">
    <CollectionView.ItemTemplate>
        <DataTemplate>
            <Button Text="{Binding}" Command="{Binding BindingContext.AfficherMessageCommande, Source={x:Reference NomDePage}}" CommandParameter="{Binding}" />
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

---

### **Exercice 3 – RelayCommand + méthode async + désactivation dynamique**

**Énoncé :**
Créez un bouton "Charger" qui appelle un service asynchrone. Pendant l’attente, le bouton est désactivé. Utilisez `RelayCommand`.

**Solution :**

```csharp
[RelayCommand(CanExecute = nameof(PeutExecuter))]
private async Task ChargerAsync()
{
    EstOccupé = true;
    await Task.Delay(2000); // Simulation d’un appel
    EstOccupé = false;
}

[ObservableProperty]
private bool estOccupé;

private bool PeutExecuter() => !EstOccupé;
```

---

### **Exercice 4 – GestureRecognizer + commande pour tap sur image**

**Énoncé :**
Créez une `Image` qui, lorsqu’on la tape, déclenche une alerte via une commande MVVM. Utilisez `TapGestureRecognizer`.

**Solution :**

```xml
<Image Source="avatar.png">
    <Image.GestureRecognizers>
        <TapGestureRecognizer Command="{Binding TapCommande}" />
    </Image.GestureRecognizers>
</Image>
```

```csharp
public ICommand TapCommande => new RelayCommand(() =>
    Application.Current.MainPage.DisplayAlert("Image", "Image tapée", "OK"));
```

---

### **Exercice 5 – PointerGestureRecognizer pour effet hover**

**Énoncé :**
Survol d’un bouton change son fond en jaune, sortie remet en blanc. Utilisez `PointerEntered/Exited`.

**Solution :**

```xml
<Button Text="Survol moi"
        BackgroundColor="{Binding CouleurFond}">
    <Button.GestureRecognizers>
        <PointerGestureRecognizer PointerEntered="OnEnter" PointerExited="OnExit"/>
    </Button.GestureRecognizers>
</Button>
```

```csharp
public Color CouleurFond { get; set; } = Colors.White;

private void OnEnter(object s, EventArgs e) => CouleurFond = Colors.Yellow;
private void OnExit(object s, EventArgs e) => CouleurFond = Colors.White;
```

---

### **Exercice 6 – Behavior personnalisé pour validation automatique**

**Énoncé :**
Créez un `Behavior` qui vérifie que le texte d’un `Entry` contient un `@` (email). En cas d’erreur, le champ devient rouge.

**Solution :**

```csharp
public class EmailValidatorBehavior : Behavior<Entry>
{
    protected override void OnAttachedTo(Entry entry)
    {
        base.OnAttachedTo(entry);
        entry.TextChanged += OnTextChanged;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        entry.BackgroundColor = e.NewTextValue.Contains("@") ? Colors.White : Colors.Red;
    }
}
```

```xml
<Entry Placeholder="Email">
    <Entry.Behaviors>
        <local:EmailValidatorBehavior />
    </Entry.Behaviors>
</Entry>
```

---

### **Exercice 7 – DataTrigger avec style pour changement visuel**

**Énoncé :**
Coloriez en vert un label si une propriété booléenne `EstValide` est `true`.

**Solution :**

```xml
<Label Text="Statut">
    <Label.Triggers>
        <DataTrigger TargetType="Label" Binding="{Binding EstValide}" Value="True">
            <Setter Property="TextColor" Value="Green"/>
        </DataTrigger>
    </Label.Triggers>
</Label>
```

---

### **Exercice 8 – MultiBinding avec valeur calculée (via IMultiValueConverter)**

**Énoncé :**
Créer une `Label` qui affiche “Formulaire Complet” seulement si `Nom`, `Email`, et `Age` sont remplis.

**Solution :**

```csharp
public class ChampRempliConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return values.All(v => !string.IsNullOrWhiteSpace(v?.ToString())) ? "Formulaire Complet" : "";
    }
}
```

```xml
<Label>
    <Label.Text>
        <MultiBinding Converter="{StaticResource ChampRempliConverter}">
            <Binding Path="Nom"/>
            <Binding Path="Email"/>
            <Binding Path="Age"/>
        </MultiBinding>
    </Label.Text>
</Label>
```

---

### **Exercice 9 – ICommand avec `CanExecute` mis à jour par interaction utilisateur**

**Énoncé :**
Une `CheckBox` active un bouton via `CanExecute`.

**Solution :**

```csharp
[ObservableProperty]
bool estAccepte;

public ICommand Valider => new RelayCommand(
    () => Application.Current.MainPage.DisplayAlert("OK", "Conditions acceptées", "Fermer"),
    () => EstAccepte
);
```

```xml
<CheckBox IsChecked="{Binding EstAccepte}" />
<Button Text="Valider" Command="{Binding Valider}" />
```

---

### **Exercice 10 – Behaviour pour afficher un message sur événement Entry.Completed**

**Énoncé :**
Créez un `Behavior` qui affiche un message à l'utilisateur lorsque l'utilisateur valide un champ (`Entry.Completed`).

**Solution :**

```csharp
public class CompletedAlertBehavior : Behavior<Entry>
{
    protected override void OnAttachedTo(Entry entry)
    {
        base.OnAttachedTo(entry);
        entry.Completed += OnCompleted;
    }

    private void OnCompleted(object sender, EventArgs e)
    {
        Application.Current.MainPage.DisplayAlert("Info", "Champ complété", "OK");
    }
}
```

```xml
<Entry Placeholder="Tapez ici">
    <Entry.Behaviors>
        <local:CompletedAlertBehavior />
    </Entry.Behaviors>
</Entry>
```


