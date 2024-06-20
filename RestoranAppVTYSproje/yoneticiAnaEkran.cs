using Npgsql;
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
    public partial class yoneticiAnaEkran : Form
    {
        public yoneticiAnaEkran()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=db_vtysproje; user ID=postgres; password=*****");
        private void mdigetir(Form frm)//yeni mdicgiparent açmak için fonksiyon
        {
            panel1.Controls.Clear();
            frm.MdiParent = this;
            frm.FormBorderStyle = FormBorderStyle.None;
            panel1.Controls.Add(frm);
            frm.Show();
        }
        private void yoneticiAnaEkran_Load(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            label1.Text = dt.ToString("dd/MM/yyyy");
            string sorgu = "select count(*) from siparis_list where tamamlanma=0";
            NpgsqlDataReader dr;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            baglanti.Open();
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                int siparissayisi = Convert.ToInt32(dr.GetValue(0));
                if(siparissayisi > 0)
                {
                    label2.Visible = true;
                    label6.Visible = true;
                    label6.Text = siparissayisi.ToString();
                }
            }
            baglanti.Close();
            sorgu = "select count(*) from yorumlar where yorum_durum=0";
            komut = new NpgsqlCommand(sorgu, baglanti);
            baglanti.Open();
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                int yorumsayisi = Convert.ToInt32(dr.GetValue(0));
                if (yorumsayisi > 0)
                {
                    label3.Visible = true;
                    label8.Visible = true;
                    label8.Text = yorumsayisi.ToString();
                }
            }
            baglanti.Close();
            sorgu = "select yetki_id from calisan_list where id=" + kullanicid.Text;
            komut = new NpgsqlCommand(sorgu, baglanti);
            baglanti.Open();
            dr = komut.ExecuteReader();
            int yetkiid = 0;
            if (dr.Read())
            {
                yetkiid = Convert.ToInt32(dr.GetValue(0));
                label11.Text = yetkiid.ToString();
            }
            baglanti.Close();
            sorgu = "select yetki_adi from calisan_yetki_adlari where yetki_id =" + yetkiid.ToString();
            komut = new NpgsqlCommand(sorgu, baglanti);
            baglanti.Open();
            dr = komut.ExecuteReader();
            string yetkiadi = "";
            if (dr.Read())
            {
                yetkiadi = dr.GetString(0);
            }
            baglanti.Close();
            label10.Text = yetkiadi;
        }

        private void yorumCevaplaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YorumislemleriForadmin yrmis = new YorumislemleriForadmin();
            yrmis.label10.Text = kullanicid.Text;
            mdigetir(yrmis);
        }

        private void yorumGüncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yorumGuncelle yrmgnc = new yorumGuncelle();
            mdigetir(yrmgnc);
        }

        private void üyeESGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(label11.Text) == 1)
            {
                MessageBox.Show("Kasiyer Olduğunuz İçin Bu İşlemi Yapamazsınız!","Yetki Hatası");
                return;
            }
            uye_e_s_gforAdmin uyeesg = new uye_e_s_gforAdmin();
            mdigetir(uyeesg);
        }

        private void üyeAramaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uyearaForAdmin uyeara = new uyearaForAdmin();
            mdigetir(uyeara);
        }

        private void yEMEKESGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(label11.Text) == 1)
            {
                MessageBox.Show("Kasiyer Olduğunuz İçin Bu İşlemi Yapamazsınız!", "Yetki Hatası");
                return;
            }
            yemekesgForAdm ymks = new yemekesgForAdm();
            mdigetir(ymks);
        }

        private void yetkiliESGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(label11.Text) == 1)
            {
                MessageBox.Show("Kasiyer Olduğunuz İçin Bu İşlemi Yapamazsınız!", "Yetki Hatası");
                return;
            }
            yetkiliesgForAdmin ytkl = new yetkiliesgForAdmin();
            mdigetir(ytkl);
        }

        private void üyeSeviyeListTriggerTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            seviyelisttriggerForadm svylst = new seviyelisttriggerForadm();
            mdigetir(svylst);
        }

        private void siparişlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            siparislerForAdm sipp = new siparislerForAdm();
            sipp.kullanicid.Text = kullanicid.Text;
            mdigetir(sipp);
        }

        private void siparişSilmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            siparissilGuncelleforAdmin sipg = new siparissilGuncelleforAdmin();
            sipg.label13.Text = kullanicid.Text;
            mdigetir(sipg);
        }

        private void yemekKategoriTriggerTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yemekKategoritrigger ymkt = new yemekKategoritrigger();
            mdigetir(ymkt);
        }

        private void yemekFiyatLoglarıtriggerTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yemekfiyatLoglari ymklog = new yemekfiyatLoglari();
            mdigetir(ymklog);
        }

        private void yemekKategoriTriggerTestToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            yemekKategoritrigger ymktr = new yemekKategoritrigger();
            mdigetir(ymktr);
        }

        private void yoneticiAnaEkran_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
