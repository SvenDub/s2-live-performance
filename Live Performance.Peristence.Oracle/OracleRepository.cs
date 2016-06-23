using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Inject;
using Live_Performance.Persistence;
using Live_Performance.Persistence.Exception;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using Util;

namespace Live_Performance.Peristence.Oracle
{
    /// <summary>
    ///     Repository that connects to a Oracle backend.
    /// </summary>
    public class OracleRepository<T> : IStrictRepository<T> where T : new()
    {
        private readonly EntityAttribute _entityAttribute;
        private readonly IdentityAttribute _identityAttribute;
        private readonly PropertyInfo _identityProperty;

        /// <summary>
        ///     Checks if the entity is valid.
        /// </summary>
        public OracleRepository()
        {
            MemberInfo info = typeof (T);

            if (!info.GetCustomAttributes(true).Any(attr => attr is EntityAttribute))
            {
                throw new EntityException($"Type {typeof (T)} is not attributed with Entity");
            }

            _entityAttribute = info.GetCustomAttributes(true)
                .OfType<EntityAttribute>()
                .First();

            PropertyInfo[] properties = typeof (T).GetProperties();

            if (!properties.Any(propertyInfo => propertyInfo.IsDefined(typeof (IdentityAttribute))))
            {
                throw new EntityException($"Type {typeof (T)} has no property attributed with Identity");
            }

            _identityProperty = properties
                .First(propertyInfo => propertyInfo.IsDefined(typeof (IdentityAttribute)));
            _identityAttribute = _identityProperty.GetCustomAttributes(true)
                .OfType<IdentityAttribute>()
                .First();

            Log.D("DB", _entityAttribute.Table + " [ " + string.Join(", ", DataMembers.Values) + " ]");
        }

        /// <summary>
        ///     Connection parameters to use.
        /// </summary>
        public IOracleConnectionParams OracleConnectionParams { set; protected get; } =
            Injector.Resolve<IOracleConnectionParams>();

        /// <summary>
        ///     All properties attributed with <see cref="DataMemberAttribute" /> and their Column name.
        /// </summary>
        private Dictionary<PropertyInfo, string> DataMembers => typeof (T)
            .GetProperties()
            .Where(propertyInfo => propertyInfo.IsDefined(typeof (IdentityAttribute), true))
            .Select(
                propertyInfo =>
                    new KeyValuePair<PropertyInfo, string>(propertyInfo,
                        propertyInfo.GetCustomAttribute<IdentityAttribute>(true).Column))
            .Concat(DataMembersWithoutIdentity)
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        /// <summary>
        ///     All properties attributed with <see cref="DataMemberAttribute" /> except <see cref="IdentityAttribute" /> and their
        ///     Column name.
        /// </summary>
        private Dictionary<PropertyInfo, string> DataMembersWithoutIdentity => typeof (T)
            .GetProperties()
            .Where(
                propertyInfo =>
                    propertyInfo.IsDefined(typeof (DataMemberAttribute), true) &&
                    propertyInfo.GetCustomAttribute<DataMemberAttribute>().Type != DataType.OneToManyEntity)
            .Select(
                propertyInfo =>
                    new KeyValuePair<PropertyInfo, string>(propertyInfo,
                        propertyInfo.GetCustomAttribute<DataMemberAttribute>(true).Column))
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        /// <summary>
        ///     All properties attributed with <see cref="DataType.OneToManyEntity" />.
        /// </summary>
        private List<PropertyInfo> DataMembersOneToMany => typeof (T)
            .GetProperties()
            .Where(
                propertyInfo =>
                    propertyInfo.IsDefined(typeof (DataMemberAttribute), true) &&
                    propertyInfo.GetCustomAttribute<DataMemberAttribute>().Type == DataType.OneToManyEntity)
            .ToList();

