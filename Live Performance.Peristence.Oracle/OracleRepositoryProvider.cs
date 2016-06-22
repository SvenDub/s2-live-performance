using System;
using Live_Performance.Persistence;

namespace Live_Performance.Peristence.Oracle
{
    /// <see cref="OracleRepository{T}"/>
    public class OracleRepositoryProvider : IRepositoryProvider
    {
        public Type GetDatabaseType<T>() where T : new() => typeof(OracleRepository<T>);
        public Type ConnectionParamsContract => typeof(IOracleConnectionParams);
        public Type ConnectionParamsImpl => typeof(ProductionOracleConnectionParams);
        public Type Setup => typeof(OracleSetup);
    }
}