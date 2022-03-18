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
    public partial class AddTipProizvodaForm : Form
    {
        TipProizvoda tip;
        public GraphClient graphClient;
        public AddTipProizvodaForm()
        {
            InitializeComponent();
            tip = new TipProizvoda();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Validation())
            {
                tip.ime = txtIme.Text;
                tip.opis = txtOpis.Text;
                tip.skladista = new List<Skladiste>(); 
                //dodati maximum da bude
                List<TipProizvoda> tipovi = DataProvider.GetTipProizvoda();
                int max = -1;
                foreach (TipProizvoda t in tipovi)
                {
                    if (int.Parse(t.id) > max)
                        max = int.Parse(t.id);
                }
                tip.id = (max + 1).ToString();
                DataProvider.AddTipProizvoda(tip);

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
                    tip.skladista.Add(a);
                    DataProvider.AddRelationTipProizvodaSkladiste(tip, a);
                    DataProvider.AddRelationSkladisteTipProizvoda(a,tip);
                }
                else
                {
                    DataProvider.AddRelationTipProizvodaSkladiste(tip, DataProvider.GetSkladiste(txtSkladiste.Text)[0]);
                    DataProvider.AddRelationSkladisteTipProizvoda(DataProvider.GetSkladiste(txtSkladiste.Text)[0],tip);
                }
                

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Niste uneli sve  podatke");
            }
        }

        private bool Validation()
        {

            if (txtIme.Text.Equals("") || txtSkladiste.Text.Equals("") || txtOpis.Text.Equals(""))
            {
                return false;
            }
            return true;

        }
    }
}
