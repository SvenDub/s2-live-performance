namespace Live_Performance.Persistence.Exception
{
    /// <summary>
    ///     Thrown when an query returns no results.
    /// </summary>
    public class EntityNotFoundException : EntityException
    {
        public EntityNotFoundException() : base("No entity found matching search criteria")
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}