        public long Count()
        {
            try
            {
                using (OracleConnection connection = CreateConnection())
                {
                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = $"SELECT COUNT(*) FROM {_entityAttribute.Table}";
                        object result = cmd.ExecuteScalar();
                        return Convert.ToInt64(result);
                    }
                }
            }
            catch (OracleException e)
            {
                throw new DataSourceException(e);
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (OracleConnection connection = CreateConnection())
                {
                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        cmd.BindByName = true;
                        cmd.CommandText = $"DELETE FROM {_entityAttribute.Table} WHERE {_identityAttribute.Column}=:Id";
                        cmd.Parameters.Add("Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException e)
            {
                throw new DataSourceException(e);
            }
        }

        public void Delete(List<T> entities)
        {
            entities.ForEach(Delete);
        }

        public void Delete(T entity)
        {
            try
            {
                Delete((int) _identityProperty.GetGetMethod().Invoke(entity, new object[] {}));
            }
            catch (OracleException e)
            {
                throw new DataSourceException(e);
            }
        }

        public void DeleteAll()
        {
            try
            {
                using (OracleConnection connection = CreateConnection())
                {
                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = $"DELETE FROM {_entityAttribute.Table}";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException e)
            {
                throw new DataSourceException(e);
            }
        }

        public bool Exists(int id)
        {
            try
            {
                using (OracleConnection connection = CreateConnection())
                {
                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        cmd.BindByName = true;
                        cmd.CommandText =
                            $"SELECT COUNT(*) FROM {_entityAttribute.Table} WHERE {_identityAttribute.Column}=:Id";
                        cmd.Parameters.Add("Id", id);
                        return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (OracleException e)
            {
                throw new DataSourceException(e);
            }
        }

        public List<T> FindAll()
        {
            try
            {
                using (OracleConnection connection = CreateConnection())
                {
                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        cmd.BindByName = true;
                        cmd.CommandText = $"SELECT \"{string.Join("\", \"", DataMembers.Values)}\" " +
                                          $"FROM {_entityAttribute.Table}";

                        return CreateListFromReader(cmd.ExecuteReader()).ToList();
                    }
                }
            }
            catch (OracleException e)
            {
                throw new DataSourceException(e);
            }
        }

        public List<T> FindAll(List<int> ids)
        {
            return ids.Select(FindOne).ToList();
        }

        public List<T> FindAllWhere(Func<T, bool> predicate)
        {
            return FindAll().Where(predicate).ToList();
        }

        public List<T> FindAllWhere(Func<T, int, bool> predicate)
        {
            return FindAll().Where(predicate).ToList();
        }

        public T FindOne(int id)
        {
            try
            {
                using (OracleConnection connection = CreateConnection())
                {
                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        cmd.BindByName = true;
                        cmd.CommandText = $"SELECT \"{string.Join("\", \"", DataMembers.Values)}\" " +
                                          $"FROM {_entityAttribute.Table} " +
                                          $"WHERE {_identityAttribute.Column}=:Id";
                        cmd.Parameters.Add("Id", id);

                        return CreateFromReader(cmd.ExecuteReader());
                    }
                }
            }
            catch (OracleException e)
            {
                throw new DataSourceException(e);
            }
        }

        public T Save(T entity)
        {
            int id = (int) _identityProperty.GetValue(entity);
            try
            {
                return Exists(id) ? Update(entity, id) : Insert(entity);
            }
            catch (OracleException e)
            {
                throw new DataSourceException(e);
            }
        }

        public List<T> Save(List<T> entities)
        {
            return entities.Select(Save).ToList();
        }

        /// <summary>
        ///     Update an entity with the given id.
        /// </summary>
        /// <param name="entity">The new entity values.</param>
        /// <param name="id">The id of the entity.</param>
        /// <returns>The saved entity.</returns>
        private T Update(T entity, int id)
        {
            using (OracleConnection connection = CreateConnection())
            {
                using (OracleCommand cmd = connection.CreateCommand())
                {
                    string[] parameters = new string[DataMembersWithoutIdentity.Count];
                    int i = -1;
                    foreach (KeyValuePair<PropertyInfo, string> keyValuePair in DataMembersWithoutIdentity)
                    {
                        i++;
                        parameters[i] = $"\"{keyValuePair.Value}\"=:{keyValuePair.Value}";
                    }

                    cmd.BindByName = true;
                    cmd.CommandText =
                        $"UPDATE {_entityAttribute.Table} " +
                        $"SET {string.Join(", ", parameters)} " +
                        $"WHERE \"{_identityAttribute.Column}\"=:Id";

                    cmd.Parameters.Add("Id", id);

                    AddUpdateParametersForEntity(cmd, entity);

                    cmd.ExecuteNonQuery();

                    T saved = FindOne(id);
                    SaveOneToMany(entity, saved, id);
                    return saved;
                }
            }
        }

