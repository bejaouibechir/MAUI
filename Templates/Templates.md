### 🔹 Exercice 1 — Définir une charte graphique via ResourceDictionary externe

**Objectif** : Externaliser couleurs, polices et styles dans un fichier `Colors.xaml` et `Styles.xaml`, les fusionner dans `App.xaml`.

**Solution** :

1. Créez le dossier `Resources/Styles`.
2. Ajoutez `Colors.xaml` :

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui">
    <Color x:Key="PrimaryColor">#3498db</Color>
    <Color x:Key="AccentColor">#f39c12</Color>
</ResourceDictionary>
```

3. Ajoutez `Styles.xaml` :

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui">
    <Style x:Key="TitleLabel" TargetType="Label">
        <Setter Property="FontSize" Value="28"/>
        <Setter Property="TextColor" Value="{StaticResource PrimaryColor}"/>
        <Setter Property="FontAttributes" Value="Bold"/>
    </Style>
</ResourceDictionary>
```

4. Dans `App.xaml` :

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="Resources/Styles/Colors.xaml"/>
            <ResourceDictionary Source="Resources/Styles/Styles.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

5. Utilisez dans une page :

```xml
<Label Text="Bienvenue" Style="{StaticResource TitleLabel}"/>
```

---

### 🔹 Exercice 2 — Comparer StaticResource vs DynamicResource

**Objectif** : Modifier dynamiquement la couleur de fond d’un `Border` à l'exécution.

**Solution** :

```xml
<ContentPage.Resources>
    <SolidColorBrush x:Key="dynamicBrush" Color="Red"/>
</ContentPage.Resources>

<StackLayout>
    <Border WidthRequest="100" HeightRequest="100" Background="{DynamicResource dynamicBrush}"/>
    <Button Text="Changer couleur" Clicked="OnClick"/>
</StackLayout>
```

```csharp
private void OnClick(object sender, EventArgs e)
{
    Resources["dynamicBrush"] = new SolidColorBrush(Colors.Green);
}
```

---

### 🔹 Exercice 3 — Utilisation de police personnalisée

**Objectif** : Ajouter une police externe (`Pacifico.ttf`) et l'utiliser dans un `Label`.

**Solution** :

1. Placez `Pacifico.ttf` dans `Resources/Fonts`.
2. Ajoutez dans le `.csproj` :

```xml
<MauiFont Include="Resources\Fonts\Pacifico.ttf" Alias="Pacifico"/>
```

3. Ou dans `MauiProgram.cs` :

```csharp
fonts.AddFont("Pacifico.ttf", "Pacifico");
```

4. Utilisation :

```xml
<Label Text="Stylish Text" FontFamily="Pacifico" FontSize="32"/>
```

---

### 🔹 Exercice 4 — Lecture de ressource embarquée `.txt`

**Objectif** : Lire le contenu d’un fichier `content.txt` placé dans `Resources/Raw`.

**Solution** :

```csharp
private async void LoadFile()
{
    var assembly = typeof(App).Assembly;
    using Stream stream = assembly.GetManifestResourceStream("YourAppNamespace.Resources.Raw.content.txt");
    using var reader = new StreamReader(stream);
    string content = await reader.ReadToEndAsync();
    Console.WriteLine(content);
}
```

---

### 🔹 Exercice 5 — Créer un `ControlTemplate` pour un composant réutilisable

**Objectif** : Appliquer une structure personnalisée pour un `NumericUpDown`.

**Solution** :

```xml
<ControlTemplate x:Key="UpDownTemplate">
    <Grid BindingContext="{Binding Source={RelativeSource Mode=TemplatedParent}}">
        <Grid.RowDefinitions>
            <RowDefinition Height="*"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        <Border Grid.RowSpan="2">
            <Label Text="{Binding Value}" FontSize="24" />
        </Border>
        <Button Text="▲" Grid.Row="0" Command="{Binding IncreaseCommand}"/>
        <Button Text="▼" Grid.Row="1" Command="{Binding DecreaseCommand}"/>
    </Grid>
</ControlTemplate>

<local:NumericUpDown ControlTemplate="{StaticResource UpDownTemplate}"/>
```

---

### 🔹 Exercice 6 — Appliquer un style contenant un `ControlTemplate`

**Objectif** : Encapsuler un `ControlTemplate` dans un `Style`.

**Solution** :

```xml
<Style TargetType="local:NumericUpDown" x:Key="StyledUpDown">
    <Setter Property="ControlTemplate">
        <Setter.Value>
            <ControlTemplate>
                <!-- même contenu que l'exercice 5 -->
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>

<local:NumericUpDown Style="{StaticResource StyledUpDown}"/>
```

---

### 🔹 Exercice 7 — Appliquer un `DataTemplate` externe à une `CollectionView`

**Objectif** : Créer un fichier `Templates.xaml` contenant un `DataTemplate`.

**Solution** :

1. `Templates.xaml` :

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui">
    <DataTemplate x:Key="PlaceTemplate">
        <Frame Padding="10">
            <Label Text="{Binding Name}" FontSize="20"/>
        </Frame>
    </DataTemplate>
</ResourceDictionary>
```

2. `App.xaml` :

```xml
<ResourceDictionary.MergedDictionaries>
    <ResourceDictionary Source="Resources/Templates.xaml"/>
</ResourceDictionary.MergedDictionaries>
```

3. Utilisation :

```xml
<CollectionView ItemsSource="{Binding Places}" ItemTemplate="{StaticResource PlaceTemplate}"/>
```

---

### 🔹 Exercice 8 — Utiliser un `DataTemplateSelector` conditionnel

**Objectif** : Changer le `DataTemplate` selon la propriété `Continent`.

**Solution** :

```csharp
public class ContinentTemplateSelector : DataTemplateSelector
{
    public DataTemplate EuropeTemplate { get; set; }
    public DataTemplate AfriqueTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var place = item as Place;
        return place.Continent == "Europe" ? EuropeTemplate : AfriqueTemplate;
    }
}
```

```xml
<local:ContinentTemplateSelector x:Key="continentSelector"
    EuropeTemplate="{StaticResource EuropeTemplate}"
    AfriqueTemplate="{StaticResource AfriqueTemplate}" />

<CollectionView ItemsSource="{Binding Places}" ItemTemplate="{StaticResource continentSelector}"/>
```

---

### 🔹 Exercice 9 — Comportement visuel dynamique avec `Trigger`

**Objectif** : Appliquer une couleur de fond conditionnelle via un `DataTrigger`.

**Solution** :

```xml
<Label Text="{Binding Name}">
    <Label.Triggers>
        <DataTrigger TargetType="Label" Binding="{Binding IsAvailable}" Value="False">
            <Setter Property="TextColor" Value="Gray"/>
        </DataTrigger>
    </Label.Triggers>
</Label>
```

---

### 🔹 Exercice 10 — Changer dynamiquement un style via `StyleClass` (multi-styles)

**Objectif** : Appliquer plusieurs styles à un bouton selon les états.

**Solution** :

1. Définir des styles dans `App.xaml` :

```xml
<Style x:Key="SuccessStyle" TargetType="Button">
    <Setter Property="BackgroundColor" Value="Green"/>
</Style>

<Style x:Key="DangerStyle" TargetType="Button">
    <Setter Property="BackgroundColor" Value="Red"/>
</Style>
```

2. Bouton :

```xml
<Button Text="Exécuter" StyleClass="SuccessStyle"/>
```

3. C# :

```csharp
myButton.StyleClass = new[] { "DangerStyle" };
```

---


