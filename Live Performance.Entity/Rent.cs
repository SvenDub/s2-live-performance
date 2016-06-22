using System;
using System.Collections.Generic;
using Live_Performance.Persistence;

namespace Live_Performance.Entity
{
    [Entity(Table = "\"RENT\"")]
    public class Rent
    {
        [Identity]
        public int Id { get; set; }

        [DataMember(Column = "BEGIN")]
        public DateTime Begin { get; set; }

        [DataMember(Column = "END")]
        public DateTime End { get; set; }

        [DataMember(Column = "USER", Type = DataType.Entity)]
        public User User { get; set; }

        [DataMember(Type = DataType.OneToManyEntity)]
        public List<AreaRent> Areas { get; set; }

        [DataMember(Type = DataType.OneToManyEntity)]
        public List<ArticleRent> Articles { get; set; }

        [DataMember(Type = DataType.OneToManyEntity)]
        public List<BoatRent> Boats { get; set; }
    }
}