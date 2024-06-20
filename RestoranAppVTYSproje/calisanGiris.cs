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
    public partial class calisanGiris : Form
    {
        public calisanGiris()
        {
            InitializeComponent();
        }
        
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
           "Database=db_vtysproje; user ID=postgres; password=******");
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Kullanici Adi Veya Şifre Boş Bırakılamaz!!", "Boş Değer Hatası", MessageBoxButtons.OK);
                return;
            }
            string sorgu = "select*from calisan_list where kullaniciad=@kullaniciad AND sifre=@sifre";
            NpgsqlDataReader dr;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@kullaniciad", textBox1.Text);
            komut.Parameters.AddWithValue("@sifre", textBox2.Text);
            baglanti.Open();
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                string hosgeldinMesaj = "Hoşgeldin " + dr.GetString(1).ToUpper() + "\nİyi Çalışmalar";
                MessageBox.Show(hosgeldinMesaj, "Hoşgeldin!");
                yoneticiAnaEkran yntc = new yoneticiAnaEkran();
                yntc.label7.Text = Convert.ToString(dr.GetString(1));
                yntc.kullanicid.Text = Convert.ToString(dr.GetValue(0));
                yntc.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı Veya Şifre Yanlış!!", "Yanlışlık Var!!");
            }
            baglanti.Close();
        }

        private void calisanGiris_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
