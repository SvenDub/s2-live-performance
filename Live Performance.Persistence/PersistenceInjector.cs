using System;
using System.Linq;
using System.Reflection;
using Inject;
using Util;

namespace Live_Performance.Persistence
{
    public static class PersistenceInjector
    {
        /// <summary>
        ///     Register all repositories found using the given provider.
        /// </summary>
        /// <param name="provider">The provider that supplies the types.</param>
        public static void Inject(IRepositoryProvider provider)
        {
            // Connection params
            if (provider.ConnectionParamsContract != null && provider.ConnectionParamsImpl != null)
            {
                Injector.Register(provider.ConnectionParamsContract, provider.ConnectionParamsImpl);
            }

            // Setup
            if (provider.Setup != null)
            {
                Injector.Register(typeof(IRepositorySetup), provider.Setup);
            }

            AppDomain.CurrentDomain.GetAssemblies()
                // Get all entities with Entity and Identity attribute
                .SelectMany(x => x.GetTypes()
                        .Where(t => t.IsDefined(typeof(EntityAttribute), true))
                        .Where(t => t.GetProperties()
                            .Any(info => info.IsDefined(typeof(IdentityAttribute), true)))
                ).ToList().ForEach(t =>
                {
                    Log.I("DISCOVERY", $"Found entity '{t.Name}'");
                    try
                    {
                        // Get database Type from IRepositoryProvider
                        Type repository = (Type) typeof(IRepositoryProvider).GetMethod("GetDatabaseType")
                            .MakeGenericMethod(t)
                            .Invoke(provider, new object[] {});

                        // Register repository
                        typeof(PersistenceInjector).GetMethod("RegisterRepository",
                                BindingFlags.NonPublic | BindingFlags.Static)
                            .MakeGenericMethod(new Type[] {t, repository})
                            .Invoke(null, new object[] {});
                    }
                    catch (System.Exception e)
                    {
                        Log.E("INJECT", e.ToString());
                    }
                });
        }

        /// <summary>
        ///     Register a repository and its armour.
        /// </summary>
        /// <typeparam name="T">The entity for which to register the repository.</typeparam>
        /// <typeparam name="TRepo">The repository to register.</typeparam>
        private static void RegisterRepository<T, TRepo>() where T : new()
        {
            // Global persistence layer
            Injector.Register<IRepository<T>, RepositoryArmour<T>>();

            // Database specific
            Injector.Register<IStrictRepository<T>, TRepo>();
        }
    }
}