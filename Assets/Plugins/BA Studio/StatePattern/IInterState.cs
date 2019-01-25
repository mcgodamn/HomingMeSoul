namespace BA_Studio.StatePattern
{
    public interface IInterState<T> where T : class
	{
		State<T> NextState { get; }
	}
}