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
    public partial class yemekesgForAdm : Form
    {
        public yemekesgForAdm()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
        "Database=db_vtysproje; user ID=postgres; password=*****");
        void menuGetir()
        {
            baglanti.Open();
            string sorgu = "SELECT yemek_list.id AS \"YEMEK ID\",yemek_list.yemekad \"Yemek ADI\",yemek_kategoriad.adi\"Yemek Kategorisi\",yemek_list.tutar \"Yemek Tutarı\" FROM yemek_list INNER JOIN yemek_kategoriad ON yemek_list.yemek_kat = yemek_kategoriad.id";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        void tummenuGetir()
        {
            baglanti.Open();
            string sorgu = "SELECT yemek_list.id AS \"YEMEK ID\",yemek_list.yemekad \"Yemek ADI\",yemek_kategoriad.adi\"Yemek Kategorisi\",yemek_list.tutar \"Yemek Tutarı\" FROM yemek_list INNER JOIN yemek_kategoriad ON yemek_list.yemek_kat = yemek_kategoriad.id";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        void yemekmenuGetir()
        {
            baglanti.Open();
            string sorgu = "SELECT yemek_list.id AS \"YEMEK ID\",yemek_list.yemekad \"Yemek ADI\",yemek_kategoriad.adi\"Yemek Kategorisi\",yemek_list.tutar \"Yemek Tutarı\" FROM yemek_list INNER JOIN yemek_kategoriad ON yemek_list.yemek_kat = yemek_kategoriad.id where yemek_list.yemek_kat=1";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        void tatlimenuGetir()
        {
            baglanti.Open();
            string sorgu = "SELECT yemek_list.id AS \"YEMEK ID\",yemek_list.yemekad \"Yemek ADI\",yemek_kategoriad.adi\"Yemek Kategorisi\",yemek_list.tutar \"Yemek Tutarı\" FROM yemek_list INNER JOIN yemek_kategoriad ON yemek_list.yemek_kat = yemek_kategoriad.id where yemek_list.yemek_kat =2";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        void icecekmenuGetir()
        {
            baglanti.Open();
            string sorgu = "SELECT yemek_list.id AS \"YEMEK ID\",yemek_list.yemekad \"Yemek ADI\",yemek_kategoriad.adi\"Yemek Kategorisi\",yemek_list.tutar \"Yemek Tutarı\" FROM yemek_list INNER JOIN yemek_kategoriad ON yemek_list.yemek_kat = yemek_kategoriad.id where yemek_list.yemek_kat =3";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        private void yemekesgForAdm_Load(object sender, EventArgs e)
        {
            menuGetir();
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select *from yemek_kategoriad", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox3.DisplayMember = "adi";
            comboBox3.ValueMember = "id";
            comboBox3.DataSource = dt;
            baglanti.Close();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
       (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void updatebtn_Click(object sender, EventArgs e)
        {
            if(textBox7.Text == "")
            {
                MessageBox.Show("Üye id yok?", "Üye id Bulunamadı");
                return;
            }
            dynamic mboxResult = MessageBox.Show(textBox1.Text + " Adlı Yemek Güncellenecek Onaylıyor musunuz?", "Yemek Güncellenecek?", MessageBoxButtons.YesNo);
            if (mboxResult == DialogResult.Yes)
            {
                if (textBox1.Text == "" || textBox3.Text == "")
                {
                    MessageBox.Show("Zorunlu alanlardan biri girilmedi!", "Boş Değer Hatası", MessageBoxButtons.OK);
                    return;
                }
                string sorgu = "update yemek_list set yemekad=@p1,yemek_kat=@p2,tutar=@p3 where id=" + textBox7.Text;
                NpgsqlCommand komut = new NpgsqlCommand(sorgu,baglanti);
                komut.Parameters.AddWithValue("@p1", textBox1.Text);
                komut.Parameters.AddWithValue("@p2", Convert.ToInt32(comboBox3.SelectedValue));
                komut.Parameters.AddWithValue("@p3", Convert.ToDouble(textBox3.Text));
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show(textBox1.Text + " Adlı Yemek İçin Güncelleme Başarılı", "Güncellendi!");
                menuGetir();
            }
            else
            {
                MessageBox.Show("Güncellenmedi", "Güncellenmedi");
            }
               
        }

        private void button4_Click(object sender, EventArgs e)
        {
            updatebtn.Visible = true;
            eklebtn.Visible = false;
            textBox7.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value); //id
            textBox1.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[1].Value); // ad
            textBox3.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[3].Value); // tutar
            string sorgu = "select*from yemek_list where id =" + textBox7.Text;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            baglanti.Open();
            NpgsqlDataReader dr;
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                comboBox3.SelectedValue = Convert.ToInt32(dr.GetValue(2));
            }
            baglanti.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            updatebtn.Visible = false;
            eklebtn.Visible = true;
            textBox1.Clear(); textBox3.Clear(); textBox7.Clear();
        }

        private void eklebtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Zorunlu alanlardan biri girilmedi!", "Boş Değer Hatası", MessageBoxButtons.OK);
                return;
            }
            string sorgu = "Call yemek_ekle(@yemekad,@yemek_kat,@tutar)";
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@yemekad", textBox1.Text);
            komut.Parameters.AddWithValue("@yemek_kat", Convert.ToInt32(comboBox3.SelectedValue));
            komut.Parameters.AddWithValue("@tutar", Convert.ToDouble(textBox3.Text));
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show(textBox1.Text + " Adlı Yemek Başarıyla Eklenmiştir!", "Ekleme Başarılı!");
            menuGetir();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            updatebtn.Visible = true;
            eklebtn.Visible = false;
            tummenuGetir();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updatebtn.Visible = true;
            eklebtn.Visible = false;
            yemekmenuGetir();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updatebtn.Visible = true;
            eklebtn.Visible = false;
            tatlimenuGetir();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updatebtn.Visible = true;
            eklebtn.Visible = false;
            icecekmenuGetir();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string yemekid = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                dynamic mboxResult = MessageBox.Show(yemekid + " İD\'li Yemek Silinecektir Onaylıyor musunuz?", "Yemek Silinecek?", MessageBoxButtons.YesNo);
                if (mboxResult == DialogResult.Yes)
                {
                    string sorgu = "DELETE FROM yemek_list where id =" +yemekid;
                    NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show(dataGridView1.CurrentRow.Cells[1].Value.ToString() + " Adlı Yemek Silinmiştir.", "Yemek Başarıyla Silindi!");
                    menuGetir();
                }
                else
                {
                    MessageBox.Show("Yemek Silinmedi!", "Silinmedi");
                }
            }
            else
            {
                MessageBox.Show("Yemek Yok Hatası!");
            }
            
        }
    }
}
