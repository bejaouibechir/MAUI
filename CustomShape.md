#  **Création d’un Shape personnalisé pas à pas dans MAUI**

#  **Les étapes**

###  Étape 1 : Shape personnalisé

Créer ta classe et définis `GetPath()` pour dessiner ton chemin.

---

###  Étape 2 : Handler

 - Créer un handler (`AnnotationShapeHandler`).
 - Déclarer un `Mapper` pour les propriétés personnalisées.
 - Forcer la mise à jour du dessin quand une propriété change.

---

###  Étape 3 : Enregistrement

 Dans `MauiProgram.cs`, tu ajoutes :

```csharp
handlers.AddHandler(typeof(AnnotationShape), typeof(AnnotationShapeHandler));
```

---

###  Étape 4 : Utilisation

 Ajouter ton shape custom dans XAML, comme n’importe quel contrôle natif.


---

##  **1️ Créer la classe Shape personnalisée**

###  AnnotationShape.cs

```csharp
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace MauiApp1;

public class AnnotationShape : Shape
{
    public float ArrowWidth { get; set; } = 10;
    public float ArrowHeight { get; set; } = 20;
    public float BoxWidth { get; set; } = 100;
    public float BoxHeight { get; set; } = 60;

    public override PathF GetPath()
    {
        var path = new PathF();

        path.MoveTo(0, 0);
        path.LineTo(BoxWidth, 0);
        path.LineTo(BoxWidth, BoxHeight - ArrowHeight);
        path.LineTo((BoxWidth + ArrowWidth) / 2, BoxHeight - ArrowHeight);
        path.LineTo(BoxWidth / 2, BoxHeight);
        path.LineTo((BoxWidth - ArrowWidth) / 2, BoxHeight - ArrowHeight);
        path.LineTo(0, BoxHeight - ArrowHeight);
        path.Close();

        return path;
    }
}
```

---

##  **2️ Créer le Handler**

###  AnnotationShapeHandler.cs

```csharp
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Handlers;

namespace MauiApp1;

public partial class AnnotationShapeHandler : ShapeViewHandler
{
    public AnnotationShapeHandler() : base(Mapper)
    {
    }

    public static IPropertyMapper<Shape, AnnotationShapeHandler> Mapper =
        new PropertyMapper<Shape, AnnotationShapeHandler>(ShapeViewHandler.Mapper)
        {
            [nameof(AnnotationShape.ArrowWidth)] = MapAnnotationShape,
            [nameof(AnnotationShape.ArrowHeight)] = MapAnnotationShape,
            [nameof(AnnotationShape.BoxWidth)] = MapAnnotationShape,
            [nameof(AnnotationShape.BoxHeight)] = MapAnnotationShape,
        };

    static void MapAnnotationShape(AnnotationShapeHandler handler, Shape shape)
    {
        // Force la mise à jour du chemin
        handler.UpdateValue(nameof(Shape.Data));
    }
}
```

---

##  **3 Enregistrer le Handler dans MauiProgram.cs**

### 📄 MauiProgram.cs

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using MauiApp1;

namespace MauiApp1;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        //  Enregistrement du handler
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler(typeof(AnnotationShape), typeof(AnnotationShapeHandler));
        });

        return builder.Build();
    }
}
```

---

##  **4️ Utiliser dans XAML**

### 📄 MainPage.xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MauiApp1"
             x:Class="MauiApp1.MainPage">

    <VerticalStackLayout Spacing="20" Padding="20">

        <Label Text="Shape personnalisé : AnnotationShape" 
               FontSize="20" />

        <local:AnnotationShape 
            Stroke="Black"
            StrokeThickness="2"
            Fill="LightBlue"
            BoxWidth="120"
            BoxHeight="80"
            ArrowWidth="20"
            ArrowHeight="30"
            HorizontalOptions="Center"
            VerticalOptions="Center" />
    </VerticalStackLayout>
</ContentPage>
```
