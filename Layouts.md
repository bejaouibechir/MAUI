# Exploration des Layouts

## StackLayout
**Introduction :**
Le **StackLayout** est un layout simple qui empile les vues les unes sur les autres, selon l'axe principal (vertical ou horizontal).

Propriétés couramment utilisées :

- **Spacing:** Espacement entre les vues.
- **HorizontalOptions**, **VerticalOptions**: Contrôle l'alignement et le comportement de mise en page.
  
Événements :

Pas d'événements spécifiques associés directement à StackLayout.
**Exemple en XAML :**

```  XML
<StackLayout Spacing="10">
    <Label Text="Première vue" />
    <Label Text="Deuxième vue" />
    <Button Text="Cliquer ici" />
</StackLayout>
```

**Exemple en C# (mode dynamique) :**

``` CSharp
var stackLayout = new StackLayout
{
    Spacing = 10,
    Children =
    {
        new Label { Text = "Première vue" },
        new Label { Text = "Deuxième vue" },
        new Button { Text = "Cliquer ici" }
    }
};
```

**Limites :**
**StackLayout** est limité à un alignement linéaire simple, ne permettant pas un positionnement absolu ou une flexibilité complexe dans la mise en page.

**Comment FlexLayout comble les limites :**
**FlexLayout** offre une flexibilité supplémentaire en permettant un placement plus dynamique des vues avec des options comme **Direction**, **JustifyContent**, **AlignItems**, et la capacité de gérer des mises en page plus complexes avec Wrap.

## AbsoluteLayout
Introduction :
AbsoluteLayout permet de positionner précisément les vues en utilisant des coordonnées absolues sur l'écran.

**Propriétés couramment utilisées :**

- **LayoutBounds**, **LayoutFlags**: Positionnement précis des vues.
- **HorizontalOptions**, **VerticalOptions**: Contrôle l'alignement et le comportement de mise en page.

**Événements :**

Pas d'événements spécifiques associés directement à AbsoluteLayout.
Exemple en XAML :

```  XML
<AbsoluteLayout>
    <Label Text="Haut gauche" AbsoluteLayout.LayoutBounds="0, 0, 150, 50" />
    <Label Text="Bas droite" AbsoluteLayout.LayoutBounds="1, 1, 150, 50"
           AbsoluteLayout.LayoutFlags="PositionProportional" />
</AbsoluteLayout>
```
**Exemple en C# (mode dynamique) :**

``` CSharp
var absoluteLayout = new AbsoluteLayout();
var topLeftLabel = new Label { Text = "Haut gauche" };
AbsoluteLayout.SetLayoutBounds(topLeftLabel, new Rectangle(0, 0, 150, 50));
absoluteLayout.Children.Add(topLeftLabel);

var bottomRightLabel = new Label { Text = "Bas droite" };
AbsoluteLayout.SetLayoutBounds(bottomRightLabel, new Rectangle(1, 1, 150, 50));
AbsoluteLayout.SetLayoutFlags(bottomRightLabel, AbsoluteLayoutFlags.PositionProportional);
absoluteLayout.Children.Add(bottomRightLabel);
```

**Limites :**
AbsoluteLayout peut devenir difficile à maintenir lors de changements de résolution ou de taille d'écran, et ne s'adapte pas automatiquement au contenu ou aux changements dynamiques.

**Comment FlexLayout comble les limites :**
**FlexLayout** offre une flexibilité dynamique tout en permettant un alignement précis avec une gestion plus intuitive de l'espace et de l'adaptabilité aux différentes tailles d'écran.

## FlexLayout

Introduction :
**FlexLayout** est un layout flexible qui permet d'organiser les vues en lignes ou en colonnes, offrant une adaptabilité aux différentes tailles d'écran.

Propriétés couramment utilisées :

- **Direction**: Orientation des éléments (Row, Column).
- **JustifyContent**: Alignement horizontal des éléments.
- **AlignItems**: Alignement vertical des éléments.
- **Wrap**: Gestion des lignes supplémentaires (Wrap ou NoWrap).

