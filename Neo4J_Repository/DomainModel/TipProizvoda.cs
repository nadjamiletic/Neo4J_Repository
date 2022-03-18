using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4J_Repository.DomainModel
{
    public class TipProizvoda
    {
        public String id { get; set; }
        public String  ime { get; set; }
        public String  opis { get; set; }
        public List<Skladiste> skladista { get; set; }
        
    }
}
