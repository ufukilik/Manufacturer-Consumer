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
    public partial class Form3 : Form
    {
        public static string baglanti = "server=localhost;database=db;user id=root;password=1234";
        MySqlConnection veritabani = new MySqlConnection(baglanti);
        private static int kid;
        private static int hid;
        private static int bid;
        private static int guncelle = 0;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.button3_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" || textBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Lütfen boş alan bırakmayınız!!");
            }
            else
            {
                veritabani.Open();
                int kurunid = -1;
                int hammadid = -1;
                int bilesenid = -1;
                try
                {
                    MySqlCommand komut = new MySqlCommand("SELECT KurunID FROM Kurun WHERE KurunAD='" + comboBox1.Text + "'", veritabani);
                    MySqlDataReader read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        kurunid = read.GetInt16("KurunID");
                    }
                    catch (Exception)
                    {

                    }
                    read.Close();
                    if (kurunid == -1)
                    {
                        komut = new MySqlCommand("INSERT INTO Kurun(KurunAD) values('" + comboBox1.Text + "')", veritabani);
                        komut.ExecuteNonQuery();
                    }

                    komut = new MySqlCommand("SELECT HammadID FROM Hammad WHERE HammadAD='" + comboBox2.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        hammadid = read.GetInt16("HammadID");
                        
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (hammadid == -1)
                    {
                        komut = new MySqlCommand("INSERT INTO Hammad(HammadAD) values('" + comboBox2.Text + "')", veritabani);
                        komut.ExecuteNonQuery();
                    }
                    
                    komut = new MySqlCommand("SELECT KurunID FROM Kurun WHERE KurunAD='" + comboBox1.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    read.Read();
                    kurunid = read.GetInt16("KurunID");
                    read.Close();

                    komut = new MySqlCommand("SELECT HammadID FROM Hammad WHERE HammadAD='" + comboBox2.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    read.Read();
                    hammadid = read.GetInt16("HammadID");
                    read.Close();

                    komut = new MySqlCommand("SELECT BilesenID FROM Bilesen WHERE KurunID='" + kurunid + "' AND HammadID='" + hammadid + "' AND Adet='" + textBox1.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        bilesenid = read.GetInt16("BilesenID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (bilesenid == -1)
                    {
                        komut = new MySqlCommand("INSERT INTO Bilesen(KurunID, HammadID, Adet) values('" + kurunid + "','" + hammadid + "','" + textBox1.Text + "')", veritabani);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Ekleme Basarili!!");
                    }
                    else
                    {
                        MessageBox.Show("Bilsen daha once eklenmis.");
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Ekleme Basarisiz!!" + exp.ToString());
                }
                veritabani.Close();
                this.button5_Click(null, null);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            veritabani.Open();
            MySqlDataAdapter vericek = new MySqlDataAdapter("SELECT * FROM Kurun", veritabani);
            DataTable table = new DataTable();
            vericek.Fill(table);
            dataGridView2.DataSource = table;

            vericek = new MySqlDataAdapter("SELECT * FROM Bilesen", veritabani);
            table = new DataTable();
            vericek.Fill(table);
            dataGridView1.DataSource = table;

            vericek = new MySqlDataAdapter("SELECT * FROM Hammad", veritabani);
            table = new DataTable();
            vericek.Fill(table);
            dataGridView3.DataSource = table;

            
            MySqlCommand komut = new MySqlCommand("SELECT KurunAD FROM Kurun", veritabani);
            MySqlDataReader read = komut.ExecuteReader();
            try
            {
                while (read.Read())
                {
                    comboBox1.Items.Add(read.GetString("KurunAD"));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Combobox1 de sorun var.");
            }
            read.Close();

            
            komut = new MySqlCommand("SELECT HammadAD FROM Hammad", veritabani);
            read = komut.ExecuteReader();
            try
            {
                while (read.Read())
                {
                    comboBox2.Items.Add(read.GetString("HammadAD"));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Combobox2 de sorun var.");
            }
            read.Close();
            veritabani.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            veritabani.Open();
            int kurunid = -1;
            int hammadid = -1;
            int bilesenid = -1;
            try
            {
                if (guncelle == 1 && comboBox1.Text != "" && comboBox2.Text != "" && textBox1.Text != "")
                {
                    MySqlCommand komut = new MySqlCommand("SELECT KurunID FROM Kurun WHERE KurunAD='" + comboBox1.Text + "'", veritabani);
                    MySqlDataReader read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        kurunid = read.GetInt16("KurunID");
                    }
                    catch (Exception)
                    {

                    }
                    read.Close();
                    if (kurunid == -1)
                    {
                        komut = new MySqlCommand("INSERT INTO Kurun(KurunAD) values('" + comboBox1.Text + "')", veritabani);
                        komut.ExecuteNonQuery();
                    }

                    komut = new MySqlCommand("SELECT HammadID FROM Hammad WHERE HammadAD='" + comboBox2.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        hammadid = read.GetInt16("HammadID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (hammadid == -1)
                    {
                        komut = new MySqlCommand("INSERT INTO Hammad(HammadAD) values('" + comboBox2.Text + "')", veritabani);
                        komut.ExecuteNonQuery();
                    }

                    komut = new MySqlCommand("SELECT KurunID FROM Kurun WHERE KurunAD='" + comboBox1.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    read.Read();
                    kurunid = read.GetInt16("KurunID");
                    read.Close();

                    komut = new MySqlCommand("SELECT HammadID FROM Hammad WHERE HammadAD='" + comboBox2.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    read.Read();
                    hammadid = read.GetInt16("HammadID");
                    read.Close();

                    komut = new MySqlCommand("SELECT BilesenID FROM Bilesen WHERE KurunID='" + kurunid + "' AND HammadID='" + hammadid + "' AND Adet='" + textBox1.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        bilesenid = read.GetInt16("BilesenID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (bilesenid == -1)
                    {
                        komut = new MySqlCommand("UPDATE Bilesen SET KurunID='" + kurunid + "', HammadID='" + hammadid + "', Adet='" + textBox1.Text + "' WHERE BilesenID='" + bid + "'", veritabani);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Basarili");
                    }
                    else
                    {
                        MessageBox.Show("Bilesen daha önce eklenmis.");
                    }
                }
                else if (guncelle == 2 && comboBox1.Text != "")
                {
                    MySqlCommand komut = new MySqlCommand("SELECT KurunID FROM Kurun WHERE KurunAD='" + comboBox1.Text + "'", veritabani);
                    MySqlDataReader read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        kurunid = read.GetInt16("KurunID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (kurunid == -1)
                    {
                        komut = new MySqlCommand("UPDATE Kurun SET KurunAD='" + comboBox1.Text + "' WHERE KurunID='" + kid + "'", veritabani);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Basarili");
                    }
                    else
                    {
                        MessageBox.Show("Urun daha once tanimlanmis.");
                    }
                }
                else if (guncelle == 3 && comboBox2.Text != "")
                {
                    MySqlCommand komut = new MySqlCommand("SELECT HammadID FROM Hammad WHERE HammadAD='" + comboBox2.Text + "'", veritabani);
                    MySqlDataReader read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        hammadid = read.GetInt16("HammadID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (hammadid == -1)
                    {
                        komut = new MySqlCommand("UPDATE Hammad SET HammadAD='" + comboBox2.Text + "' WHERE HammadID='" + hid + "'", veritabani);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Basarili");
                    }
                    else
                    {
                        MessageBox.Show("Hammadde daha once tanimlanmis.");
                    }
                }
                else if (guncelle == 0)
                {
                    MessageBox.Show("Lütfen bir alan seciniz.");
                }
                else
                {
                    MessageBox.Show("Lütfen bos alan birakmayiniz.");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
            veritabani.Close();
            this.button5_Click(null, null);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.button5_Click(null, null);
            guncelle = 1;
            veritabani.Open();
            int alan = dataGridView1.SelectedCells[0].RowIndex;
            bid = Convert.ToInt16(dataGridView1.Rows[alan].Cells[0].Value.ToString());
            kid = Convert.ToInt16(dataGridView1.Rows[alan].Cells[1].Value.ToString());
            hid = Convert.ToInt16(dataGridView1.Rows[alan].Cells[2].Value.ToString());

            MySqlCommand komut = new MySqlCommand("SELECT * FROM Kurun WHERE KurunID='" + kid + "'", veritabani);
            MySqlDataReader read = komut.ExecuteReader();
            read.Read();
            comboBox1.Text = read.GetString("KurunAD");
            read.Close();

            komut = new MySqlCommand("SELECT * FROM Hammad WHERE HammadID='" + hid + "'", veritabani);
            read = komut.ExecuteReader();
            read.Read();
            comboBox2.Text = read.GetString("HammadAD");
            read.Close();

            textBox1.Text = dataGridView1.Rows[alan].Cells[3].Value.ToString();
            veritabani.Close();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            guncelle = 2;
            comboBox1.Enabled = true;
            textBox1.Text = "";
            textBox1.Enabled = false;
            comboBox2.Text = "";
            comboBox2.Enabled = false;
            
            veritabani.Open();
            int alan = dataGridView2.SelectedCells[0].RowIndex;
            kid = Convert.ToInt16(dataGridView2.Rows[alan].Cells[0].Value.ToString());

            MySqlCommand komut = new MySqlCommand("SELECT * FROM Kurun WHERE KurunID='" + kid + "'", veritabani);
            MySqlDataReader read = komut.ExecuteReader();
            read.Read();
            comboBox1.Text = read.GetString("KurunAD");
            read.Close();
            veritabani.Close();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            guncelle = 3;
            comboBox2.Enabled = true;
            textBox1.Clear();
            textBox1.Enabled = false;
            comboBox1.Text = "";
            comboBox1.Enabled = false;

            veritabani.Open();
            int alan = dataGridView3.SelectedCells[0].RowIndex;
            hid = Convert.ToInt16(dataGridView3.Rows[alan].Cells[0].Value.ToString());

            MySqlCommand komut = new MySqlCommand("SELECT * FROM Hammad WHERE HammadID='" + hid + "'", veritabani);
            MySqlDataReader read = komut.ExecuteReader();
            read.Read();
            comboBox2.Text = read.GetString("HammadAD");
            read.Close();
            veritabani.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            textBox1.Enabled = true;
            comboBox1.Text = "";
            comboBox2.Text = "";
            textBox1.Clear();
        }
    }
}
