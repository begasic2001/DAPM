using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using app = Microsoft.Office.Interop.Excel.Application;

namespace QuanLyBanHang
{
    public partial class FrmChiTietPhieuNhap : Form
    {

        DBQuanLyBanHangDataContext db = new DBQuanLyBanHangDataContext();

        public FrmChiTietPhieuNhap()
        {
            InitializeComponent();
        }

        AutoCompleteStringCollection collection = new AutoCompleteStringCollection();

        private void BindingData()
        {
            var listCTPN = db.ChiTietPhieuNhaps.Where(n => n.SanPham.TenSP.Contains(txtTimKiem.Text.Trim()));
            foreach (var item in listCTPN)
            {
                collection.Add(item.MaSP.ToString());
                collection.Add(item.SanPham.TenSP);
                collection.Add(item.MaPN.ToString());
            }
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiem.AutoCompleteCustomSource = collection;
        }

        private void iconPictureBox8_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void iconPictureBox7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void LoadChiTietPhieuNhap() {
            dataChiTietPhieuNhap.DataSource = from a in db.ChiTietPhieuNhaps
                                              select new
                                              {
                                                  a.MaChiTietPN,
                                                  a.MaPN,
                                                  a.MaSP,
                                                  a.DonGiaNhap,
                                                  a.SoLuongNhap,
                                                  a.PhieuNhap.NgayNhap
                                              };
        }

        private void FrmChiTietPhieuNhap_Load(object sender, EventArgs e)
        {
            LoadChiTietPhieuNhap();
            BindingData();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadChiTietPhieuNhap();
        }

        private void fillGrid_ByDatetime()
        {
            var fildatetime = dateTimePicker1.Text;
            var load = from a in db.ChiTietPhieuNhaps
                       where a.PhieuNhap.NgayNhap.Value.Date.Equals(fildatetime)
                       select new
                       {
                           a.MaChiTietPN,
                           a.MaPN,
                           a.MaSP,
                           a.DonGiaNhap,
                           a.SoLuongNhap,
                           a.PhieuNhap.NgayNhap
                       };
            dataChiTietPhieuNhap.DataSource = load;
        }

        private void fillGrid_All()
        {
            var load = from a in db.ChiTietPhieuNhaps
                       where a.SanPham.TenSP.Contains(txtTimKiem.Text.Trim())
                       select new
                       {
                           a.MaChiTietPN,
                           a.MaPN,
                           a.MaSP,
                           a.DonGiaNhap,
                           a.SoLuongNhap,
                           a.PhieuNhap.NgayNhap
                       };
            dataChiTietPhieuNhap.DataSource = load;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            fillGrid_ByDatetime();
        }

        private void xuatfileExcel(DataGridView g, string path, string tenfile)
        {
            app obj = new app();
            obj.Application.Workbooks.Add(Type.Missing);
            obj.Columns.ColumnWidth = 30;

            for (int i = 1; i < g.ColumnCount + 1; i++)
            {
                obj.Cells[1, i] = g.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < g.Rows.Count; i++)
            {
                for (int j = 0; j < g.Columns.Count; j++)
                {
                    if (g.Rows[i].Cells[j].Value != null)
                    {
                        obj.Cells[i + 2, j + 1] = g.Rows[i].Cells[j].Value.ToString();
                    }
                }
            }
            obj.ActiveWorkbook.SaveCopyAs(path + tenfile + ".xlsx");
            obj.ActiveWorkbook.Saved = true;
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
                            // Đây là đường dẫn lưu file excel, tuỳ bạn muốn lưu ở đâu
            xuatfileExcel(dataChiTietPhieuNhap, @"C:\Users\admin\Desktop", "ThongKePhieuNhap");
            MessageBox.Show("Xuất file thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            fillGrid_All();
        }

        private void fillGrid_ToDay()
        {
            var totay = DateTime.Today;
            var load = from a in db.ChiTietPhieuNhaps
                       where a.PhieuNhap.NgayNhap.Value.Date.Equals(totay)
                       select new
                       {
                           a.MaChiTietPN,
                           a.MaPN,
                           a.MaSP,
                           a.DonGiaNhap,
                           a.SoLuongNhap,
                           a.PhieuNhap.NgayNhap
                       };
            dataChiTietPhieuNhap.DataSource = load;
        }


        private void btnToday_Click(object sender, EventArgs e)
        {
            fillGrid_ToDay();
        }
    }
}
