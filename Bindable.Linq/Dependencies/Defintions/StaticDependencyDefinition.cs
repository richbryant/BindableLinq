using System;
using System.Reflection;
using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Dependencies.Defintions
{
    public sealed class StaticDependencyDefinition : IDependencyDefinition
    {
        public MemberInfo Member
        {
            get;
            set;
        }

        public string PropertyPath
        {
            get;
            set;
        }

        public StaticDependencyDefinition(string propertyPath, MemberInfo member)
        {
            Member = member;
            PropertyPath = propertyPath;
        }

        public bool AppliesToCollections()
        {
            return false;
        }

        public bool AppliesToSingleElement()
        {
            return false;
        }

        public IDependency ConstructForCollection<TElement>(IBindableCollection<TElement> sourceElements, IPathNavigator pathNavigator)
        {
            throw new NotImplementedException();
        }

        public IDependency ConstructForElement<TElement>(TElement sourceElement, IPathNavigator pathNavigator)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            if (Member.DeclaringType != null)
                return $"{GetType().Name}: '{PropertyPath}' on '{Member.DeclaringType.Name}'";
            return string.Empty;
        }
    }
}