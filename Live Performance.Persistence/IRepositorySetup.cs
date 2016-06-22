namespace Live_Performance.Persistence
{
    /// <summary>
    ///     Provides the setup for a persistence layer.
    /// </summary>
    public interface IRepositorySetup
    {
        /// <summary>
        ///     Perform the setup for the persistence layer.
        /// </summary>
        /// <returns>true on success, false otherwise</returns>
        bool Setup();
    }
}