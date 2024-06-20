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
    public partial class yorumGuncelle : Form
    {
        public yorumGuncelle()
        {
            InitializeComponent();
        }
        
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
         "Database=db_vtysproje; user ID=postgres; password=*****");

        void CevaplananYorumlar()
        {
            baglanti.Open();
            string sorgu = "SELECT yorumlar.id AS \"Yorum ID\",yorumlar.yorum \"Yorum\", yorum_durumlari.durum AS \"Yorum Durumu\" FROM yorumlar INNER JOIN yorum_durumlari ON yorumlar.yorum_durum = yorum_durumlari.id where yorumlar.yorum_durum = 2";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            button1.Visible = true;
            button2.Visible = true;
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
                    label6.Text = id.ToString();
                    kullanici_id = Convert.ToInt32(dr.GetValue(7));
                }
                baglanti.Close();
                sorgu = "select ad from uye_list where id=" + Convert.ToString(kullanici_id);
                baglanti.Open();
                komut = new NpgsqlCommand(sorgu, baglanti);
                dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    label4.Text = dr.GetString(0);
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
            }
            else
            {
                MessageBox.Show("Yorum Yok ki Getireyim!", "Yorum Yok Hatasi!");
            }
        }

        private void yorumGuncelle_Load(object sender, EventArgs e)
        {
            CevaplananYorumlar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox4.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Boş Bırakılan Yer Hatası !!", "Textboxları Boş Bırakma");
            }
            else
            {
                string sorgu = "Call yorumu_guncelle(" + label6.Text + "," + "\'" + textBox3.Text + "\'," + "\'" + textBox4.Text + "\')";
                NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Güncelleme Başarılı Olarak Yapıldı!", "Güncelleme Başarılı");
                CevaplananYorumlar();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sorgu = "update yorumlar set cevap = NULL,yorum_durum= 1 where id ="+label6.Text;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Cevabınız Başarıyla Silinmiştir!", "Silinme Başarılı Başarılı");
            CevaplananYorumlar();
            textBox3.Clear();
            textBox4.Clear();
            label6.Text = "";
        }
    }
}
