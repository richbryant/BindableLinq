using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Interfaces;
namespace Bindable.Linq.Dependencies
{
    public interface IDependencyDefinition
    {
        bool AppliesToCollections();

        bool AppliesToSingleElement();

        IDependency ConstructForCollection<TElement>(IBindableCollection<TElement> sourceElements, IPathNavigator pathNavigator);

        IDependency ConstructForElement<TElement>(TElement sourceElement, IPathNavigator pathNavigator);
    }
}