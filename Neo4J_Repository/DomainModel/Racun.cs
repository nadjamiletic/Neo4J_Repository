using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4J_Repository.DomainModel
{
    public class Racun
    {
        public String id { get; set; }
        public DateTime datum { get; set; }
        public float ukupnaCena { get; set; }
        public Kupac kupac { get; set; }
        public Skladiste skladiste { get; set; }
        public Dictionary<string, int> proizvodi;//ime proizvoda+kolicina
        public List<Proizvod> listaProizvoda;
    }
}