Événements :

Pas d'événements spécifiques associés directement à FlexLayout.

**Exemple en XAML :**

```  XML
<FlexLayout Direction="Row" JustifyContent="SpaceAround" AlignItems="Center" Wrap="Wrap">
    <Label Text="Élément 1" FlexLayout.Grow="1" />
    <Label Text="Élément 2" FlexLayout.Grow="2" />
    <Label Text="Élément 3" FlexLayout.Grow="1" />
</FlexLayout>
```

**Exemple en C# (mode dynamique) :**

``` CSharp
var flexLayout = new FlexLayout
{
    Direction = FlexDirection.Row,
    JustifyContent = FlexJustify.SpaceAround,
    AlignItems = FlexAlignItems.Center,
    Wrap = FlexWrap.Wrap
};

flexLayout.Children.Add(new Label { Text = "Élément 1", FlexLayout.Grow = 1 });
flexLayout.Children.Add(new Label { Text = "Élément 2", FlexLayout.Grow = 2 });
flexLayout.Children.Add(new Label { Text = "Élément 3", FlexLayout.Grow = 1 });
```

**Limites :**
Bien que flexible, **FlexLayout** peut nécessiter des configurations complexes pour des mises en page très spécifiques, notamment lorsqu'il s'agit de contrôler précisément l'alignement des éléments dans des structures complexes.

**Comment Grid comble les limites :**
Grid offre une structure de grille plus rigide mais permet un contrôle précis et facile du placement des éléments en lignes et colonnes, idéal pour des mises en page complexes nécessitant une organisation stricte et prévisible.

## Grid
Introduction :
**Grid** est un layout qui divise l'interface utilisateur en lignes et colonnes, permettant un positionnement précis des vues.

**Propriétés couramment utilisées :**

- **RowDefinitions**, **ColumnDefinitions**: Définition des lignes et des colonnes.
- **Grid.Row**,**Grid.Column**: Placement des vues dans la grille.
- **Grid.RowSpan**, **Grid.ColumnSpan**: Fusion de cellules.

Événements :

Pas d'événements spécifiques associés directement à Grid.
**Exemple en XAML :**

```  XML
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto" />
        <RowDefinition Height="*" />
    </Grid.RowDefinitions>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="Auto" />
        <ColumnDefinition Width="*" />
    </Grid.ColumnDefinitions>

    <Label Text="En-tête" Grid.Row="0" Grid.Column="0" Grid.ColumnSpan="2" />
    <Label Text="Colonne 1" Grid.Row="1" Grid.Column="0" />
    <Label Text="Colonne 2" Grid.Row="1" Grid.Column="1" />
</Grid>
```

**Exemple en C# (mode dynamique) :**

``` CSharp
var grid = new Grid();
grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

var headerLabel = new Label { Text = "En-tête" };
Grid.SetRow(headerLabel, 0);
Grid.SetColumnSpan(headerLabel, 2);
grid.Children.Add(headerLabel);

var col1Label = new Label { Text = "Colonne 1" };
Grid.SetRow(col1Label, 1);
Grid.SetColumn(col1Label, 0);
grid.Children.Add(col1Label);

var col2Label = new Label { Text = "Colonne 2" };
Grid.SetRow(col2Label, 1);
Grid.SetColumn(col2Label, 1);
grid.Children.Add(col2Label);
```
**Limites :**
Bien que puissant, Grid peut sembler trop rigide pour des mises en page simples ou flexibles qui nécessitent une gestion dynamique de l'espace ou des lignes.

**Conclusion :**
En suivant cet exercice, les étudiants peuvent comprendre les forces et les limitations de chaque layout MAUI, ainsi que comment choisir le bon layout en fonction des exigences spécifiques de leur interface utilisateur, en passant du plus simple (StackLayout) au plus complexe (Grid) tout en explorant les alternatives intermédiaires (AbsoluteLayout et FlexLayout).
