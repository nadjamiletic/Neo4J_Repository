using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neo4J_Repository.DomainModel;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Neo4J_Repository
{
    public partial class RacunForm : Form
    {
        Racun racun;
        public GraphClient graphClient;
        List<Proizvod> pomocnalista = new List<Proizvod>();
        Dictionary<Proizvod, int> parProizvodKolicina = new Dictionary<Proizvod, int>();
        Dictionary<string, int> pomocniDictionary = new Dictionary<string, int>();
        public RacunForm()
        {
            InitializeComponent();
            cbSkladistePopuni();
            racun = new Racun();
            racun.listaProizvoda = new List<Proizvod>();
            //racun.proizvodi = new Dictionary<string, int>();
        }

        private void RacunForm_Load(object sender, EventArgs e)
        {
           
        }

        public void cbSkladistePopuni()
        {

            
            List<Proizvod> podaci = DataProvider.GetProizvodi();
            List<string> d = new List<string>();
            foreach (Proizvod s in podaci)
            {
                d.Add(s.skladisteIme);
            }

            List<string> filter = d.Distinct().ToList();

            foreach (string s in filter)
            {
                cbSkladiste.Items.Add(s);
            }
        }

        public void cbProizvodiPopuni(string sel)
        {

            List<String> d = new List<string>();
           
            List<Proizvod> podaci = DataProvider.GetProizvodi();

            foreach (Proizvod p in podaci)
            {
                
                if (p.skladisteIme==sel)
                {
                    d.Add(p.ime);
                    
                }
                    
                
            }

            List<string> filter = d.Distinct().ToList();

            foreach (string e in filter)
            {
                cbProizvodi.Items.Add(e);
            }
        }

        private void cbSkladiste_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbSkladiste.Enabled = false;
            cbProizvodi.Items.Clear();
            cbProizvodiPopuni(cbSkladiste.Text.ToString());
        }

        public float IzracunajUkupnuCenu(Dictionary<string,int> d,List<Proizvod> lp) {
            float sum = 0;
            foreach (KeyValuePair<string, int> v in d)
            {
                foreach (Proizvod p in lp)
                    if (p.ime == v.Key)
                        sum += p.cena * v.Value;

            }
            return sum;
        }

        public int IzracunajPopust(float cena) {
            if (cena < 2000)
                return 0;
            else if (cena < 5000)
                return 2;
            else if (cena < 10000)
                return 4;
            else if (cena < 20000)
                return 6;
            else if (cena > 25000)
                return 8;
            return 0;
        }

        public void SmanjiKolicinuUSkladistu(Dictionary<string,int> lista)
        {
            foreach (KeyValuePair<string, int> v in lista) {
                string s = v.Key;
                Proizvod p = DataProvider.GetProizvod(s)[0];
                p.kolicina -= v.Value;
            }
                
        }

        private void btnRacun_Click(object sender, EventArgs e)
        {
                List<Racun> racuni = DataProvider.GetRacun();
                int max = -1;
                foreach (Racun t in racuni)
                {
                    if (int.Parse(t.id) > max)
                        max = int.Parse(t.id);
                }
                racun.id = (max + 1).ToString();
            //MessageBox.Show(racun.id.ToString());
                racun.datum = DateTime.Now;
                racun.ukupnaCena = 0;

            DataProvider.AddRacun(racun);
            racun.kupac = DataProvider.GetKupac(txtKupac.Text);
            DataProvider.AddRelationKupacRacun(DataProvider.GetKupac(txtKupac.Text), racun);
            DataProvider.AddRelationRacunKupac(racun, DataProvider.GetKupac(txtKupac.Text));

            string pom = cbSkladiste.Text.ToString();
            racun.skladiste = DataProvider.GetSkladiste(pom)[0];
            DataProvider.AddRelationRacunSkladiste(racun, DataProvider.GetSkladiste(pom)[0]);
            racun.proizvodi = pomocniDictionary;
            racun.listaProizvoda = pomocnalista;
               
                //deo za dodavanje proizvoda ide preko dugmeta gde se pune liste dictionary i lista proizvoda...
                 racun.ukupnaCena = IzracunajUkupnuCenu(racun.proizvodi, racun.listaProizvoda);
            //MessageBox.Show(racun.ukupnaCena.ToString());
                 //provera postojecih popusta za datog kupca prebaciti kod dole jer ovde nema smisla kad dodajemo kupca da ru vec ima popusta
                 List<Popust> w = DataProvider.GetPopust();
                 int maxp = 0;
                 foreach (Popust t in w)
                 {
                     if (t.skladisteIme == cbSkladiste.Text&& t.kupacIme==txtKupac.Text)
                         if (maxp < t.popust)
                             maxp = t.popust;
                 }
            //MessageBox.Show(maxp.ToString());
            float y =(float)((100 - maxp) /100.0);
            
            racun.ukupnaCena = y* IzracunajUkupnuCenu(racun.proizvodi, racun.listaProizvoda);
            //MessageBox.Show(racun.ukupnaCena.ToString());
                 //dodavanje popusta 
                int popust = IzracunajPopust(racun.ukupnaCena);
                 Popust o = new Popust();

                 List<Popust> popusti = DataProvider.GetPopust();
                 int max1 = -1;
                 foreach (Popust t in popusti)
                 {
                     if (int.Parse(t.id) > max1)
                         max1 = int.Parse(t.id);
                 }
                 o.id = (max1 + 1).ToString();

                 o.popust = popust;
            o.kupacIme = txtKupac.Text;
            o.skladisteIme = cbSkladiste.Text;
            
            DataProvider.AddPopust(o);
            o.kupac = racun.kupac;
            o.skladiste = racun.skladiste;
            SmanjiKolicinuUSkladistu(pomocniDictionary);
            // DataProvider.AddPopust(o);

            DataProvider.AddRelationKupacPopust(o.kupac,o);

            //MessageBox.Show(racunStampa(pomocniDictionary, racun.ukupnaCena,y));
            MessageBox.Show(racunStampaP(parProizvodKolicina, racun.ukupnaCena, (float)maxp));
            
            this.Close();
        }
        public string racunStampa(Dictionary<string,int> d,float ukupnaCena,float y) {
            string s="Proizvodi: \n";
            foreach (KeyValuePair<string, int> t in d)
            {
                s += t.Key + "   " + t.Value.ToString()+ "\n";
            }
            s += "Popust :    " +((1-y)*100).ToString()+"% \n";
            s +="Ukupno za placanje:    "+ ukupnaCena.ToString();
            return s;

        }

        public string racunStampaP(Dictionary<Proizvod, int> d, float ukupnaCena, float y)
        {
            string s = "Proizvodi: \n";
            foreach (KeyValuePair<Proizvod, int> t in d)
            {
                s += t.Key.ime + "   " + t.Value.ToString() +"x" +t.Key.cena +"\n";
            }
            s += "Popust :    " + y.ToString() + "% \n";
            //s += "Popust :    " + ((1 - y) * 100).ToString() + "% \n";
            s += "Ukupno za placanje:    " + ukupnaCena.ToString();
            return s;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Proizvod p = DataProvider.GetProizvod(cbProizvodi.Text.ToString())[0];
            Proizvod p1 = DataProvider.GetProizvodis(cbSkladiste.Text.ToString(), cbProizvodi.Text.ToString())[0];
            //MessageBox.Show(p.kolicina.ToString());
            if (p1.kolicina - int.Parse(txtKoicina.Text) >= 0)
            {
                p1.kolicina -= int.Parse(txtKoicina.Text);
            }
            else
            {
                MessageBox.Show("Kolicinu koju trazite nemamo u skladistu,mozete uzeti samo:" + p1.kolicina.ToString() + "robe");
            }
                int u = p1.kolicina;
            //MessageBox.Show(p.kolicina.ToString());
           
            
            pomocnalista.Add(p1);
            pomocniDictionary.Add(p1.ime, int.Parse(txtKoicina.Text.ToString()));
            parProizvodKolicina.Add(p1, int.Parse(txtKoicina.Text.ToString()));
           
            DataProvider.UpdateProizvod(p1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddKupac a = new AddKupac();
            a.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
