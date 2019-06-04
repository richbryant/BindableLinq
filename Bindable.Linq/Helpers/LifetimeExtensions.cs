namespace Bindable.Linq.Helpers
{
    internal static class LifetimeExtensions
    {
        public static TItem KeepAlive<TItem>(this TItem item, LifetimeCouplings lifetime)
        {
            lifetime.Add(item);
            return item;
        }
    }
}