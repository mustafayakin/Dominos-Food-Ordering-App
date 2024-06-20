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
    public partial class YorumislemleriForadmin : Form
    {
        public YorumislemleriForadmin()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
         "Database=db_vtysproje; user ID=postgres; password=*****");
        void tumyorum()
        {
            baglanti.Open();
            string sorgu = "SELECT yorumlar.id AS \"Yorum ID\",yorumlar.yorum \"Yorum\", yorum_durumlari.durum AS \"Yorum Durumu\" FROM yorumlar INNER JOIN yorum_durumlari ON yorumlar.yorum_durum = yorum_durumlari.id";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        private void YorumislemleriForadmin_Load(object sender, EventArgs e)
        {
            tumyorum(); 
        }

        private void button1_Click(object sender, EventArgs e) // Cevap Yazma!
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                string yorumdurumu = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                if (yorumdurumu != "Cevaplandı")
                {
                    label4.Visible = true;
                    label8.Visible = true;
                    label9.Visible = true;
                    textBox2.Visible = true;
                    button2.Visible = true;
                    label9.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value);

                }
                else
                {
                    label4.Visible = false;
                    label8.Visible = false;
                    label9.Visible = false;
                    textBox2.Visible = false;
                    button2.Visible = false;
                    MessageBox.Show("Yorum Cevaplandığı İçin Yorum Yazamasınız!", "Yorum Cevaplanmış");
                }
            }
            else
            {
                MessageBox.Show("Yorum Yok ki Cevaplayasın!", "Yorum Yok Hatasi!");
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            label4.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox4.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                int kullanici_id = 0;
                string sorgu = "select*from yorumlar where id=" + id.ToString();
                baglanti.Open();
                NpgsqlDataReader dr;
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    label6.Text = Convert.ToString(dr.GetValue(2));
                    kullanici_id = Convert.ToInt32(dr.GetValue(7));

                }
                baglanti.Close();
                sorgu = "select cevap from yorumlar where id=" + id.ToString();
                baglanti.Open();
                komut = new NpgsqlCommand(sorgu, baglanti);
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    if (dr.IsDBNull(0))
                    {
                        textBox3.Text = "Cevaplanmamış !";
                    }
                    else
                    {
                        textBox3.Text = dr.GetString(0);

                    }
                }
                baglanti.Close();
                sorgu = "select ad from uye_list where id=" + Convert.ToString(kullanici_id);
                baglanti.Open();
                komut = new NpgsqlCommand(sorgu, baglanti);
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    textBox1.Text = dr.GetString(0);
                }
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Yorum Yok ki Getireyim!", "Yorum Yok Hatasi!");
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            label4.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if(dataGridView1.CurrentRow.Cells[2].Value.ToString() == "Cevaplandı")
                {
                    MessageBox.Show("Yorum Cevaplandığı İçin Okunmadı Olarak İşaretleyemezsin!", "Yorum Cevaplanmış");
                    return;
                }
                string sorgu = "update yorumlar set yorum_durum = 0 where id =" + Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value);
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Okunmadı Olarak Güncellendi!" , "Güncelleme Başarılı");
                tumyorum();
            }
            else
            {
                MessageBox.Show("Yorum Yok ki İşaretleyesin!", "Yorum Yok Hatasi!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label4.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.CurrentRow.Cells[2].Value.ToString() == "Cevaplandı")
                {
                    MessageBox.Show("Yorum Cevaplandığı İçin Okunmadı Olarak İşaretleyemezsin!", "Yorum Cevaplanmış");
                    return;
                }
                string sorgu = "Call yorumu_okundu_yap(" + Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value)+");";
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Okundu Olarak Güncellendi!", "Güncelleme Başarılı");
                tumyorum();
            }
            else
            {
                MessageBox.Show("Yorum Yok ki İşaretleyesin!", "Yorum Yok Hatasi!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox2.Text == "")
            {
                MessageBox.Show("Yorum Yazılmadı ki ? :/'(", "Yorum Yazılmadı Hatası!");
            }
            else //Gönderme İşlemleri Yapılacak!
            {
                string sorgu = "Call yorumu_cevapla("+label9.Text+","+"\'"+textBox2.Text+"\');";
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Yorum Cevaplandı !", "Yorum Cevaplandı!");
                label4.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                textBox2.Visible = false;
                button2.Visible = false;
                tumyorum();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label4.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string sorgu = "Call yorumu_sil(" + Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value) + ");";
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Yorumu Başarıyla Sildim!", "Yorum Silindi!");
                tumyorum();
            }
            else
            {
                MessageBox.Show("Yorum Yok ki Silesin!", "Yorum Yok Hatasi!");
            }
    
        }
    }
}
