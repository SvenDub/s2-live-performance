using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Live_Performance.Persistence;
using DataType = Live_Performance.Persistence.DataType;

namespace Live_Performance.Entity
{
    [Entity(Table = "\"RENT\"")]
    public class Rent
    {
        [Identity]
        public int Id { get; set; }

        [DataMember(Column = "BEGIN")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime Begin { get; set; }

        [DataMember(Column = "END")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime End { get; set; }

        [DataMember(Column = "RENTER", Type = DataType.Entity)]
        public User User { get; set; }

        [DataMember(Type = DataType.OneToManyEntity)]
        public List<AreaRent> Areas { get; set; } = new List<AreaRent>();

        [DataMember(Type = DataType.OneToManyEntity)]
        public List<ArticleRent> Articles { get; set; } = new List<ArticleRent>();

        [DataMember(Type = DataType.OneToManyEntity)]
        public List<BoatRent> Boats { get; set; } = new List<BoatRent>();
    }
}