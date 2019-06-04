using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Framework
{
    public interface IEntityStore<TEntity, TIdentity>
    {
        IBindableCollection<TEntity> GetAll();

        void Add(TEntity entity);

        void Remove(TEntity entity);

        TEntity Get(TIdentity identity);
    }
}