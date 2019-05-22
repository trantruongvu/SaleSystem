using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeThongBanHang
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            //PrintForm form = new PrintForm();
            //form.Location = this.Location;
            //form.StartPosition = FormStartPosition.Manual;
            //form.FormClosing += delegate { this.Show(); };
            //form.Show();
            //this.Hide();

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //
        // Qua trang sản phẩm
        //
        private void buttonProduct_Click(object sender, EventArgs e)
        {
            FormProduct form = new FormProduct();
            form.Location = this.Location;
            form.StartPosition = FormStartPosition.Manual;
            form.FormClosing += delegate { this.Show(); };
            form.Show();
            this.Hide();
        }

        //
        // Qua trang hóa đơn
        //
        private void buttonReceipt_Click(object sender, EventArgs e)
        {
            FormReceipt form = new FormReceipt();
            form.Location = this.Location;
            form.StartPosition = FormStartPosition.Manual;
            form.FormClosing += delegate { this.Show(); };
            form.Show();
            this.Hide();
        }
    }
}
