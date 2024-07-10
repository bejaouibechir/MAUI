# Localisation d'une application MAUI

Il faut tout d'abord créer deux fichiers ***AppResources.resx***  et ***AppResources.resx*** sous ***Resources/Strings***   contenant les ressources localisées

``` XAML

  <ScrollView>
      <VerticalStackLayout
          Padding="30,0"
          Spacing="25">
          <Image
              Source="dotnet_bot.png"
              HeightRequest="185"
              Aspect="AspectFit"
              SemanticProperties.Description="dot net bot in a race car number eight" />

          <Label 
              Text="Hello, World!"
              Style="{StaticResource Headline}"
              SemanticProperties.HeadingLevel="Level1" />
          <!-- Le label cible de l'operation de localisation -->
          <Label x:Uid="Greeting" x:Name="GreetingLabel" />

          <Picker  Background="Bisque" SelectedIndexChanged="Picker_SelectedIndexChanged">
              <Picker.ItemsSource>
                  <x:Array Type="{x:Type x:String}">
                      <x:String>Français</x:String>
                      <x:String>English</x:String>
                  </x:Array>
              </Picker.ItemsSource>
          </Picker>
          
          <Button
              x:Name="CounterBtn"
              Text="Click me" 
              Background="Red"
              FontSize="24"
              SemanticProperties.Hint="Counts the number of times you click"
              Clicked="OnCounterClicked"
              HorizontalOptions="Fill" />
      </VerticalStackLayout>
  </ScrollView>
```

et en code behind de la page

``` CSharp
using System.Globalization;
using System.Resources;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            SetLocalzation();
        }

        private void SetLocalzation()
        {
            var resourceManager = new ResourceManager("MauiApp1.Resources.Strings.AppResources", typeof(MainPage).Assembly);
            GreetingLabel.Text = resourceManager.GetString("Greeting.Text",Thread.CurrentThread.CurrentUICulture);
#if first
            try
            {
                var culture = new CultureInfo("fr"); // Change to "en" for English
                var resourceManager = new ResourceManager("MauiApp1.Resources.Strings.AppResources", typeof(MainPage).Assembly);

                // Apply localized string to the label
                GreetingLabel.Text = resourceManager.GetString("Greeting", culture);
            }
            catch (Exception ex)
            {
                // Log or display the exception message for debugging purposes
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                GreetingLabel.Text = "Error loading resource";
            }
#endif
        }

      
        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
   
            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

       
        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            if(picker.SelectedIndex == 0)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            }
                SetLocalzation();
        }
    }

}


```