        /// <summary>
        ///     Insert a new entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>The saved entity.</returns>
        private T Insert(T entity)
        {
            using (OracleConnection connection = CreateConnection())
            {
                using (OracleCommand cmd = connection.CreateCommand())
                {
                    string[] parameters = new string[DataMembersWithoutIdentity.Count];
                    int i = -1;
                    foreach (KeyValuePair<PropertyInfo, string> keyValuePair in DataMembersWithoutIdentity)
                    {
                        i++;
                        parameters[i] = $":{keyValuePair.Value}";
                    }

                    cmd.BindByName = true;
                    cmd.CommandText =
                        $"INSERT INTO {_entityAttribute.Table} (\"{string.Join("\", \"", DataMembersWithoutIdentity.Values)}\") " +
                        $"VALUES ({string.Join(", ", parameters)}) RETURNING \"{_identityAttribute.Column}\" INTO :AUTOINCREMENT";

                    AddInsertParametersForEntity(cmd, entity);

                    OracleParameter p = new OracleParameter
                    {
                        ParameterName = ":AUTOINCREMENT",
                        SourceColumn = $"\"{_identityAttribute.Column}\"",
                        OracleDbType = OracleDbType.Int32,
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(p);

                    cmd.ExecuteNonQuery();

                    int id = ((OracleDecimal) p.Value).ToInt32();
                    T saved = FindOne(id);
                    SaveOneToMany(entity, saved, id);
                    return saved;
                }
            }
        }

        /// <summary>
        ///     Save all <see cref="DataType.OneToManyEntity" /> properties after the entity itself has been saved.
        ///     The new id has to be known at this point.
        /// </summary>
        /// <param name="entity">The old entity.</param>
        /// <param name="saved">The saved entity.</param>
        /// <param name="id">The id of the saved entity.</param>
        /// <exception cref="EntityException">If <see cref="DataType.OneToManyEntity" /> was incorrectly defined.</exception>
        private void SaveOneToMany(T entity, T saved, int id)
        {
            DataMembersOneToMany.ForEach(key =>
            {
                if (key.PropertyType.IsGenericType && key.PropertyType.GetGenericTypeDefinition()
                    == typeof (List<>))
                {
                    Type itemType = key.PropertyType.GetGenericArguments()[0];

                    // Save one to many entities to their repo
                    object repo = typeof (OracleRepository<T>).GetMethod("ResolveRepository")
                        .MakeGenericMethod(itemType)
                        .Invoke(this, new object[] {});

                    IList entities = (IList) key.GetValue(entity);
                    entities.Cast<object>().ToList().ForEach(e =>
                    {
                        e.GetType()
                            .GetProperties()
                            .Where(
                                info =>
                                    info.IsDefined(typeof (DataMemberAttribute)) &&
                                    info.GetCustomAttribute<DataMemberAttribute>().RawType == typeof (T))
                            .ToList()
                            .ForEach(info => info.SetValue(e, id));
                    });

                    object savedValues = repo.GetType().GetMethod("Save", new[] {key.PropertyType})
                        .Invoke(repo, new object[] {entities});

                    // Set entities to new object
                    key.SetValue(saved, savedValues);
                }
                else
                {
                    throw new EntityException("DataType.OneToMany can only be defined on a List<>.");
                }
            });
        }

        /// <summary>
        ///     Open a new connection to the database.
        /// </summary>
        /// <returns>The open connection.</returns>
        /// <exception cref="ConnectException">When the connection could not be established.</exception>
        private OracleConnection CreateConnection()
        {
            string connectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" +
                                      OracleConnectionParams.Host +
                                      ")(PORT=" + OracleConnectionParams.Port +
                                      ")))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" +
                                      OracleConnectionParams.ServiceName + ")));" +
                                      "User ID=" + OracleConnectionParams.Username + ";" +
                                      "PASSWORD=" + OracleConnectionParams.Password + ";";

            try
            {
                OracleConnection mySqlConnection =
                    new OracleConnection(connectionString);
                mySqlConnection.Open();
                return mySqlConnection;
            }
            catch (Exception e)
            {
                throw new ConnectException(e);
            }
        }

        /// <summary>
        ///     Create a new entity from the given reader.
        /// </summary>
        /// <param name="reader">The reader that contains the data.</param>
        /// <returns>A new entity.</returns>
        private T CreateFromReader(OracleDataReader reader)
        {
            using (reader)
            {
                while (reader.Read())
                {
                    return CreateFromRow(reader);
                }

                throw new EntityNotFoundException();
            }
        }

        /// <summary>
        ///     Create a new list of entities from the given reader.
        /// </summary>
        /// <param name="reader">The reader that contains the data.</param>
        /// <returns>A new list of entities.</returns>
        private IEnumerable<T> CreateListFromReader(OracleDataReader reader)
        {
            using (reader)
            {
                while (reader.Read())
                {
                    yield return CreateFromRow(reader);
                }
            }
        }

        /// <summary>
        ///     Create a new entity from the given reader.
        ///     Expects reader to contain data at current row. Does not clean up reader.
        /// </summary>
        /// <param name="reader">The reader that contains the data.</param>
        /// <returns>A new entity.</returns>
        /// <exception cref="EntityException">When a property can not be populated.</exception>
        private T CreateFromRow(OracleDataReader reader)
        {
            try
            {
                T entity = new T();
                foreach (KeyValuePair<PropertyInfo, string> keyValuePair in DataMembers)
                {
                    // Do not set null values
                    if (reader.IsDBNull(reader.GetOrdinal(keyValuePair.Value))) continue;

                    // Set the Identity
                    if (keyValuePair.Key.IsDefined(typeof (IdentityAttribute)))
                    {
                        object value = reader[keyValuePair.Value];

                        value = ConvertValue(keyValuePair, value);

                        keyValuePair.Key.SetValue(entity, value);
                    }
                    // Set DataMember
                    else if (keyValuePair.Key.IsDefined(typeof (DataMemberAttribute)))
                    {
                        DataMemberAttribute attribute = keyValuePair.Key.GetCustomAttribute<DataMemberAttribute>();

                        switch (attribute.Type)
                        {
                            // Set raw value. Perform conversion if necessary and supported.
                            case DataType.Value:
                            {
                                object value = reader[keyValuePair.Value];

                                value = ConvertValue(keyValuePair, value);

                                keyValuePair.Key.SetValue(entity, value);
                                break;
                            }
                            // Resolve entity from other IRepository
                            case DataType.Entity:
                            {
                                object repo = typeof (OracleRepository<T>).GetMethod("ResolveRepository")
                                    .MakeGenericMethod(keyValuePair.Key.PropertyType)
                                    .Invoke(this, new object[] {});
                                object value = repo.GetType().GetMethod("FindOne")
                                    .Invoke(repo, new object[] {Convert.ToInt32(reader[keyValuePair.Value])});
                                keyValuePair.Key.SetValue(entity, value);
                                break;
                            }
                            // Resolve one to many entities
                            case DataType.OneToManyEntity:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                return entity;
            }
            catch (ArgumentException e)
            {
                throw new EntityException(
                    "The data type does not match and conversion is not yet implemented for this type.", e);
            }
        }

        /// <summary>
        ///     Add parameters to insert statement.
        /// </summary>
        /// <param name="cmd">The statement to which the parameters should be added.</param>
        /// <param name="entity">The entity to insert.</param>
        /// <exception cref="EntityException">When a property can not be populated.</exception>
        private void AddInsertParametersForEntity(OracleCommand cmd, T entity)
        {
            try
            {
                foreach (KeyValuePair<PropertyInfo, string> keyValuePair in DataMembersWithoutIdentity)
                {
                    DataMemberAttribute attribute = keyValuePair.Key.GetCustomAttribute<DataMemberAttribute>();

                    switch (attribute.Type)
                    {
                        case DataType.Value:
                            // Boolean
                            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                            if (keyValuePair.Key.PropertyType == typeof (bool))
                            {
                                cmd.Parameters.Add(attribute.Column, (bool) keyValuePair.Key.GetValue(entity) ? 1 : 0);
                            }
                            // Generic
                            else
                            {
                                cmd.Parameters.Add(attribute.Column, keyValuePair.Key.GetValue(entity));
                            }
                            break;
                        case DataType.Entity:
                            object repo = typeof (OracleRepository<T>).GetMethod("ResolveRepository")
                                .MakeGenericMethod(keyValuePair.Key.PropertyType)
                                .Invoke(this, new object[] {});

                            object nestedEntity = repo.GetType()
                                .GetMethod("Save", new[] {keyValuePair.Key.PropertyType})
                                .Invoke(repo, new[] {keyValuePair.Key.GetValue(entity)});

                            object nestedId = nestedEntity.GetType()
                                .GetProperties()
                                .First(propertyInfo => propertyInfo.IsDefined(typeof (IdentityAttribute)))
                                .GetValue(nestedEntity);
                            cmd.Parameters.Add(attribute.Column, nestedId);
                            break;
                        case DataType.OneToManyEntity:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (ArgumentException e)
            {
                throw new EntityException(
                    "The data type does not match and conversion is not yet implemented for this type.", e);
            }
        }

        /// <summary>
        ///     Add parameters to update statement.
        /// </summary>
        /// <param name="cmd">The statement to which the parameters should be added.</param>
        /// <param name="entity">The entity to insert.</param>
        /// <exception cref="EntityException">When a property can not be populated.</exception>
        private void AddUpdateParametersForEntity(OracleCommand cmd, T entity)
        {
            try
            {
                foreach (KeyValuePair<PropertyInfo, string> keyValuePair in DataMembers)
                {
                    if (keyValuePair.Key.IsDefined(typeof (IdentityAttribute)))
                    {
                        IdentityAttribute attribute =
                            keyValuePair.Key.GetCustomAttribute<IdentityAttribute>();

                        cmd.Parameters.Add(attribute.Column, keyValuePair.Key.GetValue(entity));
                    }
                    else if (keyValuePair.Key.IsDefined(typeof (DataMemberAttribute)))
                    {
                        DataMemberAttribute attribute =
                            keyValuePair.Key.GetCustomAttribute<DataMemberAttribute>();

                        switch (attribute.Type)
                        {
                            case DataType.Value:
                                // Boolean
                                // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                                if (keyValuePair.Key.PropertyType == typeof (bool))
                                {
                                    cmd.Parameters.Add(attribute.Column,
                                        (bool) keyValuePair.Key.GetValue(entity) ? 1 : 0);
                                }
                                // Generic
                                else
                                {
                                    cmd.Parameters.Add(attribute.Column, keyValuePair.Key.GetValue(entity));
                                }
                                break;
                            case DataType.Entity:
                                object repo = typeof (OracleRepository<T>).GetMethod("ResolveRepository")
                                    .MakeGenericMethod(keyValuePair.Key.PropertyType)
                                    .Invoke(this, new object[] {});
                                object nestedEntity = repo.GetType()
                                    .GetMethod("Save", new[] {keyValuePair.Key.PropertyType})
                                    .Invoke(repo, new[] {keyValuePair.Key.GetValue(entity)});
                                object nestedId = nestedEntity.GetType()
                                    .GetProperties()
                                    .First(propertyInfo => propertyInfo.IsDefined(typeof (IdentityAttribute)))
                                    .GetValue(nestedEntity);
                                cmd.Parameters.Add(attribute.Column, nestedId);
                                break;
                            case DataType.OneToManyEntity:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
            catch (ArgumentException e)
            {
                throw new EntityException(
                    "The data type does not match and conversion is not yet implemented for this type.", e);
            }
        }

        private static object ConvertValue(KeyValuePair<PropertyInfo, string> keyValuePair, object value)
        {
            if (keyValuePair.Key.PropertyType == typeof(bool))
            {
                value = Convert.ToBoolean(value);
            }
            else if (keyValuePair.Key.PropertyType == typeof(int))
            {
                value = Convert.ToInt32(value);
            }
            else if (keyValuePair.Key.PropertyType == typeof(int?))
            {
                value = Convert.ToInt32(value);
            }
            else if (keyValuePair.Key.PropertyType == typeof(double?))
            {
                value = Convert.ToDouble(value);
            }
            return value;
        }

        /// <summary>
        ///     Resolve the repository for the given entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity to resolve.</typeparam>
        /// <returns>The repository for the given entity.</returns>
        public IRepository<TEntity> ResolveRepository<TEntity>() where TEntity : new()
        {
            return Injector.Resolve<IRepository<TEntity>>();
        }
    }
}