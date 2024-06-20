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
    public partial class uyeGiris : Form
    {
        public uyeGiris()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
            "Database=db_vtysproje; user ID=postgres; password=*****");
        private void button2_Click(object sender, EventArgs e)
        {
            kayit_ol kayit = new kayit_ol();
            kayit.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Kullanici Adi Veya Şifre Boş Bırakılamaz!!", "Boş Değer Hatası", MessageBoxButtons.OK);
                return;
            }
            string sorgu = "select*from uye_list where kullaniciad=@kullaniciad AND sifre=@sifre";
            NpgsqlDataReader dr;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@kullaniciad",textBox1.Text);
            komut.Parameters.AddWithValue("@sifre", textBox2.Text);
            baglanti.Open();
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                if (Convert.ToInt32(dr.GetValue(9)) != 0){
                    MessageBox.Show("Admin\'ler Buraya Giremez", "Bir Admin Gördüm Sanki!");
                }
                else
                {
                    string hosgeldinMesaj = "Hoşgeldin " + dr.GetString(1).ToUpper() + "\nKeyifli Siparişler!";
                    MessageBox.Show(hosgeldinMesaj, "Hoşgeldin!");
                    uyeEkrani uyeekran = new uyeEkrani();
                    uyeekran.label7.Text = dr.GetString(1);
                    uyeekran.kullanicid.Text = Convert.ToString(dr.GetValue(0));
                    uyeekran.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Şifre Veya Kullanici İD Yanlış!!", "Yanlışlık Var!!");
            }
            baglanti.Close();
        }

        private void uyeGiris_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
