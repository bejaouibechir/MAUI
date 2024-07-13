using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

public class CustomNavigation : INavigation
{
    private Stack<Page> _navigationStack = new Stack<Page>();

    public IReadOnlyList<Page> NavigationStack => _navigationStack.ToArray();
    public IReadOnlyList<Page> ModalStack => new List<Page>();

    public Task PushAsync(Page page)
    {
        _navigationStack.Push(page);
        // Code pour afficher la page, par exemple :
        DisplayPage(page);
        return Task.CompletedTask;
    }

    public Task<Page> PopAsync()
    {
        if (_navigationStack.Count > 1)
        {
            var page = _navigationStack.Pop();
            // Code pour retirer la page de l'affichage, par exemple :
            RemovePage(page);
            return Task.FromResult(page);
        }
        return Task.FromResult<Page>(null);
    }

    public Task PushModalAsync(Page page)
    {
        // Implémentation personnalisée pour les modaux si nécessaire
        return Task.CompletedTask;
    }

    public Task<Page> PopModalAsync()
    {
        // Implémentation personnalisée pour les modaux si nécessaire
        return Task.FromResult<Page>(null);
    }

    public void RemovePage(Page page)
    {
        var stack = new Stack<Page>(_navigationStack);
        var newStack = new Stack<Page>();

        while (stack.Count > 0)
        {
            var p = stack.Pop();
            if (p != page)
            {
                newStack.Push(p);
            }
        }

        _navigationStack = new Stack<Page>(newStack);
    }

    public void InsertPageBefore(Page page, Page before)
    {
        var stack = new Stack<Page>(_navigationStack);
        var newStack = new Stack<Page>();

        while (stack.Count > 0)
        {
            var p = stack.Pop();
            if (p == before)
            {
                newStack.Push(page);
            }
            newStack.Push(p);
        }

        _navigationStack = new Stack<Page>(newStack);
    }

    private void DisplayPage(Page page)
    {
        // Code pour afficher la page, par exemple :
        Console.WriteLine("Displaying " + page.Title);
    }

    private void RemovePageFromDisplay(Page page)
    {
        // Code pour retirer la page de l'affichage, par exemple :
        Console.WriteLine("Removing " + page.Title);
    }

    // Les autres méthodes de INavigation doivent également être implémentées
    public Task<Page> PopToRootAsync()
    {
        // Implémentation de PopToRootAsync
        return Task.FromResult<Page>(null);
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
