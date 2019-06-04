using System;
using Bindable.Linq.Dependencies.Instances;
using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Dependencies.Defintions
{
    public sealed class ItemDependencyDefinition : IDependencyDefinition
    {
        public string ParameterName
        {
            get;
            set;
        }

        public string PropertyPath
        {
            get;
            set;
        }

        public ItemDependencyDefinition(string propertyPath)
        {
            PropertyPath = propertyPath;
        }

        public ItemDependencyDefinition(string propertyPath, string parameterName)
        {
            PropertyPath = propertyPath;
            ParameterName = parameterName;
        }

        public bool AppliesToCollections()
        {
            return true;
        }

        public bool AppliesToSingleElement()
        {
            return false;
        }

        IDependency IDependencyDefinition.ConstructForElement<TElement>(TElement sourceElement, IPathNavigator pathNavigator)
        {
            return ConstructForElement(sourceElement, pathNavigator);
        }

        public IDependency ConstructForCollection<TElement>(IBindableCollection<TElement> sourceElements, IPathNavigator pathNavigator)
        {
            return new ItemDependency<TElement>(PropertyPath, sourceElements, pathNavigator);
        }

        public IDependency ConstructForElement<TElement>(TElement sourceElement, IPathNavigator pathNavigator)
        {
            throw new NotSupportedException();
        }

        public override string ToString()
        {
            return $"{GetType().Name}: '{PropertyPath}' on '{ParameterName}'";
        }
    }
}