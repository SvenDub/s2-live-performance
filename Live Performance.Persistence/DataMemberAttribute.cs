using System;

namespace Live_Performance.Persistence
{
    /// <summary>
    ///     A field that can be retrieved from the database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DataMemberAttribute : Attribute
    {
        /// <summary>
        ///     The name of the column that contains the data.
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        ///     The way this field should be populated.
        /// </summary>
        public DataType Type { get; set; } = DataType.Value;

        /// <summary>
        ///     The data type that is used in the reverse of a <see cref="DataType.OneToManyEntity"/>.
        /// </summary>
        public Type RawType { get; set; } = typeof(int);
    }
}