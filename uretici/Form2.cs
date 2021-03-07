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
    public partial class Form2 : Form
    {

        public static string baglanti = "server=localhost;database=db;user id=root;password=1234";
        MySqlConnection veritabani = new MySqlConnection(baglanti);
        private static int musteriid;
        private static int siparisid;
        private static int kurunid;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.button5_Click(null, null);

            veritabani.Open();
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
                MessageBox.Show("Combobox da sorun var.");
            }
            read.Close();
            veritabani.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            veritabani.Open();
            MySqlDataAdapter vericek = new MySqlDataAdapter("SELECT * FROM Siparis", veritabani);
            DataTable table = new DataTable();
            vericek.Fill(table);
            dataGridView1.DataSource = table;

            vericek = new MySqlDataAdapter("SELECT * FROM Musteri", veritabani);
            table = new DataTable();
            vericek.Fill(table);
            dataGridView2.DataSource = table;
            veritabani.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" && textBox3.Text != "")
            {
                MessageBox.Show("Lütfen bir Kimyasal Ürün giriniz!!\nGirmek istemiyorsanız Miktar'ı boşaltınız.!!");
            }
            else if (textBox3.Text == "" && comboBox1.Text != "")
            {
                MessageBox.Show("Lütfen bir Miktar seçiniz!!\nSeçmek istemiyorsanız Kimyasal Ürün'ü boşaltınız.!!");
            }
            else if(textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Lütfen Müşteri Adı veye Adresi kısımlarını boş bırakmayınız!!");
            }
            else
            {
                veritabani.Open();
                int id = -1;
                try
                {
                    MySqlCommand komut = new MySqlCommand("SELECT MusteriID FROM Musteri WHERE MusteriAD='" + textBox1.Text + "' AND Adres='" + textBox2.Text + "'", veritabani);
                    MySqlDataReader read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        id = read.GetInt16("MusteriID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (id == -1)
                    {
                        komut = new MySqlCommand("INSERT INTO Musteri(MusteriAD, Adres) values('" + textBox1.Text + "','" + textBox2.Text + "')", veritabani);
                        komut.ExecuteNonQuery();
                    }
                    if (comboBox1.Text != "" && textBox3.Text != "")
                    {
                        komut = new MySqlCommand("SELECT MusteriID FROM Musteri WHERE MusteriAD = '" + textBox1.Text + "' AND Adres = '" + textBox2.Text + "'", veritabani);
                        read = komut.ExecuteReader();
                        read.Read();
                        id = read.GetInt16("MusteriID");
                        read.Close();
                        komut = new MySqlCommand("SELECT KurunID FROM Kurun WHERE KurunAD='" + comboBox1.Text + "'", veritabani);
                        read = komut.ExecuteReader();
                        read.Read();
                        int id1 = read.GetInt16("KurunID");
                        read.Close();
                        komut = new MySqlCommand("INSERT INTO Siparis(MusteriID, KurunID, Miktar) values('" + id + "','" + id1 + "','" + textBox3.Text + "')", veritabani);
                        komut.ExecuteNonQuery();
                    }
                    MessageBox.Show("Ekleme Basarili!!");
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Ekleme Basarisiz!!" + exp.ToString());
                }
                veritabani.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Lütfen boş alan bırakmayınız!!");
            }
            else
            {
                int id = -1;
                veritabani.Open();
                MySqlCommand komut = new MySqlCommand("SELECT MusteriID FROM Musteri WHERE MusteriAD='" + textBox1.Text + "' AND Adres='" + textBox2.Text + "'", veritabani);
                MySqlDataReader read = komut.ExecuteReader();
                try
                {
                    read.Read();
                    id = read.GetInt16("MusteriID");
                }
                catch (Exception)
                {
                    
                }
                read.Close();
                if (id == -1)
                {
                    komut = new MySqlCommand("UPDATE Musteri SET Adres='" + textBox2.Text + "' WHERE MusteriID='" + musteriid + "'", veritabani);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Basarili.");
                }
                else
                {
                    MessageBox.Show("Musteri daha once tanimlanmis.");
                }
                
                if(comboBox1.Text != "" && textBox3.Text != "")
                {
                    komut = new MySqlCommand("SELECT KurunID FROM Kurun WHERE KurunAD='" + comboBox1.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    read.Read();
                    int kid = read.GetInt16("KurunID");
                    read.Close();
                    komut = new MySqlCommand("UPDATE Siparis SET KurunID='" + kid + "', Miktar='" + textBox3.Text + "' WHERE SiparisID='" + siparisid + "'", veritabani);
                    komut.ExecuteNonQuery();
                }
                veritabani.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            veritabani.Open();
            int alan = dataGridView1.SelectedCells[0].RowIndex;
            siparisid = Convert.ToInt16(dataGridView1.Rows[alan].Cells[0].Value.ToString());
            musteriid = Convert.ToInt16(dataGridView1.Rows[alan].Cells[1].Value.ToString());
            kurunid = Convert.ToInt16(dataGridView1.Rows[alan].Cells[2].Value.ToString());

            MySqlCommand komut = new MySqlCommand("SELECT * FROM Musteri WHERE MusteriID='" + musteriid + "'", veritabani);
            MySqlDataReader read = komut.ExecuteReader();
            read.Read();
            textBox1.Text = read.GetString("MusteriAD");
            textBox2.Text = read.GetString("Adres");
            read.Close();

            komut = new MySqlCommand("SELECT * FROM Kurun WHERE KurunID='" + kurunid + "'", veritabani);
            read = komut.ExecuteReader();
            read.Read();
            comboBox1.Text = read.GetString("KurunAD");
            read.Close();

            textBox3.Text = dataGridView1.Rows[alan].Cells[3].Value.ToString();
            textBox1.Enabled = false;
            veritabani.Close();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            veritabani.Open();
            int alan = dataGridView2.SelectedCells[0].RowIndex;
            musteriid = Convert.ToInt16(dataGridView2.Rows[alan].Cells[0].Value.ToString());
            textBox1.Text = dataGridView2.Rows[alan].Cells[1].Value.ToString();
            textBox2.Text = dataGridView2.Rows[alan].Cells[2].Value.ToString();
            textBox1.Enabled = false;
            veritabani.Close();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.Text = "";
            textBox3.Clear();
        }
    }
}
