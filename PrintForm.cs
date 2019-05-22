using Microsoft.Reporting.WinForms;
using Microsoft.ReportingServices.Interfaces;
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


    public partial class PrintForm : Form
    {
        ProductDataAccess dataAccess;
        long id;

        public PrintForm(long id)
        {
            this.id = id;
            dataAccess = new ProductDataAccess();
            InitializeComponent();
        }

        private void PrintForm_Load(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            string stringtime = "Ngày " + dateTime.Day + " tháng " + dateTime.Month + " năm " + dateTime.Year;

            ReportDataSource data = new ReportDataSource("DataSet1", dataAccess.dbContext.rp_print(id).ToList());
            ReportParameter[] param = new ReportParameter[]
            {
                new ReportParameter("receipid", id.ToString()),
                //new ReportParameter("receipid", id.ToString()),
                new ReportParameter("datetime", stringtime)
            };
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(data);
            this.reportViewer1.LocalReport.SetParameters(param);
            this.reportViewer1.RefreshReport();
        }
    }
}
