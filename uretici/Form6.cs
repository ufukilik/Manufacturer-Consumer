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
    public partial class Form6 : Form
    {
        public static string baglanti = "server=localhost;database=db;user id=root;password=1234;Allow User Variables=True;";
        MySqlConnection veritabani = new MySqlConnection(baglanti);

        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            veritabani.Open();
            MySqlCommand komut = new MySqlCommand("SELECT * FROM Hammad", veritabani);
            MySqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboBox1.Items.Add(read.GetString("HammadAD"));
            }
            read.Close();

            MySqlDataAdapter vericek = new MySqlDataAdapter("SELECT * FROM AHam", veritabani);
            DataTable table = new DataTable();
            vericek.Fill(table);
            dataGridView1.DataSource = table;
            veritabani.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && comboBox1.Text != "")
            {
                veritabani.Open();
                int adet = Convert.ToInt16(textBox1.Text);
                MySqlDataAdapter vericek = new MySqlDataAdapter("SELECT T.FirmaID, T.FirmaAD, K.Stok, '" + adet + "' AS IstenenAdet, (IF (K.Stok>='" + adet + "','" + adet + "', K.Stok) * K.Fiyat) + (Ko.Mesafe * Ka.Maliyet) AS AlisFiyati, H.HammadAD, Ko.Mesafe, Ka.Maliyet, K.Fiyat, H.HammadID FROM Tedarikci AS T, Kunye AS K, Hammad AS H, Kargo AS Ka, Konum AS Ko WHERE T.KonumID=Ko.KonumID AND T.KargoID=Ka.KargoID AND K.HammadID=H.HammadID AND T.FirmaID=K.FirmaID AND H.HammadAD='" + comboBox1.Text + "' AND K.Stok>0 ORDER BY AlisFiyati ASC", veritabani);
                DataTable table = new DataTable();
                vericek.Fill(table);
                dataGridView3.DataSource = table;

                //("DECLARE @Miktar INT;IF (SELECT * FROM Kunye AS K, Hammad AS H WHERE K.HammadID=H.HammadID AND K.Stok>='" + adet + "') BEGIN SET @Miktar=K.Stok END ELSE BEGIN SET @Miktar='" + adet +"' END ORDER BY AlisFiyati ASC", veritabani);

                //("SELECT T.FirmaID, T.FirmaAD, K.Stok, H.HammadID, H.HammadAD, Ko.Mesafe, Ka.Maliyet, K.Fiyat, '" + adet + "' AS adet,  ( '" + adet + "' * K.Fiyat) + (Ko.Mesafe * Ka.Maliyet) AS AlisFiyati FROM Tedarikci AS T, Kunye AS K, Hammad AS H, Kargo AS Ka, Konum AS Ko WHERE T.KonumID=Ko.KonumID AND T.KargoID=Ka.KargoID AND K.HammadID=H.HammadID AND T.FirmaID=K.FirmaID AND H.HammadAD='" + comboBox1.Text + "' AND K.Stok>0 ORDER BY AlisFiyati ASC", veritabani);
                //("SELECT T.FirmaID, T.FirmaAD, K.Stok, '" + adet + "' AS IstenenAdet, ( '" + adet + "' * K.Fiyat) + (Ko.Mesafe * Ka.Maliyet) AS AlisFiyati, H.HammadAD, Ko.Mesafe, Ka.Maliyet, K.Fiyat, H.HammadID FROM Tedarikci AS T, Kunye AS K, Hammad AS H, Kargo AS Ka, Konum AS Ko WHERE T.KonumID=Ko.KonumID AND T.KargoID=Ka.KargoID AND K.HammadID=H.HammadID AND T.FirmaID=K.FirmaID AND H.HammadAD='" + comboBox1.Text + "' AND K.Stok>0 ORDER BY AlisFiyati ASC", veritabani);
                //("SELECT T.FirmaID, T.FirmaAD, K.Stok, '" + adet + "' AS IstenenAdet, ( '" + adet + "' * K.Fiyat) + (Ko.Mesafe * Ka.Maliyet) AS AlisFiyati, H.HammadAD, Ko.Mesafe, Ka.Maliyet, K.Fiyat, H.HammadID FROM Tedarikci AS T, Kunye AS K, Hammad AS H, Kargo AS Ka, Konum AS Ko WHERE T.KonumID=Ko.KonumID AND T.KargoID=Ka.KargoID AND K.HammadID=H.HammadID AND T.FirmaID=K.FirmaID AND H.HammadAD='" + comboBox1.Text + "' AND K.Stok>0 ORDER BY AlisFiyati ASC", veritabani);

                vericek = new MySqlDataAdapter("SELECT * FROM AHam", veritabani);
                table = new DataTable();
                vericek.Fill(table);
                dataGridView1.DataSource = table;
                veritabani.Close();
            }
            else
            {
                MessageBox.Show("Lütfen bos alan birakmayiniz.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && comboBox1.Text != "")
            {
                veritabani.Open();
                int adet = Convert.ToInt16(textBox1.Text);
                int stok;
                int firmaid;
                int hammadid;
                decimal alisfiyati;
                while (adet > 0)
                {
                    MySqlCommand komut = new MySqlCommand("SELECT T.FirmaID, T.FirmaAD, K.Stok, (IF (K.Stok>='" + adet + "','" + adet + "', K.Stok) * K.Fiyat) + (Ko.Mesafe * Ka.Maliyet) AS AlisFiyati, H.HammadAD, Ko.Mesafe, Ka.Maliyet, K.Fiyat, H.HammadID FROM Tedarikci AS T, Kunye AS K, Hammad AS H, Kargo AS Ka, Konum AS Ko WHERE T.KonumID=Ko.KonumID AND T.KargoID=Ka.KargoID AND K.HammadID=H.HammadID AND T.FirmaID=K.FirmaID AND H.HammadAD='" + comboBox1.Text + "' AND K.Stok>0 ORDER BY AlisFiyati ASC", veritabani);
                    MySqlDataReader read = komut.ExecuteReader();
                    read.Read();
                    stok = read.GetInt16("Stok");
                    firmaid = read.GetInt16("FirmaID");
                    hammadid = read.GetInt16("HammadID");
                    alisfiyati = read.GetDecimal("AlisFiyati");
                    read.Close();

                    if (adet >= stok)
                    {
                        adet = adet - stok;
                        komut = new MySqlCommand("UPDATE Kunye SET Stok='" + 0 + "' WHERE FirmaID='" + firmaid + "'AND HammadID='" + hammadid + "'", veritabani);
                        komut.ExecuteNonQuery();


                        komut = new MySqlCommand("INSERT INTO AHam(UreticiID,FirmaID,HammadID,Fiyat,HamStok) VALUES('" + 1 + "','" + firmaid + "','" + hammadid + "','" + alisfiyati.ToString().Replace(",", ".") + "','" + stok + "')", veritabani);
                        komut.ExecuteNonQuery();
                    }
                    else if (adet < stok)
                    {
                        stok = stok - adet;
                        komut = new MySqlCommand("UPDATE Kunye SET Stok='" + stok + "' WHERE FirmaID='" + firmaid + "' AND HammadID='" + hammadid + "'", veritabani);
                        komut.ExecuteNonQuery();


                        komut = new MySqlCommand("INSERT INTO AHam(UreticiID,FirmaID,HammadID,Fiyat,HamStok) VALUES('" + 1 + "','" + firmaid + "','" + hammadid + "','" + alisfiyati.ToString().Replace(",", ".") + "','" + adet + "')", veritabani);
                        komut.ExecuteNonQuery();
                        adet = 0;
                    }
                }
                MySqlDataAdapter vericek = new MySqlDataAdapter("SELECT * FROM AHam", veritabani);
                DataTable table = new DataTable();
                vericek.Fill(table);
                dataGridView1.DataSource = table;
                veritabani.Close();
            }
            else
            {
                MessageBox.Show("Lütfen bos alan birakmayiniz.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form7 frm = new Form7();
            frm.Show();
            this.Visible = false;
        }
    }
}
