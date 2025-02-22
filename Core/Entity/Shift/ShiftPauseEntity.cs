using Core.Entity.Shift;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Model.WorkRecord
{
    public class ShiftPauseEntity
    {
        public long ShiftID { get; set; }
        [Column(TypeName = "timestamp without time zone")]
        public DateTime Start { get; set; }
        [Column(TypeName = "timestamp without time zone")]
        public DateTime? Stop { get; set; }
        public ShiftEntity Shift { get; set; }
    }
}
