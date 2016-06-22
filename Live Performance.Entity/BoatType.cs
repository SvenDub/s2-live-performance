using Live_Performance.Persistence;

namespace Live_Performance.Entity
{
    [Entity(Table = "\"BOATTYPE\"")]
    public class BoatType : IProduct
    {
        [Identity]
        public int Id { get; set; }

        [DataMember(Column = "MOTORIZED")]
        public bool Motorized { get; set; }

        [DataMember(Column = "FUEL_CAPACITY")]
        public int? FuelCapacity { get; set; }

        [DataMember(Column = "FUEL_ECONOMY")]
        public double? FuelEconomy { get; set; }

        public double? Range => FuelCapacity*FuelEconomy;

        [DataMember(Column = "NAME")]
        public string Name { get; set; }

        [DataMember(Column = "COST")]
        public int Cost { get; set; }
    }
}