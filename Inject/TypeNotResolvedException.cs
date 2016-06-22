using System;

namespace Inject
{
    /// <summary>
    ///     Thrown when a Type could not be resolved by the <see cref="Injector"/>.
    /// </summary>
    public class TypeNotResolvedException : Exception
    {
        /// <summary>
        ///     The Type that could not be resolved.
        /// </summary>
        public Type Type { get; }

        /// <param name="t">The Type that could not be resolved.</param>
        public TypeNotResolvedException(Type t) : this(t, $"Could not resolve \"{t}\".")
        {
        }

        /// <param name="t">The Type that could not be resolved.</param>
        /// <param name="message">Additional message.</param>
        public TypeNotResolvedException(Type t, string message) : base(message)
        {
            Type = t;
        }

        /// <param name="t">The Type that could not be resolved.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public TypeNotResolvedException(Type t, Exception innerException)
            : this(t, $"Could not resolve \"{t}\".", innerException)
        {
        }

        /// <param name="t">The Type that could not be resolved.</param>
        /// <param name="message">Additional message.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public TypeNotResolvedException(Type t, string message, Exception innerException)
            : base(message, innerException)
        {
            Type = t;
        }
    }
}