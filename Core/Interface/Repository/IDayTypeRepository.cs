using Core.Entity.DayType;

namespace Core.Interface.Repository
{
    public interface IDayTypeRepository
    {
        Task<DayTypeEntity?> GetAsync(long dayTypeID);
        Task<List<DayTypeEntity>> GetDayTypesByUserAsync(long userID);
        Task<long> CreateAsync(DayTypeEntity model);
        Task UpdateAsync(DayTypeEntity model);
        Task DeleteAsync(DayTypeEntity model);
    }
}
