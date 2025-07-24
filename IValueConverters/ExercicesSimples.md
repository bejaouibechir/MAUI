### 🔧 **Exercice 1 – BooleanToColorConverter**

**Objectif :** Afficher un bouton vert si une tâche est terminée (`IsDone = true`), sinon rouge.

**Convertisseur :**

```csharp
public class BooleanToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => (bool)value ? Colors.Green : Colors.Red;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

**XAML :**

```xml
<ContentPage.Resources>
    <local:BooleanToColorConverter x:Key="boolToColor"/>
</ContentPage.Resources>

<Button Text="Statut tâche"
        BackgroundColor="{Binding IsDone, Converter={StaticResource boolToColor}}"/>
```

---

### 🔧 **Exercice 2 – HexToColorConverter**

**Objectif :** Convertir une chaîne hexadécimale (`#FF5733`) en `Color` MAUI.

**Convertisseur :**

```csharp
public class HexToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => Color.FromArgb((string)value);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => ((Color)value).ToArgbHex(); // nécessite extension custom
}
```

**XAML :**

```xml
<ContentPage.Resources>
    <local:HexToColorConverter x:Key="hexToColor"/>
</ContentPage.Resources>

<Label Text="Couleur personnalisée"
       BackgroundColor="{Binding HexColor, Converter={StaticResource hexToColor}}"/>
```

---

### 🔧 **Exercice 3 – MultiBinding : Prix \* Quantité**

**Objectif :** Afficher le prix total d’un produit avec `IMultiValueConverter`.

**Convertisseur :**

```csharp
public class TotalPriceConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double prix = (double)values[0];
        int quantite = (int)values[1];
        return $"{prix * quantite:C}";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

**XAML :**

```xml
<ContentPage.Resources>
    <local:TotalPriceConverter x:Key="totalPrice"/>
</ContentPage.Resources>

<Label>
    <Label.Text>
        <MultiBinding Converter="{StaticResource totalPrice}">
            <Binding Path="PrixUnitaire"/>
            <Binding Path="Quantite"/>
        </MultiBinding>
    </Label.Text>
</Label>
```

---

### 🔧 **Exercice 4 – Date en texte relatif**

**Objectif :** Afficher une date au format “Il y a 3 jours”, “Hier”, etc.

**Convertisseur :**

```csharp
public class DateToRelativeTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var date = (DateTime)value;
        var diff = DateTime.Now - date;

        return diff.TotalDays switch
        {
            < 1 => "Aujourd'hui",
            < 2 => "Hier",
            < 7 => $"{(int)diff.TotalDays} jours",
            _ => date.ToString("d MMM yyyy", culture)
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

---

### 🔧 **Exercice 5 – Niveau de priorité en couleur de bordure**

**Objectif :** Colorier un encadré en fonction d’une énumération : Basse, Moyenne, Haute.

**Enum :**

```csharp
public enum Priorite { Basse, Moyenne, Haute }
```

**Convertisseur :**

```csharp
public class PriorityToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value switch
        {
            Priorite.Basse => Colors.Green,
            Priorite.Moyenne => Colors.Orange,
            Priorite.Haute => Colors.Red,
            _ => Colors.Gray
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

---

### 🔧 **Exercice 6 – Affichage conditionnel de mot de passe**

**Objectif :** Basculer entre `IsPassword=true/false` selon une icône cliquée (œil ouvert/fermé).

**Convertisseur :**

```csharp
public class BoolToPasswordToggleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => !(bool)value;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => !(bool)value;
}
```

**Utilisation (dans `Entry`):**

```xml
<Entry IsPassword="{Binding IsVisible, Converter={StaticResource boolToPasswordToggle}}"/>
```

---

### 🔧 **Exercice 7 – Pourcentage -> ProgressBar**

**Objectif :** Convertir un entier 0–100 en valeur 0.0–1.0 pour `ProgressBar`.

**Convertisseur :**

```csharp
public class PercentageToProgressConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => (int)value / 100.0;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => (double)value * 100;
}
```

---

### 🔧 **Exercice 8 – Texte vide -> Visibilité masquée**

**Objectif :** Si une chaîne est vide ou `null`, masquer un `Label`.

**Convertisseur :**

```csharp
public class EmptyStringToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => string.IsNullOrWhiteSpace((string)value) ? false : true;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

---

### 🔧 **Exercice 9 – List<string> vers texte concaténé**

**Objectif :** Prendre une `List<string>` (tags, catégories) et les afficher sous forme de texte.

**Convertisseur :**

```csharp
public class StringListToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => string.Join(", ", (IEnumerable<string>)value);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => ((string)value).Split(',').Select(x => x.Trim()).ToList();
}
```

---

### 🔧 **Exercice 10 – MultiBinding : Vérification d’état de formulaire**

**Objectif :** Afficher “Formulaire valide” si tous les champs obligatoires sont remplis.

**Convertisseur :**

```csharp
public class FormValidConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        => values.All(v => !string.IsNullOrWhiteSpace(v?.ToString()));

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

**XAML :**

```xml
<Label>
    <Label.Text>
        <MultiBinding Converter="{StaticResource formValid}">
            <Binding Path="Nom"/>
            <Binding Path="Email"/>
            <Binding Path="Telephone"/>
        </MultiBinding>
    </Label.Text>
</Label>
```
