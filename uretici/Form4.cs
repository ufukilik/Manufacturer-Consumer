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
    public partial class Form4 : Form
    {
        public static string baglanti = "server=localhost;database=db;user id=root;password=1234";
        MySqlConnection veritabani = new MySqlConnection(baglanti);
        private static int guncelle = 0;
        private static int uid;
        private static int kid;
        private static int kargo;
        private static int fid;
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            this.button2_Click(null, null);
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            veritabani.Open();
            MySqlDataAdapter vericek = new MySqlDataAdapter("SELECT * FROM Tedarikci", veritabani);
            DataTable table = new DataTable();
            vericek.Fill(table);
            dataGridView1.DataSource = table;

            vericek = new MySqlDataAdapter("SELECT * FROM Ulke", veritabani);
            table = new DataTable();
            vericek.Fill(table);
            dataGridView2.DataSource = table;

            vericek = new MySqlDataAdapter("SELECT * FROM Konum", veritabani);
            table = new DataTable();
            vericek.Fill(table);
            dataGridView3.DataSource = table;

            vericek = new MySqlDataAdapter("SELECT * FROM Kargo", veritabani);
            table = new DataTable();
            vericek.Fill(table);
            dataGridView4.DataSource = table;
            
            
            MySqlCommand komut = new MySqlCommand("SELECT UlkeAD FROM Ulke", veritabani);
            MySqlDataReader read = komut.ExecuteReader();
            try
            {
                while (read.Read())
                {
                    comboBox1.ValueMember = "UlkeID";
                    comboBox1.Items.Add(read.GetString("UlkeAD"));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Combobox1 de sorun var.");
            }
            read.Close();

            komut = new MySqlCommand("SELECT KonumAD FROM Konum", veritabani);
            read = komut.ExecuteReader();
            try
            {
                while (read.Read())
                {
                    comboBox2.Items.Add(read.GetString("KonumAD"));
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
            if (textBox1.Text == "" || textBox2.Text == "" || comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Lütfen boş alan bırakmayınız!!");
            }
            else
            {
                veritabani.Open();
                int firmaid = -1;
                int ulkeid = -1;
                int konumid = -1;
                int kargoid;
                try
                {
                    MySqlCommand komut = new MySqlCommand("SELECT UlkeID FROM Ulke WHERE UlkeAD='" + comboBox1.Text + "'", veritabani);
                    MySqlDataReader read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        ulkeid = read.GetInt16("UlkeID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (ulkeid == -1)
                    {
                        komut = new MySqlCommand("INSERT INTO Ulke(UlkeAD) values('" + comboBox1.Text + "')", veritabani);
                        komut.ExecuteNonQuery();
                    }

                    komut = new MySqlCommand("SELECT UlkeID FROM Ulke WHERE UlkeAD='" + comboBox1.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    read.Read();
                    ulkeid = read.GetInt16("UlkeID");
                    read.Close();

                    komut = new MySqlCommand("SELECT KonumID FROM Konum WHERE UlkeID='" + ulkeid + "' AND KonumAD='" + comboBox2.Text + "' AND Mesafe='" + textBox2.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        konumid = read.GetInt16("KonumID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (konumid == -1)
                    {
                        komut = new MySqlCommand("INSERT INTO Konum(UlkeID,KonumAD,Mesafe) values('" + ulkeid + "','" + comboBox2.Text + "','" + textBox2.Text + "')", veritabani);
                        komut.ExecuteNonQuery();
                    }

                    komut = new MySqlCommand("SELECT KonumID FROM Konum WHERE KonumAD='" + comboBox2.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    read.Read();
                    konumid = read.GetInt16("KonumID");
                    read.Close();

                    if(comboBox1.Text == "Turkiye" || comboBox1.Text == "turkiye")
                    {
                        kargoid = 2;
                    }
                    else
                    {
                        kargoid = 1;
                    }

                    komut = new MySqlCommand("SELECT FirmaID FROM Tedarikci WHERE FirmaAD='" + textBox1.Text + "' AND KonumID='" + konumid + "' AND KargoID='" + kargoid + "'", veritabani);
                    read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        firmaid = read.GetInt16("FirmaID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (firmaid == -1)
                    {
                        komut = new MySqlCommand("INSERT INTO Tedarikci(FirmaAD,KonumID,KargoID) values('" + textBox1.Text + "','" + konumid + "','" + kargoid + "')", veritabani);
                        komut.ExecuteNonQuery();
                    }
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

        private void button6_Click(object sender, EventArgs e)
        {
            Form5 frm = new Form5();
            frm.Show();
            this.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            comboBox2.Enabled = true;
            textBox2.Clear();
            textBox2.Enabled = true;
            textBox1.Clear();
            comboBox1.Text = "";
            comboBox2.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.button5_Click(null, null);
            textBox1.Enabled = true;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            guncelle = 1;
            veritabani.Open();
            int alan = dataGridView1.SelectedCells[0].RowIndex;
            fid = Convert.ToInt16(dataGridView1.Rows[alan].Cells[0].Value.ToString());
            textBox1.Text = dataGridView1.Rows[alan].Cells[1].Value.ToString();
            kid = Convert.ToInt16(dataGridView1.Rows[alan].Cells[2].Value.ToString());

            MySqlCommand komut = new MySqlCommand("SELECT * FROM Konum WHERE KonumID='" + kid + "'", veritabani);
            MySqlDataReader read = komut.ExecuteReader();
            read.Read();
            uid = read.GetInt16("UlkeID");
            comboBox2.Text = read.GetString("KonumAD");
            textBox2.Text = read.GetString("Mesafe");
            read.Close();
            
            komut = new MySqlCommand("SELECT * FROM Ulke WHERE UlkeID='" + uid + "'", veritabani);
            read = komut.ExecuteReader();
            read.Read();
            comboBox1.Text = read.GetString("UlkeAD");
            read.Close();
            veritabani.Close();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.button5_Click(null, null);
            textBox1.Enabled = false;
            comboBox2.Enabled = false;
            textBox2.Enabled = false;
            guncelle = 2;
            veritabani.Open();
            int alan = dataGridView2.SelectedCells[0].RowIndex;
            uid = Convert.ToInt16(dataGridView2.Rows[alan].Cells[0].Value.ToString());
            comboBox1.Text = dataGridView2.Rows[alan].Cells[1].Value.ToString();
            veritabani.Close();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.button5_Click(null, null);
            textBox1.Enabled = false;
            comboBox2.Enabled = true;
            guncelle = 3;
            veritabani.Open();
            int alan = dataGridView3.SelectedCells[0].RowIndex;
            kid = Convert.ToInt16(dataGridView3.Rows[alan].Cells[0].Value.ToString());
            uid = Convert.ToInt16(dataGridView3.Rows[alan].Cells[1].Value.ToString());

            MySqlCommand komut = new MySqlCommand("SELECT * FROM Ulke WHERE UlkeID='" + uid + "'", veritabani);
            MySqlDataReader read = komut.ExecuteReader();
            read.Read();
            comboBox1.Text = read.GetString("UlkeAD");
            read.Close();
            
            comboBox2.Text = dataGridView3.Rows[alan].Cells[2].Value.ToString();
            textBox2.Text = dataGridView3.Rows[alan].Cells[3].Value.ToString();
            veritabani.Close();
            textBox2.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            veritabani.Open();
            int firmaid = -1;
            int ulkeid = -1;
            int konumid = -1;
            int kargoid;
            try
            {
                if (guncelle == 1 && comboBox1.Text != "" && comboBox2.Text != "" && textBox1.Text != "" && textBox2.Text != "")
                {
                    MySqlCommand komut = new MySqlCommand("SELECT UlkeID FROM Ulke WHERE UlkeAD='" + comboBox1.Text + "'", veritabani);
                    MySqlDataReader read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        ulkeid = read.GetInt16("UlkeID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (ulkeid == -1)
                    {
                        komut = new MySqlCommand("INSERT INTO Ulke(UlkeAD) values('" + comboBox1.Text + "')", veritabani);
                        komut.ExecuteNonQuery();
                    }

                    komut = new MySqlCommand("SELECT UlkeID FROM Ulke WHERE UlkeAD='" + comboBox1.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    read.Read();
                    ulkeid = read.GetInt16("UlkeID");
                    read.Close();

                    komut = new MySqlCommand("SELECT KonumID FROM Konum WHERE UlkeID='" + ulkeid + "' AND KonumAD='" + comboBox2.Text + "' AND Mesafe='" + textBox2.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        konumid = read.GetInt16("KonumID");
                        
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (konumid == -1)
                    {
                        komut = new MySqlCommand("INSERT INTO Konum(UlkeID,KonumAD,Mesafe) values('" + ulkeid + "','" + comboBox2.Text + "','" + textBox2.Text + "')", veritabani);
                        komut.ExecuteNonQuery();
                    }

                    komut = new MySqlCommand("SELECT KonumID FROM Konum WHERE KonumAD='" + comboBox2.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    read.Read();
                    konumid = read.GetInt16("KonumID");
                    read.Close();

                    if (comboBox1.Text == "Turkiye" || comboBox1.Text == "turkiye" || comboBox1.Text == "Türkiye" || comboBox1.Text == "türkiye")
                    {
                        kargoid = 2;
                    }
                    else
                    {
                        kargoid = 1;
                    }

                    komut = new MySqlCommand("SELECT FirmaID FROM Tedarikci WHERE FirmaAD='" + textBox1.Text + "' AND KonumID='" + konumid + "' AND KargoID='" + kargoid + "'", veritabani);
                    read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        firmaid = read.GetInt16("FirmaID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (firmaid == -1)
                    {
                        komut = new MySqlCommand("UPDATE Tedarikci SET FirmaAD='" + textBox1.Text + "', KonumID='" + konumid + "', KargoID='" + kargoid + "' WHERE FirmaID='" + fid + "'", veritabani);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Basarili");
                    }
                    else
                    {
                        MessageBox.Show("Firma daha once tanimlanmis.");
                    }
                }
                else if (guncelle == 2 && comboBox1.Text != "")
                {
                    MySqlCommand komut = new MySqlCommand("SELECT UlkeID FROM Ulke WHERE UlkeAD='" + comboBox1.Text + "'", veritabani);
                    MySqlDataReader read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        ulkeid = read.GetInt16("UlkeID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (ulkeid == -1)
                    {
                        komut = new MySqlCommand("UPDATE Ulke SET UlkeAD='" + comboBox1.Text + "' WHERE UlkeID='" + uid + "'", veritabani);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Basarili");
                    }
                    else
                    {
                        MessageBox.Show("Ulke daha once tanimlanmis.");
                    }
                }
                else if (guncelle == 3 && comboBox1.Text != "" && comboBox2.Text != "" && textBox2.Text != "")
                {
                    MySqlCommand komut = new MySqlCommand("SELECT UlkeID FROM Ulke WHERE UlkeAD='" + comboBox1.Text + "'", veritabani);
                    MySqlDataReader read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        ulkeid = read.GetInt16("UlkeID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (ulkeid == -1)
                    {
                        komut = new MySqlCommand("INSERT INTO Ulke(UlkeAD) values('" + comboBox1.Text + "')", veritabani);
                        komut.ExecuteNonQuery();
                    }

                    komut = new MySqlCommand("SELECT UlkeID FROM Ulke WHERE UlkeAD='" + comboBox1.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    read.Read();
                    ulkeid = read.GetInt16("UlkeID");
                    read.Close();

                    komut = new MySqlCommand("SELECT KonumID FROM Konum WHERE UlkeID='" + ulkeid + "' AND KonumAD='" + comboBox2.Text + "' AND Mesafe='" + textBox2.Text + "'", veritabani);
                    read = komut.ExecuteReader();
                    try
                    {
                        read.Read();
                        konumid = read.GetInt16("KonumID");
                    }
                    catch (Exception)
                    {
                        
                    }
                    read.Close();
                    if (konumid == -1)
                    {
                        komut = new MySqlCommand("UPDATE Konum SET UlkeID='" + ulkeid + "', KonumAD='" + comboBox2.Text + "', Mesafe='" + textBox2.Text + "' WHERE KonumID='" + kid + "'", veritabani);
                        komut.ExecuteNonQuery();
                        MessageBox.Show("Basarili");
                    }
                    else
                    {
                        MessageBox.Show("Konum daha once tanimlanmis.");
                    }
                }
                else if (guncelle == 0)
                {
                    MessageBox.Show("Lütfen bir alan seciniz.");
                }
                else
                {
                    MessageBox.Show("Bos alan birakmayiniz.");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
            veritabani.Close();
            this.button5_Click(null, null);
        }
    }
}
