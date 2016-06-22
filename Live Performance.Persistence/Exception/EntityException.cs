namespace Live_Performance.Persistence.Exception
{
    /// <summary>
    ///     Thrown when an entity does not match its data representation or is not defined correctly.
    /// </summary>
    public class EntityException : System.Exception
    {
        public EntityException() : base("The type is not a valid entity")
        {
        }

        public EntityException(string message) : base(message)
        {
        }

        public EntityException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}