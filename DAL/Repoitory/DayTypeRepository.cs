using Core.Entity.DayType;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repoitory
{
    public class DayTypeRepository
    {
        private readonly EntityDbContext _db;
        public DayTypeRepository(EntityDbContext db)
        {
            _db = db;
        }

        public async Task<DayTypeEntity?> GetAsync(long dayTypeID)
        {
            return await _db.DayType.Include(dt => dt.AdditionalHours.OrderBy(ah => ah.Order)).SingleOrDefaultAsync(dt => dt.DayTypeID == dayTypeID);
        }

        public async Task<List<DayTypeEntity>> GetDayTypesByUserAsync(long userID)
        {
            return await _db.DayType.Include(dt => dt.AdditionalHours.OrderBy(ah => ah.Order)).Where(dt => dt.UserID == userID).ToListAsync();
        }

        public async Task<long> CreateAsync(DayTypeEntity model)
        {
            _db.DayType.Add(model);
            await _db.SaveChangesAsync();
            return model.DayTypeID;
        }

        public async Task UpdateAsync(DayTypeEntity model)
        {
            _db.DayType.Update(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(DayTypeEntity model)
        {
            _db.DayType.Remove(model);
            await _db.SaveChangesAsync();   
        }
    }
}
