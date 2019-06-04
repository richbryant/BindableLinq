using System;

namespace Bindable.Linq.Dependencies.PathNavigation.Tokens
{
    public interface IToken : IDisposable
    {
        IToken NextToken
        {
            get;
        }

        void AcquireTarget(object target);
    }
}