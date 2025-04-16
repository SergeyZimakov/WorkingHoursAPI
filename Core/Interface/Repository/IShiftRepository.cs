using Core.Entity.Shift;

namespace Core.Interface.Repository
{
    public interface IShiftRepository
    {
        Task<List<ShiftEntity>> GetAsync(long userID, int year, int month);
        Task<ShiftEntity?> GetAsync(long userID, DateOnly date);
        Task<ShiftEntity?> GetByIDAsync(long shiftID);
        Task<long> CreateAsync(ShiftEntity model);
        Task UpdateAsync(ShiftEntity model);
        Task DeleteAsync(ShiftEntity model);
    }
}
