using System;

namespace Live_Performance.Persistence
{
    /// <summary>
    ///     Get this class from the database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute : Attribute
    {
        /// <summary>
        ///     The name of the table in which the Entity is stored.
        /// </summary>
        public string Table { get; set; }
    }
}