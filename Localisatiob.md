# Localisation d'une application MAUI

**Objectif**
Dans cet exercice, vous allez apprendre à créer une application MAUI qui change dynamiquement l'affichage des drapeaux en fonction de la langue sélectionnée par l'utilisateur.

## Etape 1 Ajout des ressources

**Resources/Images:**

- Ajoutez vos images de drapeaux dans le dossier Resources/Images du projet.
- Les images doivent avoir les noms suivants :

    - france.png
    - angleterre.png
    - espagne.png
    - allemagne.png
      
**Resources/Raw/Locales:**

Créez les fichiers:
- en.json 
- fr.json 
- es.json
- de.json

**en.json**:
``` JSON 
{
  "FormTitle": "Form",
  "nomEntryPlaceholder": "Name",
  "prenomEntryPlaceholder": "First Name",
  "ageEntryPlaceholder": "Age",
  "dateNaissanceEntryPlaceholder": "Date of Birth mm/dd/yyyy",
  "LanguagePickerTitle": "Choose a language",
  "CountryPickerTitle": "Country of Birth",
  "ReservationPickerTitle": "Reservation Type",
  "CancelButtonText": "Cancel",
  "ApplyButtonText": "Apply",
  "Image": "angleterre.png",
  "DisplayAlertTitle": "Reservation confirmation",
  "DisplayAlertBody": "Do you want to confirm the reservation?",
  "DisplayAlertOK": "OK",
  "DisplayAlertCancel": "Cancel"
}
```
**fr.json**:

``` JSON 
{
  "FormTitle": "Formulaire",
  "nomEntryPlaceholder": "Nom",
  "prenomEntryPlaceholder": "Prénom",
  "ageEntryPlaceholder": "Âge",
  "dateNaissanceEntryPlaceholder": "Date de naissance jj/mm/aaaa",
  "LanguagePickerTitle": "Choisir une langue",
  "CountryPickerTitle": "Pays de naissance",
  "ReservationPickerTitle": "Type de réservation",
  "CancelButtonText": "Annuler",
  "ApplyButtonText": "Réserver",
  "Image": "france.png",
  "DisplayAlertTitle": "Confirmation de réservation",
  "DisplayAlertBody": "Voulez vous confirmer la reservation?",
  "DisplayAlertOK": "D'accord'",
  "DisplayAlertCancel": "Annulation"

}
```
**es.json**:
``` JSON 
{
  "FormTitle": "Formulario",
  "nomEntryPlaceholder": "Nombre",
  "prenomEntryPlaceholder": "Apellido",
  "ageEntryPlaceholder": "Edad",
  "dateNaissanceEntryPlaceholder": "Fecha de nacimiento dd/mm/aaaa",
  "LanguagePickerTitle": "Elige un idioma",
  "CountryPickerTitle": "País de nacimiento",
  "ReservationPickerTitle": "Tipo de reserva",
  "CancelButtonText": "Cancelar",
  "ApplyButtonText": "Reservar",
  "Image": "espagne.png",
  "DisplayAlertTitle": "Confirmación de reserva",
  "DisplayAlertBody": "¿Quieres confirmar la reserva?",
  "DisplayAlertOK": "Vale",
  "DisplayAlertCancel": "Cancelar"
}
```
**de.json:**
``` JSON 
{
  "FormTitle": "Formular",
  "nomEntryPlaceholder": "Name",
  "prenomEntryPlaceholder": "Vorname",
  "ageEntryPlaceholder": "Alter",
  "dateNaissanceEntryPlaceholder": "Geburtsdatum tt/mm/jjjj",
  "LanguagePickerTitle": "Sprache wählen",
  "CountryPickerTitle": "Geburtsland",
  "ReservationPickerTitle": "Reservierungsart",
  "CancelButtonText": "Abbrechen",
  "ApplyButtonText": "Reservieren",
  "Image": "allemagne.png",
  "DisplayAlertTitle": "Reservierungsbestätigung",
  "DisplayAlertBody": "Möchten Sie die Reservierung bestätigen?",
  "DisplayAlertOK": "Ya",
  "DisplayAlertCancel": "Stornieren"
}
```

