namespace Bindable.Linq.Framework
{
    public interface IIdentifier<TEntity, TIdentity>
    {
        TIdentity GetIdentity(TEntity entity);
    }

}