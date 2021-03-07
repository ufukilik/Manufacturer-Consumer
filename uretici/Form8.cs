using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace uretici
{
    public partial class Form8 : Form
    {
        public static string baglanti = "server=localhost;database=db;user id=root;password=1234";
        MySqlConnection veritabani = new MySqlConnection(baglanti);
        private static int mid;
        private static int sid;
        private static int kid;
        private static int adet;
        public Form8()
        {
            InitializeComponent();
        }

        private void Form8_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            veritabani.Open();
            MySqlDataAdapter vericek = new MySqlDataAdapter("SELECT * FROM Siparis", veritabani);
            DataTable table = new DataTable();
            vericek.Fill(table);
            dataGridView1.DataSource = table;

            vericek = new MySqlDataAdapter("SELECT * FROM SUrun", veritabani);
            table = new DataTable();
            vericek.Fill(table);
            dataGridView2.DataSource = table;
            veritabani.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form7 frm = new Form7();
            frm.Show();
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                veritabani.Open();
                int stok;
                decimal kar = Convert.ToDecimal(textBox3.Text);

                MySqlCommand komut = new MySqlCommand("SELECT SUM(UrunStok) AS UrunStok FROM GUrun WHERE KurunID='" + kid + "' AND UrunStok>0", veritabani);
                MySqlDataReader read = komut.ExecuteReader();
                read.Read();
                stok = read.GetInt16("UrunStok");
                read.Close();

                int adetyedek = adet;
                
                if (stok >= adet)
                {
                    int gurunid;
                    decimal topmal;
                    decimal isci;
                    decimal topfiyat = 0;
                    decimal topisci = 0;
                    while (adet > 0)
                    {
                        komut = new MySqlCommand("SELECT GUrunID, TopMal, IsciMal, UrunStok FROM GUrun WHERE KurunID='" + kid + "' AND UrunStok>0 ORDER BY TopMal DESC", veritabani);
                        read = komut.ExecuteReader();
                        read.Read();
                        gurunid = read.GetInt16("GUrunID");
                        topmal = read.GetDecimal("TopMal");
                        isci = read.GetDecimal("IsciMal");
                        stok = read.GetInt16("UrunStok");
                        read.Close();

                        topfiyat += topmal;
                        topisci += isci;

                        if (adet >= stok)
                        {
                            adet = adet - stok;
                            komut = new MySqlCommand("UPDATE GUrun SET UrunStok='" + 0 + "' WHERE GUrunID='" + gurunid + "' AND KurunID='" + kid + "'", veritabani);
                            komut.ExecuteNonQuery();
                        }
                        else if (adet < stok)
                        {
                            stok = stok - adet;
                            komut = new MySqlCommand("UPDATE GUrun SET UrunStok='" + stok + "' WHERE GUrunID='" + gurunid + "' AND KurunID='" + kid + "'", veritabani);
                            komut.ExecuteNonQuery();
                            adet = 0;
                        }
                    }

                    decimal satis = topfiyat + ((topfiyat * kar)/100);
                    decimal sonuc = satis - (topfiyat - topisci);

                    komut = new MySqlCommand("INSERT INTO SUrun(MusteriID,KurunID,USatFiyat,Adet,Kar) VALUES('" + mid + "','" + kid + "','" + satis.ToString().Replace(",",".") + "','" + adetyedek + "','" + sonuc.ToString().Replace(",", ".") + "')", veritabani);
                    komut.ExecuteNonQuery();

                    komut = new MySqlCommand("DELETE FROM Siparis WHERE SiparisID='" + sid + "'", veritabani);
                    komut.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show("Stokta urun yok.");
                }
                MySqlDataAdapter vericek = new MySqlDataAdapter("SELECT * FROM SUrun", veritabani);
                DataTable table = new DataTable();
                vericek.Fill(table);
                dataGridView2.DataSource = table;

                vericek = new MySqlDataAdapter("SELECT * FROM Siparis", veritabani);
                table = new DataTable();
                vericek.Fill(table);
                dataGridView1.DataSource = table;
                veritabani.Close();
            }
            else
            {
                MessageBox.Show("Lutfen bos alan birakmayiniz.");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            veritabani.Open();
            int alan = dataGridView1.SelectedCells[0].RowIndex;
            sid = Convert.ToInt16(dataGridView1.Rows[alan].Cells[0].Value.ToString());
            mid = Convert.ToInt16(dataGridView1.Rows[alan].Cells[1].Value.ToString());
            kid = Convert.ToInt16(dataGridView1.Rows[alan].Cells[2].Value.ToString());
            adet = Convert.ToInt16(dataGridView1.Rows[alan].Cells[3].Value.ToString());
            MySqlCommand komut = new MySqlCommand("SELECT * FROM Musteri WHERE MusteriID='" + mid + "'", veritabani);
            MySqlDataReader read = komut.ExecuteReader();
            read.Read();
            textBox1.Text = read.GetString("MusteriAD");
            read.Close();

            komut = new MySqlCommand("SELECT * FROM Kurun WHERE KurunID='" + kid + "'", veritabani);
            read = komut.ExecuteReader();
            read.Read();
            textBox2.Text = read.GetString("KurunAD");
            read.Close();
            veritabani.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                veritabani.Open();
                MySqlDataAdapter vericek = new MySqlDataAdapter("SELECT * FROM GUrun WHERE KurunID='" + kid + "' AND UrunStok>0 ORDER BY TopMal DESC", veritabani);
                DataTable table = new DataTable();
                vericek.Fill(table);
                dataGridView3.DataSource = table;
                veritabani.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            veritabani.Open();
            MySqlDataAdapter vericek = new MySqlDataAdapter("SELECT DISTINCT MusteriID, SUM(Kar) AS ToplamKar FROM SUrun", veritabani);
            DataTable table = new DataTable();
            vericek.Fill(table);
            dataGridView3.DataSource = table;
            veritabani.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            veritabani.Open();
            MySqlDataAdapter vericek = new MySqlDataAdapter("SELECT DISTINCT KurunID, SUM(Kar) AS ToplamKar FROM SUrun", veritabani);
            DataTable table = new DataTable();
            vericek.Fill(table);
            dataGridView3.DataSource = table;
            veritabani.Close();
        }
    }
}
