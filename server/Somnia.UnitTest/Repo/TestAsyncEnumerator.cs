using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Somnia.UnitTest.Repo
{
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public ValueTask<bool> MoveNextAsync()
        {
            throw new System.NotImplementedException();
        }

        public ValueTask DisposeAsync()
        {
            throw new System.NotImplementedException();
        }

        public T Current
        {
            get { return _inner.Current; }
        }

    }
}
