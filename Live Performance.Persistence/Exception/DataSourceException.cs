namespace Live_Performance.Persistence.Exception
{
    /// <summary>
    ///     Thrown when an error occured while querying or reading from the data source.
    /// </summary>
    public class DataSourceException : System.Exception
    {
        private const string DefaultMessage = "Could not read from data source";

        public DataSourceException() : base(DefaultMessage)
        {
        }

        public DataSourceException(string message) : base(message)
        {
        }

        public DataSourceException(System.Exception innerException) : this(DefaultMessage, innerException)
        {
        }

        public DataSourceException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}