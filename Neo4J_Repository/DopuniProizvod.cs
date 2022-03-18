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

namespace Neo4J_Repository
{
    public partial class DopuniProizvod : Form
    {
        public DopuniProizvod()
        {
            InitializeComponent();
            cbSkladistePopuni();
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
                
                if (p.skladisteIme == sel)
                {
                    d.Add(p.ime);
                    
                }
                

            }

            List<string> filter = d.Distinct().ToList();

            foreach (string e in filter)
            {
                cbIme.Items.Add(e);
            }
        }

        private void cbSkladiste_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbIme.Items.Clear();
            cbProizvodiPopuni(cbSkladiste.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Proizvod p1 = DataProvider.GetProizvodis(cbSkladiste.Text.ToString(), cbIme.Text.ToString())[0];
            p1.kolicina += int.Parse(txtKolicina.Text);
            DataProvider.UpdateProizvod(p1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
