using Bindable.Linq.Dependencies.Instances;
using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Dependencies.Defintions
{
    public sealed class ExternalDependencyDefinition : IDependencyDefinition
    {
        public string PropertyPath
        {
            get;
            set;
        }

        public object TargetObject
        {
            get;
            set;
        }

        public ExternalDependencyDefinition(string propertyPath, object targetObject)
        {
            PropertyPath = propertyPath;
            TargetObject = targetObject;
        }

        public bool AppliesToCollections()
        {
            return true;
        }

        public bool AppliesToSingleElement()
        {
            return true;
        }

        public IDependency ConstructForCollection<TElement>(IBindableCollection<TElement> sourceElements, IPathNavigator pathNavigator)
        {
            return new ExternalDependency(TargetObject, PropertyPath, pathNavigator);
        }

        public IDependency ConstructForElement<TElement>(TElement sourceElement, IPathNavigator pathNavigator)
        {
            return new ExternalDependency(TargetObject, PropertyPath, pathNavigator);
        }

        public override string ToString()
        {
            return $"{GetType().Name}: '{PropertyPath}' on '{TargetObject.GetType().Name}'";
        }
    }
}