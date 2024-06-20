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
    public partial class seviyelisttriggerForadm : Form
    {
        public seviyelisttriggerForadm()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=db_vtysproje; user ID=postgres; password=*****");
        void seviyelistgetir()
        {
            baglanti.Open();
            string sorgu = "SELECT seviyeli_uyeler.kullanici_id AS \"Kullanıcı ID\",seviyeli_uyeler.kullaniciadi \"Kullanıcı ADI:\", seviyeadlari.seviyead AS \"Seviye ADI:\" FROM seviyeli_uyeler INNER JOIN seviyeadlari ON seviyeli_uyeler.seviyeid = seviyeadlari.seviyeid";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        private void seviyelisttriggerForadm_Load(object sender, EventArgs e)
        {
            seviyelistgetir();
        }
    }
}
