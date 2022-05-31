namespace RepositoryFramework
{
    /// <summary>
    /// You may set the milliseconds (in range) for each request to simulate a real external database or storage.
    /// You may set a list of exceptions with a random percentage of throwing.
    /// </summary>
    /// <typeparam name="T">Model used for your repository</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
    public class RepositoryBehaviorSettings<T, TKey>
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