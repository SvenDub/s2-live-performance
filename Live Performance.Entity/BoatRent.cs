using Live_Performance.Persistence;

namespace Live_Performance.Entity
{
    [Entity(Table = "\"BOATRENT\"")]
    public class BoatRent
    {
        [Identity]
        public int Id { get; set; }

        [DataMember(Column = "BOAT", Type = DataType.Entity)]
        public Boat Boat { get; set; }

        [DataMember(Column = "RENT", RawType = typeof (Rent))]
        public int Rent { get; set; }

        [DataMember(Column = "COST")]
        public int Cost { get; set; }
    }
}