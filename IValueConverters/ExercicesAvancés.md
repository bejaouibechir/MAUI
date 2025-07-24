---

###  **Exercice 1 – Validation dynamique de formulaire complexe**

**Objectif :** Activer le bouton “Soumettre” seulement si :

* Le nom est non vide,
* L’email contient un `@` et un `.`,
* Le mot de passe contient au moins 8 caractères.

**Convertisseur (IMultiValueConverter) :**

```csharp
public class FormReadyConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        string nom = values[0]?.ToString() ?? "";
        string email = values[1]?.ToString() ?? "";
        string pwd = values[2]?.ToString() ?? "";

        return !string.IsNullOrWhiteSpace(nom)
            && email.Contains('@') && email.Contains('.')
            && pwd.Length >= 8;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

**XAML :**

```xml
<Button Text="Soumettre">
    <Button.IsEnabled>
        <MultiBinding Converter="{StaticResource formReady}">
            <Binding Path="Nom"/>
            <Binding Path="Email"/>
            <Binding Path="MotDePasse"/>
        </MultiBinding>
    </Button.IsEnabled>
</Button>
```

---

###  **Exercice 2 – Statut de connexion (Ping + Auth)**

**Objectif :** Afficher l’état d’une connexion utilisateur selon 2 paramètres :

* Connectivité réseau (`IsOnline`)
* Jeton d’authentification valide (`IsTokenValid`)

**Convertisseur :**

```csharp
public class ConnectionStatusConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        bool online = (bool)values[0];
        bool tokenValid = (bool)values[1];

        if (!online) return "🟥 Hors ligne";
        return tokenValid ? "🟩 Connecté" : "🟧 Authentification requise";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

---

###  **Exercice 3 – Représentation visuelle du niveau de sécurité**

**Objectif :** Utiliser un `ProgressBar` colorée dynamiquement selon un score de sécurité (0-100) :

* < 40 : rouge
* < 70 : orange
* > \= 70 : vert

**IMultiValueConverter** : score + barre → value + couleur

```csharp
public class SecurityScoreConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double score = Convert.ToDouble(values[0]);

        if (targetType == typeof(Color))
        {
            return score switch
            {
                < 40 => Colors.Red,
                < 70 => Colors.Orange,
                _ => Colors.Green
            };
        }
        return score / 100.0;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

---

###  **Exercice 4 – Affichage conditionnel selon rôle + statut**

**Objectif :** Afficher ou non un bouton “Supprimer” si :

* L’utilisateur est un administrateur
* ET l’objet n’est pas verrouillé (`IsLocked == false`)

**Convertisseur :**

```csharp
public class CanDeleteConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        bool isAdmin = values[0]?.ToString() == "Admin";
        bool isLocked = (bool)values[1];

        return isAdmin && !isLocked;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

---

###  **Exercice 5 – Affichage intelligent de disponibilité produit**

**Objectif :** Si le stock est nul ou le produit est expiré (`DateExpiration < DateTime.Now`), afficher "Indisponible".

**Convertisseur :**

```csharp
public class ProductAvailabilityConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        int stock = (int)values[0];
        DateTime expiration = (DateTime)values[1];

        return (stock <= 0 || expiration < DateTime.Now) ? "❌ Indisponible" : "✅ En stock";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
    }
}
```

---

###  **Exercice 6 – Interactivité avancée : slider de simulation de crédit**

**Objectif :** À partir d’un montant, d’un taux et d’une durée en mois, calculer et afficher la mensualité estimée :

```csharp
M = (capital × taux) / (1 - (1 + taux)^-n)
```

**Convertisseur :**

```csharp
public class MonthlyPaymentConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double capital = Convert.ToDouble(values[0]);
        double taux = Convert.ToDouble(values[1]) / 100.0 / 12.0;
        int mois = (int)values[2];

        if (taux == 0) return (capital / mois).ToString("C2");

        double mensualité = (capital * taux) / (1 - Math.Pow(1 + taux, -mois));
        return mensualité.ToString("C2");
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

---

###  **Exercice 7 – Indicateur météo graphique avancé**

**Objectif :** Selon température, humidité et vent, afficher une icône météo personnalisée (fichier SVG/PNG).

**Convertisseur :**

```csharp
public class WeatherIconConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double temp = (double)values[0];
        double humidity = (double)values[1];
        double wind = (double)values[2];

        if (temp > 30 && humidity < 40) return "sunny.png";
        if (humidity > 80) return "rainy.png";
        if (wind > 50) return "windy.png";
        return "cloudy.png";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

---

###  **Exercice 8 – Affichage des résultats d’examen (moyenne + présence)**

**Objectif :** Afficher “Validé” seulement si :

* Moyenne ≥ 10
* Taux de présence ≥ 75 %

**Convertisseur :**

```csharp
public class ExamResultConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double moyenne = Convert.ToDouble(values[0]);
        double presence = Convert.ToDouble(values[1]);

        return (moyenne >= 10 && presence >= 75) ? "✅ Validé" : "❌ Non Validé";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

---

###  **Exercice 9 – Texte dynamique multilingue (fr/en) selon langue système**

**Objectif :** Retourner la bonne traduction d’une clé selon la culture courante.

**Convertisseur :**

```csharp
public class LocalizationKeyConverter : IValueConverter
{
    private Dictionary<string, Dictionary<string, string>> _translations = new()
    {
        { "Hello", new() { ["fr"] = "Bonjour", ["en"] = "Hello" } },
        { "Submit", new() { ["fr"] = "Soumettre", ["en"] = "Submit" } }
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var key = value?.ToString() ?? "";
        var lang = culture.TwoLetterISOLanguageName;

        return _translations.ContainsKey(key) && _translations[key].ContainsKey(lang)
            ? _translations[key][lang]
            : key;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

---

###  **Exercice 10 – Mode sombre dynamique avec inversion conditionnelle**

**Objectif :** Inverser automatiquement les couleurs foreground/background selon le thème et un booléen d'inversion personnalisé.

**Convertisseur :**

```csharp
public class ThemeColorInverterConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        AppTheme theme = (AppTheme)values[0];
        bool invert = (bool)values[1];

        var color = theme == AppTheme.Dark ? Colors.White : Colors.Black;
        return invert ? (color == Colors.White ? Colors.Black : Colors.White) : color;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
```

