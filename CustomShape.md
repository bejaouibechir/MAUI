# Implémentation d'une forme personalisée AnnotationShape

Création d'un contrôle d'annotation simple avec une flèche, souvent utilisé dans les applications de dessin ou de marqueur d'image :

``` CSharp 

using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Shapes;

public class AnnotationShape : Shape
{
    public float ArrowWidth { get; set; } = 10;
    public float ArrowHeight { get; set; } = 20;
    public float BoxWidth { get; set; } = 100;
    public float BoxHeight { get; set; } = 60;

    public override void Draw(ICanvas canvas, RectangleF dirtyRect)
    {
        var path = new Path();
        
        // Définir le chemin du rectangle de base avec une flèche au-dessus
        path.MoveTo(0, 0);
        path.LineTo(BoxWidth, 0);
        path.LineTo(BoxWidth, BoxHeight - ArrowHeight);
        path.LineTo((BoxWidth + ArrowWidth) / 2, BoxHeight - ArrowHeight);
        path.LineTo(BoxWidth / 2, BoxHeight);
        path.LineTo((BoxWidth - ArrowWidth) / 2, BoxHeight - ArrowHeight);
        path.LineTo(0, BoxHeight - ArrowHeight);
        path.Close();

        // Remplir le rectangle de base
        canvas.FillPath(path, Colors.LightBlue);

        // Dessiner le contour du rectangle
        canvas.DrawPath(path, Colors.Black, 2);

        // Dessiner le texte d'annotation
        canvas.DrawText("Annotation", 10, 10, Colors.Black, null);
    }

    public override bool Contains(float x, float y)
    {
        // Implémentation facultative pour détecter si un point est contenu dans la forme
        return false;
    }
}

```

**Dans cet exemple :**

**AnnotationShape** dérive de Shape et surcharge les méthodes **Draw** et éventuellement **Contains**.

**Draw**  définit un chemin (Path) qui représente une forme de boîte avec une flèche au-dessus, utilisée pour annoter une zone spécifique.
Le chemin est dessiné avec une couleur de remplissage (LightBlue) et un contour noir (Colors.Black).

Un texte "Annotation" est également dessiné à l'intérieur de la forme pour indiquer son but.

Ce type de forme personnalisée peut être utilisé dans une application de dessin pour permettre aux utilisateurs de marquer et d'annoter des images de manière visuelle et efficace.

Pour illustrer l'utilisation de la forme personnalisée AnnotationShape que nous avons définie précédemment, voici comment vous pourriez l'intégrer dans une application MAUI simple pour annoter une image :

Supposons que vous avez une page MAUI avec une image et vous souhaitez permettre à l'utilisateur d'ajouter une annotation au-dessus de cette image. Voici comment vous pourriez procéder :

## Création de la Forme Personnalisée AnnotationShape :

Définissez la classe **AnnotationShape** comme nous l'avons fait précédemment.

## Utilisation de AnnotationShape dans une Vue MAUI :
Voici un exemple simplifié d'utilisation dans une vue MAUI où vous affichez une image et ajoutez une annotation au clic de l'utilisateur :

**en C#**

``` CSharp 
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

public class AnnotationPage : ContentPage
{
    public AnnotationPage()
    {
        var image = new Image
        {
            Source = "sampleimage.jpg", // Remplacez par votre image réelle
            Aspect = Aspect.AspectFit,
            Margin = new Thickness(20)
        };

        var annotationShape = new AnnotationShape
        {
            ArrowWidth = 15,
            ArrowHeight = 30,
            BoxWidth = 150,
            BoxHeight = 80,
            Margin = new Thickness(50, 100, 0, 0)
        };

        var annotationButton = new Button
        {
            Text = "Add Annotation",
            Margin = new Thickness(20)
        };
        annotationButton.Clicked += (sender, e) =>
        {
            // Ajouter l'annotation au dessus de l'image
            image.Canvas.Children.Add(annotationShape);
            image.InvalidateMeasure();
        };

        Content = new StackLayout
        {
            Children =
            {
                image,
                annotationButton
            }
        };
    }
}

```

** en XAML: **

Définir l'interface utilisateur en XAML (AnnotationPage.xaml)

``` XML
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiApp1.AnnotationPage"
             BackgroundColor="White">

    <StackLayout VerticalOptions="FillAndExpand" HorizontalOptions="FillAndExpand">
        <Image x:Name="image" Source="sampleimage.jpg" Aspect="AspectFit" Margin="20" />
        <Button Text="Add Annotation" Clicked="OnAddAnnotationClicked" Margin="20" />
    </StackLayout>

</ContentPage>
```


## Code-behind pour AnnotationPage (AnnotationPage.xaml.cs):

``` CSharp 
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

public partial class AnnotationPage : ContentPage
{
    public AnnotationPage()
    {
        InitializeComponent();
    }

    private void OnAddAnnotationClicked(object sender, EventArgs e)
    {
        var annotationShape = new AnnotationShape
        {
            ArrowWidth = 15,
            ArrowHeight = 30,
            BoxWidth = 150,
            BoxHeight = 80,
            Margin = new Thickness(50, 100, 0, 0)
        };

        // Ajouter l'annotation au dessus de l'image
        image.Canvas.Children.Add(annotationShape);
        image.InvalidateMeasure();
    }
}
```

### Explication :**
### XAML (AnnotationPage.xaml) :

Image affiche une image (sampleimage.jpg dans cet exemple).
Button est utilisé pour déclencher l'ajout de l'annotation.
L'événement Clicked du bouton est lié à la méthode OnAddAnnotationClicked dans le code-behind.
Code-behind (AnnotationPage.xaml.cs) :

Lorsque le bouton est cliqué (OnAddAnnotationClicked), un nouvel AnnotationShape est créé avec les dimensions spécifiées.
AnnotationShape est ajouté au Canvas de l'image (image.Canvas.Children.Add(annotationShape)).
image.InvalidateMeasure() est appelé pour forcer le recalcul des mesures et le redessin de l'image avec l'annotation ajoutée.
Ce modèle permet d'ajouter dynamiquement des annotations personnalisées au-dessus d'une image dans une application MAUI en réponse à l'interaction utilisateur. Assurez-vous d'adapter les chemins d'accès aux images et les dimensions des formes selon vos besoins spécifiques.








