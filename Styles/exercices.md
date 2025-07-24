
###  **Exercice 1 : Style implicite pour des Labels de formulaire**

**Objectif :** Appliquer un style implicite pour homogénéiser la présentation des champs de formulaire.

**Énoncé :**
Créez un style implicite pour tous les `Label` d’un formulaire afin qu’ils aient une taille de police de 20, une couleur `SlateGray` et soient alignés à gauche.

**Solution :**

```xml
<ContentPage.Resources>
    <Style TargetType="Label">
        <Setter Property="FontSize" Value="20" />
        <Setter Property="TextColor" Value="SlateGray" />
        <Setter Property="HorizontalTextAlignment" Value="Start" />
    </Style>
</ContentPage.Resources>

<StackLayout Padding="20">
    <Label Text="Nom:" />
    <Entry Placeholder="Entrez votre nom" />
    <Label Text="Email:" />
    <Entry Placeholder="Entrez votre email" />
</StackLayout>
```

---

###  **Exercice 2 : Style explicite avec BasedOn**

**Objectif :** Créer un style hérité d’un style de base.

**Énoncé :**
Définissez un style `baseStyle` pour les `Button` avec une taille de police de 18. Créez un autre style basé dessus nommé `confirmButtonStyle` qui change le fond en `Green` et la couleur du texte en `White`.

**Solution :**

```xml
<ContentPage.Resources>
    <Style x:Key="baseStyle" TargetType="Button">
        <Setter Property="FontSize" Value="18" />
    </Style>

    <Style x:Key="confirmButtonStyle" TargetType="Button" BasedOn="{StaticResource baseStyle}">
        <Setter Property="BackgroundColor" Value="Green" />
        <Setter Property="TextColor" Value="White" />
    </Style>
</ContentPage.Resources>

<Button Text="Confirmer" Style="{StaticResource confirmButtonStyle}" />
```

---

###  **Exercice 3 : Trigger de propriété avec `IsFocused`**

**Objectif :** Changer dynamiquement la couleur de l’Entry lorsqu’il est focus.

**Énoncé :**
Utilisez un `Trigger` pour qu’un `Entry` ait un `TextColor` noir quand il est focus, sinon `Gray`.

**Solution :**

```xml
<Entry Placeholder="Tapez ici">
    <Entry.Style>
        <Style TargetType="Entry">
            <Setter Property="TextColor" Value="Gray" />
            <Style.Triggers>
                <Trigger TargetType="Entry" Property="IsFocused" Value="True">
                    <Setter Property="TextColor" Value="Black" />
                </Trigger>
            </Style.Triggers>
        </Style>
    </Entry.Style>
</Entry>
```

---

###  **Exercice 4 : Trigger + Action personnalisée pour changer couleur de fond**

**Objectif :** Appliquer une `TriggerAction` personnalisée lors du focus d’un Entry.

**Énoncé :**
Quand l’utilisateur entre dans le champ, la couleur de fond devient verte, sinon blanche.

**Code C# :**

```csharp
public class ChangeBackgroundColorAction : TriggerAction<Entry>
{
    public Color Color { get; set; }

    protected override void Invoke(Entry sender)
    {
        sender.BackgroundColor = Color;
    }
}
```

**XAML :**

```xml
<Entry Placeholder="Nom">
    <Entry.Triggers>
        <Trigger TargetType="Entry" Property="IsFocused" Value="True">
            <Trigger.EnterActions>
                <local:ChangeBackgroundColorAction Color="LightGreen" />
            </Trigger.EnterActions>
            <Trigger.ExitActions>
                <local:ChangeBackgroundColorAction Color="White" />
            </Trigger.ExitActions>
        </Trigger>
    </Entry.Triggers>
</Entry>
```

---

###  **Exercice 5 : EventTrigger personnalisé**

**Objectif :** Afficher une alerte lorsqu’un Entry perd le focus.

**Énoncé :**
Créez un `EventTrigger` sur l’événement `Unfocused` qui affiche un message avec le texte saisi.

**Code C# :**

