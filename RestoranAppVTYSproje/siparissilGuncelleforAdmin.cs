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
    public partial class siparissilGuncelleforAdmin : Form
    {
        public siparissilGuncelleforAdmin()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=db_vtysproje; user ID=postgres; password=*****");
        void aramabaslar()
        {
            label5.Visible = false; dataGridView3.Visible = false; button7.Visible = false;
            icerikbtn.Visible = true; kullaniciidbtn.Visible = true; siparisidbtn.Visible = true;
            icerikboxlbl.Visible = false; useridlbl.Visible = false;
            icerikbox.Visible = false; useridtxt.Visible = false; siparisidtxt.Visible = false;
            icerikarabtn.Visible = false; siparisidarabtn.Visible = false; useridarabtn.Visible = false;
        }
        void aramabiter()
        {
            label5.Visible = true;
            dataGridView3.Visible = true;
            button7.Visible = true;
            icerikbtn.Visible = false; kullaniciidbtn.Visible = false; siparisidbtn.Visible = false;
            icerikboxlbl.Visible = false; useridlbl.Visible = false;
            icerikbox.Visible = false; useridtxt.Visible = false; siparisidtxt.Visible = false;
            icerikarabtn.Visible = false; siparisidarabtn.Visible = false; useridarabtn.Visible = false;
        }
        void yemekGetir()
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT yemek_list.id AS \"Yemek ID\",yemek_list.yemekad AS \"Yemek Adı\",yemek_list.tutar AS \"Fiyatı\", yemek_kategoriad.adi AS \"Yemek Kategorisi\" FROM yemek_list INNER JOIN yemek_kategoriad ON yemek_list.yemek_kat = yemek_kategoriad.id", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView3.DataSource = tablo;
            baglanti.Close();
        }
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
        private void siparissilGuncelleforAdmin_Load(object sender, EventArgs e)
        {
            aramabiter();
            tumsiparisGetir();
            yemekGetir();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            aramabiter();
            tumsiparisGetir();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            aramabiter();
            tamamlanmayansiparisGetir();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            aramabiter();
            iadesiparisGetir();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            aramabiter();
            tamamlanansiparisGetir();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            aramabiter();
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            label8.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            label11.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
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
            sorgu = "select*from uye_list where id=" + label8.Text;
            int seviyeid = 0;
            komut = new NpgsqlCommand(sorgu, baglanti);
            baglanti.Open();
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                label3.Text = dr.GetString(1);
                seviyeid = Convert.ToInt32(dr.GetValue(8));
            }
            baglanti.Close();
            sorgu = "select seviyedisc from seviyediscount where seviyeid=" + seviyeid.ToString();
            komut = new NpgsqlCommand(sorgu, baglanti);
            baglanti.Open();
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                label6.Text = Convert.ToString(dr.GetValue(0));
            }
            baglanti.Close();
            baglanti.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("select secilenyemek from siparis_list where id=@p1", baglanti);
            cmd.Parameters.AddWithValue("@p1", Convert.ToInt32(secilenid));
            NpgsqlDataReader reader = cmd.ExecuteReader();
            int[] menum = new int[menuboyut];
            string[] yemekadim = new string[menuboyut];
            double[] yemekfiyat = new double[menuboyut];
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
                    yemekadim[i] = dr.GetString(0);
                }
                baglanti.Close();
            }
            sorgu1 = "select tutar from yemek_list where id=@p1";
            for (int i = 0; i < menuboyut; i++)
            {
                baglanti.Open();
                NpgsqlCommand komut1 = new NpgsqlCommand(sorgu1, baglanti);
                komut1.Parameters.AddWithValue("@p1", menum[i]);
                dr = komut1.ExecuteReader();
                if (dr.Read())
                {
                    yemekfiyat[i] = Convert.ToDouble(dr.GetValue(0));
                }
                baglanti.Close();
            }
            for(int i=0;i < menuboyut; i++)
            {
                dataGridView2.Rows.Add(yemekadim[i], yemekfiyat[i], menum[i]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            aramabiter();
            if (dataGridView2.SelectedRows.Count > 0)
            {
                dataGridView2.Rows.RemoveAt(dataGridView2.SelectedRows[0].Index);
            }
            else
            {
                MessageBox.Show("Silinecek Ürün Bulunamadı", "Ürün yok ki :'(");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            aramabiter();
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int secilenindex = int.Parse(dataGridView3.CurrentRow.Cells[0].Value.ToString());
                string secileninadi = dataGridView3.CurrentRow.Cells[1].Value.ToString();
                string secilenfiyat = dataGridView3.CurrentRow.Cells[2].Value.ToString();
                dataGridView2.Rows.Add(secileninadi, secilenfiyat, secilenindex);
            }
            else
            {
                MessageBox.Show("Önce Bilgileri Getirin!", "Seçilen Sipariş Yok!");

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            aramabiter();
            if (dataGridView2.SelectedRows.Count > 0)
            {
                double tutar = 0,sontutar=0;
                for(int i = 0; i < dataGridView2.RowCount; i++)
                {
                    tutar += Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value);
                }
                int disc = Convert.ToInt32(label6.Text);
                sontutar = (tutar * (100 - disc)) / 100;
                sontutar = Math.Round(sontutar, 3);
                string gelendeger = sontutar.ToString().Replace(',', '.');
                dynamic mboxResult = MessageBox.Show("Sipariş Toplamı ="+sontutar.ToString()+"TL Siparişi Güncelleyecek misiniz?", "Güncelleme Onayı?", MessageBoxButtons.YesNo);
                if (mboxResult == DialogResult.Yes)
                {
                    StringBuilder sb = new StringBuilder();
                    string sorgu = "UPDATE siparis_list set siparisialan=" +label13.Text+ ",toplam=" +gelendeger+ ",secilenyemek = ARRAY[";
                    sb.Append(sorgu);
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        if (i == dataGridView2.RowCount - 1)
                        {
                            sb.Append(dataGridView2.Rows[i].Cells[2].Value.ToString() + "");
                            sb.Append("]");
                            sorgu = sb.ToString();
                            break;
                        }
                        sb.Append(dataGridView2.Rows[i].Cells[2].Value.ToString() + ",");
                        sorgu = sb.ToString();
                    }
                    sb.Append("where id=" + label11.Text);
                    sorgu = sb.ToString();
                    NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Sipariş Başarıyla Güncellendi!", "Sipariş Güncellendi");
                    tumsiparisGetir();
                }
                else
                {
                    MessageBox.Show("Siparişi Güncellemedik", "Sipariş Güncellenmedi");
                }
                
            }
            else
            {
                MessageBox.Show("Siparişlerim Kısmı Boş Bırakılamaz!", "Sipariş Listesi Boş!");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            aramabiter();
            string secilensip = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            if(dataGridView1.SelectedRows.Count > 0)
            {
                dynamic mboxResult = MessageBox.Show(secilensip+" ID\'li Sipariş Silinecektir Onaylıyor musunuz?" ,"Silme Onayı?", MessageBoxButtons.YesNo);
                if (mboxResult == DialogResult.Yes)
                {

                    string sorgu = "select count(*) from yorumlar where siparis_id = @p1";
                    NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                    NpgsqlDataReader dr;
                    komut.Parameters.AddWithValue("@p1", Convert.ToInt32(secilensip));
                    baglanti.Open();
                    dr = komut.ExecuteReader();
                    if (dr.Read())
                    {
                        if(Convert.ToInt32(dr.GetValue(0)) != 0)
                        {
                            baglanti.Close();
                            sorgu = "delete from yorumlar where siparis_id=" + secilensip;
                            komut = new NpgsqlCommand(sorgu, baglanti);
                            baglanti.Open();
                            komut.ExecuteNonQuery();
                            baglanti.Close();
                            sorgu = "delete from siparis_list where id=" + secilensip;
                            komut = new NpgsqlCommand(sorgu, baglanti);
                            baglanti.Open();
                            komut.ExecuteNonQuery();
                            baglanti.Close();
                            MessageBox.Show("Sipariş Ve Sipariş Hakkında Yapılan Yorumlar Silindi", "Sipariş ve Yorumlar Silindi!");
                        }
                        else
                        {
                            baglanti.Close();
                            sorgu = "delete from siparis_list where id=" + secilensip;
                            komut = new NpgsqlCommand(sorgu, baglanti);
                            baglanti.Open();
                            komut.ExecuteNonQuery();
                            baglanti.Close();
                            MessageBox.Show("Sipariş Başarıyla Silindi", "Sipariş Silindi!!");
                        }
                    }
                    baglanti.Close();
                    tumsiparisGetir();
                }
                else
                {
                    MessageBox.Show("Sipariş Silinmedi", "Silinmedi");
                }
            }
            else
            {
                MessageBox.Show("Silinecek Sipariş Yok", "Sipariş Yok Hatası!!");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            aramabaslar();
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select *from yemek_list", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            icerikbox.DisplayMember = "yemekad";
            icerikbox.ValueMember = "id";
            icerikbox.DataSource = dt;
            baglanti.Close();
        }

        private void kullaniciidbtn_Click(object sender, EventArgs e)
        {
            aramabaslar();
            useridlbl.Visible = true; useridtxt.Visible = true; useridarabtn.Visible = true;

        }

        private void siparisidbtn_Click(object sender, EventArgs e)
        {
            aramabaslar();
            useridlbl.Visible = true; siparisidtxt.Visible = true; siparisidarabtn.Visible = true;

        }

        private void siparisidtxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
             (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void useridtxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
       (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void icerikbtn_Click(object sender, EventArgs e)
        {
            aramabaslar();
            icerikboxlbl.Visible = true; icerikbox.Visible = true; icerikarabtn.Visible = true;

        }

        private void useridarabtn_Click(object sender, EventArgs e)
        {
            if(useridtxt.Text != "")
            {
                string sorgu = "SELECT siparis_list.kullanicis AS \"Kullancı ID:\",siparis_list.id AS \"Sipariş ID\",siparis_list.toplam AS \"Tutar\", siparis_tamamlandimi.durum AS \"Tamamlanma Durumu\" FROM siparis_list INNER JOIN siparis_tamamlandimi ON siparis_list.tamamlanma = siparis_tamamlandimi.id where siparis_list.kullanicis =" + useridtxt.Text;
                baglanti.Open();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
                DataTable tablo = new DataTable();
                da.Fill(tablo);
                dataGridView1.DataSource = tablo;
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("User id Boş Bırakılamaz!", "User ID Hatası!");
            }
        }

        private void siparisidarabtn_Click(object sender, EventArgs e)
        {
            if (useridtxt.Text != "")
            {
                string sorgu = "SELECT siparis_list.kullanicis AS \"Kullancı ID:\",siparis_list.id AS \"Sipariş ID\",siparis_list.toplam AS \"Tutar\", siparis_tamamlandimi.durum AS \"Tamamlanma Durumu\" FROM siparis_list INNER JOIN siparis_tamamlandimi ON siparis_list.tamamlanma = siparis_tamamlandimi.id where siparis_list.id =" + siparisidtxt.Text;
                baglanti.Open();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
                DataTable tablo = new DataTable();
                da.Fill(tablo);
                dataGridView1.DataSource = tablo;
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Sipariş ID Boş Bırakılamaz!", "SİPARİŞ ID Hatası!");
            }
        }

        private void icerikarabtn_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(icerikbox.SelectedValue);
            string sorgu = "SELECT siparis_list.kullanicis AS \"Kullancı ID:\",siparis_list.id AS \"Sipariş ID\",siparis_list.toplam AS \"Tutar\", siparis_tamamlandimi.durum AS \"Tamamlanma Durumu\" FROM siparis_list INNER JOIN siparis_tamamlandimi ON siparis_list.tamamlanma = siparis_tamamlandimi.id where siparis_list.secilenyemek @> \'{" + id.ToString() + "}\'";
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
    }
}
