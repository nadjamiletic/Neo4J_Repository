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

namespace Neo4J_Repository
{
    public partial class AddProizvod : Form
    {
        Proizvod proizvod;
        public GraphClient graphClient;
        public AddProizvod()
        {
            InitializeComponent();
            proizvod = new Proizvod();
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            if (this.Validation())
            {
                proizvod.ime = txtIme.Text;
                proizvod.tezina = float.Parse(txtTezina.Text);
                proizvod.cena = float.Parse(txtCena.Text);
                proizvod.roktrajanja = dateTimePicker1.Value;
                proizvod.kolicina = int.Parse(txtKolicina.Text);
                //proizvod.tip = new TipProizvoda();
                //dodati maximum da bude
                List<Proizvod> proizvodi = DataProvider.GetProizvodi();
                int max = -1;
                foreach (Proizvod t in proizvodi)
                {
                    if (int.Parse(t.id) > max)
                        max = int.Parse(t.id);
                }
                proizvod.id = (max + 1).ToString();

                proizvod.skladisteIme = txtSkladiste.Text; 
                DataProvider.AddProizvod(proizvod);

                if (DataProvider.GetTipProizvoda1(txtTip.Text).Count == 0)
                {
                    TipProizvoda a = new TipProizvoda
                    {
                        ime = txtTip.Text,


                    };
                    int m = -1;
                    foreach (TipProizvoda s in DataProvider.GetTipProizvoda())
                    {
                        if (int.Parse(s.id) > m)
                            m = int.Parse(s.id);
                    }
                    a.id = (m + 1).ToString();
                    DataProvider.AddTipProizvoda(a);
                    proizvod.tip = a;
                    DataProvider.AddRelationProizvodTipProizvoda(proizvod, a);
                    DataProvider.AddRelationTipProizvodaProizvod(a, proizvod);
                }
                else
                {
                    proizvod.tip = DataProvider.GetTipProizvoda1(txtTip.Text)[0];
                    DataProvider.AddRelationProizvodTipProizvoda(proizvod, DataProvider.GetTipProizvoda1(txtTip.Text)[0]);
                    DataProvider.AddRelationTipProizvodaProizvod(DataProvider.GetTipProizvoda1(txtTip.Text)[0], proizvod);
                }

                if (DataProvider.GetSkladiste(txtSkladiste.Text).Count == 0)
                {
                    Skladiste a = new Skladiste
                    {
                        ime = txtSkladiste.Text,
                        
                    };
                    int m = -1;
                    foreach (Skladiste s in DataProvider.GetSkladista())
                    {
                        if (int.Parse(s.id) > m)
                            m = int.Parse(s.id);
                    }
                    a.id = (m + 1).ToString();
                 
                    
                    DataProvider.AddSkladiste(a);
                   
                    proizvod.skladisteIme = txtSkladiste.Text;

                    DataProvider.AddRelationProizvodSkladiste(proizvod,a);
                    DataProvider.AddRelationSkladisteProizvod(a, proizvod);
                    
                    
                }
                else
                {
                   
                    proizvod.skladisteIme = txtSkladiste.Text;
                    proizvod.skladiste = DataProvider.GetSkladiste(txtSkladiste.Text)[0];
                    DataProvider.AddRelationProizvodSkladiste(proizvod, DataProvider.GetSkladiste(txtSkladiste.Text)[0]);
                    DataProvider.AddRelationSkladisteProizvod(DataProvider.GetSkladiste(txtSkladiste.Text)[0], proizvod);
                    
                }
                this.Close();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Niste uneli sve  podatke");
            }
        }

        private bool Validation()
        {

            if (txtIme.Text.Equals("") || txtTezina.Text.Equals("") || txtCena.Text.Equals("") || txtTip.Text.Equals("") || dateTimePicker1.Value.Equals("")||txtSkladiste.Text.Equals("")||txtKolicina.Text.Equals(""))
            {
                return false;
            }
            return true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
