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
using DGVPrinterHelper;

namespace EatBetter
{
    public partial class Form2 : Form
    {
        SqlConnection koneksi = new SqlConnection(@"Data Source=LAPTOP-CLP7IGP4\SQLEXPRESS;Initial Catalog=Restoran;Integrated Security=True");
        public Form2()
        {
            InitializeComponent();
        }

        string ImageLocation = "";
        SqlCommand cmd;

        int Topping = 0;

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            bobaDrink.Visible = false;
            sirupDrink.Visible = false;

            if (comboBox3.Text == "Boba Drink")
            {
                cbHargaMinuman.Items.Clear();
                cbHargaMinuman.Items.Add(10000);
                cbHargaMinuman.Text = cbHargaMinuman.Items[0].ToString();
                bobaDrink.Visible = true;
            }
            else if (comboBox3.Text == "Sirup Drink")
            {
                cbHargaMinuman.Items.Clear();
                cbHargaMinuman.Items.Add(5000);
                cbHargaMinuman.Text = cbHargaMinuman.Items[0].ToString();
                sirupDrink.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Warning!!!", "Yakin Ingin Keluar?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int hargaMakanan = int.Parse(cbHargaMakanan.Text);
            int item = int.Parse(numericUpDown1.Value.ToString());
            int total = hargaMakanan * item;
            cbHargaMakanan.Text = total.ToString();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            int hargaMinuman = int.Parse(cbHargaMinuman.Text);
            int item = int.Parse(numericUpDown2.Value.ToString());
            int total = hargaMinuman * item;
            cbHargaMinuman.Text = total.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Data Siswa";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date.ToString("dddd-MMMM-yyyy"));
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Center;
            printer.Footer = "Andika Setya Eka Natha";
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(dataGridView1);
            dataGridView1.Columns[1].Width = 108;
            dataGridView1.Columns[2].Width =
                dataGridView1.Width
                - dataGridView1.Columns[0].Width
                - dataGridView1.Columns[1].Width - 92;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            nasiGoreng.Visible = false;
            mieGoreng.Visible = false;
            
            if (comboBox1.Text == "Nasi Goreng")
            {
                cbHargaMakanan.Items.Clear();
                cbHargaMakanan.Items.Add(18000);
                cbHargaMakanan.Text = cbHargaMakanan.Items[0].ToString();
                nasiGoreng.Visible = true;
            }
            else if (comboBox1.Text == "Mie Goreng")
            {
                cbHargaMakanan.Items.Clear();
                cbHargaMakanan.Items.Add(17000);
                cbHargaMakanan.Text = cbHargaMakanan.Items[0].ToString();
                mieGoreng.Visible = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            comboBox1.Text = "";
            numericUpDown1.Text = "";
            comboBox3.Text = "";
            numericUpDown2.Text = "";
            textBox4.Text = "";
            cbHargaMakanan.Text = "";
            cbHargaMinuman.Text = "";
            Barcode.ImageLocation = null;
            textBox5.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            nasiGoreng.Visible = false;
            mieGoreng.Visible = false;
            sirupDrink.Visible = false;
            bobaDrink.Visible=false;
        }

        private void cbHargaMakanan_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string barCode = cbHargaMakanan.Text + cbHargaMinuman;
            try
            {
                Zen.Barcode.Code128BarcodeDraw brCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                Barcode.Image = brCode.Draw(barCode, 40);
            }
            catch (Exception)
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            koneksi.Open();
            SqlCommand cmd = koneksi.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into [tb_restoran] (kode,namamakanan,jumlahmakanan,namaminuman,jumlahminuman,topping) values ('" + textBox1.Text + "', '" + comboBox1.Text + "', '" + numericUpDown1.Text + "', '" + comboBox3.Text + "', '" + numericUpDown2.Text + "', '" + Topping + "')";
            cmd.ExecuteNonQuery();
            koneksi.Close();
            MessageBox.Show("data insert sukses");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Topping = 0;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Topping = 1;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            int hargaMakanan = int.Parse(cbHargaMakanan.Text);
            int hargaMinuman = int.Parse(cbHargaMakanan.Text);
            int total = hargaMakanan + hargaMinuman;
            textBox5.Text = total.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            display_data();
        }

        public void display_data()
        {
            koneksi.Open();
            SqlCommand cmd = koneksi.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from [tb_restoran]";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dt);
            dataGridView1.DataSource = dt;
            koneksi.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            koneksi.Open();
            SqlCommand cmd = koneksi.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update [tb_restoran] set kode = '" + this.textBox1.Text + "', namamakanan = '" + this.comboBox1.Text + "' , jumlahmakanan = '" + this.numericUpDown1.Text + "' , namaminuman = '" + this.comboBox3.Text + "' , jumlahminuman = '" + this.numericUpDown2.Text + "', topping = '" + Topping + "'";
            cmd.ExecuteNonQuery();
            koneksi.Close();
            display_data();
            MessageBox.Show("Data berhasil di update");
            ;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            koneksi.Open();
            SqlCommand cmd = koneksi.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from [tb_restoran] where kode='" + textBox4.Text + "'";
            DataTable dt = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dt);
            dataGridView1.DataSource = dt;
            koneksi.Close();
            textBox4.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Form1().Show();
        }
    }
}
