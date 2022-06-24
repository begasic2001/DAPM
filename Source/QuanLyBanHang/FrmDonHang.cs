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
    public partial class FrmDonHang : Form
    {

        DBQuanLyBanHangDataContext db = new DBQuanLyBanHangDataContext();

        public FrmDonHang()
        {
            InitializeComponent();
        }

        private void iconPictureBox8_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void iconPictureBox7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        AutoCompleteStringCollection collection = new AutoCompleteStringCollection();

        private void BindingData()
        {
            var listDDH = db.HoaDons.Where(n => n.MaHD.Contains(txtTimKiem.Text.Trim()) || n.KhachHang.Hoten.Contains(txtTimKiem.Text.Trim()) || n.KhachHang.SoDienThoai.Contains(txtTimKiem.Text.Trim()));
            foreach (var item in listDDH)
            {
                collection.Add(item.MaHD);
                collection.Add(item.KhachHang.Hoten);
                collection.Add(item.KhachHang.SoDienThoai);
            }
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiem.AutoCompleteCustomSource = collection;
        }


        private void LoadDataDonHang() {
            dataHoaDon.DataSource = from a in db.HoaDons
                                    select new
                                    {
                                        a.MaHD,
                                        a.NgayBan,
                                        a.MaKH,
                                        a.MaNV,
                                        a.TongTien
                                    };
        }

        private void fillGrid_All()
        {
            var load = from a in db.HoaDons
                       where a.MaHD.Contains(txtTimKiem.Text.Trim()) || a.KhachHang.Hoten.Contains(txtTimKiem.Text.Trim()) || a.KhachHang.SoDienThoai.Contains(txtTimKiem.Text.Trim())
                       select new
                       {
                           a.MaHD,
                           a.NgayBan,
                           a.MaKH,
                           a.MaNV,
                           a.TongTien
                       };
            dataHoaDon.DataSource = load;
        }

        private void FrmDonHang_Load(object sender, EventArgs e)
        {
            LoadDataDonHang();
            BindingData();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            fillGrid_All();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadDataDonHang();
        }

        private void fillGrid_ByDatetime()
        {
            var fildatetime = dateTimePicker1.Text;
            var load = from a in db.HoaDons
                       where a.NgayBan.Value.Date.Equals(fildatetime)
                       select new
                       {
                           a.MaHD,
                           a.NgayBan,
                           a.MaKH,
                           a.MaNV,
                           a.TongTien
                       };
            dataHoaDon.DataSource = load;
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
            xuatfileExcel(dataHoaDon, @"C:\Users\admin\Desktop\", "fileThongKe");
            MessageBox.Show("Xuất file thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
