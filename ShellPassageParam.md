# Techniques de passage des paramètres dans un contexte Shell

## Création d'une classe de modèle pour le produit :

Définissons une classe Product avec quelques propriétés pour simuler un produit :

``` CSharp
// Product.cs
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}


```

**Page Main (passage de l'Id) :**

Modifions la page **MainPage** pour afficher une liste de produits et permettre à l'utilisateur de sélectionner un produit pour voir les détails.

**MainPage.xaml:**

``` xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiApp1.Pages.MainPage"
             Title="MainPage">
    <StackLayout>
        <ListView x:Name="ProductsListView" ItemsSource="{Binding Products}" 
                  ItemSelected="ProductsListView_OnItemSelected">
            <ListView.ItemTemplate>
                <DataTemplate>
                    <TextCell Text="{Binding Name}" />
                </DataTemplate>
            </ListView.ItemTemplate>
        </ListView>
    </StackLayout>
</ContentPage>

```

**MainPage.cs:**

``` CSharp 
[XamlCompilation(XamlCompilationOptions.Compile)] //Pour compiler le XAML imediatement 
public partial class MainPage : ContentPage
{


    public MainPage()
	{
		InitializeComponent();

        App.Products = new ObservableCollection<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10.5m },
                new Product { Id = 2, Name = "Product 2", Price = 15.75m },
                new Product { Id = 3, Name = "Product 3", Price = 20m }
            };
        BindingContext = this;
        ProductsListView.ItemsSource = App.Products;    
    }

    private void ProductsListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Product selectedProduct)
        {
            Navigation.PushAsync(new DetailsPage { ProductId = selectedProduct.Id });
        }
    }

}
```

## Page Details (réception de l'Id avec QueryPropertyAttribute) :


Utilisons **QueryPropertyAttribute** dans **DetailsPage** pour récupérer l'Id du produit et afficher ses détails 

``` XML
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MauiApp1.Pages.DetailsPage"
             Title="DetailsPage">

    <StackLayout>
        <Label x:Name="ProductInfo" HorizontalOptions="CenterAndExpand" VerticalOptions="CenterAndExpand" />
    </StackLayout>
</ContentPage>
```


``` CSharp 
namespace MauiApp1.Pages;

[QueryProperty("ProductId", "id")]
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DetailsPage : ContentPage
{
    private int _productId;
    private Product _product;

    public int ProductId
    {
        get => _productId;
        set
        {
            _productId = value;
            LoadProduct(_productId); // Charger le produit lorsque l'Id est défini
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Utiliser _productId pour charger les détails du produit
        // Exemple : Charger et afficher les détails du produit à partir de l'Id
        ProductInfo.Text = $"Le produit séléctionné est {_product.Name} don't le prix est {_product.Price}";
    }

    private void LoadProduct(int productId)
    {
        // Exemple : Simuler la récupération des détails du produit à partir de l'Id
        _product = GetProductById(productId);

        if (_product == null)
        {
            // Exemple : Gérer le cas où le produit n'est pas trouvé
            _product = new Product { Name = "Product not found", Price = 0m };
        }
    }

    private Product GetProductById(int productId)
    {
        // Exemple : Simuler la récupération des détails du produit à partir de l'Id
        return App.Products.FirstOrDefault(p => p.Id == productId);
    }

    public DetailsPage()
	{
		InitializeComponent();
	}  
}

```



