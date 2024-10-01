using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Pastahane_Uygulamsı
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection("Data Source=faruk\\sqlexpress;Initial Catalog=Pastahane_Uygulaması;Integrated Security=True;");

        void MalzemeListe()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * from TBLMALZEMELER", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        void Kasa()
        {
            SqlDataAdapter da3 = new SqlDataAdapter("select * from TBLKASA", conn);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            dataGridView1.DataSource = dt3;
        }
        void UrunListesi()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("Select * from TBLURUNLER", conn);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView1.DataSource = dt2;
        }
        void Urunler()
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("select * from TBLURUNLER", conn);
            DataTable dt2 = new DataTable();
            da.Fill(dt2);
            cmb_UrunOlustur.ValueMember = "URUNID";
            cmb_UrunOlustur.DisplayMember = "AD";
            cmb_UrunOlustur.DataSource = dt2;
            conn.Close();
        }
        void Malzemeler()
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * from TBLMALZEMELER", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmb_malzeme.ValueMember = "MALZEMEID";
            cmb_malzeme.DisplayMember = "AD";
            cmb_malzeme.DataSource = dt;
            conn.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MalzemeListe();
            Malzemeler();
            Urunler();
        }
        private void btn_urunList_Click(object sender, EventArgs e)
        {
            UrunListesi();
        }

        private void btn_malzemeList_Click(object sender, EventArgs e)
        {
            MalzemeListe();
        }

        private void Btn_kasa_Click(object sender, EventArgs e)
        {
            Kasa();
        }

        private void btn_cıkıs_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_malzemeEkle_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("insert into TBLMALZEMELER (AD,STOK,FİYAT,NOTLAR) values (@P1,@P2,@P3,@P4)", conn);
            cmd.Parameters.AddWithValue("@P1", txt_MalzemeAd.Text);
            cmd.Parameters.AddWithValue("@P2", decimal.Parse(txt_MalzemeStok.Text));
            cmd.Parameters.AddWithValue("@P3", decimal.Parse(txt_MalzemeFiyat.Text));
            cmd.Parameters.AddWithValue("@P4", txt_notlar.Text);
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Malzeme Sisteme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_urunEkle_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("insert into TBLURUNLER (AD) values (@P1)", conn);
            cmd.Parameters.AddWithValue("@P1", txt_UrunAd.Text);
            //cmd.Parameters.AddWithValue("@P2",txt_UrunMFiyat.Text);
            //cmd.Parameters.AddWithValue("@P3",txt_UrunSfiyat.Text);
            //cmd.Parameters.AddWithValue("@P4", txt_UrunStok.Text);
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Ürün Sisteme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UrunListesi();
        }

        private void btn_ekle_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("insert into TBLFIRIN (URUNID,MALZEMEID,MIKTAR,MALIYET) values (@P1,@P2,@P3,@P4)", conn);
            cmd.Parameters.AddWithValue("@P1", cmb_UrunOlustur.SelectedValue);
            cmd.Parameters.AddWithValue("@P2", cmb_malzeme.SelectedValue);
            cmd.Parameters.AddWithValue("@P3", decimal.Parse(txt_UrunMiktar.Text));
            cmd.Parameters.AddWithValue("@P4", decimal.Parse(txt_UrunMaliyet.Text));
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Malzeme Sisteme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void txt_UrunMiktar_TextChanged(object sender, EventArgs e)
        {
            double maliyet;
            if (txt_UrunMaliyet.Text == "")
            {
                txt_UrunMaliyet.Text = "0";
            }

            conn.Open();

            SqlCommand cmd = new SqlCommand("select * from TBLMALZEMELER where MALZEMEID=@P1", conn);
            cmd.Parameters.AddWithValue("@P1", cmb_malzeme.SelectedValue);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                txt_UrunMaliyet.Text = dr[3].ToString();
            }

            conn.Close();

            maliyet = Convert.ToDouble(txt_UrunMaliyet.Text) / 1000 * Convert.ToDouble(txt_UrunMiktar.Text);

            txt_UrunMaliyet.Text = maliyet.ToString();


        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            txt_UrunID.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            txt_UrunAd.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();

            conn.Open();

            SqlCommand cmd = new SqlCommand("select sum(MALIYET) from TBLFIRIN WHERE URUNID=@P1", conn);
            cmd.Parameters.AddWithValue("@P1", txt_UrunID.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                txt_UrunMFiyat.Text = dr[0].ToString();

            }
            conn.Close();
        }
    }
}
