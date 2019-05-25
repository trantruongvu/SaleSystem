using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HeThongBanHang;


namespace HeThongBanHang
{
    public partial class FormReceipt : Form
    {
        //ProductDataAccess productDataAccess;
        public DataContext dataContext;
        long TotalMoney = 0;

        private long _ReceiptId;
        public long ReceiptId { get { return _ReceiptId; } set { _ReceiptId = value; } }
        
        // Start
        //
        public FormReceipt()
        {
            InitializeComponent();
            dataContext = new DataContext();

            InitSourceAutoComplete();
        }

        // Tạo danh sách AutoComplete
        //
        private void InitSourceAutoComplete ()
        {
            this.dataGridViewProduct.RowTemplate.Height = 35;

            var source = new AutoCompleteStringCollection();
            List<Product> products = dataContext.dbContext.Products.ToList();
            int s= dataContext.dbContext.rp_print(13).ToList().Count;
            foreach (Product product in products)
            {
                source.AddRange(new string[]
                {
                    product.Code.Trim()
                });
            }

            // Thêm suggestion cho Mã sản phẩm
            this.textBoxProductCode.AutoCompleteCustomSource = source;
        }


        // Khi bấm Enter
        //
        private void onFinishEnterCode (object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Product product = dataContext.dbContext.Products.FirstOrDefault(p => p.Code == textBoxProductCode.Text);

                if (product == null)
                    return;

                this.textBoxProductName.Text = product.Name.Trim();
                this.textBoxProductPrice.Text = product.Price.ToString().Trim();
            }

            if (e.KeyCode == Keys.Escape)
            {
                this.textBoxProductCode.Text = "";
                this.textBoxProductName.Text = "";
                this.textBoxProductPrice.Text = "0";
                this.textBoxProductQuantity.Text = "0";
            }
        }

        // Tự văng ra thông tin hàng hóa khi bấm
        //
        private void onEnteringCode(object sender, KeyPressEventArgs e)
        {
            Product product = dataContext.dbContext.Products.FirstOrDefault(p => p.Code == textBoxProductCode.Text);

            if (product == null)
                return;

            this.textBoxProductName.Text = product.Name.Trim();
            this.textBoxProductPrice.Text = product.Price.ToString().Trim();
        }


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
        
       
        // Thêm hàng vào Table hàng hóa
        // 
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBoxProductCode.Text) || string.IsNullOrEmpty(this.textBoxProductName.Text))
            {
                MessageBox.Show("Mã hoặc tên sản phẩm bị trống");
                return;
            }

            int countSTT = dataGridViewProduct.RowCount;
            long productTotalMoney = long.Parse(this.textBoxProductPrice.Text) * long.Parse(this.textBoxProductQuantity.Text);

            this.dataGridViewProduct.Rows.Add(
                /* Số thứ tự */     countSTT,
                /* Tên hàng hóa */  this.textBoxProductName.Text,
                /* Đơn giá */       this.textBoxProductPrice.Text,
                /* Số lượng */      this.textBoxProductQuantity.Text,
                /* Thành tiền */    productTotalMoney.ToString()); 

            this.textBoxProductCode.Text = "";
            this.textBoxProductName.Text = "";
            this.textBoxProductPrice.Text = "0";
            this.textBoxProductQuantity.Text = "0";

            // Update + tổng tiền 
            TotalMoney += productTotalMoney;
            this.textBoxTotal.Text = String.Format("{0:n}", TotalMoney);
        }


        // Xóa món hàng vừa chọn
        //
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int currentRowIndex = dataGridViewProduct.CurrentCell.RowIndex;

            DataGridViewRow currentRow = dataGridViewProduct.Rows[currentRowIndex];
            dataGridViewProduct.Rows.RemoveAt(currentRowIndex);

            // Trừ - tổng tiền 
            TotalMoney -= long.Parse(currentRow.Cells[4].Value.ToString());
            this.textBoxTotal.Text = String.Format("{0:n}", TotalMoney);
        }


        // Tạo hóa đơn để In
        // 
        public void CreateReceiptForPrint()
        {

        }

        // Cho mất số 0 ở Giá
        //
        private void textPrice_Click(object sender, EventArgs e)

        {
            if (textBoxProductPrice.Text == "0")
                textBoxProductPrice.Text = "";
        }

        // Cho mất số 0 ở Số lượng
        //
        private void textQuantity_Click(object sender, EventArgs e)
        {
            if (textBoxProductQuantity.Text == "0")
                textBoxProductQuantity.Text = "";
        }

        // Nút Tạo hóa đơn
        //
        private void button1_Click(object sender, EventArgs e)
        {

            Receipt receipt = new Receipt();
            receipt.Id = DateTime.Now.Ticks;
            receipt.SellerName = this.textBoxSellerName.Text.Trim();
            receipt.SellerAddress = this.textBoxSellerPhone.Text.Trim();
            receipt.SellerPhone = this.textBoxSellerPhone.Text.Trim();
            receipt.BuyerName = this.textBoxBuyerName.Text.Trim();
            receipt.BuyerAddress = this.textBoxBuyerAddress.Text.Trim();
            receipt.BuyerPhone = this.textBoxSellerPhone.Text.Trim();
            receipt.TotalMoneyReceipt = String.Format("{0:n}", TotalMoney).Trim();

            ReceiptId = receipt.Id;

            if (dataContext.Insert(receipt))
            {
                //MessageBox.Show("Tạo thành công!");

                foreach (DataGridViewRow row in dataGridViewProduct.Rows)
                {
                    if (row.Cells[1].Value == null)
                        continue;

                    ReceiptDetail receiptDetail = new ReceiptDetail();
                    receiptDetail.Id = DateTime.Now.Ticks;
                    receiptDetail.ReportId = receipt.Id;
                    receiptDetail.Name = row.Cells[1].Value.ToString().Trim();
                    receiptDetail.Price = long.Parse(row.Cells[2].Value.ToString().Trim());
                    receiptDetail.Quantity = long.Parse(row.Cells[3].Value.ToString().Trim());
                    receiptDetail.TotalMoneyDetail = long.Parse(row.Cells[4].Value.ToString().Trim());

                    Debug.WriteLine( dataContext.Insert(receiptDetail) );
                    Debug.WriteLine( "receiptDetail : " + receiptDetail.Name );
                }

                //MessageBox.Show("Tạo thành công!");


                PrintForm form = new PrintForm(ReceiptId);
                form.Location = this.Location;
                form.StartPosition = FormStartPosition.Manual;
                form.FormClosing += delegate { this.Show(); };
                form.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Tạo không thành công!");
            }
        }
    }
}
