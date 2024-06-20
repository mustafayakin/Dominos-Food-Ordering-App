using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestoranAppVTYSproje
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            uyeGiris uye = new uyeGiris();
            uye.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            calisanGiris clsn = new calisanGiris();
            clsn.Show();
            this.Hide();
        }
    }
}