Il faut ensuite présenter ces fichiers comme des ressources intégrées à l'application:

Ajouter une configuration dans le fichier ***.csproj** pour rendre ces fichiers comme resources intégrées:
``` xml
<ItemGroup>
  <EmbeddedResource Include="Resources\Raw\locales\de.json" />
  <EmbeddedResource Include="Resources\Raw\locales\en.json" />
  <EmbeddedResource Include="Resources\Raw\locales\es.json" />
  <EmbeddedResource Include="Resources\Raw\locales\fr.json" />
</ItemGroup>
```

## Étape 2  Implémenter le Service de Localisation

Créez un dossier **Services** dans votre projet et ajoutez un fichier **LocalizationService.cs**.

**LocalizationService.cs: **

``` CSharp
using System.Reflection;
using System.Text.Json;

public static class LocalizationService
{
    private static Dictionary<string, string> _localizedResources;

    public static async Task LoadLocalizationResourcesAsync(string languageCode)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"MauiApp1.Resources.Raw.locales.{languageCode}.json";

        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
            {
                throw new FileNotFoundException($"Resource '{resourceName}' not found.");
            }

            using (StreamReader reader = new StreamReader(stream))
            {
                var json = await reader.ReadToEndAsync();
                _localizedResources = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
        }
    }

    public static string Translate(string key)
    {
        return _localizedResources.TryGetValue(key, out var value) ? value : key;
    }
}
```
## Étape 3  Implémenter l'extension de balisage TranslateExtension

``` CSharp 
using MauiApp1.Services;

[ContentProperty(nameof(Key))]
public class TranslateExtension : IMarkupExtension
{
    public string Key { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Key == null)
            return "";

        return LocalizationService.Translate(Key);
    }
}

```

## Étape 4  Implémenter l'extension de balisage TranslateExtension

Ajouter ces lignes pour definir la culture de l'application et initialiser le service de localisation

``` CSharp
var cultureInfo = CultureInfo.CurrentCulture;
LocalizationService.LoadLocalizationResourcesAsync(cultureInfo.TwoLetterISOLanguageName).Wait();
```

## Etape 5 Mettre à jour l'interface utilisateur en XAML


``` XAML
 <ContentPage.Resources>
     <local:TranslateExtension x:Key="Translate" />
 </ContentPage.Resources>

 <ScrollView>
     <StackLayout Padding="20">

         <!-- Grand titre -->
         <Label Text="{local:Translate Key=FormTitle}" 
                FontSize="36"
                HorizontalOptions="Center" />

         <!-- Image du pays -->
         <Image x:Name="flagImg" Source="{local:Translate Key=image}" VerticalOptions="Center" HorizontalOptions="Center"
                 HeightRequest="200" WidthRequest="200" />

         <!-- Picker pour les langues -->
         <Picker x:Name="langPkr" 
                 SelectedIndexChanged="langPkr_SelectedIndexChanged"
                 Title="{local:Translate Key=LanguagePickerTitle}">
             <Picker.ItemsSource>
                 <x:Array Type="{x:Type x:String}">
                     <x:String>Langue française</x:String>
                     <x:String>English language</x:String>
                     <x:String>Lingua Española</x:String>
                     <x:String>Deutsche Sprache</x:String>
                 </x:Array>
             </Picker.ItemsSource>
         </Picker>

         <!-- Champs du formulaire -->
         <Entry x:Name="nomEntry" Placeholder="{local:Translate Key=nomEntryPlaceholder}" />
         <Entry x:Name="prenomEntry" Placeholder="{local:Translate Key=prenomEntryPlaceholder}" />
         <Entry x:Name="ageEntry" Placeholder="{local:Translate Key=ageEntryPlaceholder}" Keyboard="Numeric" />
         <Entry x:Name="dateniassancePkr" Placeholder="{local:Translate Key=dateNaissanceEntryPlaceholder}" />

         <!-- Picker pour le pays de naissance -->
         <Picker x:Name="paysnaissancePkr" Title="{local:Translate Key=CountryPickerTitle}">
             <Picker.ItemsSource>
                 <x:Array Type="{x:Type x:String}">
                     <x:String>France</x:String>
                     <x:String>Allemagne</x:String>
                     <x:String>Espagne</x:String>
                     <!-- Ajoutez plus de pays ici -->
                 </x:Array>
             </Picker.ItemsSource>
         </Picker>

         <!-- Picker pour la réservation -->
         <Picker x:Name="reservationPkr" Title="{local:Translate Key=ReservationPickerTitle}">
             <Picker.ItemsSource>
                 <x:Array Type="{x:Type x:String}">
                     <x:String>VIP</x:String>
                     <x:String>Standard</x:String>
                 </x:Array>
             </Picker.ItemsSource>
         </Picker>

         <!-- Time Picker pour le temps de réservation -->
         <TimePicker />

         <!-- Boutons -->
         <StackLayout Orientation="Horizontal" HorizontalOptions="CenterAndExpand">
             <Button x:Name="btnCancel" Text="{local:Translate Key=CancelButtonText}" Margin="5" Clicked="btnCancel_Clicked" />
             <Button x:Name="btnApply" Text="{local:Translate Key=ApplyButtonText}" Margin="5" Clicked="btnApply_Clicked" />
         </StackLayout>

     </StackLayout>
 </ScrollView>  
```

