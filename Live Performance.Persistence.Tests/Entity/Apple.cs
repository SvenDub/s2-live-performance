namespace Live_Performance.Persistence.Tests.Entity
{
    [Entity]
    public class Apple
    {
        [Identity]
        public int Id { get; set; }

        [DataMember(Type = DataType.Value)]
        public string Type { get; set; }
    }
}