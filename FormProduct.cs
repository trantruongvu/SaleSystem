using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VuTran.DataAccess;

namespace HeThongBanHang
{
    public partial class FormProduct : Form
    {
        ProductDataAccess productDataAccess;

        //
        // Start
        //
        public FormProduct()
        {
            InitializeComponent();
            productDataAccess = new ProductDataAccess();
        }

        //
        // Load lại trang Product
        //
        private void FormProduct_Load(object sender, EventArgs e)
        {
            textBoxProductCode.Text = "";
            textBoxProductName.Text = "";
            textBoxProductPrice.Text = "0";

            dataGridViewProduct.AllowUserToAddRows = true;
            this.dataGridViewProduct.ReadOnly = true;
            this.dataGridViewProduct.RowTemplate.Height = 50;
            this.dataGridViewProduct.DataSource = productDataAccess.dbContext.Products;
            this.dataGridViewProduct.Columns[0].Visible = false;
            this.dataGridViewProduct.Columns[1].HeaderText = "Mã";
            this.dataGridViewProduct.Columns[1].Width = 150;
            this.dataGridViewProduct.Columns[2].HeaderText = "Tên";
            this.dataGridViewProduct.Columns[2].Width = 600;
            this.dataGridViewProduct.Columns[3].HeaderText = "Giá";
            this.dataGridViewProduct.Columns[3].Width = 200;
        }

        //
        // Chỉ cho nhập số ở Giá
        //
        private void textBoxProductPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        // 
        // Nút Thêm sản phẩm
        // 
        private void button1_Click(object sender, EventArgs e)
        {
            AddProduct(sender,e);
        }

        public void AddProduct(object sender, EventArgs e)
        {
            Product product = new Product();
            product.Id = DateTime.Now.Ticks;
            product.Code = textBoxProductCode.Text;
            product.Name = textBoxProductName.Text;
            product.Price = long.Parse(textBoxProductPrice.Text);
    
            //MessageBox.Show(productDataAccess.Insert(product));

            if (productDataAccess.Insert(product))
            {
                MessageBox.Show("Tạo thành công!");

                //DataTable dataTable = (DataTable)dataGridViewProduct.DataSource;
                //DataRow newRow = dataTable.NewRow();
                //newRow["Id"] = product.Id;
                //newRow["Code"] = product.Code;
                //newRow["Name"] = product.Name;
                //newRow["Price"] = product.Price;
                //dataGridViewProduct.Rows.Add(newRow);

                FormProduct_Load(sender, e);
            }
            else
            {
                MessageBox.Show("Tạo không thành công!");
            }
        }

        //
        // Nút xóa sản phẩm
        //
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int currentRow = dataGridViewProduct.CurrentCell.RowIndex;
            dataGridViewProduct.Rows.RemoveAt(currentRow);
        }
    }
}
