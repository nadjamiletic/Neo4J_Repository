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
    public partial class AddSkladiste : Form
    {
        Skladiste skladiste;
        public GraphClient graphClient;
        public AddSkladiste()
        {
            InitializeComponent();
            skladiste = new Skladiste();
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            if (this.Validation())
            {
                skladiste.ime = txtIme.Text;
                
                //dodati maximum da bude
                List<Skladiste> skladista = DataProvider.GetSkladista();
                int max = -1;
                foreach (Skladiste t in skladista)
                {
                    if (int.Parse(t.id) > max)
                        max = int.Parse(t.id);
                }
                skladiste.id = (max + 1).ToString();
                DataProvider.AddSkladiste(skladiste);

 

                

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Niste uneli sve  podatke");
            }
        }

        private bool Validation()
        {

            if (txtIme.Text.Equals("") )
            {
                return false;
            }
            return true;

        }

        private void btnDodajProizvodUSkladiste_Click(object sender, EventArgs e)
        {
            AddProizvod p = new AddProizvod();
            p.Show();
        }
    }
}
