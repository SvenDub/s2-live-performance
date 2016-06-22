using Live_Performance.Persistence;

namespace Live_Performance.Entity
{
    [Entity(Table = "\"ARTICLERENT\"")]
    public class ArticleRent
    {
        [Identity]
        public int Id { get; set; }

        [DataMember(Column = "ARTICLE", Type = DataType.Entity)]
        public Article Article { get; set; }

        [DataMember(Column = "RENT", RawType = typeof (Rent))]
        public int Rent { get; set; }

        [DataMember(Column = "AMOUNT")]
        public int Amount { get; set; }

        [DataMember(Column = "COST")]
        public int Cost { get; set; }
    }
}