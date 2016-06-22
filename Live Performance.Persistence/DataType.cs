namespace Live_Performance.Persistence
{
    /// <summary>
    ///     The way this field should be populated.
    /// </summary>
    public enum DataType
    {
        /// <summary>
        ///     Save the raw value.
        /// </summary>
        Value,

        /// <summary>
        ///     Resolve using a <see cref="IRepository{T}"/>.
        /// </summary>
        Entity,

        /// <summary>
        ///     Populate a list.
        /// </summary>
        OneToManyEntity
    }
}