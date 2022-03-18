using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4J_Repository.DomainModel;
using Neo4jClient;

namespace Neo4J_Repository
{
    public class DataProvider
    {
        public static GraphClient client;

        public static void ConnectToTheBase()
        {
            client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "edukacija");
            try
            {
                client.Connect();
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.Message);
            }

        }
        #region add
        internal static void AddTipProizvoda(TipProizvoda newTipProizvoda)
        {
            ConnectToTheBase();
            client.Cypher
               .Create("(t:TipProizvoda {newTipProizvoda})")
               .WithParam("newTipProizvoda", newTipProizvoda)
               .ExecuteWithoutResults();
        }

        internal static void AddSkladiste(Skladiste newSkladiste)
        {
            ConnectToTheBase();
            client.Cypher
               .Create("(s: Skladiste {newSkladiste})")
               .WithParam("newSkladiste", newSkladiste)
               .ExecuteWithoutResults();
        }

        internal static Proizvod UpdateProizvod(Proizvod  p)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("ime", p.ime);
            queryDict.Add("kolicina", p.kolicina);
            var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*)where(n:Proizvod)and exists(n.ime) and n.ime=~ '" + p.ime +
                "'set n.kolicina= '" + p.kolicina + "'return n", queryDict, Neo4jClient.Cypher.CypherResultMode.Set);
            List<Proizvod> proizvods = ((IRawGraphClient)client).ExecuteGetCypherResults<Proizvod>(query).ToList();
            Proizvod proizvod = proizvods.Find(x => x.ime == p.ime);
            return proizvod;
        }

        internal static void AddProizvod(Proizvod newProizvod)
        {
            ConnectToTheBase();
            client.Cypher
               .Create("(p :Proizvod {newProizvod})")
               .WithParam("newProizvod", newProizvod)
               .ExecuteWithoutResults();
        }

        internal static void UpdateProizvod(Proizvod newProizvod,int k)
        {
            ConnectToTheBase();
            client.Cypher.Match("(a:Proizvod)")
                  //.Set("newProizvod.kolicina = k")
                  .Where("a.id == {newProizvod.id}")
                  .Set("newProizvod.kolicina = k")
                  .ExecuteWithoutResults();
        }

        internal static void AddKupac(Kupac newKupac)
        {
            ConnectToTheBase();
            client.Cypher
               .Create("(k:Kupac {newKupac})")
               .WithParam("newKupac", newKupac)
               .ExecuteWithoutResults();
        }

        internal static void AddRacun(Racun newRacun)
        {
            ConnectToTheBase();
            client.Cypher
               .Create("(r:Racun {newRacun})")
               .WithParam("newRacun", newRacun)
               .ExecuteWithoutResults();
        }

        internal static void AddPopust(Popust newPopust)
        {
            ConnectToTheBase();
            client.Cypher
               .Create("(o:Popust {newPopust})")
               .WithParam("newPopust", newPopust)
               .ExecuteWithoutResults();
        }

        #endregion add
        
        #region relations
        public static void AddRelationRacunProizvod(Racun racun, Proizvod proizvod)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(r:Racun)", "(p:Proizvod)")
                  .Where((Racun r) => r.id == racun.id)
                  .AndWhere((Proizvod p) => p.id == proizvod.id)
                  .Create("(r)-[:sadrzi]->(p)")
                  .ExecuteWithoutResults();
        }

        public static void AddRelationRacunKupac(Racun racun, Kupac kupac)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(r:Racun)", "(k:Kupac)")
                  .Where((Racun r) => r.id == racun.id)
                  .AndWhere((Kupac k) => k.id == kupac.id)
                  .Create("(r)-[:pripada]->(k)")
                  .ExecuteWithoutResults();
        }

        public static void AddRelationRacunSkladiste(Racun racun, Skladiste skladiste)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(r:Racun)", "(s:Skladiste)")
                  .Where((Racun r) => r.id == racun.id)
                  .AndWhere((Skladiste s) => s.id == skladiste.id)
                  .Create("(r)-[:za_skladiste]->(s)")
                  .ExecuteWithoutResults();
        }

        public static void AddRelationProizvodTipProizvoda(Proizvod proizvod, TipProizvoda tipProizvoda)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(p:Proizvod)", "(t:TipProizvoda)")
                  .Where((Proizvod p) => p.id == proizvod.id)
                  .AndWhere((TipProizvoda t) => t.id == tipProizvoda.id)
                  .Create("(p)-[:ima_tip]->(t)")
                  .ExecuteWithoutResults();
        }

        public static void AddRelationTipProizvodaSkladiste(TipProizvoda tipProizvoda, Skladiste skladiste)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(t:TipProizvoda)", "(s:Skladiste)")
                  .Where((TipProizvoda t) => t.id == tipProizvoda.id)
                  .AndWhere((Skladiste s) => s.id == skladiste.id)
                  .Create("(t)-[:IMA_U]->(s)")
                  .ExecuteWithoutResults();
        }

        public static void AddRelationTipProizvodaProizvod(TipProizvoda tipProizvoda, Proizvod proizvod)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(t:TipProizvoda)", "(p:Proizvod)")
                  .Where((TipProizvoda t) => t.id == tipProizvoda.id)
                  .AndWhere((Proizvod p) => p.id == proizvod.id)
                  .Create("(t)-[:JE_TIP]->(p)")
                  .ExecuteWithoutResults();
        }

        public static void AddRelationSkladisteTipProizvoda(Skladiste skladiste,TipProizvoda tipProizvoda)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(s:Skladiste)", "(t:TipProizvoda)")
                  .Where((Skladiste s) => s.id == skladiste.id)
                  .AndWhere((TipProizvoda t) => t.id == tipProizvoda.id)
                  .Create("(s)-[:ima_tip_proizvoda]->(t)")
                  .ExecuteWithoutResults();
        }

        public static void AddRelationSkladisteProizvodKolicina(Skladiste skladiste, Proizvod proizvod,int kolicina)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(s:Skladiste)", "(p:Proizvod)")
                  .Where((Skladiste s) => s.id == skladiste.id)
                  .AndWhere((Proizvod p) => p.id == proizvod.id)
                  .WithParam(proizvod.ToString(),kolicina)
                  .Create("(s)-[:ima_proizvod_kolicina]->(p)")
                  .ExecuteWithoutResults();
        }

        public static void AddRelationSkladisteProizvod(Skladiste skladiste, Proizvod proizvod)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(s:Skladiste)", "(p:Proizvod)")
                  .Where((Skladiste s) => s.id == skladiste.id)
                  .AndWhere((Proizvod p) => p.id == proizvod.id)
                  .Create("(s)-[:ima_proizvod_kolicina]->(p)")
                  .ExecuteWithoutResults();
        }

        public static void AddRelationProizvodSkladiste( Proizvod proizvod, Skladiste skladiste)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(p:Proizvod)", "(s:Skladiste)")
                  .Where((Proizvod p) => p.id == proizvod.id)
                  .AndWhere((Skladiste s) => s.id == skladiste.id)
                  .Create("(p)-[:pripada_skladistu]->(s)")
                  .ExecuteWithoutResults();
        }

        public static void AddRelationPopustKupac(Popust popust, Kupac kupac)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(o:Popust)", "(k:Kupac)")
                  .Where((Popust o) => o.id == popust.id)
                  .AndWhere((Kupac k) => k.id == kupac.id)
                  .Create("(o)-[:je_ostvario]->(k)")
                  .ExecuteWithoutResults();
        }

        public static void AddRelationPopustTipProizvoda(Popust popust, TipProizvoda tipProizvoda)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(o:Popust)", "(t:TipProizvoda)")
                  .Where((Popust o) => o.id == popust.id)
                  .AndWhere((TipProizvoda t) => t.id == tipProizvoda.id)
                  .Create("(o)-[:za_tip]->(t)")
                  .ExecuteWithoutResults();
        }

        public static void AddRelationPopustSkladiste(Popust popust, Skladiste skladiste)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(o:Popust)", "(s:Skladiste)")
                  .Where((Popust o) => o.id == popust.id)
                  .AndWhere((Skladiste s) => s.id == skladiste.id)
                  .Create("(o)-[:u_skladistu]->(s)")
                  .ExecuteWithoutResults();
        }

        public static void AddRelationKupacRacun(Kupac kupac, Racun racun)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(k:Kupac)", "(r:Racun)")
                  .Where((Kupac k) => k.id == kupac.id)
                  .AndWhere((Racun r) => r.id == racun.id)
                  .Create("(k)-[:pazario]->(r)")
                  .ExecuteWithoutResults();
        }

        public static void AddRelationKupacPopust(Kupac kupac, Popust popust)
        {
            ConnectToTheBase();
            client.Cypher
                  .Match("(k:Kupac)", "(o:Popust)")
                  .Where((Kupac k) => k.id == kupac.id)
                  .AndWhere((Popust o) => o.id == popust.id)
                  .Create("(k)-[:ostvario" +
                  "]->(o)")
                  .ExecuteWithoutResults();
        }
        #endregion relations

        #region gets
        public static Kupac GetKupac(String ime)
        {
            ConnectToTheBase();
            List<Kupac> kupci = client.Cypher.Match("(a:Kupac)")
                .Where("a.ime={ime}")
                .WithParam("ime", ime).
                Return(a => a.As<Kupac>()).Results.ToList();
            if (kupci.Count != 0)
                return kupci[0];
            else return null;
        }

        public static List<Racun> GetRacune(Kupac kupac)
        {
            ConnectToTheBase();
            List<Racun> at =client.Cypher.Match("(r:Racun)")
                .Where("r.kupac.id={kupac.id}").
                WithParam("kupac", kupac).
                Return(r => r.As<Racun>()).Results.ToList();
            return at;
        }

        public static List<Racun> GetRacun()
        {
            ConnectToTheBase();
            List<Racun> racuni =client.Cypher.Match("(racun:Racun)")
                     .Return(racun => racun.As<Racun>())
                     .Results.ToList();
            return racuni;
        }
        public static List<Popust> GetPopuste(Kupac kupac)
        {
            ConnectToTheBase();
            List<Popust> at =
              client.Cypher.Match("(a:Popust)")
                .Where("a.kupac={kupac}").
                WithParam("kupac", kupac).
                Return(a => a.As<Popust>()).Results.ToList();
            return at;
        }

        public static List<Popust> GetPopust()
        {
            ConnectToTheBase();
            List<Popust> popusti =
             client.Cypher
                     .Match("(popust:Popust)")
                     .Return(popust => popust.As<Popust>())
                     .Results.ToList();
            return popusti;
        }
        public static List<TipProizvoda> GetTipProizvoda()
        {
            ConnectToTheBase();
            List<TipProizvoda> tipovi =
             client.Cypher
                     .Match("(tip:TipProizvoda)")
                     .Return(tip => tip.As<TipProizvoda>())
                     .Results.ToList();
            return tipovi;
        }

        public static List<TipProizvoda> GetTipProizvoda1(string ime)
        {
            ConnectToTheBase();
            return client.Cypher.Match("(s:TipProizvoda)")
                .Where("s.ime={ime}").
                WithParam("ime", ime).
                Return(s => s.As<TipProizvoda>()).Results.ToList();
        }

        public static List<Skladiste> GetSkladista()
        {
            ConnectToTheBase();
            List<Skladiste> skladista =
             client.Cypher
                     .Match("(s:Skladiste)")
                     .Return(s => s.As<Skladiste>())
                     .Results.ToList();
            return skladista;
        }

        public static List<Kupac> GetKupce()
        {
            ConnectToTheBase();
            List<Kupac> kupci =
             client.Cypher
                     .Match("(k:Kupac)")
                     .Return(k => k.As<Kupac>())
                     .Results.ToList();
            return kupci;
        }

        
        public static List<Skladiste> GetSkladiste(string skladiste)
        {
            ConnectToTheBase();
            return client.Cypher.Match("(s:Skladiste)")
                .Where("s.ime={skladiste}").
                WithParam("skladiste", skladiste).
                Return(s => s.As<Skladiste>()).Results.ToList();
        }

        public static List<Skladiste> GetSkladistee(Skladiste skladiste)
        {
            ConnectToTheBase();
            return client.Cypher.Match("(s:Skladiste)")
                .Where("s={skladiste}").
                WithParam("skladiste", skladiste).
                Return(s => s.As<Skladiste>()).Results.ToList();
        }
        public static List<Proizvod> GetProizvod(string proizvod)
        {
            ConnectToTheBase();
            return client.Cypher.Match("(p:Proizvod)")
                .Where("p.ime={proizvod}").
                WithParam("proizvod", proizvod).
                Return(p => p.As<Proizvod>()).Results.ToList();
        }

        public static List<Proizvod> GetProizvodi()
        {
            ConnectToTheBase();
            List<Proizvod> proizvodi =
             client.Cypher
                     .Match("(p:Proizvod)")
                     .Return(p => p.As<Proizvod>())
                     .Results.ToList();
            return proizvodi;
        }

        public static List<Proizvod> GetProizvodis(string skladiste,string p)
        {
            ConnectToTheBase();
            List<Proizvod> at =
              client.Cypher.Match("(a:Proizvod)")
                .Where("a.ime={p}").
               AndWhere("a.skladisteIme={skladiste}").
                WithParam("skladiste", skladiste).WithParam("p", p).
                Return(a => a.As<Proizvod>()).Results.ToList();
            return at;

        }
        #endregion gets
    }
}
