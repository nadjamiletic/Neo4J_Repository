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
    public partial class AddKupac : Form
    {
        Kupac kupac;
        public GraphClient graphClient;
        public AddKupac()
        {
            InitializeComponent();
            kupac = new Kupac();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnDodajK_Click(object sender, EventArgs e)
        {
            if (this.Validation())
            {
                kupac.ime = txtIme.Text;
                kupac.adresa = txtAdresa.Text;
                kupac.telefon = txtTelefon.Text;
                kupac.email = txtEmail.Text;
                kupac.racuni = new List<Racun>();
                kupac.popusti = new List<Popust>();
                //dodati maximum da bude
                List<Kupac> kupci = DataProvider.GetKupce();
                int max = -1;
                foreach (Kupac t in kupci)
                {
                    if (int.Parse(t.id) > max)
                        max = int.Parse(t.id);
                }
                kupac.id = (max + 1).ToString();
                DataProvider.AddKupac(kupac);

               
                this.DialogResult = DialogResult.OK;
                
            }
            else
            {
                MessageBox.Show("Niste uneli sve  podatke");
            }
        }

        private bool Validation()
        {

            if (txtIme.Text.Equals("") || txtAdresa.Text.Equals("") || txtTelefon.Text.Equals("") || txtEmail.Text.Equals(""))
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
