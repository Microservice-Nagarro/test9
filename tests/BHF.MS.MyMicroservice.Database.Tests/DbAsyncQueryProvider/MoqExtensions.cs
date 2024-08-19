using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language;
using Moq.Language.Flow;

namespace BHF.MS.MyMicroservice.Database.Tests.DbAsyncQueryProvider
{
    internal static class MoqExtensions
    {
        public static IReturnsResult<T> ReturnsDbSet<T, TEntity>(this ISetupGetter<T, DbSet<TEntity>> setupResult, IEnumerable<TEntity> entities, Mock<DbSet<TEntity>>? dbSetMock = null) where T : class where TEntity : class
        {
            dbSetMock ??= new Mock<DbSet<TEntity>>();

            ConfigureMock(dbSetMock, entities);

            return setupResult.Returns(dbSetMock.Object);
        }

        public static IReturnsResult<T> ReturnsDbSet<T, TEntity>(this ISetup<T, DbSet<TEntity>> setupResult, IEnumerable<TEntity> entities, Mock<DbSet<TEntity>>? dbSetMock = null) where T : class where TEntity : class
        {
            dbSetMock ??= new Mock<DbSet<TEntity>>();

            ConfigureMock(dbSetMock, entities);

            return setupResult.Returns(dbSetMock.Object);
        }

        public static ISetupSequentialResult<DbSet<TEntity>> ReturnsDbSet<TEntity>(this ISetupSequentialResult<DbSet<TEntity>> setupResult, IEnumerable<TEntity> entities, Mock<DbSet<TEntity>>? dbSetMock = null) where TEntity : class
        {
            dbSetMock ??= new Mock<DbSet<TEntity>>();

            ConfigureMock(dbSetMock, entities);

            return setupResult.Returns(dbSetMock.Object);
        }

        private static void ConfigureMock<TEntity>(Mock dbSetMock, IEnumerable<TEntity> entities) where TEntity : class
        {
            var entitiesAsQueryable = entities.AsQueryable();

            dbSetMock.As<IAsyncEnumerable<TEntity>>()
               .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
               .Returns(new InMemoryDbAsyncEnumerator<TEntity>(entitiesAsQueryable.GetEnumerator()));

            dbSetMock.As<IQueryable<TEntity>>()
                .Setup(m => m.Provider)
                .Returns(new InMemoryAsyncQueryProvider<TEntity>(entitiesAsQueryable.Provider));

            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(entitiesAsQueryable.Expression);
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(entitiesAsQueryable.ElementType);
            dbSetMock.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesAsQueryable.GetEnumerator);
        }
    }
}
