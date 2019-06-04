namespace Bindable.Linq.Operators
{
    public interface ISwitchDeclaration<TInput>
    {
        ISwitch<TInput, TResult> CreateForResultType<TResult>();
    }

}