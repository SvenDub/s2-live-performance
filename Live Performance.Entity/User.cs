using Live_Performance.Persistence;
using System.ComponentModel.DataAnnotations;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace Live_Performance.Entity
{
    [Entity(Table = "\"USER\"")]
    public class User
    {
        [Identity]
        public int Id { get; set; }

        [DataMember(Column = "NAME")]
        public string Name { get; set; }

        [DataMember(Column = "EMAIL")]
        public string Email { get; set; }

        [DataMember(Column = "PASSWORD")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataMember(Column = "ADMIN")]
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