using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeThongBanHang
{
    public class DataContext
    {
        public DataAccessDataContext dbContext = new DataAccessDataContext();

        public bool Insert(Product product)
        {
            try
            {
                dbContext.Products.InsertOnSubmit(product);
                dbContext.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Remove (Product product)
        {
            try
            {
                dbContext.Products.DeleteOnSubmit(product);
                dbContext.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Insert(Receipt receipt)
        {
            try
            {
                dbContext.Receipts.InsertOnSubmit(receipt);
                dbContext.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 
        //
        public bool Insert(ReceiptDetail receiptDetail)
        {
            //try
            //{
            dbContext.ReceiptDetails.InsertOnSubmit(receiptDetail);
            dbContext.SubmitChanges();
            return true;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
        }
    }
}
