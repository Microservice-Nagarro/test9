using System.Globalization;
using BHF.MS.MyMicroservice.Database.Context;
using BHF.MS.MyMicroservice.Database.Context.Entities;
using BHF.MS.MyMicroservice.Database.Services;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;

namespace BHF.MS.MyMicroservice.Database.Tests.Services
{
    public sealed class DbItemServiceTests
    {
        private readonly DbItemService _sut;
        private readonly Mock<CustomDbContext> _contextMock = new();

        public DbItemServiceTests()
        {
            _sut = new DbItemService(_contextMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsTop100OrderedByLastUpdated()
        {
            // Arrange
            var entityList = new List<DbItem>();
            for (var i = 0; i < 101; i++)
            {
                entityList.Add(new DbItem
                {
                    Id = Guid.NewGuid(),
                    Name = i.ToString(CultureInfo.InvariantCulture)
                });
            }
            _contextMock.SetupGet(x => x.DbItems).ReturnsDbSet(entityList);

            // Act
            var result = await _sut.GetAll();

            // Assert
            result.Should().HaveCountLessOrEqualTo(100);

            var minLastUpdated = entityList.MinBy(x => x.LastUpdated);
            result.Should().NotContain(x => x.Id == minLastUpdated!.Id);
        }
    }
}
