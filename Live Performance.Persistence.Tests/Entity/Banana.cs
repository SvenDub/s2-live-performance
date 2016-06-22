namespace Live_Performance.Persistence.Tests.Entity
{
    [Entity]
    public class Banana
    {
        [Identity]
        public int SomeIdField { get; set; }

        [DataMember]
        public bool Bend { get; set; }
    }
}