**Explication du code:**

Ce code XAML définit la mise en page d'une page de contenu dans une application MAUI avec des ressources localisées et une interface utilisateur interactive. Voici une explication brève de ce qu'il fait :

**Définition des Ressources:**

``` xml
<ContentPage.Resources>
    <local:TranslateExtension x:Key="Translate" />
</ContentPage.Resources>

```

Déclare une ressource nommée Translate, utilisant une extension de balisage personnalisée TranslateExtension pour gérer la traduction des textes dans l'interface utilisateur.

**Contenu de la Page:**

``` xml
<ScrollView>
    <StackLayout Padding="20">
```

**Grand Titre:**

``` xml
<Label Text="{local:Translate Key=FormTitle}" 
       FontSize="36"
       HorizontalOptions="Center" />
```
Affiche un grand titre, dont le texte est traduit dynamiquement en utilisant la clé FormTitle, avec une taille de police de 36 et centré horizontalement.

**Image du Pays:**
``` xml
<Image x:Name="flagImg" Source="{local:Translate Key=image}" VerticalOptions="Center" HorizontalOptions="Center"
       HeightRequest="200" WidthRequest="200" />

```
Affiche une image de drapeau, dont la source est traduite dynamiquement avec la clé image. L'image est centrée horizontalement et verticalement, avec des dimensions de 200x200.

**Picker pour les Langues:**
Déclare un Picker pour sélectionner la langue, avec un événement déclenché lors du changement de sélection (SelectedIndexChanged). Le titre du Picker est traduit dynamiquement.

``` xml
<Picker x:Name="langPkr" 
        SelectedIndexChanged="langPkr_SelectedIndexChanged"
        Title="{local:Translate Key=LanguagePickerTitle}">
    <Picker.ItemsSource>
        <x:Array Type="{x:Type x:String}">
            <x:String>Langue française</x:String>
            <x:String>English language</x:String>
            <x:String>Lingua Española</x:String>
            <x:String>Deutsche Sprache</x:String>
        </x:Array>
    </Picker.ItemsSource>
</Picker>
```
**Champs du Formulaire**:
Définit plusieurs champs de saisie (Entry) pour le **nom**, le **prénom**, **l'âge** et **la date de naissance**, avec des *placeholders traduits dynamiquement*.

``` xml
<Entry x:Name="nomEntry" Placeholder="{local:Translate Key=nomEntryPlaceholder}" />
<Entry x:Name="prenomEntry" Placeholder="{local:Translate Key=prenomEntryPlaceholder}" />
<Entry x:Name="ageEntry" Placeholder="{local:Translate Key=ageEntryPlaceholder}" Keyboard="Numeric" />
<Entry x:Name="dateniassancePkr" Placeholder="{local:Translate Key=dateNaissanceEntryPlaceholder}" />

```
**Picker pour le Pays de Naissance:**
**Picker** pour sélectionner le pays de naissance avec des options prédéfinies et un titre traduit dynamiquement.
``` xml
<Picker x:Name="paysnaissancePkr" Title="{local:Translate Key=CountryPickerTitle}">
    <Picker.ItemsSource>
        <x:Array Type="{x:Type x:String}">
            <x:String>France</x:String>
            <x:String>Allemagne</x:String>
            <x:String>Espagne</x:String>
        </x:Array>
    </Picker.ItemsSource>
</Picker>

```

