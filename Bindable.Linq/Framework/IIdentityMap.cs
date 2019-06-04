using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Framework
{
    public interface IIdentityMap<TEntity>
    {
        TEntity Store(TEntity entity);

        IBindableCollection<TEntity> GetAll();
    }
}