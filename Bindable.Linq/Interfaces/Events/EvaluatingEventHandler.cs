namespace Bindable.Linq.Interfaces.Events
{
    public delegate void EvaluatingEventHandler<TElement>(object sender, EvaluatingEventArgs<TElement> args);
}