**Picker pour la Réservation:**

**Picker** pour sélectionner le type de réservation, avec des options "VIP" et "Standard" et un titre traduit dynamiquement.
``` xml
<Picker x:Name="reservationPkr" Title="{local:Translate Key=ReservationPickerTitle}">
    <Picker.ItemsSource>
        <x:Array Type="{x:Type x:String}">
            <x:String>VIP</x:String>
            <x:String>Standard</x:String>
        </x:Array>
    </Picker.ItemsSource>
</Picker>
```
**Time Picker pour le Temps de Réservation:**
Un sélecteur de temps (TimePicker) pour choisir l'heure de la réservation.
``` xml
<TimePicker />
```

**Boutons:**

Deux boutons, **Annuler** et **Appliquer**, avec des textes traduits dynamiquement et des événements de clic associés.

``` xml
<StackLayout Orientation="Horizontal" HorizontalOptions="CenterAndExpand">
    <Button x:Name="btnCancel" Text="{local:Translate Key=CancelButtonText}" Margin="5" Clicked="btnCancel_Clicked" />
    <Button x:Name="btnApply" Text="{local:Translate Key=ApplyButtonText}" Margin="5" Clicked="btnApply_Clicked" />
</StackLayout>

```


## Etape 6 Mettre à jour le code behind

``` CSharp

using System.Globalization;
using System.Text.Json;
using MauiApp1.Services;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();   
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        flagImg.Source= ImageSource.FromFile("angleterre.png");
    }

    async private void btnApply_Clicked(object sender, EventArgs e)
    {
        // Récupérer les données du formulaire
        var formData = new
        {
            Nom = nomEntry.Text,
            Prenom = prenomEntry.Text,
            Age = ageEntry.Text,
            DateNaissance = dateniassancePkr.Text,
            Langue = langPkr.SelectedItem as string,
            PaysNaissance = paysnaissancePkr.SelectedItem as string,
            Reservation = reservationPkr.SelectedItem as string
        };

        // Convertir les données en JSON
        string jsonString = JsonSerializer.Serialize(formData);

        // Afficher un message de confirmation
        var result  = await DisplayAlert(LocalizationService.Translate("DisplayAlertTitle"),
            LocalizationService.Translate("DisplayAlertBody"), 
            LocalizationService.Translate("DisplayAlertOK"), 
            LocalizationService.Translate("DisplayAlertCancel"));

        if(result==true)
        {
            // Définir le chemin du fichier
                    var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, $"formData{formData.GetHashCode()}.json");

           // Écrire les données dans le fichier
           await File.WriteAllTextAsync(filePath, jsonString);
        }
        else
        {
            // Vider les champs d'entrée
            nomEntry.Text = string.Empty;
            prenomEntry.Text = string.Empty;
            ageEntry.Text = string.Empty;
            dateniassancePkr.Text = string.Empty;
            langPkr.SelectedIndex = -1;
            paysnaissancePkr.SelectedIndex = -1;
            reservationPkr.SelectedIndex = -1;
        }
    }

    private void btnCancel_Clicked(object sender, EventArgs e)
    {
        // Vider les champs d'entrée
        nomEntry.Text = string.Empty;
        prenomEntry.Text = string.Empty;
        ageEntry.Text = string.Empty;
        dateniassancePkr.Text = string.Empty;
        langPkr.SelectedIndex = -1;
        paysnaissancePkr.SelectedIndex = -1;
        reservationPkr.SelectedIndex = -1;
    }

    async private void langPkr_SelectedIndexChanged(object sender, EventArgs e)
    {
        Picker picker = sender as Picker;
        string selectedLanguageCode = "en"; // Default to English if something goes wrong

        switch (picker.SelectedIndex)
        {
            case 0:
                selectedLanguageCode = "fr";
                break;
            case 1:
                selectedLanguageCode = "en";
                break;
            case 2:
                selectedLanguageCode = "es";
                break;
            case 3:
                selectedLanguageCode = "de";
                break;
        }

        // Change the current culture
        CultureInfo.CurrentCulture = new CultureInfo($"{selectedLanguageCode}-{selectedLanguageCode.ToUpper()}");

        // Load localization resources for the selected language
        await LocalizationService.LoadLocalizationResourcesAsync(selectedLanguageCode);

        // Update the UI
        UpdateUI();
    }

    ImageSource imageSrc;
    private void UpdateUI()
    {
        
        // Update the placeholders and titles with translated text
        nomEntry.Placeholder = LocalizationService.Translate("nomEntryPlaceholder");
        prenomEntry.Placeholder = LocalizationService.Translate("prenomEntryPlaceholder");
        ageEntry.Placeholder = LocalizationService.Translate("ageEntryPlaceholder");
        dateniassancePkr.Placeholder = LocalizationService.Translate("dateNaissanceEntryPlaceholder");
        langPkr.Title = LocalizationService.Translate("LanguagePickerTitle");
        paysnaissancePkr.Title = LocalizationService.Translate("CountryPickerTitle");
        reservationPkr.Title = LocalizationService.Translate("ReservationPickerTitle");
        btnCancel.Text = LocalizationService.Translate("CancelButtonText");
        btnApply.Text = LocalizationService.Translate("ApplyButtonText");

        // Update the flag image
        string flagImageName = LocalizationService.Translate("Image");
        flagImg.Source = ImageSource.FromFile(flagImageName);


        // You may need to refresh other parts of the UI here
    }
}

```

