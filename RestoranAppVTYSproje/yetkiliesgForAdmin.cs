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
    public partial class yetkiliesgForAdmin : Form
    {
        public yetkiliesgForAdmin()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
         "Database=db_vtysproje; user ID=postgres; password=*****");
        void uyeleriGetir()
        {
            baglanti.Open();
            string sorgu = "SELECT calisan_list.id AS \"CALİSAN ID\",calisan_list.ad \"ADI\",calisan_list.soyad\"SOYADI\",calisan_list.kullaniciad \"Kullanıcı ADI\", calisan_yetki_adlari.yetki_adi AS \"Yetki ADI\" FROM calisan_list INNER JOIN calisan_yetki_adlari ON calisan_list.yetki_id = calisan_yetki_adlari.yetki_id";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        private void yetkiliesgForAdmin_Load(object sender, EventArgs e)
        {
            uyeleriGetir();
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select *from calisan_yetki_adlari", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox3.DisplayMember = "yetki_adi";
            comboBox3.ValueMember = "yetki_id";
            comboBox3.DataSource = dt;
            baglanti.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updatebtn.Visible = true;
            eklebtn.Visible = false;
            textBox7.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value); //id
            textBox1.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[1].Value); // ad
            textBox2.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[2].Value); // soyad
            textBox3.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[3].Value); // k.adi
            string sorgu = "select*from calisan_list where id =" + textBox7.Text;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            baglanti.Open();
            NpgsqlDataReader dr;
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                textBox4.Text = dr.GetString(6); textBox8.Text = dr.GetString(6);
                comboBox3.SelectedValue = Convert.ToInt32(dr.GetValue(9));
            }
            baglanti.Close();
        }

        private void updatebtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Zorunlu alanlardan biri girilmedi!", "Boş Değer Hatası", MessageBoxButtons.OK);
                return;
            }
            if (textBox4.Text != textBox8.Text)
            {
                MessageBox.Show("Şifreleri Aynı Girmediniz!", "Şifreler Aynı Değil!");
                return;
            }

            string sorgu = "UPDATE calisan_list set ad=@ad,soyad=@soyad,kullaniciad=@kullaniciad,sifre=@sifre,yetki_id=@yetki_id WHERE id =" + textBox7.Text;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@ad", textBox1.Text);
            komut.Parameters.AddWithValue("@kullaniciad", textBox3.Text);
            komut.Parameters.AddWithValue("@soyad", textBox2.Text);
            komut.Parameters.AddWithValue("@sifre", textBox4.Text);
            komut.Parameters.AddWithValue("@yetki_id", Convert.ToInt32(comboBox3.SelectedValue));
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kaydınız Başarıyla Güncellenmiştir " + textBox1.Text + " " + textBox2.Text, "Güncelleme Başarılı");
            uyeleriGetir();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updatebtn.Visible = true;
            eklebtn.Visible = false;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string ismi = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                dynamic mboxResult = MessageBox.Show(ismi + " Kullanıcı Adlı Üye Silinecek Onaylıyor musunuz?", "Üye Silinecek?", MessageBoxButtons.YesNo);
                if (mboxResult == DialogResult.Yes)
                {
                    string sorgu = "delete from calisan_list where id =" + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show(ismi + " Kullanıcı Adlı Üye Silinmiştir.", "Üye Başarıyla Silindi!");
                    uyeleriGetir();
                }
                else
                {
                    MessageBox.Show(ismi + " Kullanıcı Adlı Üye Silinmedi!", "Üyeyi Silmedik!");
                }
            }
            else
                MessageBox.Show("Silinecek Üye Yok!", "Üye Yok Hatası!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updatebtn.Visible = false;
            eklebtn.Visible = true;
            textBox1.Clear(); textBox2.Clear(); textBox3.Clear(); textBox4.Clear(); textBox8.Clear(); textBox7.Clear();
        }

        private void eklebtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Zorunlu alanlardan biri girilmedi!", "Boş Değer Hatası", MessageBoxButtons.OK);
                return;
            }
            if (textBox4.Text != textBox8.Text)
            {
                MessageBox.Show("Şifreleri Aynı Girmediniz!", "Şifreler Aynı Değil!");
                return;
            }
            string sorgu = "select count(*) from calisan_list where kullaniciad = @p1";
            int countu = 0;
            baglanti.Open();
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@p1", textBox3.Text);
            NpgsqlDataReader dr;
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                countu = Convert.ToInt32(dr.GetValue(0));
            }
            baglanti.Close();
            if (countu == 1)
            {
                MessageBox.Show("Bu Kullanıcı Adı Başkası Tarafından Kullanılıyor!", "Kullanıcı Adı hatası!");
                return;
            }
            sorgu = "Call calisan_ekle(@ad,@soyad,@kullaniciad,@sifre,@yetki_id)";
            komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@ad", textBox1.Text);
            komut.Parameters.AddWithValue("@kullaniciad", textBox3.Text);
            komut.Parameters.AddWithValue("@soyad", textBox2.Text);
            komut.Parameters.AddWithValue("@sifre", textBox4.Text);
            komut.Parameters.AddWithValue("@yetki_id", Convert.ToInt32(comboBox3.SelectedValue));
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kaydınız Başarıyla Alınmıştır! " + textBox1.Text + " " + textBox2.Text, "Ekleme Başarılı!");
            uyeleriGetir();
        }
    }
}
