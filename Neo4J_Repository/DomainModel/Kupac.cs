﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4J_Repository.DomainModel
{
    public class Kupac
    {
        public String id { get; set; }
        public String adresa { get; set; }
        public String  ime { get; set; }
        public String telefon { get; set; }
        public String  email { get; set; }
        public List<Racun> racuni;
        public List<Popust> popusti;
    }
}
