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

namespace HeThongBanHang
{
    public partial class FormProduct : Form
    {
        DataContext dataContext;
        bool show = true;
        public Action OnClosing;

        //
        // Start
        //
        public FormProduct()
        {
            InitializeComponent();
            dataContext = new DataContext();
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
            this.dataGridViewProduct.DataSource = dataContext.dbContext.Products;
            this.dataGridViewProduct.Columns[0].Visible = false;
            this.dataGridViewProduct.Columns[1].HeaderText = "Mã";
            this.dataGridViewProduct.Columns[1].Width = 150;
            this.dataGridViewProduct.Columns[2].HeaderText = "Tên";
            this.dataGridViewProduct.Columns[2].Width = 600;
            this.dataGridViewProduct.Columns[3].HeaderText = "Giá";
            this.dataGridViewProduct.Columns[3].Width = 200;
            dataGridViewProduct.Refresh();

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


        // Nút Thêm sản phẩm
        // 
        public void AddProduct(object sender, EventArgs e)
        {
            // Kiểm tra trùng Mã sản phẩm
            List<Product> products = dataContext.dbContext.Products.ToList();

            Product product = products.FirstOrDefault(p => p.Code == textBoxProductCode.Text);
            if (product != null)
            {
                MessageBox.Show("Mã sản phẩm tồn tại");
            }

            product = new Product();
            product.Id = DateTime.Now.Ticks;
            product.Code = textBoxProductCode.Text;
            product.Name = textBoxProductName.Text;
            product.Price = long.Parse(textBoxProductPrice.Text);

            //MessageBox.Show(productDataAccess.Insert(product));

            if (dataContext.Insert(product))
            {
                MessageBox.Show("Tạo thành công!");

                show = false;
                FormProduct form = new FormProduct();
                form.Location = this.Location;
                form.StartPosition = FormStartPosition.Manual;
                form.Show();
                form.OnClosing = OnClosing;
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Tạo không thành công!");
            }
        }

        //
        // Nút xóa sản phẩm
        //
        private void RemoveProduct(object sender, EventArgs e)
        {
            int currentRow = dataGridViewProduct.CurrentCell.RowIndex;
            string code = dataGridViewProduct.Rows[currentRow].Cells[1].Value.ToString().Trim();

            Product product = dataContext.dbContext.Products.FirstOrDefault(p => p.Code.Equals(code));

            MessageBox.Show(code);

            if (dataContext.Remove(product))
            {
                MessageBox.Show("Xóa thành công!");
                dataGridViewProduct.Rows.RemoveAt(currentRow);
            }
            else
            {
                MessageBox.Show("Xóa không thành công!");
            }

        }

        private void FormProduct_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (show)
                OnClosing();
        }
    }
}