```csharp
public class ShowTextAlertAction : TriggerAction<Entry>
{
    protected override void Invoke(Entry sender)
    {
        Application.Current.MainPage.DisplayAlert("Saisie", sender.Text, "OK");
    }
}
```

**XAML :**

```xml
<Entry Text="Bonjour">
    <Entry.Triggers>
        <EventTrigger Event="Unfocused">
            <local:ShowTextAlertAction />
        </EventTrigger>
    </Entry.Triggers>
</Entry>
```

---


###  **Exercice 6 : DataTrigger sur un CheckBox**

**Objectif :** Modifier un `Label` en fonction de l’état d’un `CheckBox`.

**Énoncé :**
Afficher “Oui” en vert si la case est cochée, “Non” en rouge sinon.

**Solution :**

```xml
<StackLayout>
    <CheckBox x:Name="chkConsent" />
    <Label>
        <Label.Style>
            <Style TargetType="Label">
                <Setter Property="Text" Value="Non" />
                <Setter Property="TextColor" Value="Red" />
                <Style.Triggers>
                    <DataTrigger TargetType="Label" Binding="{Binding Source={x:Reference chkConsent}, Path=IsChecked}" Value="True">
                        <Setter Property="Text" Value="Oui" />
                        <Setter Property="TextColor" Value="Green" />
                    </DataTrigger>
                </Style.Triggers>
            </Style>
        </Label.Style>
    </Label>
</StackLayout>
```

---

###  **Exercice 7 : MultiTrigger sur deux CheckBox**

**Objectif :** Modifier un `Label` uniquement si deux conditions sont vraies.

**Énoncé :**
Afficher “Vérifié” en vert si les deux cases sont cochées, sinon “Non vérifié” en rouge.

**Solution :**

```xml
<StackLayout>
    <CheckBox x:Name="chk1" />
    <CheckBox x:Name="chk2" />
    <Label>
        <Label.Style>
            <Style TargetType="Label">
                <Setter Property="Text" Value="Non vérifié" />
                <Setter Property="TextColor" Value="Red" />
                <Style.Triggers>
                    <MultiTrigger TargetType="Label">
                        <MultiTrigger.Conditions>
                            <BindingCondition Binding="{Binding Source={x:Reference chk1}, Path=IsChecked}" Value="True" />
                            <BindingCondition Binding="{Binding Source={x:Reference chk2}, Path=IsChecked}" Value="True" />
                        </MultiTrigger.Conditions>
                        <Setter Property="Text" Value="Vérifié" />
                        <Setter Property="TextColor" Value="Green" />
                    </MultiTrigger>
                </Style.Triggers>
            </Style>
        </Label.Style>
    </Label>
</StackLayout>
```

---

###  **Exercice 8 : VisualStateManager pour un Entry**

**Objectif :** Modifier l’apparence d’un `Entry` selon son état (`Normal`, `Focused`, `Disabled`).

**Énoncé :**
Changer la couleur de fond selon l’état de focus ou de désactivation.

**Solution :**

```xml
<Entry FontSize="18" IsEnabled="True">
    <VisualStateManager.VisualStateGroups>
        <VisualStateGroupList>
            <VisualStateGroup>
                <VisualState x:Name="Normal">
                    <VisualState.Setters>
                        <Setter Property="BackgroundColor" Value="LightGray" />
                    </VisualState.Setters>
                </VisualState>
                <VisualState x:Name="Focused">
                    <VisualState.Setters>
                        <Setter Property="BackgroundColor" Value="LightGreen" />
                    </VisualState.Setters>
                </VisualState>
                <VisualState x:Name="Disabled">
                    <VisualState.Setters>
                        <Setter Property="BackgroundColor" Value="DarkGray" />
                        <Setter Property="TextColor" Value="White" />
                    </VisualState.Setters>
                </VisualState>
            </VisualStateGroup>
        </VisualStateGroupList>
    </VisualStateManager.VisualStateGroups>
</Entry>
```

---

###  **Exercice 9 : VisualStates personnalisés (Validation Email)**

**Objectif :** Modifier visuellement un champ d’e-mail selon sa validité.

**Énoncé :**
Si le mail est valide (regex), fond vert, sinon fond rouge et message d’erreur.

**Code C# :**

