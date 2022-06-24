using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanLyBanHang
{
    public partial class FrmNhapKho : Form
    {
        public FrmNhapKho()
        {
            InitializeComponent();
        }
        DBQuanLyBanHangDataContext db = new DBQuanLyBanHangDataContext();
        Bitmap imgDefault = Properties.Resources._default;
        AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
        SqlConnection con = Connection.connect;



        private void BindingData()
        {
            var listSP = db.SanPhams.Where(n => n.TenSP.Contains(txtTimKiem.Text));
            foreach (var item in listSP)
            {
                collection.Add(item.TenSP);
            }
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiem.AutoCompleteCustomSource = collection;
        }
        private void fillGrid()
        {
            var load = from a in db.SanPhams
                       where a.TenSP.Contains(txtTimKiem.Text)
                       select new
                       {
                           a.MaSP,
                           a.TenSP,
                           a.DonGia,
                           a.SoLuongTon,
                           a.DaBan,
                           a.MaNSX,
                           a.MaLoaiSP
                       };
            dataNhapKho.DataSource = load;
        }

        private void ChartSanPhamHetHang() {
            DataSet ds = new DataSet();
            con.Open();
            chart1.Series["Số lượng còn lại"].ChartType = SeriesChartType.Pie;
            SqlDataAdapter adapt = new SqlDataAdapter("select TenSP,SoLuongTon from SanPham where SoLuongTon <= 30", con);
            adapt.Fill(ds);
            chart1.DataSource = ds;
            chart1.Series["Số lượng còn lại"].XValueMember = "TenSP";
            chart1.Series["Số lượng còn lại"].YValueMembers = "SoLuongTon";
            chart1.Series["Số lượng còn lại"].IsValueShownAsLabel = true;
            con.Close();
        }

        private void FrmNhapKho_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = imgDefault;
            cbNSX.DropDownStyle = ComboBoxStyle.DropDownList;
            BindingData();
            LoadDataSanPham();
            LoadComboNSX();
            ChartSanPhamHetHang();
        }


        private void LoadComboNSX() {
            cbNSX.DataSource = from a in db.NhaSanXuats select a.TenNSX;
        }

        private void LoadDataSanPham()
        {
            dataNhapKho.DataSource = from a in db.SanPhams where a.SoLuongTon <= 30
                                     select new
                                     {
                                         a.MaSP,
                                         a.TenSP,
                                         a.DonGia,
                                         a.SoLuongTon,
                                         a.DaBan,
                                         a.MaNSX,
                                         a.MaLoaiSP
                                     };
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            fillGrid();
        }

        private void iconPictureBox8_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void iconPictureBox7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Clear() {
            txtSoLuongNhap.Clear();
            txtDonGiaNhap.Clear();
            txtMaSP.Clear();
            pictureBox1.Image = imgDefault;
            txtSoLuongNhap.Focus();
        }

        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaSP.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần nhập hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (txtSoLuongNhap.Text.Trim().Length.Equals(0))
                    {
                        MessageBox.Show("Vui lòng nhập số lượng cần nhập hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoLuongNhap.Focus();
                    }
                    else if (!Model.checkIsDigit(txtSoLuongNhap.Text.Trim()))
                    {
                        MessageBox.Show("Số lượng nhập không hợp lệ. Vui lòng nhập lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoLuongNhap.Focus();
                    }
                    else if (int.Parse(txtSoLuongNhap.Text.Trim()) <= 0)
                    {
                        MessageBox.Show("Số lượng nhập không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoLuongNhap.Focus();
                    }
                    else if (txtDonGiaNhap.Text.Trim().Length.Equals(0))
                    {
                        MessageBox.Show("Vui lòng nhập đơn giá nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDonGiaNhap.Focus();
                    }
                    else if (!Model.checkIsDigit(txtDonGiaNhap.Text.Trim()))
                    {
                        MessageBox.Show("Đơn giá nhập không hợp lệ!. Vui lòng nhập lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDonGiaNhap.Focus();
                    }
                    else if (decimal.Parse(txtDonGiaNhap.Text.Trim()) <= 0)
                    {
                        MessageBox.Show("Đơn giá nhập không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDonGiaNhap.Focus();
                    }
                    else
                    {
                        var maNSX = db.NhaSanXuats.SingleOrDefault(n => n.TenNSX.Equals(cbNSX.Text)).MaNSX;
                        PhieuNhap pn = new PhieuNhap();
                        pn.MaNSX = maNSX;
                        pn.NgayNhap = DateTime.Now;
                        db.PhieuNhaps.InsertOnSubmit(pn);
                        db.SubmitChanges();
                        ChiTietPhieuNhap ctpn = new ChiTietPhieuNhap();
                        ctpn.MaPN = pn.MaPN;
                        SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP.Equals(int.Parse(txtMaSP.Text)));
                        ctpn.MaSP = int.Parse(txtMaSP.Text);
                        ctpn.DonGiaNhap = decimal.Parse(txtDonGiaNhap.Text.Trim());
                        ctpn.SoLuongNhap = int.Parse(txtSoLuongNhap.Text.Trim());
                        sp.SoLuongTon += int.Parse(txtSoLuongNhap.Text.Trim());
                        db.ChiTietPhieuNhaps.InsertOnSubmit(ctpn);
                        db.SubmitChanges();
                        MessageBox.Show("Nhập hàng cho sản phẩm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Clear();
                        LoadDataSanPham();
                        ChartSanPhamHetHang();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnChiTietPN_Click(object sender, EventArgs e)
        {
            FrmChiTietPhieuNhap frmctpn = new FrmChiTietPhieuNhap();
            frmctpn.ShowDialog();
        }

        private void dataNhapKho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataNhapKho.SelectedRows)
                {
                    txtMaSP.Text = dr.Cells[0].Value.ToString();
                    var img = db.SanPhams.SingleOrDefault(n => n.MaSP.Equals(txtMaSP.Text.Trim())).HinhAnh;
                    pictureBox1.Image = Image.FromFile(img);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadDataSanPham();
            Clear();
        }
    }
}
