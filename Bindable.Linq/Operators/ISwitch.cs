namespace Bindable.Linq.Operators
{
    public interface ISwitch<TInput, TResult>
    {
        ISwitch<TInput, TResult> AddCase(ISwitchCase<TInput, TResult> customCase);

        ISwitch<TInput, TResult> AddDefault(ISwitchCase<TInput, TResult> defaultCase);
    }
}