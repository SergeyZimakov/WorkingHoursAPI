﻿using Core.Entity.Shift;
using Core.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repoitory
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly EntityDbContext _db;
        public ShiftRepository(EntityDbContext db)
        {
            _db = db;
        }

        public async Task<List<ShiftEntity>> GetAsync(long userID, int year, int month)
        {
            var searchDate = new DateOnly(year, month, 1);
            var nextDate = searchDate.AddMonths(1);
            return await GetByDatesAsync(userID, searchDate, nextDate);
        }

        public async Task<ShiftEntity?> GetAsync(long userID, DateOnly date)
        {
            var nextDate = date.AddDays(1);
            var res = await GetByDatesAsync(userID, date, nextDate);
            return res.FirstOrDefault();
        }

        public async Task<ShiftEntity?> GetByIDAsync(long shiftID)
        {
            return await _db.Shift
                .Include(sh => sh.ShiftPauses.OrderBy(p => p.Start))
                .SingleOrDefaultAsync(sh => sh.ShiftID == shiftID);
        }

        public async Task<long> CreateAsync(ShiftEntity model)
        {
            _db.Shift.Add(model);
            await _db.SaveChangesAsync();
            return model.ShiftID;
        }

        public async Task UpdateAsync(ShiftEntity model)
        {
            _db.Shift.Update(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(ShiftEntity model)
        {
            _db.Shift.Remove(model);
            await _db.SaveChangesAsync();   
        }

        private async Task<List<ShiftEntity>> GetByDatesAsync(long userID, DateOnly searchDate, DateOnly nextDate)
        {
            return await _db.Shift
                .Include(sh => sh.ShiftPauses.OrderBy(p => p.Start))
                .Where(sh => sh.UserID == userID && sh.ShiftDate >= searchDate && sh.ShiftDate < nextDate)
                .OrderBy(sh => sh.ShiftDate)
                .ToListAsync();
        }
    }
}
