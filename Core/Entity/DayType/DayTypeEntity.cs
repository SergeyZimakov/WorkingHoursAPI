using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Core.Consts;

namespace Core.Entity.DayType
{
    public class DayTypeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DayTypeID { get; set; }
        public long UserID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public TimeSpan ShiftTime { get; set; }
        public TimeSpan BreakTime { get; set; }
        public TimeSpan ShiftTimeWithoutBreak { get; set; }
        public List<AdditionalHoursEntity> AdditionalHours { get; set; }
    }
}
