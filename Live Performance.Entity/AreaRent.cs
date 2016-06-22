using Live_Performance.Persistence;

namespace Live_Performance.Entity
{
    [Entity(Table = "\"AREARENT\"")]
    public class AreaRent
    {
        [Identity]
        public int Id { get; set; }

        [DataMember(Column = "AREA", Type = DataType.Entity)]
        public Area Area { get; set; }

        [DataMember(Column = "RENT", RawType = typeof (Rent))]
        public int Rent { get; set; }

        [DataMember(Column = "COST")]
        public int Cost { get; set; }
    }
}