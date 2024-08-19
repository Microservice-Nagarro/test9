namespace BHF.MS.MyMicroservice.Database.Tests.DbAsyncQueryProvider
{
    internal sealed class InMemoryDbAsyncEnumerator<T>(IEnumerator<T> enumerator) : IAsyncEnumerator<T>
    {
        public void Dispose()
        {
            enumerator.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            Dispose();
            return new ValueTask();
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(Task.Delay(AsyncDbDelayConst.Delay).ContinueWith(t => enumerator.MoveNext()));
        }
        public T Current => enumerator.Current;
    }
}
