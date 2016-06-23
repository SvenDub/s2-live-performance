using Live_Performance.Persistence;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace Live_Performance.Entity
{
    [Entity(Table = "\"USER\"")]
    [DataContract]
    public class User
    {
        [Identity]
        [System.Runtime.Serialization.DataMember]
        public int Id { get; set; }

        [Persistence.DataMember(Column = "NAME")]
        [System.Runtime.Serialization.DataMember]
        public string Name { get; set; }

        [Persistence.DataMember(Column = "EMAIL")]
        [System.Runtime.Serialization.DataMember]
        public string Email { get; set; }

        [Persistence.DataMember(Column = "PASSWORD")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Persistence.DataMember(Column = "ADMIN")]
        [System.Runtime.Serialization.DataMember]
        public bool Admin { get; set; }

        protected bool Equals(User other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(User left, User right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(User left, User right)
        {
            return !Equals(left, right);
        }
    }
}