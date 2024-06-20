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
    public partial class memberSiparislerim : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=db_vtysproje; user ID=postgres; password=*****");
        void siparisGetir()
        {
            baglanti.Open();
            string sorgu = "SELECT siparis_list.id AS \"Sipariş ID\",siparis_list.toplam AS \"Tutar\", siparis_tamamlandimi.durum AS \"Tamamlanma Durumu\" FROM siparis_list INNER JOIN siparis_tamamlandimi ON siparis_list.tamamlanma = siparis_tamamlandimi.id where siparis_list.kullanicis ="+label2.Text;
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        public memberSiparislerim()
        {           
            InitializeComponent();
        }

        private void memberSiparislerim_Load(object sender, EventArgs e)
        {
            siparisGetir();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Visible = false;
            if(dataGridView1.SelectedRows.Count > 0) {

                textBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                listBox1.Items.Clear();
                string secilenid = dataGridView1.CurrentRow.Cells[0].Value.ToString();
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
                komut.Parameters.AddWithValue("@p1", Convert.ToInt32(label2.Text));
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    textBox4.Text = dr.GetString(0);
                }
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Gösterilecek Sipariş Bilgileri Bulunamadı!", "Sipariş Yok!");
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string secilenindex = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            baglanti.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("select tamamlanma from siparis_list where id=@p1", baglanti);
            cmd.Parameters.AddWithValue("@p1", Convert.ToInt32(secilenindex));
            NpgsqlDataReader reader;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int bul = Convert.ToInt32(reader.GetValue(0));
                if(bul == 0)
                {
                    MessageBox.Show("Sipariş Tamamlanmadığı İçin Yorum Yapamazsınız!", "Sipariş Tamamlanmadı!");
                }
                else
                {
                    baglanti.Close();
                    baglanti.Open();
                    string sorgu = "select from yorumlar where siparis_id=" + Convert.ToString(secilenindex);
                    NpgsqlCommand komut = new NpgsqlCommand(sorgu,baglanti);
                    reader = komut.ExecuteReader();
                    if (reader.Read())
                    {
                        MessageBox.Show("Bu Siparişe Zaten Yorum Yapmışsınız!", "Yorum Yapılmış!");
                    }
                    else
                    {
                        button2.Visible = true;
                    }
                    baglanti.Close();
                }
            }
            baglanti.Close();

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            radioButton4.Select();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            radioButton3.Select();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            radioButton2.Select();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            radioButton1.Select();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow.Cells[2].Value.ToString() == "Tamamlanmadı")
            {
                MessageBox.Show("Yorum Yapmaya Çalıştığınız Sipariş Tamamlanmamış", "Programı Bozamazsın :)");
                return;
            }
            if(textBox3.Text == "")
            {
                MessageBox.Show("Yorum Yazmadınız!", "Yorum Yazılmadı");
                return;
            }

            string siparisid = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string yorum = textBox3.Text;
            int yildiz = 1;
            int siparisdurum = 1;
            if (radioButton1.Checked) yildiz = 1;
            if (radioButton2.Checked) yildiz = 2;
            if (radioButton3.Checked) yildiz = 3;
            if (radioButton4.Checked) yildiz = 4;
            if (dataGridView1.CurrentRow.Cells[2].Value.ToString() == "Tamamlandı") siparisdurum = 1;
            if (dataGridView1.CurrentRow.Cells[2].Value.ToString() == "İade") siparisdurum = 2;

            string sorgu = "INSERT INTO yorumlar(yorum,yildiz,siparis_id,siparis_durum,kullanici_id) VALUES(@yorum,@yildiz,@siparis_id,@siparis_durum,@kullanici_id)";
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@yorum", textBox3.Text);
            komut.Parameters.AddWithValue("@yildiz", yildiz);
            komut.Parameters.AddWithValue("@siparis_id", Convert.ToInt32(siparisid));
            komut.Parameters.AddWithValue("@siparis_durum", siparisdurum);
            komut.Parameters.AddWithValue("@kullanici_id", Convert.ToInt32(label2.Text));
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            if(yildiz == 1||yildiz ==2) {
                MessageBox.Show("Yaşadığınız Olumsuzluklar İçin Çok Üzgünüz :'(","Olmasaydı Sonumuz böyle");
            }
            else
            {
                MessageBox.Show("Yorumunuz Gönderilmiştir Teşekkürler!!", "Yorumunuzu Dikkate Alacağız!");
            }
            button2.Visible = false;
        }
    }
}
