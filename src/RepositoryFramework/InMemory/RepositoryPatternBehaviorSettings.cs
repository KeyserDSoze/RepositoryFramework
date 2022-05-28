namespace RepositoryFramework
{
    public class RepositoryPatternBehaviorSettings<T, TKey>
        where TKey : notnull
    {
        public Range MillisecondsOfWaitForDelete { get; set; }
        public Range MillisecondsOfWaitForInsert { get; set; }
        public Range MillisecondsOfWaitForUpdate { get; set; }
        public Range MillisecondsOfWaitForGet { get; set; }
        public Range MillisecondsOfWaitForQuery { get; set; }
        public Range MillisecondsOfWaitBeforeExceptionForDelete { get; set; }
        public Range MillisecondsOfWaitBeforeExceptionForInsert { get; set; }
        public Range MillisecondsOfWaitBeforeExceptionForUpdate { get; set; }
        public Range MillisecondsOfWaitBeforeExceptionForGet { get; set; }
        public Range MillisecondsOfWaitBeforeExceptionForQuery { get; set; }
        public List<ExceptionOdds> ExceptionOddsForDelete { get; } = new();
        public List<ExceptionOdds> ExceptionOddsForInsert { get; } = new();
        public List<ExceptionOdds> ExceptionOddsForUpdate { get; } = new();
        public List<ExceptionOdds> ExceptionOddsForGet { get; } = new();
        public List<ExceptionOdds> ExceptionOddsForQuery { get; } = new();
    }
}