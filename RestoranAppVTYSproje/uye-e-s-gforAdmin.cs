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
    public partial class uye_e_s_gforAdmin : Form
    {
        public uye_e_s_gforAdmin()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
         "Database=db_vtysproje; user ID=postgres; password=*****");

        void uyeleriGetir()
        {
            baglanti.Open();
            string sorgu = "SELECT uye_list.id AS \"UYE ID\",uye_list.ad \"Uye ADI\",uye_list.soyad\"Uye SOYADI\",uye_list.kullaniciad \"Kullanıcı ADI\", uye_list.sifre AS \"Üye Şifresi\" FROM uye_list where uye_list.yetki_id = 0";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        private void uye_e_s_gforAdmin_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select *from sehirler", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DisplayMember = "sehirad";
            comboBox1.ValueMember = "id";
            comboBox1.DataSource = dt;
            baglanti.Close();
            comboBox3.SelectedIndex = 1;
            uyeleriGetir();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox7.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value);
            updatebtn.Visible = true;
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
            komut.Parameters.AddWithValue("@p1", Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
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
                comboBox3.Text = Convert.ToString(dr.GetValue(8));
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

        private void updatebtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Zorunlu alanlardan biri girilmedi!", "Boş Değer Hatası", MessageBoxButtons.OK);
                return;
            }
            if (textBox4.Text != textBox8.Text)
            {
                MessageBox.Show("Şifreleri Aynı Girmediniz!", "Şifreler Aynı Değil!");
                return;
            }
            //MessageBox.Show("Buraya geliyor mu?");
            //Kullanıcı adı başkası tarafından kullanılma durumunu denetleyen kod-->
            string sorgu = "select count(*) from uye_list where kullaniciad = @p1 and id != @p2";
            int countu = 0;
            baglanti.Open();
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@p1", textBox3.Text);
            komut.Parameters.AddWithValue("@p2", Convert.ToInt32(textBox7.Text));
            NpgsqlDataReader dr;
            
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                countu = Convert.ToInt32(dr.GetValue(0));
            }
            baglanti.Close();
            if(countu == 1)
            {
                MessageBox.Show("Bu Kullanıcı Adı Başkası Tarafından Kullanılıyor!", "Kullanıcı Adı hatası!");
                return;
            }

            //En Son Update Komutu 
            
            sorgu = "UPDATE uye_list set ad=@ad,soyad=@soyad,sehirid=@sehirid,ilceid=@ilceid,kullaniciad=@kullaniciad,sifre=@sifre,adres=@adres,seviyeid=@seviyeid WHERE id =" + textBox7.Text;
            komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@ad", textBox1.Text);
            komut.Parameters.AddWithValue("@kullaniciad", textBox3.Text);
            komut.Parameters.AddWithValue("@soyad", textBox2.Text);
            komut.Parameters.AddWithValue("@sifre", textBox4.Text);
            komut.Parameters.AddWithValue("@adres", textBox6.Text);
            komut.Parameters.AddWithValue("@sehirid", comboBox1.SelectedValue);
            komut.Parameters.AddWithValue("@ilceid", comboBox2.SelectedValue);
            komut.Parameters.AddWithValue("@seviyeid", Convert.ToInt32(comboBox3.SelectedItem));
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kaydınız Başarıyla Güncellenmiştir " + textBox1.Text + " " + textBox2.Text, "Güncelleme Başarılı");
            uyeleriGetir();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updatebtn.Visible = false;
            eklebtn.Visible = true;
            textBox1.Clear(); textBox2.Clear(); textBox3.Clear(); textBox4.Clear(); textBox6.Clear(); textBox8.Clear(); textBox7.Clear();
        }

        private void eklebtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Zorunlu alanlardan biri girilmedi!", "Boş Değer Hatası", MessageBoxButtons.OK);
                return;
            }
            if (textBox4.Text != textBox8.Text)
            {
                MessageBox.Show("Şifreleri Aynı Girmediniz!", "Şifreler Aynı Değil!");
                return;
            }
            string sorgu = "select count(*) from uye_list where kullaniciad = @p1 AND yetki_id=0";
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
            sorgu = "Call uye_ekle(@ad,@soyad,@sehirid,@ilceid,@kullaniciad,@sifre,@adres,@seviyeid)";
            komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@ad", textBox1.Text);
            komut.Parameters.AddWithValue("@kullaniciad", textBox3.Text);
            komut.Parameters.AddWithValue("@soyad", textBox2.Text);
            komut.Parameters.AddWithValue("@sifre", textBox4.Text);
            komut.Parameters.AddWithValue("@adres", textBox6.Text);
            komut.Parameters.AddWithValue("@sehirid", comboBox1.SelectedValue);
            komut.Parameters.AddWithValue("@ilceid", comboBox2.SelectedValue);
            komut.Parameters.AddWithValue("@seviyeid", Convert.ToInt32(comboBox3.SelectedItem));
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kaydınız Başarıyla Alınmıştır! " + textBox1.Text + " " + textBox2.Text, "Ekleme Başarılı!");
            uyeleriGetir();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string ismi = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                dynamic mboxResult = MessageBox.Show(ismi + " Kullanıcı Adlı Üye Silinecek Onaylıyor musunuz?", "Üye Silinecek?", MessageBoxButtons.YesNo);
                if (mboxResult == DialogResult.Yes)
                {
                    
                        string sorgu = "select count(*) from yorumlar where kullanici_id=" + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                        NpgsqlDataReader dr;
                        baglanti.Open();
                        dr = komut.ExecuteReader();
                        if (dr.Read())
                        {
                            if(Convert.ToInt32(dr.GetValue(0)) != 0)
                            {
                                baglanti.Close();
                                sorgu = "delete from yorumlar where kullanici_id=" + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                                komut = new NpgsqlCommand(sorgu, baglanti);
                                baglanti.Open();
                                komut.ExecuteNonQuery();
                                baglanti.Close();
                            }
                        baglanti.Close();
                        sorgu = "select count(*) from siparis_list where kullanicis=" + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        komut = new NpgsqlCommand(sorgu, baglanti);
                        baglanti.Open();
                        dr = komut.ExecuteReader();
                        if (dr.Read())
                        {
                            if (Convert.ToInt32(dr.GetValue(0)) != 0)
                            {
                                baglanti.Close();
                                sorgu = "delete from siparis_list where kullanicis=" + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                                komut = new NpgsqlCommand(sorgu, baglanti);
                                baglanti.Open();
                                komut.ExecuteNonQuery();
                                baglanti.Close();
                            }
                            baglanti.Close();
                        }
                        baglanti.Close();
                        sorgu = "delete from uye_list where id =" + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        komut = new NpgsqlCommand(sorgu, baglanti);
                        baglanti.Open();
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        MessageBox.Show(ismi + " Kullanıcı Adlı Üye Diğer Tüm Verileriyle Birlikte Silinmiştir!", "Üye Başarıyla Silindi!");
                        uyeleriGetir();

                    }
                    
                }
                else
                {
                    MessageBox.Show(ismi + " Kullanıcı Adlı Üye Silinmedi!", "Üyeyi Silmedik!");
                }
            }
            else
                MessageBox.Show("Silinecek Üye Yok!", "Üye Yok Hatası!");
        }
    }
}
