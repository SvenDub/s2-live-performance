using System;

namespace Live_Performance.Persistence
{
    /// <summary>
    ///     A field that is the primary key of an entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityAttribute : Attribute
    {
        /// <summary>
        ///     The name of the column that contains the data.
        /// </summary>
        public string Column { get; set; } = "ID";
    }
}