### Explication du code

- Constructeur MainPage() :
  - Initialise la page en appelant InitializeComponent(), qui charge les composants définis dans le fichier XAML associé.

- Méthode OnAppearing() :
  - Override de la méthode OnAppearing() qui est appelée lorsque la page devient visible. Ici, elle configure l'image du drapeau par défaut (angleterre.png).

- Méthode btnApply_Clicked :
  - Méthode asynchrone déclenchée lorsque le bouton "Appliquer" est cliqué.
     - Récupère les données du formulaire et les convertit en objet anonyme.
     - Sérialise cet objet en JSON.
     - Affiche une boîte de dialogue de confirmation localisée.
     - Si l'utilisateur confirme, les données sont enregistrées dans un fichier JSON dans le répertoire d'application.
     -  Si l'utilisateur annule, les champs du formulaire sont vidés.

- Méthode btnCancel_Clicked :
  - Vider les champs du formulaire lorsque le bouton "Annuler" est cliqué.

- Méthode langPkr_SelectedIndexChanged :
  - Méthode asynchrone déclenchée lorsque l'utilisateur change la langue sélectionnée dans le Picker de langue.

- Définit la culture courante en fonction de la langue sélectionnée.
  - Charge les ressources de localisation pour la langue sélectionnée.
  - Met à jour l'interface utilisateur (UI).
- Méthode UpdateUI :
  - Met à jour les éléments de l'interface utilisateur avec les textes traduits :

- Placeholders des champs de saisie.
  - Titres des Picker.
  - Textes des boutons.
  - Image du drapeau correspondant à la langue sélectionnée.



Dans le cas de confirmation les données seront sauvegardées selon la plateforme: 
**Sauvegarde des fichiers**
- **Sur un appareil Android :** Le fichier sera enregistré dans le répertoire interne de l'application, ce qui est inaccessible aux autres applications. Vous pouvez le trouver dans un répertoire similaire à /data/data/com.yourapp/files/ (le chemin exact peut varier).

- **Sur un appareil iOS :** Le fichier sera enregistré dans le répertoire de documents de l'application. Vous pouvez le trouver dans le répertoire Documents de l'application, qui est accessible via les outils de débogage Xcode ou d'autres moyens d'accès aux fichiers de l'application.

- **Sur un appareil Windows :** Le fichier sera enregistré dans le répertoire des données d'application de l'utilisateur. Vous pouvez le trouver dans le chemin de répertoires des applications de l'utilisateur sous AppData\Local ou AppData\Roaming.

