using System;
using System.Linq;
using Bindable.Linq.Collections;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Framework
{
    public class InMemoryEntityStore<TEntity, TIdentity> : DispatcherBound, IEntityStore<TEntity, TIdentity> where TEntity : class
    {
        private readonly BindableCollection<WeakReference> _itemReferences;

        private readonly IIdentifier<TEntity, TIdentity> _identifier;

        public InMemoryEntityStore(IDispatcher dispatcher, IIdentifier<TEntity, TIdentity> identifier)
            : base(dispatcher)
        {
            _itemReferences = new BindableCollection<WeakReference>(dispatcher);
            _identifier = identifier;
        }

        public IBindableCollection<TEntity> GetAll()
        {
            AssertDispatcherThread();
            return from weak in _itemReferences
                select weak.Target as TEntity into entity
                where entity != null
                select entity;
        }

        public void Add(TEntity entity)
        {
            AssertDispatcherThread();
            _itemReferences.Add(new WeakReference(entity, trackResurrection: true));
        }

        public void Remove(TEntity entity)
        {
            AssertDispatcherThread();
            WeakReference weakReference = (from weak in _itemReferences.ToList()
                where weak.Target == entity
                select weak).FirstOrDefault();
            if (weakReference != null)
            {
                _itemReferences.Remove(weakReference);
            }
        }

        public TEntity Get(TIdentity identity)
        {
            AssertDispatcherThread();
            return (from entity in GetAll().ToList()
                where _identifier.GetIdentity(entity).Equals(identity)
                select entity).FirstOrDefault();
        }
    }
}