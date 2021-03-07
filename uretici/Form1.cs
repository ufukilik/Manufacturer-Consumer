using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
    public partial class Form1 : Form
    {
        public static string baglanti = "server=localhost;database=db;user id=root;password=1234";
        MySqlConnection veritabani = new MySqlConnection(baglanti);
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
            this.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3();
            frm.Show();
            this.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form4 frm = new Form4();
            frm.Show();
            this.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 frm = new Form6();
            frm.Show();
            this.Visible = false;
        }
    }
}