namespace Bindable.Linq.Dependencies
{
    public interface IAcceptsDependencies
    {
        void AcceptDependency(IDependencyDefinition definition);
    }
}