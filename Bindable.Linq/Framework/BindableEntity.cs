using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Framework
{
    public class BindableIdentityMap<TEntity, TIdentity> : DispatcherBound, IIdentityMap<TEntity> where TEntity : class
    {
        private readonly IEntityStore<TEntity, TIdentity> _store;

        private readonly IIdentifier<TEntity, TIdentity> _identifier;

        public BindableIdentityMap(IDispatcher dispatcher, IIdentifier<TEntity, TIdentity> identifier, IEntityStore<TEntity, TIdentity> store)
            : base(dispatcher)
        {
            _identifier = identifier;
            _store = store;
        }

        public TEntity Store(TEntity entity)
        {
            AssertDispatcherThread();
            var identity = _identifier.GetIdentity(entity);
            var val = _store.Get(identity);
            if (val == null)
            {
                _store.Add(entity);
            }
            return entity;
        }

        public IBindableCollection<TEntity> GetAll()
        {
            AssertDispatcherThread();
            return _store.GetAll();
        }
    }

}