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
    public partial class Form7 : Form
    {
        public static string baglanti = "server=localhost;database=db;user id=root;password=1234";
        MySqlConnection veritabani = new MySqlConnection(baglanti);

        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            veritabani.Open();
            MySqlCommand komut = new MySqlCommand("SELECT * FROM Kurun", veritabani);
            MySqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboBox2.Items.Add(read.GetString("KurunAD"));
            }
            read.Close();
            
            MySqlDataAdapter vericek = new MySqlDataAdapter("SELECT * FROM GUrun", veritabani);
            DataTable table = new DataTable();
            vericek.Fill(table);
            dataGridView2.DataSource = table;

            vericek = new MySqlDataAdapter("SELECT * FROM AHam", veritabani);
            table = new DataTable();
            vericek.Fill(table);
            dataGridView5.DataSource = table;
            veritabani.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && comboBox2.Text != "" && dateTimePicker1.Text != "" && textBox3.Text != "")
            {
                veritabani.Open();
                int adet = Convert.ToInt16(textBox2.Text);
                int[] hammadid = new int[3];
                int[] hamadet = new int[3];
                int kurunid = 0;
                int i = 0;
                MySqlDataAdapter vericek;
                DataTable table;
                MySqlCommand komut = new MySqlCommand("SELECT B.HammadID, K.KurunID, B.Adet FROM Bilesen AS B, Kurun AS K WHERE K.KurunID=B.KurunID AND K.KurunAD='" + comboBox2.Text + "'", veritabani);
                MySqlDataReader read = komut.ExecuteReader();
                while (read.Read())
                {
                    kurunid = read.GetInt16("KurunID");
                    hammadid[i] = read.GetInt16("HammadID");
                    hamadet[i] = read.GetInt16("Adet");
                    i++;
                }
                read.Close();

                int j = 0;

                while (j < i)
                {
                    hamadet[j] = hamadet[j] * adet;
                    j++;
                }

                j = 0;

                int kontrol = 0;

                while (j < i)
                {
                    komut = new MySqlCommand("SELECT DISTINCT HammadID FROM AHam WHERE HammadID='" + hammadid[j] + "' AND HamStok>0", veritabani);
                    read = komut.ExecuteReader();
                    while (read.Read())
                    {
                        kontrol++;
                    }
                    read.Close();
                    j++;
                }
                

                if (kontrol == i)
                {
                    j = 0;
                    int[] stok = new int[3];
                    decimal[] fiyat = new decimal[3];

                    while (j < i)
                    {
                        komut = new MySqlCommand("SELECT SUM(HamStok) AS Stok FROM AHam WHERE HammadID='" + hammadid[j] + "' AND HamStok>0", veritabani);
                        read = komut.ExecuteReader();
                        read.Read();
                        stok[j] = read.GetInt16("Stok");
                        read.Close();
                        j++;
                    }

                    int yeterlimi = -1;
                    for (int k = 0; k < i; k++)
                    {
                        if (hamadet[k] <= stok[k])
                        {
                            yeterlimi = 1;
                        }
                        else
                        {
                            yeterlimi = 0;
                            break;
                        }
                    }

                    j = 0;

                    while (j < i)
                    {
                        if (j == 0)
                        {
                            vericek = new MySqlDataAdapter("SELECT H.HammadAD, HamStok, Fiyat FROM AHam AS A, Hammad AS H WHERE A.HammadID='" + hammadid[j] + "' AND A.HammadID=H.HammadID AND A.HamStok>0 ORDER BY Fiyat DESC", veritabani);
                            table = new DataTable();
                            vericek.Fill(table);
                            dataGridView4.DataSource = table;
                        }
                        if (j == 1)
                        {
                            vericek = new MySqlDataAdapter("SELECT H.HammadAD, HamStok, Fiyat FROM AHam AS A, Hammad AS H WHERE A.HammadID='" + hammadid[j] + "' AND A.HammadID=H.HammadID AND A.HamStok>0 ORDER BY Fiyat DESC", veritabani);
                            table = new DataTable();
                            vericek.Fill(table);
                            dataGridView1.DataSource = table;
                        }
                        if (j == 2)
                        {
                            vericek = new MySqlDataAdapter("SELECT H.HammadAD, HamStok, Fiyat FROM AHam AS A, Hammad AS H WHERE A.HammadID='" + hammadid[j] + "' AND A.HammadID=H.HammadID AND A.HamStok>0 ORDER BY Fiyat DESC", veritabani);
                            table = new DataTable();
                            vericek.Fill(table);
                            dataGridView3.DataSource = table;
                        }
                        j++;
                    }

                    j = 0;
                    decimal topfiyat = 0;
                    int ahamid;
                    if (yeterlimi == 1)
                    {
                        while (j < i)
                        {
                            while (hamadet[j] > 0)
                            {
                                komut = new MySqlCommand("SELECT AHamID, HamStok, Fiyat FROM AHam WHERE HammadID='" + hammadid[j] + "' AND HamStok>0 ORDER BY Fiyat DESC", veritabani);
                                read = komut.ExecuteReader();
                                read.Read();
                                ahamid = read.GetInt16("AHamID");
                                stok[j] = read.GetInt16("HamStok");
                                fiyat[j] = read.GetDecimal("Fiyat");
                                read.Close();
                                topfiyat += fiyat[j];

                                if (hamadet[j] >= stok[j])
                                {
                                    hamadet[j] = hamadet[j] - stok[j];
                                    komut = new MySqlCommand("UPDATE AHam SET HamStok='" + 0 + "' WHERE HammadID='" + hammadid[j] + "' AND AHamID='" + ahamid + "'", veritabani);
                                    komut.ExecuteNonQuery();
                                }
                                else if (hamadet[j] < stok[j])
                                {
                                    stok[j] = stok[j] - hamadet[j];
                                    komut = new MySqlCommand("UPDATE AHam SET HamStok='" + stok[j] + "' WHERE HammadID='" + hammadid[j] + "' AND AHamID='" + ahamid + "'", veritabani);
                                    komut.ExecuteNonQuery();
                                    hamadet[j] = 0;
                                }
                            }
                            j++;
                        }

                        komut = new MySqlCommand("INSERT INTO GUrun(KurunID,Tarihi,Omru,UrunStok,IsciMal,TopMal,UreticiID) VALUES('" + kurunid + "','" + dateTimePicker1.Text + "','" + textBox3.Text + "','" + adet + "','" + adet.ToString().Replace(",", ".") + "','" + (adet + topfiyat).ToString().Replace(",", ".") + "','" + 1 + "')", veritabani);
                        komut.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("Istenilen adet kadar uretim gerceklestirilemiyor. YETERLI HAMMADDE YOK!!");
                    }
                }
                else
                {
                    MessageBox.Show("HAMMADDE YOK!!");
                }
                
                vericek = new MySqlDataAdapter("SELECT * FROM GUrun", veritabani);
                table = new DataTable();
                vericek.Fill(table);
                dataGridView2.DataSource = table;

                vericek = new MySqlDataAdapter("SELECT * FROM AHam", veritabani);
                table = new DataTable();
                vericek.Fill(table);
                dataGridView5.DataSource = table;
                veritabani.Close();
            }
            else
            {
                MessageBox.Show("Lutfen bos alan birakmayiniz.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 frm = new Form6();
            frm.Show();
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form8 frm = new Form8();
            frm.Show();
            this.Visible = false;
        }
    }
}
