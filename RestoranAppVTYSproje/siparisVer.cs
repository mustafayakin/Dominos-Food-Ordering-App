using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;


namespace RestoranAppVTYSproje
{
    public partial class siparisVer : Form
    {
        public siparisVer()
        {
            InitializeComponent();
        }
        
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
           "Database=db_vtysproje; user ID=postgres; password=*****");

        void yemekGetir()
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT yemek_list.id AS \"Yemek ID\",yemek_list.yemekad AS \"Yemek Adı\",yemek_list.tutar AS \"Fiyatı\", yemek_kategoriad.adi AS \"Yemek Kategorisi\" FROM yemek_list INNER JOIN yemek_kategoriad ON yemek_list.yemek_kat = yemek_kategoriad.id", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT yemek_list.id AS \"Yemek ID\",yemek_list.yemekad AS \"Yemek Adı\",yemek_list.tutar AS \"Fiyatı\", yemek_kategoriad.adi AS \"Yemek Kategorisi\" FROM yemek_list INNER JOIN yemek_kategoriad ON yemek_list.yemek_kat = yemek_kategoriad.id where yemek_list.yemek_kat = 1", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void siparisVer_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            string sorgu = "select*from uye_list where id=@p1";
            NpgsqlDataReader dr;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@p1", Convert.ToInt32(label4.Text));
            baglanti.Open();
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                label6.Text = Convert.ToString(dr.GetValue(8)); 
            }
            baglanti.Close();
            //seviye discount bulunacak ---
            //
            sorgu = "select*from seviyediscount where seviyeid=@p1";
            komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@p1", int.Parse(label6.Text));
            baglanti.Open();
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                label10.Text = Convert.ToString(dr.GetValue(1));
            }
            baglanti.Close();
            string sorgu1 = "select*from seviyeadlari where seviyeid=@p2";
            NpgsqlCommand komut1 = new NpgsqlCommand(sorgu1, baglanti);
            komut1.Parameters.AddWithValue("@p2", Convert.ToInt32(label6.Text));
            baglanti.Open();
            dr = komut1.ExecuteReader();
            if (dr.Read())
            {
                label8.Text = dr.GetString(1);
            }
            baglanti.Close();
            sorgu = "select adres from uye_list where id=" + label4.Text;
            baglanti.Open();
            komut = new NpgsqlCommand(sorgu, baglanti);
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                textBox6.Text = dr.GetString(0);
            }
            baglanti.Close();
            yemekGetir();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT yemek_list.id AS \"Yemek ID\",yemek_list.yemekad AS \"Yemek Adı\",yemek_list.tutar AS \"Fiyatı\", yemek_kategoriad.adi AS \"Yemek Kategorisi\" FROM yemek_list INNER JOIN yemek_kategoriad ON yemek_list.yemek_kat = yemek_kategoriad.id where yemek_list.yemek_kat = 2", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT yemek_list.id AS \"Yemek ID\",yemek_list.yemekad AS \"Yemek Adı\",yemek_list.tutar AS \"Fiyatı\", yemek_kategoriad.adi AS \"Yemek Kategorisi\" FROM yemek_list INNER JOIN yemek_kategoriad ON yemek_list.yemek_kat = yemek_kategoriad.id where yemek_list.yemek_kat = 3", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            yemekGetir();
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            label5.Visible = false;
            label3.Font = new Font("Microsoft Sans Serif",16, FontStyle.Bold);
            int secilenindex = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            string secileninindex = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string secileninadi = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            string secilenfiyat = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            dataGridView2.Rows.Add(secileninadi, secilenfiyat, secilenindex);
            double toplam = 0;
            for(int i =0; i< dataGridView2.RowCount; ++i)
            {
                toplam += Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value);
            }
            label3.Text = toplam.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label5.Visible = false;
            label3.Font = new Font("Microsoft Sans Serif", 16, FontStyle.Bold);
            double toplam = 0;
            if (dataGridView2.SelectedRows.Count > 0)
            {
                dataGridView2.Rows.RemoveAt(dataGridView2.SelectedRows[0].Index);
                for (int i = 0; i < dataGridView2.RowCount; ++i)
                {
                    toplam += Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value);
                }
                label3.Text = toplam.ToString();
            }
            else
            {
                MessageBox.Show("Silinecek Ürün Bulunamadı","Ürün yok ki :'(");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label5.Visible = true;
            double indirimorani;
            double sontoplam;
            string sorgu = "select*from seviyediscount where seviyeid=@p1";
            NpgsqlDataReader dr;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@p1", int.Parse(label6.Text));
            baglanti.Open();
            dr = komut.ExecuteReader();
            if (dr.Read())
            {

                label3.Font = new Font("Microsoft Sans Serif", 16,FontStyle.Strikeout);
                indirimorani = Convert.ToDouble(dr.GetValue(1));
                sontoplam = Convert.ToDouble(label3.Text) * (100 - indirimorani) / 100;
                label5.Text = Convert.ToString(sontoplam);
            }
            baglanti.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            int[] menu = new int[dataGridView2.RowCount];
            if(dataGridView2.RowCount > 0)
            {
                dynamic mboxResult = MessageBox.Show("Siparişinizi Onaylıyor Musunuz?","Sipariş Onay?",MessageBoxButtons.YesNo);
                if(mboxResult == DialogResult.Yes)
                {
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        menu[i] = Convert.ToInt32(dataGridView2.Rows[i].Cells[2].Value);
                    }
                    //for(int i = 0; i < dataGridView2.RowCount; i++) Görmek için yaptım
                    //{
                    //    MessageBox.Show(menu[i].ToString());
                    //}
                    //INSERT INTO siparisler(menus) VALUES('" + menu[i] + "')
                    //-----------------

                    StringBuilder sb = new StringBuilder();
                    string sorgu = "INSERT INTO siparis_list(secilenyemek,kullanicis,odemebicimi,toplam,tamamlanma) VALUES(ARRAY[";
                    sb.Append(sorgu);
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        if (i == dataGridView2.RowCount - 1)
                        {
                            sb.Append(menu[i] + "");
                            sb.Append("]");
                            sorgu = sb.ToString();
                            break;
                        }
                        sb.Append(menu[i] + ",");
                        sorgu = sb.ToString();
                    }
                    sb.Append(",@p1,@p2,@p3,@p4)");
                    sorgu = sb.ToString();
                    //------------------------
                    NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                    //komut.Parameters.AddWithValue("@p5", menu);
                    komut.Parameters.AddWithValue("@p1", int.Parse(label4.Text));
                    if (radioButton1.Checked)
                    {
                        komut.Parameters.AddWithValue("@p2", 1);
                    }
                    if (radioButton2.Checked)
                    {
                        komut.Parameters.AddWithValue("@p2", 2);
                    }
                    if (radioButton3.Checked)
                    {
                        komut.Parameters.AddWithValue("@p2", 3);
                    }
                    double sontutar = Convert.ToSingle(label3.Text) * (100 - Convert.ToSingle(label10.Text)) / 100;
                    komut.Parameters.AddWithValue("@p3", sontutar);
                    komut.Parameters.AddWithValue("@p4", 0);
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Siparişiniz oluşturuldu!\nÖdemeniz Gereken Tutar:" + Convert.ToString(sontutar) + " TL", "Siparişiniz Alındı!");
                }
                else
                {
                    MessageBox.Show("Siparişinizi Almadık.","Sipariş Alınmadı");
                }
                
            }
            else
            {
                MessageBox.Show("Sepetim Kısmında Bir şey yok", "Sepet Boş Hatası");
            }
        }
    }
}
