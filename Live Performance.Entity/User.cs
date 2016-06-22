﻿using Live_Performance.Persistence;

namespace Live_Performance.Entity
{
    [Entity(Table = "\"USER\"")]
    public class User
    {
        [Identity]
        public int Id { get; set; }

        [DataMember(Column = "NAME")]
        public string Name { get; set; }

        [DataMember(Column = "EMAIL")]
        public string Email { get; set; }

        [DataMember(Column = "PASSWORD")]
        public string Password { get; set; }

        [DataMember(Column = "ADMIN")]
        public bool Admin { get; set; }
    }
}