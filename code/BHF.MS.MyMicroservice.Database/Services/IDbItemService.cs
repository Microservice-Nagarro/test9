using BHF.MS.MyMicroservice.Database.Models.DbItem;

namespace BHF.MS.MyMicroservice.Database.Services
{
    public interface IDbItemService
    {
        public Task<IList<DbItemDto>> GetAll(CancellationToken token = default);

        public Task<DbItemDto?> GetById(Guid id, CancellationToken token = default);

        public Task<bool> Update(DbItemDto model, CancellationToken token = default);

        public Task<DbItemDto> Add(DbItemCreateDto model, CancellationToken token = default);

        public Task<bool> Delete(Guid id, CancellationToken token = default);
    }
}
