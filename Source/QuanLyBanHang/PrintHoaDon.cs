using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang
{
    public partial class PrintHoaDon : Form
    {
        List<Receipt> _list;
        string _tongThanhToan, _date, _tenKH;
        public PrintHoaDon(List<Receipt> dataSource, string tongThanhToan, string date, string tenKH)
        {
            InitializeComponent();
            _list = dataSource;
            _tongThanhToan = tongThanhToan;
            _date = date;
            _tenKH = tenKH;
        }

        private void PrintHoaDon_Load(object sender, EventArgs e)
        {
            ReceiptBindingSource.DataSource = _list;
            Microsoft.Reporting.WinForms.ReportParameter[] para = new Microsoft.Reporting.WinForms.ReportParameter[] {
                new Microsoft.Reporting.WinForms.ReportParameter("pTongThanhToan",_tongThanhToan),
                new Microsoft.Reporting.WinForms.ReportParameter("pDate",_date),
                new Microsoft.Reporting.WinForms.ReportParameter("pName",_tenKH),
            };
            this.reportViewer1.LocalReport.SetParameters(para);
            this.reportViewer1.RefreshReport();
        }
    }
}
