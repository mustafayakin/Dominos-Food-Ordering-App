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
    public partial class kayit_ol : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
           "Database=db_vtysproje; user ID=postgres; password=*****");
        public kayit_ol()
        {
            InitializeComponent();
        }

        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox7.Text == "")
                {
                    MessageBox.Show("Zorunlu alanlardan biri girilmedi!", "Boş Değer Hatası", MessageBoxButtons.OK);
                    baglanti.Close();
                    return;
                }
                bool kullaniciVar = false;
                string sorgu = "select*from uye_list where kullaniciad=@kullaniciad";
                string sorgu1 = "INSERT INTO uye_list(ad,soyad,kullaniciad,sifre,adres,seviyeid,sehirid,ilceid,yetki_id) VALUES (@ad,@soyad,@kullaniciad,@sifre,@adres,@seviyeid,@sehirid,@ilceid,@yetki_id)";
                NpgsqlDataReader dr;       
                NpgsqlCommand komut1 = new NpgsqlCommand(sorgu, baglanti);
                komut1.Parameters.AddWithValue("@kullaniciad", textBox3.Text);
                baglanti.Open();
                dr = komut1.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("Başka biri bu kullanıcı adını kullanıyor!", "Bu kullanıcı adı alınamaz!");
                    baglanti.Close();
                    return;
                }
                else
                {
                    baglanti.Close();
                    NpgsqlCommand komut = new NpgsqlCommand(sorgu1, baglanti);
                    komut.Parameters.AddWithValue("@ad", textBox1.Text);
                    komut.Parameters.AddWithValue("@soyad", textBox2.Text);
                    komut.Parameters.AddWithValue("@kullaniciad", textBox3.Text);
                    komut.Parameters.AddWithValue("@sifre", textBox4.Text);
                    komut.Parameters.AddWithValue("@adres", textBox7.Text);
                    komut.Parameters.AddWithValue("@sehirid", comboBox1.SelectedValue);
                    komut.Parameters.AddWithValue("@ilceid", comboBox2.SelectedValue);
                    komut.Parameters.AddWithValue("@seviyeid",0);
                    komut.Parameters.AddWithValue("@yetki_id", 0);
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Kaydınız başarıyla alınmıştır. Aramıza Hoşgeldin " + textBox1.Text,"Kayıt Başarılı !!");
                    uyeGiris uye = new uyeGiris();
                    uye.Show();
                    this.Hide();
                }
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Şartlarımızı Kabul Etmediniz !!", "Alacağınız Olsun !");
                baglanti.Close();
            }
        }

        private void kayit_ol_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select *from sehirler", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DisplayMember = "sehirad";
            comboBox1.ValueMember = "id";
            comboBox1.DataSource = dt;
            baglanti.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Close();
            baglanti.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM ilceler where il_id=@p1", baglanti);
            cmd.Parameters.AddWithValue("@p1",comboBox1.SelectedValue);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox2.DisplayMember = "ilcead";
            comboBox2.ValueMember = "id";
            comboBox2.DataSource = dt;
            baglanti.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
        }

        private void kayit_ol_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
