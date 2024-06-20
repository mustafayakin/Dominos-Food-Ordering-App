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
    public partial class uyeEkrani : Form
    {
        public uyeEkrani()
        {
            InitializeComponent();
        }
        private void mdigetir(Form frm)//yeni mdicgiparent açmak için fonksiyon
        {
            panel1.Controls.Clear();
            frm.MdiParent = this;
            frm.FormBorderStyle = FormBorderStyle.None;
            panel1.Controls.Add(frm);
            frm.Show();
        }
        private void siparişVerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            siparisVer sipver = new siparisVer();
            sipver.label4.Text = kullanicid.Text;
            mdigetir(sipver);
        }

        private void uyeEkrani_Load(object sender, EventArgs e)
        {   

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            siparisVer sipver = new siparisVer();
            sipver.label4.Text = kullanicid.Text;
            mdigetir(sipver);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            siparisVer sipver = new siparisVer();
            sipver.label4.Text = kullanicid.Text;
            mdigetir(sipver);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            siparisVer sipver = new siparisVer();
            sipver.label4.Text = kullanicid.Text;
            mdigetir(sipver);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            siparisVer sipver = new siparisVer();
            sipver.label4.Text = kullanicid.Text;
            mdigetir(sipver);
        }

        private void siparişlerimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            memberSiparislerim msip = new memberSiparislerim();
            msip.label2.Text = kullanicid.Text;
            mdigetir(msip);
        }

        private void bilgilerimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            memberInformation meminf = new memberInformation();
            meminf.label10.Text = kullanicid.Text;
            mdigetir(meminf);

        }

        private void label4_Click(object sender, EventArgs e)
        {
            siparisVer sipver = new siparisVer();
            sipver.label4.Text = kullanicid.Text;
            mdigetir(sipver);
        }

        private void yorumlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            memberYorumlar memyorum = new memberYorumlar();
            memyorum.label10.Text = kullanicid.Text;
            mdigetir(memyorum);
        }

        private void uyeEkrani_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
