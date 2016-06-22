using Live_Performance.Persistence;

namespace Live_Performance.Entity
{
    [Entity(Table = "\"BOAT\"")]
    public class Boat : IProduct
    {
        [Identity]
        public int Id { get; set; }

        [DataMember(Column = "BOATTYPE", Type = DataType.Entity)]
        public BoatType BoatType { get; set; }

        [DataMember(Column = "NAME")]
        public string Name { get; set; }

        [DataMember(Column = "COST")]
        public int Cost { get; set; }
    }
}