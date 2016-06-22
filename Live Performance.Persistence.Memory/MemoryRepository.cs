using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Live_Performance.Persistence.Exception;
using Util;

namespace Live_Performance.Persistence.Memory
{
    /// <summary>
    ///     In-memory database that loses its data as soon as the application exits.
    /// </summary>
    public class MemoryRepository<T> : IStrictRepository<T> where T : new()
    {
        protected List<T> Entities = new List<T>();

        private readonly EntityAttribute _entityAttribute;
        private readonly IdentityAttribute _identityAttribute;
        private readonly PropertyInfo _identityProperty;

        public MemoryRepository()
        {
            MemberInfo info = typeof(T);

            if (!info.GetCustomAttributes(true).Any(attr => attr is EntityAttribute))
            {
                throw new EntityException($"Type {typeof(T)} is not attributed with Entity");
            }

            _entityAttribute = info.GetCustomAttributes(true)
                .OfType<EntityAttribute>()
                .First();

            PropertyInfo[] properties = typeof(T).GetProperties();

            if (!properties.Any(propertyInfo => propertyInfo.IsDefined(typeof(IdentityAttribute))))
            {
                throw new EntityException($"Type {typeof(T)} has no property attributed with Identity");
            }

            _identityProperty = properties
                .First(propertyInfo => propertyInfo.IsDefined(typeof(IdentityAttribute)));
            _identityAttribute = _identityProperty.GetCustomAttributes(true)
                .OfType<IdentityAttribute>()
                .First();

            Log.D("DB", _entityAttribute.Table + " [ " + string.Join(", ", DataMembers) + " ]");
        }

        public long Count() => Entities
            .Count;

        public void Delete(int id) => Entities
            .RemoveAll(entity => (int) _identityProperty.GetValue(entity) == id);

        public void Delete(List<T> entities) => entities
            .ForEach(Delete);

        public void Delete(T entity) => Delete((int) _identityProperty.GetValue(entity));

        public void DeleteAll() => Entities
            .Clear();

        public bool Exists(int id) => Entities
            .Exists(entity => (int) _identityProperty.GetValue(entity) == id);

        public List<T> FindAll() => Entities
            .Select(Copy)
            .ToList();

        public List<T> FindAll(List<int> ids) => Entities
            .Where(entity => ids.Contains((int) _identityProperty.GetValue(entity)))
            .Select(Copy)
            .ToList();

        public List<T> FindAllWhere(Func<T, bool> predicate) => Entities
            .Where(predicate)
            .Select(Copy)
            .ToList();

        public List<T> FindAllWhere(Func<T, int, bool> predicate) => Entities
            .Where(predicate)
            .Select(Copy)
            .ToList();

        public T FindOne(int id) => Copy(Entities.Find(entity => (int) _identityProperty.GetValue(entity) == id));

        public T Save(T entity)
        {
            T copy = Copy(entity);
            int id = (int) _identityProperty.GetValue(copy);
            if (Exists(id))
            {
                Delete(id);
            }
            else
            {
                if (id == 0)
                {
                    id = Entities.Select(e => (int) _identityProperty.GetValue(e)).DefaultIfEmpty(0).Max() + 1;
                }
                _identityProperty.SetValue(copy, id);
                _identityProperty.SetValue(entity, id);
            }
            Entities.Add(copy);
            return entity;
        }

        public List<T> Save(List<T> entities) => entities
            .Select(Save)
            .ToList();

        private T Copy(T original)
        {
            if (original == null) return default(T);
            T copy = new T();
            DataMembers
                .ForEach(info => info.SetValue(copy, info.GetValue(original)));
            return copy;
        }

        private List<PropertyInfo> DataMembers => typeof(T)
            .GetProperties()
            .Where(propertyInfo => propertyInfo.IsDefined(typeof(IdentityAttribute), true))
            .Concat(DataMembersWithoutIdentity)
            .ToList();

        private List<PropertyInfo> DataMembersWithoutIdentity => typeof(T)
            .GetProperties()
            .Where(
                propertyInfo =>
                    propertyInfo.IsDefined(typeof(DataMemberAttribute), true) &&
                    propertyInfo.GetCustomAttribute<DataMemberAttribute>().Type != DataType.OneToManyEntity)
            .ToList();

        private List<PropertyInfo> DataMembersOneToMany => typeof(T)
            .GetProperties()
            .Where(
                propertyInfo =>
                    propertyInfo.IsDefined(typeof(DataMemberAttribute), true) &&
                    propertyInfo.GetCustomAttribute<DataMemberAttribute>().Type == DataType.OneToManyEntity)
            .ToList();
    }
}