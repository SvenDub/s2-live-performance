namespace Live_Performance.Persistence.Memory
{
    /// <see cref="MemoryRepository{T}"/>
    public class MemorySetup : IRepositorySetup
    {
        /// <summary>
        ///     No setup is required for an in memory database, so this method does nothing.
        /// </summary>
        public bool Setup() => true;
    }
}