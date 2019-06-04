using System.Linq;

namespace Bindable.Linq.Interfaces.Internal
{
    public interface IBindableGrouping<TKey, TElement> : IGrouping<TKey, TElement>, IBindableCollection<TElement>
    {
    }
}