```csharp
private void OnTextChanged(object sender, TextChangedEventArgs e)
{
    bool isValid = Regex.IsMatch(e.NewTextValue, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
    VisualStateManager.GoToState(stackLayout, isValid ? "Valid" : "Invalid");
}
```

**XAML :**

```xml
<StackLayout x:Name="stackLayout">
    <Entry x:Name="entry" Placeholder="Email" TextChanged="OnTextChanged" />
    <Label x:Name="helpLabel" Text="Mail invalide" IsVisible="False" />
    <Button x:Name="submitButton" Text="Envoyer" />

    <VisualStateManager.VisualStateGroups>
        <VisualStateGroup>
            <VisualState x:Name="Valid">
                <VisualState.Setters>
                    <Setter TargetName="entry" Property="BackgroundColor" Value="LightGreen" />
                    <Setter TargetName="helpLabel" Property="IsVisible" Value="False" />
                    <Setter TargetName="submitButton" Property="IsEnabled" Value="True" />
                </VisualState.Setters>
            </VisualState>
            <VisualState x:Name="Invalid">
                <VisualState.Setters>
                    <Setter TargetName="entry" Property="BackgroundColor" Value="Pink" />
                    <Setter TargetName="helpLabel" Property="IsVisible" Value="True" />
                    <Setter TargetName="submitButton" Property="IsEnabled" Value="False" />
                </VisualState.Setters>
            </VisualState>
        </VisualStateGroup>
    </VisualStateManager.VisualStateGroups>
</StackLayout>
```

---

###  **Exercice 10 : Behavior pour focus**

**Objectif :** Créer un `Behavior` qui change la couleur de fond quand l’utilisateur entre dans un `Entry`.

**C# :**

```csharp
public class FocusHighlightBehavior : Behavior<Entry>
{
    protected override void OnAttachedTo(Entry bindable)
        => bindable.Focused += (s, e) => bindable.BackgroundColor = Colors.LightBlue;

    protected override void OnDetachingFrom(Entry bindable)
        => bindable.Focused -= (s, e) => bindable.BackgroundColor = Colors.White;
}
```

**XAML :**

```xml
<Entry Placeholder="Nom">
    <Entry.Behaviors>
        <local:FocusHighlightBehavior />
    </Entry.Behaviors>
</Entry>
```

---

###  **Exercice 11 : Behavior de validation par regex**

**Objectif :** Affiche un fond rouge si la valeur d’un `Entry` ne correspond pas à une regex.

**Code C# :**

```csharp
public class RegexValidationBehavior : Behavior<Entry>
{
    public string Pattern { get; set; } = "";

    protected override void OnAttachedTo(Entry bindable)
    {
        bindable.TextChanged += OnTextChanged;
    }

    protected override void OnDetachingFrom(Entry bindable)
    {
        bindable.TextChanged -= OnTextChanged;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            bool isValid = Regex.IsMatch(e.NewTextValue ?? "", Pattern);
            entry.BackgroundColor = isValid ? Colors.LightGreen : Colors.IndianRed;
        }
    }
}
```

**XAML :**

```xml
<Entry Placeholder="Email">
    <Entry.Behaviors>
        <local:RegexValidationBehavior Pattern="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" />
    </Entry.Behaviors>
</Entry>
```

---

###  **Exercice 12 : Comportement réutilisé sur plusieurs champs**

**Objectif :** Appliquer un `Behavior` de mise en relief (fond jaune) à plusieurs `Entry`.

**XAML :**

```xml
<Entry Placeholder="Nom">
    <Entry.Behaviors>
        <local:FocusHighlightBehavior />
    </Entry.Behaviors>
</Entry>

<Entry Placeholder="Prénom">
    <Entry.Behaviors>
        <local:FocusHighlightBehavior />
    </Entry.Behaviors>
</Entry>
```

**C# (déjà fourni dans exercice 10)**

---

###  **Exercice 13 : VisualState avec `DeviceStateTrigger`**

**Objectif :** Appliquer un fond spécifique selon la plateforme (`Windows`, `Android`, etc.).

**XAML :**

