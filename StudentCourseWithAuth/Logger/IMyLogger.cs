namespace StudentCourseWithAuth.Logger
{
    public interface IMyLogger
    {
        public void LogInfo(string logDetail);
        public void LogWarning(string logDetail);
        public void LogError(string logDetail);

    }
}
