namespace FacetAPI.Runtime
{
    public static class FacetExtensions
    {
        public static void InvokeIf<T>(this IFacetCallback<Action<T>> callback, T arg, Func<T, bool> condition)
        {
            if (condition(arg))
                callback.Invoke(arg);
        }

        public static async Task WatchAndInvokeAsync<T>(
            this IFacetCallback<Action<T>> callback,
            T arg,
            Func<bool> condition,
            TimeSpan checkInterval,
            System.Threading.CancellationToken cancellationToken = default)
        {
            bool previousState = condition();

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(checkInterval, cancellationToken);
                bool currentState = condition();

                if (currentState && !previousState)
                {
                    callback.Invoke(arg);
                }

                previousState = currentState;
            }
        }
    }
}