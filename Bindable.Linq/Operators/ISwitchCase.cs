namespace Bindable.Linq.Operators
{
    public interface ISwitchCase<TInput, TResult>
    {
        bool Evaluate(TInput input);

        TResult Return(TInput input);
    }
}