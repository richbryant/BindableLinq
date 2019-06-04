namespace Bindable.Linq.Helpers
{
    internal interface IPropertyReader<TCast>
    {
        TCast GetValue(object input);
    }
}