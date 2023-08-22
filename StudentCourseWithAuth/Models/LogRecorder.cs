using System.ComponentModel.DataAnnotations;

namespace StudentCourseWithAuth.Models
{
    public class LogRecorder
    {
        [Key]
        public int RecorderId { get; set; }
        public DateTime RecordedTime { get; set; }
        public string Log { get; set; }

    }
}
