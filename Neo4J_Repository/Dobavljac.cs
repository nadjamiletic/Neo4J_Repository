using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neo4J_Repository
{
    public partial class Dobavljac : Form
    {
        public Dobavljac()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddProizvod a = new AddProizvod();
            a.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DopuniProizvod d = new DopuniProizvod();
            d.Show();
        }
    }
}
