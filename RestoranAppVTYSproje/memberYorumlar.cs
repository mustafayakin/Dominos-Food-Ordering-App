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
    public partial class memberYorumlar : Form
    {
        public memberYorumlar()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=db_vtysproje; user ID=postgres; password=*****");

        void benimyorum()
        {
            baglanti.Open();
            string sorgu = "SELECT yorumlar.id AS \"Yorum ID\",yorumlar.yorum \"Yorum\", yorum_durumlari.durum AS \"Yorum Durumu\" FROM yorumlar INNER JOIN yorum_durumlari ON yorumlar.yorum_durum = yorum_durumlari.id where yorumlar.kullanici_id =" +label10.Text;
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView2.DataSource = tablo;
            baglanti.Close();
        }
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
        private void memberYorumlar_Load(object sender, EventArgs e)
        {
            benimyorum();
            tumyorum(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridView2.Rows.Count > 0)
            {
                textBox4.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                textBox2.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
                int id = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value);
                string sorgu = "select yildiz from yorumlar where id=" + id.ToString();
                baglanti.Open();
                NpgsqlDataReader dr;
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    label6.Text = Convert.ToString(dr.GetValue(0));
                }
                baglanti.Close();
                sorgu = "select kullaniciad from uye_list where id=" + label10.Text;
                komut = new NpgsqlCommand(sorgu, baglanti);
                baglanti.Open();
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    textBox3.Text = dr.GetString(0);
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
                        textBox1.Text = "Cevaplanmamış !";
                    }
                    else
                    {
                        textBox1.Text = dr.GetString(0);

                    }
                }
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Getirilecek Yorum Yok!", "Yorum Yok Hatası!");
            }
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count > 0)
            {
                textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                textBox4.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                string sorgu = "select yildiz from yorumlar where id=" + id.ToString();
                baglanti.Open();
                NpgsqlDataReader dr;
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    label6.Text = Convert.ToString(dr.GetValue(0));
                }
                baglanti.Close();
                baglanti.Open();
                sorgu = "select kullanici_id from yorumlar where id=" + id.ToString();
                int kullanicininki = 0;
                komut = new NpgsqlCommand(sorgu, baglanti);
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    kullanicininki = Convert.ToInt32(dr.GetValue(0));
                }
                baglanti.Close();
                sorgu = "select kullaniciad from uye_list where id=" + kullanicininki.ToString();
                komut = new NpgsqlCommand(sorgu, baglanti);
                baglanti.Open();
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    textBox3.Text = dr.GetString(0);
                }
                baglanti.Close();
                sorgu = "select cevap from yorumlar where id=" + id.ToString();
                baglanti.Open();
                komut = new NpgsqlCommand(sorgu, baglanti);
                dr = komut.ExecuteReader();
                
                if (dr.Read())
                {
                    if(dr.IsDBNull(0))
                    {
                        textBox1.Text = "Cevaplanmamış !";
                    }
                    else
                    {
                        textBox1.Text = dr.GetString(0);

                    }
                }
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Getirilecek Yorum Yok!", "Yorum Yok Hatası!");
            }
        }
    }
}
