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
    public partial class yemekfiyatLoglari : Form
    {
        public yemekfiyatLoglari()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
        "Database=db_vtysproje; user ID=postgres; password=*****");
        private void yemekfiyatLoglari_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            string sorgu = "SELECT yemekfiyatdegisikligi.urunno AS \"YEMEK ID\", yemek_list.yemekad \"Yemek ADI\", yemekfiyatdegisikligi.eskifiyat \"Eski Fiyatı\",yemekfiyatdegisikligi.yenifiyat \"Yeni Fiyatı\",yemekfiyatdegisikligi.degisiklik_tarih \"Deg. Tarihi\" FROM yemekfiyatdegisikligi INNER JOIN yemek_list ON yemekfiyatdegisikligi.urunno = yemek_list.id";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
    }
}
