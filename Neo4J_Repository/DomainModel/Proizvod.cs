using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4J_Repository.DomainModel
{
    public class Proizvod
    {
        public String id { get; set; }
        public String ime { get; set; }
        public DateTime roktrajanja { get; set; }
        public float tezina { get; set; }
        public TipProizvoda tip { get; set; }
        public float cena { get; set; }
        public int kolicina { get; set; }
        public string skladisteIme { get; set; }
        public Skladiste skladiste{ get; set; }
    }
}