```xml
<ContentPage.Resources>
    <Style x:Key="deviceStyle" TargetType="Label">
        <Setter Property="VisualStateManager.VisualStateGroups">
            <VisualStateGroupList>
                <VisualStateGroup>
                    <VisualState Name="Windows">
                        <VisualState.StateTriggers>
                            <DeviceStateTrigger Device="Windows" />
                        </VisualState.StateTriggers>
                        <VisualState.Setters>
                            <Setter Property="BackgroundColor" Value="LightSkyBlue" />
                        </VisualState.Setters>
                    </VisualState>
                    <VisualState Name="Android">
                        <VisualState.StateTriggers>
                            <DeviceStateTrigger Device="Android" />
                        </VisualState.StateTriggers>
                        <VisualState.Setters>
                            <Setter Property="BackgroundColor" Value="LightGreen" />
                        </VisualState.Setters>
                    </VisualState>
                </VisualStateGroup>
            </VisualStateGroupList>
        </Setter>
    </Style>
</ContentPage.Resources>

<Label Text="Plateforme test" Style="{StaticResource deviceStyle}" />
```

---

###  **Exercice 14 : OrientationStateTrigger**

**Objectif :** Changer la couleur d’arrière-plan selon l’orientation de l’écran.

**XAML :**

```xml
<ContentPage.Resources>
    <Style x:Key="orientationStyle" TargetType="Label">
        <Setter Property="VisualStateManager.VisualStateGroups">
            <VisualStateGroupList>
                <VisualStateGroup>
                    <VisualState Name="Portrait">
                        <VisualState.StateTriggers>
                            <OrientationStateTrigger Orientation="Portrait" />
                        </VisualState.StateTriggers>
                        <VisualState.Setters>
                            <Setter Property="BackgroundColor" Value="LightYellow" />
                        </VisualState.Setters>
                    </VisualState>
                    <VisualState Name="Landscape">
                        <VisualState.StateTriggers>
                            <OrientationStateTrigger Orientation="Landscape" />
                        </VisualState.StateTriggers>
                        <VisualState.Setters>
                            <Setter Property="BackgroundColor" Value="LightBlue" />
                        </VisualState.Setters>
                    </VisualState>
                </VisualStateGroup>
            </VisualStateGroupList>
        </Setter>
    </Style>
</ContentPage.Resources>

<Label Text="Orientation test" Style="{StaticResource orientationStyle}" />
```

---

###  **Exercice 15 : Style + Trigger intégré**

**Objectif :** Créer un style avec un `Trigger` interne qui change la couleur si `IsEnabled` est `false`.

**XAML :**

```xml
<ContentPage.Resources>
    <Style x:Key="buttonStyle" TargetType="Button">
        <Setter Property="FontSize" Value="20" />
        <Setter Property="BackgroundColor" Value="RoyalBlue" />
        <Style.Triggers>
            <Trigger TargetType="Button" Property="IsEnabled" Value="False">
                <Setter Property="BackgroundColor" Value="Gray" />
            </Trigger>
        </Style.Triggers>
    </Style>
</ContentPage.Resources>

<Button Text="Envoyer" Style="{StaticResource buttonStyle}" IsEnabled="False" />
```

---

###  **Exercice 16 : `CollectionView` avec item sélectionné stylisé**

**Objectif :** Appliquer un style visuel à un item sélectionné.

**XAML :**

