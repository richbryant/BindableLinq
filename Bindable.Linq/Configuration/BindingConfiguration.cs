namespace Bindable.Linq.Configuration
{
    public static class BindingConfigurations
    {
        private static readonly IBindingConfiguration _default = new DefaultBindingConfiguration();

        private static readonly IBindingConfiguration _explicitDependenciesOnly = new ExplicitBindingConfiguration();

        public static IBindingConfiguration Default => _default;

        public static IBindingConfiguration ExplicitDependenciesOnly => _explicitDependenciesOnly;
    }
}