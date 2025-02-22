using Core.Model.WorkRecord;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Shift
{
    public class ShiftEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ShiftID { get; set; }
        public long? DayTypeID { get; set; }
        public long UserID { get; set; }
        public DateTime ShiftDate { get; set; }
        [Column(TypeName = "timestamp without time zone")]
        public DateTime Start { get; set; }
        [Column(TypeName = "timestamp without time zone")]
        public DateTime? Stop { get; set; }
        public List<ShiftPauseEntity> ShiftPauses { get; set; }
    }
}
