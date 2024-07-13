# Implémentation Personnalisée de l'Interface INavigation pour les Tests Unitaires

Cette implémentation de **INavigation** permet de simuler la navigation dans vos tests unitaires, garantissant que votre logique de navigation fonctionne correctement sans nécessiter une infrastructure réelle d'interface utilisateur. En utilisant ce mock, vous pouvez facilement écrire des tests unitaires robustes et fiables pour votre application MAUI.

## Note: 
Il faut ajouter une référence vers Microsoft.MAUI.Controls dans le projet de test 

## Explications
- **MockNavigation:**

Implémenter **INavigation** et maintient une pile de navigation et une pile modale en mémoire.
Fournit des méthodes pour pousser, retirer, insérer et supprimer des pages de ces piles.

- **Tests Unitaires:**

Utiliser **NUnit** pour vérifier le comportement de la navigation.
Tests pour ***PushAsync***, ***PopAsync***, ***PushModalAsync***, ***PopModalAsync***, ***RemovePage***, et ***InsertPageBefore***.

### Étape 1 : Implémentation de INavigation

Nous allons créer une classe **MockNavigation** qui implémente **INavigation** et *maintient une pile de navigation en mémoire*. Cette classe permettra de simuler la navigation sans réellement afficher les pages.

``` CSharp
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

public class MockNavigation : INavigation
{
    private Stack<Page> _navigationStack = new Stack<Page>();
    private Stack<Page> _modalStack = new Stack<Page>();

    public IReadOnlyList<Page> NavigationStack => _navigationStack.ToList();
    public IReadOnlyList<Page> ModalStack => _modalStack.ToList();

    public Task PushAsync(Page page)
    {
        _navigationStack.Push(page);
        return Task.CompletedTask;
    }

    public Task<Page> PopAsync()
    {
        if (_navigationStack.Count > 1)
        {
            return Task.FromResult(_navigationStack.Pop());
        }
        return Task.FromResult<Page>(null);
    }

    public Task PushModalAsync(Page page)
    {
        _modalStack.Push(page);
        return Task.CompletedTask;
    }

    public Task<Page> PopModalAsync()
    {
        if (_modalStack.Any())
        {
            return Task.FromResult(_modalStack.Pop());
        }
        return Task.FromResult<Page>(null);
    }

    public void RemovePage(Page page)
    {
        _navigationStack = new Stack<Page>(_navigationStack.Where(p => p != page));
    }

    public void InsertPageBefore(Page page, Page before)
    {
        var stack = new Stack<Page>(_navigationStack.Reverse());
        var newStack = new Stack<Page>();

        while (stack.Any())
        {
            var current = stack.Pop();
            if (current == before)
            {
                newStack.Push(page);
            }
            newStack.Push(current);
        }

        _navigationStack = new Stack<Page>(newStack.Reverse());
    }

    public Task<Page> PopToRootAsync()
    {
        while (_navigationStack.Count > 1)
        {
            _navigationStack.Pop();
        }
        return Task.FromResult(_navigationStack.Peek());
    }

    public Task PushAsync(Page page, bool animated)
    {
        return PushAsync(page);
    }

    public Task<Page> PopAsync(bool animated)
    {
        return PopAsync();
    }

    public Task PushModalAsync(Page page, bool animated)
    {
        return PushModalAsync(page);
    }

    public Task<Page> PopModalAsync(bool animated)
    {
        return PopModalAsync();
    }
}

```
### Étape 2 : Utilisation dans les Tests Unitaires

Nous allons maintenant écrire un test unitaire pour vérifier le comportement de la navigation. Nous utiliserons une framework de test comme NUnit pour cela.

### Exemple de Test Unitaire avec NUnit:

``` CSharp
 [TestFixture]
 public class NavigationTests
 {
     private MockNavigation _mockNavigation;
     private Page _mainPage;
     private Page _page1;
     private Page _page2;


     [SetUp]
     public void Setup()
     {
         _mockNavigation = new MockNavigation();
         _mainPage = new ContentPage { Title = "MainPage" };
         _page1 = new ContentPage { Title = "Page1" };
         _page2 = new ContentPage { Title = "Page2" };
         _mockNavigation.PushAsync(_mainPage);
     }

     [Test]
     public async Task PushAsync_AddsPageToStack()
     {
         await _mockNavigation.PushAsync(_page1);
         Assert.That(2, Is.EqualTo(_mockNavigation.NavigationStack.Count));
         Assert.That(_page1,Is.EqualTo(_mockNavigation.NavigationStack.Last()));
     }

     [Test]
     public async Task PopAsync_RemovesPageFromStack()
     {
         await _mockNavigation.PushAsync(_page1);
         await _mockNavigation.PopAsync();
         Assert.That(1, Is.EqualTo(_mockNavigation.NavigationStack.Count));
         Assert.That(_mainPage, Is.EqualTo(_mockNavigation.NavigationStack.Last()));
     }

     [Test]
     public async Task PushModalAsync_AddsPageToModalStack()
     {
         await _mockNavigation.PushModalAsync(_page1);
         Assert.That(1, Is.EqualTo(_mockNavigation.ModalStack.Count));
         Assert.That(_page1, Is.EqualTo(_mockNavigation.ModalStack.Last()));
     }

     [Test]
     public async Task PopModalAsync_RemovesPageFromModalStack()
     {
         await _mockNavigation.PushModalAsync(_page1);
         await _mockNavigation.PopModalAsync();
         Assert.That(0, Is.EqualTo(_mockNavigation.ModalStack.Count));
     }

     [Test]
     public void RemovePage_RemovesSpecificPage()
     {
         _mockNavigation.PushAsync(_page1);
         _mockNavigation.PushAsync(_page2);
         _mockNavigation.RemovePage(_page1);
         Assert.That(2, Is.EqualTo(_mockNavigation.NavigationStack.Count));
         Assert.IsFalse(_mockNavigation.NavigationStack.Contains(_page1));
     }

     [Test]
     public void InsertPageBefore_InsertsPageCorrectly()
     {
         _mockNavigation.PushAsync(_page1);
         _mockNavigation.InsertPageBefore(_page2, _page1);
         Assert.That(3, Is.EqualTo(_mockNavigation.NavigationStack.Count));
         Assert.That(_page2, Is.EqualTo(_mockNavigation.NavigationStack.ElementAt(1)));
     }

 }

```
