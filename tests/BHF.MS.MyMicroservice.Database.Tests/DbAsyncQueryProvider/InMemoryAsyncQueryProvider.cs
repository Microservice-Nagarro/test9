using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace BHF.MS.MyMicroservice.Database.Tests.DbAsyncQueryProvider
{
    internal sealed class InMemoryAsyncQueryProvider<TEntity>(IQueryProvider innerQueryProvider) : IAsyncQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            return new InMemoryAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new InMemoryAsyncEnumerable<TElement>(expression);
        }

        public object? Execute(Expression expression)
        {
            return innerQueryProvider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return innerQueryProvider.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            var result = Execute(expression);

            var expectedResultType = typeof(TResult).GetGenericArguments().FirstOrDefault();
            if (expectedResultType == null)
            {
                return Activator.CreateInstance<TResult>();
            }

            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(expectedResultType)
                .Invoke(null, [result])!;
        }
    }
}
