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
    public partial class memberInformation : Form
    {
        public memberInformation()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=db_vtysproje; user ID=postgres; password=*****");
        private void memberInformation_Load(object sender, EventArgs e)
        {
            //illeri yerleştiriyorum
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select *from sehirler", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DisplayMember = "sehirad";
            comboBox1.ValueMember = "id";
            comboBox1.DataSource = dt;
            baglanti.Close();
            string sorgu = "select*from uye_list where id=@p1";
            int sehirid = 1;
            int ilceid = 1;
            baglanti.Open();
            NpgsqlDataReader dr;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@p1", Convert.ToInt32(label10.Text));
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                sehirid = Convert.ToInt32(dr.GetValue(3));
                ilceid = Convert.ToInt32(dr.GetValue(4));
                textBox1.Text = Convert.ToString(dr.GetString(1));
                textBox2.Text = Convert.ToString(dr.GetString(2));
                textBox3.Text = Convert.ToString(dr.GetString(5));
                textBox4.Text = Convert.ToString(dr.GetString(6));
                textBox8.Text = Convert.ToString(dr.GetString(6));
                textBox5.Text = Convert.ToString(dr.GetValue(8));
                textBox6.Text = Convert.ToString(dr.GetString(7));
            }
            comboBox1.SelectedValue = sehirid;
            baglanti.Close();
            //ilçe yerleştiriyorum
            baglanti.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM ilceler where il_id=@p1", baglanti);
            cmd.Parameters.AddWithValue("@p1", Convert.ToInt32(sehirid));
            da = new NpgsqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt1 = new DataTable();
            da.Fill(dt1);
            comboBox2.DisplayMember = "ilcead";
            comboBox2.ValueMember = "id";
            comboBox2.DataSource = dt1;
            baglanti.Close();
            comboBox2.SelectedValue = ilceid;

            //Seviye adını almak için
            baglanti.Open();
            sorgu = "select seviyead from seviyeadlari where seviyeid =@p1";
            komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@p1", Convert.ToInt32(textBox5.Text));
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                textBox7.Text = dr.GetString(0);
            }
            baglanti.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Close();
            baglanti.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM ilceler where il_id=@p1", baglanti);
            cmd.Parameters.AddWithValue("@p1", comboBox1.SelectedValue);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox2.DisplayMember = "ilcead";
            comboBox2.ValueMember = "id";
            comboBox2.DataSource = dt;
            baglanti.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Zorunlu alanlardan biri girilmedi!", "Boş Değer Hatası", MessageBoxButtons.OK);
                return;
            }
            if(textBox4.Text != textBox8.Text)
            {
                MessageBox.Show("Şifreleri Aynı Girmediniz!", "Şifreler Aynı Değil!");
                return;
            }
            //MessageBox.Show("Buraya geliyor mu?");
            string sorgu = "UPDATE uye_list set ad=@ad,soyad=@soyad,sehirid=@sehirid,ilceid=@ilceid,sifre=@sifre,adres=@adres WHERE id =" + label10.Text;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@ad", textBox1.Text);
            komut.Parameters.AddWithValue("@soyad", textBox2.Text);
            komut.Parameters.AddWithValue("@sifre", textBox4.Text);
            komut.Parameters.AddWithValue("@adres", textBox6.Text);
            komut.Parameters.AddWithValue("@sehirid", comboBox1.SelectedValue);
            komut.Parameters.AddWithValue("@ilceid", comboBox2.SelectedValue);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kaydınız Başarıyla Güncellenmiştir " + textBox1.Text +" "+textBox2.Text, "Güncelleme Başarılı");
        }
    }
}
