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
    public partial class Form5 : Form
    {
        public static string baglanti = "server=localhost;database=db;user id=root;password=1234";
        MySqlConnection veritabani = new MySqlConnection(baglanti);
        private static int fid;
        private static int hid;
        private static int kid;
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            this.button2_Click(null, null);
        }

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 frm = new Form4();
            frm.Show();
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            veritabani.Open();

            MySqlDataAdapter vericek = new MySqlDataAdapter("SELECT * FROM Kunye", veritabani);
            DataTable table = new DataTable();
            vericek.Fill(table);
            dataGridView1.DataSource = table;
            
            MySqlCommand komut = new MySqlCommand("SELECT FirmaAD FROM Tedarikci", veritabani);
            MySqlDataReader read = komut.ExecuteReader();
            try
            {
                while (read.Read())
                {
                    comboBox1.Items.Add(read.GetString("FirmaAD"));
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || dateTimePicker1.Text == "" || comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Lutfen bos alan birakmayiniz!.");
            }else
            {
                veritabani.Open();
                int firmaid = -1;
                int hammadid = -1;
                try
                {
                    MySqlCommand komut = new MySqlCommand("SELECT FirmaID FROM Tedarikci WHERE FirmaAD='" + comboBox1.Text + "'", veritabani);
                    MySqlDataReader read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        firmaid = read.GetInt16("FirmaID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();

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
                    
                    komut = new MySqlCommand("INSERT INTO Kunye(FirmaID,HammadID,Stok,Tarihi,Omru,Fiyat) values('" + firmaid + "','" + hammadid + "','" + textBox2.Text + "','" + dateTimePicker1.Text + "','" + textBox1.Text + "','" + textBox3.Text + "')", veritabani);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Ekleme Basarili!!");
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Ekleme Basarisiz!!" + exp.ToString());
                }
                veritabani.Close();
                this.button5_Click(null, null);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            veritabani.Open();
            int alan = dataGridView1.SelectedCells[0].RowIndex;
            kid = Convert.ToInt16(dataGridView1.Rows[alan].Cells[0].Value.ToString());
            fid = Convert.ToInt16(dataGridView1.Rows[alan].Cells[1].Value.ToString());
            hid = Convert.ToInt16(dataGridView1.Rows[alan].Cells[2].Value.ToString());

            MySqlCommand komut = new MySqlCommand("SELECT * FROM Tedarikci WHERE FirmaID='" + fid + "'", veritabani);
            MySqlDataReader read = komut.ExecuteReader();
            read.Read();
            comboBox1.Text = read.GetString("FirmaAD");
            read.Close();

            komut = new MySqlCommand("SELECT * FROM Hammad WHERE HammadID='" + hid + "'", veritabani);
            read = komut.ExecuteReader();
            read.Read();
            comboBox2.Text = read.GetString("HammadAD");
            read.Close();

            textBox1.Text = dataGridView1.Rows[alan].Cells[5].Value.ToString();
            textBox2.Text = dataGridView1.Rows[alan].Cells[3].Value.ToString();
            textBox3.Text = dataGridView1.Rows[alan].Cells[6].Value.ToString();
            if(dataGridView1.Rows[alan].Cells[4].Value.ToString() != "")
            {
                dateTimePicker1.Value = DateTime.ParseExact(dataGridView1.Rows[alan].Cells[4].Value.ToString(), "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {

            }

            veritabani.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            veritabani.Open();
            int firmaid = -1;
            int hammadid = -1;
            try
            {
                if (comboBox1.Text != "" && comboBox2.Text != "" && dateTimePicker1.Text != "" && textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
                {
                    MySqlCommand komut = new MySqlCommand("SELECT FirmaID FROM Tedarikci WHERE FirmaAD='" + comboBox1.Text + "'", veritabani);
                    MySqlDataReader read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        firmaid = read.GetInt16("FirmaID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();

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

                    komut = new MySqlCommand("UPDATE Kunye SET FirmaID='" + firmaid + "', HammadID='" + hammadid + "', Stok='" + textBox2.Text + "', Tarihi='" + dateTimePicker1.Text + "', Omru='" + textBox1.Text + "', Fiyat='" + textBox3.Text + "' WHERE KunyeID='" + kid + "'", veritabani);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Basarili!!");
                }
                else
                {
                    MessageBox.Show("Lutfen box alan birakmayiniz.");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Ekleme Basarisiz!!" + exp.ToString());
            }
            veritabani.Close();
            this.button5_Click(null, null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
            dateTimePicker1.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }
    }
}
