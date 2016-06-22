namespace Live_Performance.Persistence.Exception
{
    /// <summary>
    ///     Thrown when the connection to the data source failed.
    /// </summary>
    public class ConnectException : DataSourceException
    {
        private const string DefaultMessage = "Could not connect to data source";

        public ConnectException() : base(DefaultMessage)
        {
        }

        public ConnectException(string message) : base(message)
        {
        }

        public ConnectException(System.Exception innerException) : this(DefaultMessage, innerException)
        {
        }

        public ConnectException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}