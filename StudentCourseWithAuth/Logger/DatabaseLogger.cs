using StudentCourseWithAuth.Models;

namespace StudentCourseWithAuth.Logger
{
    public class DatabaseLogger : IMyLogger
    {
        public StudentContext _context;
        public DatabaseLogger(StudentContext context) { 
            _context = context;
        }
        public void LogError(string logDetail)
        {
            // write into the database "ERROR : message"
            _context.Add(new LogRecorder { RecordedTime = DateTime.Now, Log = "ERROR : " + logDetail });
            _context.SaveChanges();
        }

        public void LogInfo(string logDetail)
        {
            // write into database "INFO : message"
            _context.Add(new LogRecorder { RecordedTime = DateTime.Now, Log = "INFO : " + logDetail });
            _context.SaveChanges();
        }

        public void LogWarning(string logDetail)
        {
            // write into database "WARNING : "
            _context.Add(new LogRecorder { RecordedTime = DateTime.Now, Log = "WARNINIG : " + logDetail });
            _context.SaveChanges();
        }
    }
}
