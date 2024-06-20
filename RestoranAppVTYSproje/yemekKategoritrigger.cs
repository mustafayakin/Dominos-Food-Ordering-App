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
    public partial class yemekKategoritrigger : Form
    {
        public yemekKategoritrigger()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
        "Database=db_vtysproje; user ID=postgres; password=*****");
        private void yemekKategoritrigger_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            string sorgu = "SELECT yemek_kategori.yemek_id AS \"YEMEK ID\", yemek_kategoriad.adi \"Kategori ADI\" FROM yemek_kategori INNER JOIN yemek_kategoriad ON yemek_kategori.kategori_id = yemek_kategoriad.id";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
    }
}
