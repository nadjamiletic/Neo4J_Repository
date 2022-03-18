using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4J_Repository.DomainModel
{
    public class Popust
    {
        public String id { get; set; }
        public string tipS { get; set; }
        public TipProizvoda tip { get; set; }
        public int popust { get; set; }
        public string kupacIme { get; set; }
        public Kupac kupac { get; set; }
        public string skladisteIme { get; set; }
        public Skladiste skladiste { get; set; }
        
    }
}
