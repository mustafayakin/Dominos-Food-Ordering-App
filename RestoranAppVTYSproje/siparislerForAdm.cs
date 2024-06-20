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
    public partial class siparislerForAdm : Form
    {
        public siparislerForAdm()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=db_vtysproje; user ID=postgres; password=*****");
        void tumsiparisGetir()
        {
            baglanti.Open();
            string sorgu = "SELECT siparis_list.kullanicis AS \"Kullancı ID:\",siparis_list.id AS \"Sipariş ID\",siparis_list.toplam AS \"Tutar\", siparis_tamamlandimi.durum AS \"Tamamlanma Durumu\" FROM siparis_list INNER JOIN siparis_tamamlandimi ON siparis_list.tamamlanma = siparis_tamamlandimi.id";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        void tamamlanmayansiparisGetir()
        {
            baglanti.Open();
            string sorgu = "SELECT siparis_list.kullanicis AS \"Kullancı ID:\",siparis_list.id AS \"Sipariş ID\",siparis_list.toplam AS \"Tutar\", siparis_tamamlandimi.durum AS \"Tamamlanma Durumu\" FROM siparis_list INNER JOIN siparis_tamamlandimi ON siparis_list.tamamlanma = siparis_tamamlandimi.id where siparis_list.tamamlanma = 0";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        void tamamlanansiparisGetir()
        {
            baglanti.Open();
            string sorgu = "SELECT siparis_list.kullanicis AS \"Kullancı ID:\",siparis_list.id AS \"Sipariş ID\",siparis_list.toplam AS \"Tutar\", siparis_tamamlandimi.durum AS \"Tamamlanma Durumu\" FROM siparis_list INNER JOIN siparis_tamamlandimi ON siparis_list.tamamlanma = siparis_tamamlandimi.id where siparis_list.tamamlanma = 1";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        void iadesiparisGetir()
        {
            baglanti.Open();
            string sorgu = "SELECT siparis_list.kullanicis AS \"Kullancı ID:\",siparis_list.id AS \"Sipariş ID\",siparis_list.toplam AS \"Tutar\", siparis_tamamlandimi.durum AS \"Tamamlanma Durumu\" FROM siparis_list INNER JOIN siparis_tamamlandimi ON siparis_list.tamamlanma = siparis_tamamlandimi.id where siparis_list.tamamlanma = 2";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        private void siparislerForAdm_Load(object sender, EventArgs e)
        {
            tumsiparisGetir();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox2.Text = Math.Round(Convert.ToDouble(dataGridView1.CurrentRow.Cells[2].Value.ToString()),2).ToString();
                listBox1.Items.Clear();
                string secilenid = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                string sorgu = "select cardinality(secilenyemek) from siparis_list where id=@p1";
                NpgsqlDataReader dr;
                baglanti.Open();
                int menuboyut = 0;
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@p1", Convert.ToInt32(secilenid));
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    menuboyut = Convert.ToInt32(dr.GetValue(0));
                }
                baglanti.Close();
                baglanti.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("select secilenyemek from siparis_list where id=@p1", baglanti);
                cmd.Parameters.AddWithValue("@p1", Convert.ToInt32(secilenid));
                NpgsqlDataReader reader = cmd.ExecuteReader();
                int[] menum = new int[menuboyut];
                while (reader.Read())
                {
                    menum = (int[])reader.GetValue(0);
                }
                baglanti.Close();
                string sorgu1 = "select yemekad from yemek_list where id=@p1";
                for (int i = 0; i < menuboyut; i++)
                {
                    baglanti.Open();
                    NpgsqlCommand komut1 = new NpgsqlCommand(sorgu1, baglanti);
                    komut1.Parameters.AddWithValue("@p1", menum[i]);
                    dr = komut1.ExecuteReader();
                    if (dr.Read())
                    {
                        listBox1.Items.Add(dr.GetString(0));
                    }
                    baglanti.Close();
                }
                baglanti.Open();
                sorgu = "select adres from uye_list where id=@p1";
                komut = new NpgsqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@p1", Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString()));
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    textBox4.Text = dr.GetString(0);
                }
                baglanti.Close();
                sorgu = "select *from uye_list where id =" + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                baglanti.Open();
                komut = new NpgsqlCommand(sorgu, baglanti);
                int seviyeidsi = 0,seviyediscount = 0;
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    textBox1.Text = dr.GetString(1);
                    seviyeidsi = Convert.ToInt32(dr.GetValue(8));
                }
                label8.Text = seviyeidsi.ToString();
                baglanti.Close();
                sorgu = "select seviyedisc from seviyediscount where seviyeid =" + seviyeidsi.ToString();
                baglanti.Open();
                komut = new NpgsqlCommand(sorgu, baglanti);
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    seviyediscount = Convert.ToInt32(dr.GetValue(0));
                }
                baglanti.Close();
                string gelendeger= textBox2.Text.Replace(',', '.');
                textBox2.Text = gelendeger;
                // FUNCTİON KULLANDIĞIM YER ------->>>>>
                sorgu = "select indirimsizfiyat(" + gelendeger + "," + seviyediscount.ToString() + ")";
                baglanti.Open();
                komut = new NpgsqlCommand(sorgu, baglanti);
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    textBox3.Text = Convert.ToDouble(dr.GetValue(0)).ToString();
                }
                double indirimsiz = Math.Round(Convert.ToDouble(textBox3.Text), 2);
                textBox3.Text = Convert.ToString(indirimsiz);
                baglanti.Close();
                sorgu = "select siparisialan from siparis_list where id =" + dataGridView1.CurrentRow.Cells[1].Value.ToString();
                komut = new NpgsqlCommand(sorgu,baglanti);
                baglanti.Open();
                int siparisialan = 0;
                bool varmi = false;
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    if(Convert.ToInt32(dr.GetValue(0)) != 0)
                    {
                        siparisialan = Convert.ToInt32(dr.GetValue(0));
                        varmi = true;
                    }
                }
                baglanti.Close();
                if (varmi)
                {
                    sorgu = "select kullaniciad from calisan_list where id=" + siparisialan.ToString();
                    komut = new NpgsqlCommand(sorgu, baglanti);
                    baglanti.Open();
                    dr = komut.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox5.Text = dr.GetString(0);
                    }
                    baglanti.Close();
                }
                else
                {
                    textBox5.Text = "NULL";
                }
                
            }
            else
            {
                MessageBox.Show("Tamamlanmayan Sipariş Yok!", "Sipariş Yok!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                dynamic mboxResult = MessageBox.Show("Sipariş Tamamlandı Olarak İşaretlenecek?", "Tamamlandı mı?", MessageBoxButtons.YesNo);
                if (mboxResult == DialogResult.Yes)
                {
                    string sorgu = "update siparis_list set tamamlanma = 1,siparisialan ="+kullanicid.Text+" where id=" + dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Sipariş Başarıyla Tamamlandı Olarak İşaretlendi!", "Sipariş Tamamlandı!!");
                    tamamlanmayansiparisGetir();
                }
                else
                {
                    MessageBox.Show("Siparişi Tamamlandı İşaretlemedim!", "Sipariş Tamamlanmadı!");
                }

            }
            else
            {
                MessageBox.Show("Tamamlanmayan Sipariş Yok!", "Sipariş Yok Hatası!");
            }
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tamamlanansiparisGetir();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            iadesiparisGetir();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tamamlanmayansiparisGetir();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                dynamic mboxResult = MessageBox.Show("Sipariş Tamamlanmadı Olarak İşaretlenecek?", "Tamamlanmadı mı?", MessageBoxButtons.YesNo);
                if (mboxResult == DialogResult.Yes)
                {
                    string sorgu = "update siparis_list set tamamlanma = 0,siparisialan ="+kullanicid.Text+" where id=" + dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Sipariş Başarıyla Tamamlanmadı Olarak İşaretlendi!", "Sipariş Tamamlanmadı!!");
                    tamamlanansiparisGetir();
                }
                else
                {
                    MessageBox.Show("Siparişi Tamamlanmadı İşaretlemedim!", "Sipariş Tamamlanmadı!");
                }

            }
            else
            {
                MessageBox.Show("Sipariş Yok!", "Sipariş Yok Hatası!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tumsiparisGetir();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                dynamic mboxResult = MessageBox.Show("Sipariş İADE Olarak İşaretlenecek?", "İade Edeyim mi?", MessageBoxButtons.YesNo);
                if (mboxResult == DialogResult.Yes)
                {
                    string sorgu = "update siparis_list set tamamlanma = 2,siparisialan =" + kullanicid.Text + " where id=" + dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Sipariş Başarıyla İADE Olarak İşaretlendi!", "Sipariş İADE!!");
                    iadesiparisGetir();
                }
                else
                {
                    MessageBox.Show("Siparişi İADE Olarak İşaretlemedim!", "Sipariş İADE Edilmedi!");
                }

            }
            else
            {
                MessageBox.Show("Sipariş Yok!", "Sipariş Yok Hatası!");
            }
        }
    }
}
