using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Util;

namespace Inject
{
    /// <summary>
    ///     Handles dependency injection. Types can be registered using <see cref="Register{T,T}" /> or in bulk with
    ///     <see cref="Register(System.Collections.Generic.IDictionary{System.Type,System.Type})" />. Types can be resolved using <see cref="Resolve{T}" />.
    /// </summary>
    public static class Injector
    {
        /// <summary>
        ///     Mapping of all types to their implementation.
        /// </summary>
        private static readonly IDictionary<Type, Type> Types = new Dictionary<Type, Type>();

        /// <summary>
        ///     Mapping of all instances of implementations.
        /// </summary>
        private static readonly IDictionary<Type, object> Instances = new Dictionary<Type, object>();

        private const string Tag = "INJECT";

        /// <summary>
        ///     Register the implementation of a type.
        /// </summary>
        /// <typeparam name="TContract">The type to register.</typeparam>
        /// <typeparam name="TImplementation">The implementation of the type.</typeparam>
        public static void Register<TContract, TImplementation>()
        {
            Types[typeof(TContract)] = typeof(TImplementation);

            Log.I(Tag, $"Registering {typeof(TContract)} => {typeof(TImplementation)}");
        }

        /// <summary>
        ///     Register the implementation of a type.
        /// </summary>
        /// <param name="contract">The type to register.</param>
        /// <param name="implementation">The implementation of the type.</param>
        public static void Register(Type contract, Type implementation)
        {
            Types[contract] = implementation;

            Log.I(Tag, $"Registering {contract} => {implementation}");
        }

        /// <summary>
        ///     Register multiple types and their implementations.
        /// </summary>
        /// <param name="types">
        ///     The <see cref="IDictionary{T,T}" /> that contains the types and implementations. The left
        ///     <see cref="Type" /> is the contract, the right <see cref="Type" /> is the implementation.
        /// </param>
        public static void Register(IDictionary<Type, Type> types)
        {
            foreach (KeyValuePair<Type, Type> keyValuePair in types)
            {
                Types[keyValuePair.Key] = keyValuePair.Value;

                Log.I(Tag, $"Registring {keyValuePair.Key} => {keyValuePair.Value}");
            }
        }

        /// <summary>
        ///     Resolve a given <see cref="Type"/> to its implementation.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to resolve.</typeparam>
        /// <returns>The implementation of the given type.</returns>
        public static T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        ///     Resolve a given <see cref="Type"/> to its implementation.
        /// </summary>
        /// <param name="contract">The <see cref="Type"/> to resolve.</param>
        /// <returns>The implementation of the given type.</returns>
        private static object Resolve(Type contract)
        {
            if (Instances.ContainsKey(contract))
            {
                return Instances[contract];
            }

            try
            {
                // Resolve the type
                Type implementation = Types[contract];


                // Get constructor for type
                ConstructorInfo constructor = implementation.GetConstructors()[0];
                ParameterInfo[] constructorParameters = constructor.GetParameters();

                // Use default constructor if available
                if (constructorParameters.Length == 1)
                {
                    object instance = Activator.CreateInstance(implementation);
                    Instances[contract] = instance;
                    return instance;
                }

                // Create using available constructor that takes parameters that are registered and therefore can also be inject
                List<object> parameters = new List<object>(constructorParameters.Length);
                parameters.AddRange(constructorParameters.Select(parameterInfo => Resolve(parameterInfo.ParameterType)));

                object resolve = constructor.Invoke(parameters.ToArray());
                Instances[contract] = resolve;
                return resolve;
            }
            catch (KeyNotFoundException e)
            {
                throw new TypeNotResolvedException(contract, e);
            }
        }

        /// <summary>
        ///     Remove all data from the Injector.
        /// </summary>
        public static void Reset()
        {
            Types.Clear();
            Instances.Clear();
        }
    }
}