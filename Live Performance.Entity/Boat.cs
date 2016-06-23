using Live_Performance.Persistence;

namespace Live_Performance.Entity
{
    [Entity(Table = "\"BOAT\"")]
    public class Boat
    {
        [Identity]
        public int Id { get; set; }

        [DataMember(Column = "TYPE", Type = DataType.Entity)]
        public BoatType BoatType { get; set; }

        [DataMember(Column = "NAME")]
        public string Name { get; set; }

        public string Display => Name + (BoatType != null ? $"({BoatType.Name})" : "");
    }
}