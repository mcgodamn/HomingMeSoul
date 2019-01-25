namespace BA_Studio.StatePattern
{
    public interface ITimedState<T> where T : class
	{
		float EnteredTime { get; }
	}
}