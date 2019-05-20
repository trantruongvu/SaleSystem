using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VuTran.DataAccess;

namespace HeThongBanHang
{
    public partial class FormReceipt : Form
    {
        ProductDataAccess productDataAccess;

        //
        // Start
        //
        public FormReceipt()
        {
            InitializeComponent();
            productDataAccess = new ProductDataAccess();

            var source = new AutoCompleteStringCollection();
            List<Product> products = productDataAccess.dbContext.Products.ToList();
            foreach (Product product in products)
            {
                source.AddRange(new string[]
                {
                    product.Code
                });
            }

            // Thêm suggestion cho Mã sản phẩm
            this.textBoxProductCode.AutoCompleteCustomSource = source;
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
        // Thêm hàng vào Table hàng hóa
        // 
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBoxProductCode.Text) || string.IsNullOrEmpty(this.textBoxProductName.Text))
            {
                MessageBox.Show("Mã hoặc tên sản phẩm bị trống");
                return;
            }

            int countSTT = dataGridViewProduct.RowCount;

            this.dataGridViewProduct.Rows.Add(
                countSTT, // Số thứ tự
                this.textBoxProductName.Text, // Tên hàng hóa
                this.textBoxProductPrice.Text, // Đơn giá
                this.textBoxProductQuantity.Text, // Số lượng
                long.Parse(this.textBoxProductPrice.Text) * long.Parse(this.textBoxProductQuantity.Text)); // Thành tiền

            this.textBoxProductCode.Text = "";
            this.textBoxProductName.Text = "";
        }

        // 
        // Tạo hóa đơn để In
        // 
        public void CreateReceiptForPrint()
        {

        }
    }
}
