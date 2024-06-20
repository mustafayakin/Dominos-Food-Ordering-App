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
using System.Globalization;


namespace RestoranAppVTYSproje
{
    public partial class uyearaForAdmin : Form
    {
        public uyearaForAdmin()
        {
            InitializeComponent();
        }
        void tumFalse()
        {
            adlbl.Visible = false; adtxt.Visible = false; adara.Visible = false;
            idlbl.Visible = false; idtxt.Visible = false; idara.Visible = false;
            seviyelbl.Visible = false; seviyebox.Visible = false; seviyeara.Visible = false;
            sehirlbl.Visible = false; sehirbox.Visible = false; sehirara.Visible = false;
            kadilbl.Visible = false; kaditxt.Visible = false; kadiara.Visible = false;
        }
        void guncelleFalse()
        {
            label10.Visible = false; textBox7.Visible = false; label2.Visible = false; textBox1.Visible = false;
            label3.Visible = false; textBox2.Visible = false; label4.Visible = false; comboBox1.Visible = false;
            label5.Visible = false; comboBox2.Visible = false; label6.Visible = false; textBox3.Visible = false;
            label7.Visible = false; textBox4.Visible = false; label12.Visible = false; textBox8.Visible = false;
            label9.Visible = false; comboBox3.Visible = false; label8.Visible = false; textBox6.Visible = false;
            updatebtn.Visible = false;
        }
        void guncelleTrue()
        {
            label10.Visible = true; textBox7.Visible = true; label2.Visible = true; textBox1.Visible = true;
            label3.Visible = true; textBox2.Visible = true; label4.Visible = true; comboBox1.Visible = true;
            label5.Visible = true; comboBox2.Visible = true; label6.Visible = true; textBox3.Visible = true;
            label7.Visible = true; textBox4.Visible = true; label12.Visible = true; textBox8.Visible = true;
            label9.Visible = true; comboBox3.Visible = true; label8.Visible = true; textBox6.Visible = true;
            updatebtn.Visible = true;
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=db_vtysproje; user ID=postgres; password=*****");
        private void uyearaForAdmin_Load(object sender, EventArgs e)
        {
            guncelleFalse();
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select *from sehirler", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            sehirbox.DisplayMember = "sehirad";
            sehirbox.ValueMember = "id";
            sehirbox.DataSource = dt;
            baglanti.Close();
        }
        private void button2_Click(object sender, EventArgs e) //ad
        {
            tumFalse();
            guncelleFalse();
            adlbl.Visible = true; adtxt.Visible = true; adara.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e) //id
        {
            tumFalse();
            guncelleFalse();
            idlbl.Visible = true; idtxt.Visible = true; idara.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e) //seviye
        {
            tumFalse();
            guncelleFalse();
            seviyebox.SelectedIndex = 2;
            seviyelbl.Visible = true; seviyebox.Visible = true; seviyeara.Visible = true;

        }

        private void button4_Click(object sender, EventArgs e) // sehir
        {
            tumFalse();
            guncelleFalse();
            sehirlbl.Visible = true; sehirbox.Visible = true; sehirara.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e) //k.adi
        {
            tumFalse();
            guncelleFalse();
            kadilbl.Visible = true; kaditxt.Visible = true; kadiara.Visible = true;
        }

        private void adara_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            string adi = adtxt.Text;
            adi = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(adi); //ilk harfini büyük yapıyor
            string sorgu = "SELECT * FROM uye_list WHERE ad LIKE \'"+adi+"\' AND yetki_id=0";
            NpgsqlDataReader dr;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            baglanti.Open();
            dr = komut.ExecuteReader();
            while(dr.Read())
            {
                dataGridView1.Rows.Add(dr.GetValue(0).ToString(),dr.GetString(5),dr.GetString(1),dr.GetString(2),dr.GetValue(8).ToString(),dr.GetValue(3).ToString());
            }
            baglanti.Close();
        }

        private void idara_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            string sorgu = "SELECT * FROM uye_list WHERE id="+ idtxt.Text.ToString()+ " AND yetki_id=0";
            NpgsqlDataReader dr;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            baglanti.Open();
            dr = komut.ExecuteReader();
            while(dr.Read())
            {
                dataGridView1.Rows.Add(dr.GetValue(0).ToString(), dr.GetString(5), dr.GetString(1), dr.GetString(2), dr.GetValue(8).ToString(), dr.GetValue(3).ToString());
            }
            baglanti.Close();
        }

        private void seviyeara_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            int secilen = Convert.ToInt32(seviyebox.SelectedItem);
            string sorgu = "SELECT * FROM uye_list WHERE seviyeid=" + secilen.ToString()+ " AND yetki_id=0";
            NpgsqlDataReader dr;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            baglanti.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr.GetValue(0).ToString(), dr.GetString(5), dr.GetString(1), dr.GetString(2), dr.GetValue(8).ToString(), dr.GetValue(3).ToString());
            }
            baglanti.Close();
        }

        private void sehirara_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            int secilensehir = Convert.ToInt32(sehirbox.SelectedValue);
            string sorgu = "select*from uye_list where sehirid =" + secilensehir.ToString()+ " AND yetki_id=0";
            NpgsqlDataReader dr;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            baglanti.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr.GetValue(0).ToString(), dr.GetString(5), dr.GetString(1), dr.GetString(2), dr.GetValue(8).ToString(), dr.GetValue(3).ToString());
            }
            baglanti.Close();
        }

        private void kadiara_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            string girilen = kaditxt.Text;
            string sorgu = "SELECT * FROM uye_list WHERE kullaniciad LIKE \'" +girilen+ "\' AND yetki_id=0";
            NpgsqlDataReader dr;
            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            baglanti.Open();
            dr = komut.ExecuteReader();
            while(dr.Read())
            {
                dataGridView1.Rows.Add(dr.GetValue(0).ToString(), dr.GetString(5), dr.GetString(1), dr.GetString(2), dr.GetValue(8).ToString(), dr.GetValue(3).ToString());
            }
            baglanti.Close();
        }

        private void idtxt_KeyPress(object sender, KeyPressEventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                string ismi = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                dynamic mboxResult = MessageBox.Show(ismi + " Kullanıcı Adlı Üye Silinecek Onaylıyor musunuz?", "Üye Silinecek?", MessageBoxButtons.YesNo);
                if (mboxResult == DialogResult.Yes)
                {
                    string sorgu = "delete from uye_list where id =" + dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show(ismi + " Kullanıcı Adlı Üye Silinmiştir.", "Üye Başarıyla Silindi!");
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();
                }
                else
                {
                    MessageBox.Show(ismi + " Kullanıcı Adlı Üye Silinmedi!", "Üyeyi Silmedik!");
                }
            }
            else
            {
                MessageBox.Show("Üye yok ki Sileyim!", "Uye Yok Hatası!");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                tumFalse();
                guncelleTrue();
                textBox7.Text = Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value);
                
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
            else
            {
                MessageBox.Show("Üye yok ki Sileyim!", "Uye Yok Hatası!");
            }
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
            if (countu == 1)
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
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }
    }
}
