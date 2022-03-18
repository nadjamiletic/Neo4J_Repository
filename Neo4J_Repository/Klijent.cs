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
    public partial class Klijent : Form
    {
        public Klijent()
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
            AddKupac a = new AddKupac();
            a.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RacunForm r = new RacunForm();
            r.Show();
        }
    }
}
