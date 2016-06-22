using Live_Performance.Persistence;

namespace Live_Performance.Entity
{
    [Entity(Table = "\"AREA\"")]
    public class Area : IProduct
    {
        [Identity]
        public int Id { get; set; }

        [DataMember(Column = "NAME")]
        public string Name { get; set; }

        [DataMember(Column = "COST")]
        public int Cost { get; set; }
    }
}