```xml
<CollectionView SelectionMode="Single" x:Name="list">
    <CollectionView.ItemTemplate>
        <DataTemplate>
            <Grid Padding="10">
                <VisualStateManager.VisualStateGroups>
                    <VisualStateGroup>
                        <VisualState Name="Selected">
                            <VisualState.Setters>
                                <Setter Property="BackgroundColor" Value="LightBlue" />
                            </VisualState.Setters>
                        </VisualState>
                        <VisualState Name="Normal">
                            <VisualState.Setters>
                                <Setter Property="BackgroundColor" Value="White" />
                            </VisualState.Setters>
                        </VisualState>
                    </VisualStateGroup>
                </VisualStateManager.VisualStateGroups>
                <Label Text="{Binding}" />
            </Grid>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

---

###  **Exercice 17 : TemplateSelector + VisualStates**

**Objectif :** Afficher un `Label` dont l’état “Disponible” ou “Indisponible” influence la couleur.

**Modèle :**

```csharp
public class Product
{
    public string Name { get; set; }
    public bool Disponible { get; set; }
}
```

**XAML (simplifié avec DataTrigger) :**

```xml
<CollectionView ItemsSource="{Binding Produits}">
    <CollectionView.ItemTemplate>
        <DataTemplate>
            <Label Text="{Binding Name}">
                <Label.Style>
                    <Style TargetType="Label">
                        <Setter Property="TextColor" Value="Red" />
                        <Style.Triggers>
                            <DataTrigger Binding="{Binding Disponible}" Value="True">
                                <Setter Property="TextColor" Value="Green" />
                            </DataTrigger>
                        </Style.Triggers>
                    </Style>
                </Label.Style>
            </Label>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

---

###  **Exercice 18 : MultiDataTrigger (Champ non vide + Case cochée)**

**Objectif :** Valider un champ si l’utilisateur remplit un champ *et* coche une case.

**XAML :**

```xml
<StackLayout>
    <Entry x:Name="entryName" />
    <CheckBox x:Name="chkTerms" />
    <Button Text="Valider">
        <Button.Style>
            <Style TargetType="Button">
                <Setter Property="IsEnabled" Value="False" />
                <Style.Triggers>
                    <MultiTrigger TargetType="Button">
                        <MultiTrigger.Conditions>
                            <BindingCondition Binding="{Binding Source={x:Reference entryName}, Path=Text.Length}" Value="0" />
                            <BindingCondition Binding="{Binding Source={x:Reference chkTerms}, Path=IsChecked}" Value="True" />
                        </MultiTrigger.Conditions>
                        <Setter Property="IsEnabled" Value="True" />
                    </MultiTrigger>
                </Style.Triggers>
            </Style>
        </Button.Style>
    </Button>
</StackLayout>
```

---

###  **Exercice 19 : DataTrigger inversé sur Entry vide**

**Objectif :** Afficher un Label d’erreur si un champ est vide.

**XAML :**

```xml
<Entry x:Name="entryNom" Placeholder="Nom" />
<Label Text="Champ requis">
    <Label.Style>
        <Style TargetType="Label">
            <Setter Property="IsVisible" Value="False" />
            <Style.Triggers>
                <DataTrigger Binding="{Binding Source={x:Reference entryNom}, Path=Text.Length}" Value="0">
                    <Setter Property="IsVisible" Value="True" />
                    <Setter Property="TextColor" Value="Red" />
                </DataTrigger>
            </Style.Triggers>
        </Style>
    </Label.Style>
</Label>
```

---

###  **Exercice 20 : VisualStates avec animation personnalisée**

**Objectif :** Appliquer une animation entre états `Valid` et `Invalid`.

**C# (simplifié, effet visuel par changement d’opacité ou de couleur) :**

```csharp
private async void OnTextChanged(object sender, TextChangedEventArgs e)
{
    bool isValid = Regex.IsMatch(e.NewTextValue ?? "", @"^\d{4}$");
    if (isValid)
    {
        await myFrame.FadeTo(1, 250);
        VisualStateManager.GoToState(myFrame, "Valid");
    }
    else
    {
        await myFrame.FadeTo(0.5, 250);
        VisualStateManager.GoToState(myFrame, "Invalid");
    }
}
```

**XAML :**

```xml
<Frame x:Name="myFrame" Padding="10" BackgroundColor="LightGray">
    <VisualStateManager.VisualStateGroups>
        <VisualStateGroup>
            <VisualState Name="Valid">
                <VisualState.Setters>
                    <Setter Property="BackgroundColor" Value="Green" />
                </VisualState.Setters>
            </VisualState>
            <VisualState Name="Invalid">
                <VisualState.Setters>
                    <Setter Property="BackgroundColor" Value="Pink" />
                </VisualState.Setters>
            </VisualState>
        </VisualStateGroup>
    </VisualStateManager.VisualStateGroups>
    <Entry TextChanged="OnTextChanged" Placeholder="Code à 4 chiffres" />
</Frame>
```

---


