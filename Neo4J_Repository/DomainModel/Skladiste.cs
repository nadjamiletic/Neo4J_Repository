using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4J_Repository.DomainModel
{
    public class Skladiste
    {
        
        public String id { get; set; }
        public String ime { get; set; }
     

        public Dictionary<object, int> kolicina;
    }
}
