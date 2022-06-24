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
    public partial class FrmQLNhanVien : Form
    {

        DBQuanLyBanHangDataContext db = new DBQuanLyBanHangDataContext();

        AutoCompleteStringCollection collection = new AutoCompleteStringCollection();

        private void BindingData()
        {
            var listNV = db.NhanViens.Where(n => n.HoTen.Contains(txtTimKiem.Text));
            foreach (var item in listNV)
            {
                collection.Add(item.HoTen);
            }
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiem.AutoCompleteCustomSource = collection;
        }

        private void fillGrid()
        {
            var load = from a in db.NhanViens
                       where a.HoTen.Contains(txtTimKiem.Text)
                       select new
                       {
                           a.MaNV,
                           a.HoTen,
                           a.NgaySinh,
                           a.DiaChi,
                           a.ChucVu,
                           a.Luong,
                           a.SoDienThoai,
                       };
            dataNhanVien.DataSource = load;
        }

        public FrmQLNhanVien()
        {
            InitializeComponent();
        }

        private void LoadDataNhanVien()
        {
            var load = from a in db.NhanViens
                       select new
                       {
                           a.MaNV,
                           a.HoTen,
                           a.NgaySinh,
                           a.DiaChi,
                           a.ChucVu,
                           a.Luong,
                           a.SoDienThoai,
                       };
            dataNhanVien.DataSource = load;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = true;
        }

        private void ClearTxt()
        {
            txtMaNV.Clear();
            txtTenNV.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            txtChucVu.Clear();
            txtLuong.Clear();
            txtTenNV.Focus();
        }
        

        private void FrmQLNhanVien_Load(object sender, EventArgs e)
        {
            LoadDataNhanVien();
            BindingData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTenNV.Text.Trim().Length.Equals(0) || txtDiaChi.Text.Trim().Length.Equals(0) || txtSDT.Text.Trim().Length.Equals(0) || txtSDT.Text.Length.Equals(1) || txtChucVu.Text.Trim().Length.Equals(0) || txtLuong.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (dtNgaySinh.Value.Date >= DateTime.Now.Date)
                    {
                        MessageBox.Show("Ngày sinh không hợp lệ vui lòng thử lại!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (!Model.checkPhoneNumber(txtSDT.Text))
                    {
                        MessageBox.Show("Số điện thoại không đúng định dạng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        NhanVien nv = new NhanVien();
                        Random _r = new Random();
                        var temp1 = txtTenNV.Text.Trim().Substring(0, 1);
                        var temp2 = _r.Next(0, 99999);
                        var temp3 = "";
                        if (txtSDT.Text.Trim().Length.Equals(10))
                        {
                            temp3 += txtSDT.Text.Trim().Substring(7, 3);
                        }
                        else
                        {
                            temp3 += txtSDT.Text.Trim().Substring(8, 3);
                        }
                        var manv = temp1 + temp2 + temp3;
                        nv.MaNV = manv.ToString();
                        nv.HoTen = txtTenNV.Text.Trim();
                        nv.DiaChi = txtDiaChi.Text;
                        nv.NgaySinh = DateTime.Parse(dtNgaySinh.Value.ToShortDateString());
                        nv.SoDienThoai = txtSDT.Text;
                        nv.ChucVu = txtChucVu.Text.Trim();
                        nv.Luong = int.Parse(txtLuong.Text.Trim());
                        db.NhanViens.InsertOnSubmit(nv);
                        db.SubmitChanges();
                        MessageBox.Show("Thêm nhân viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearTxt();
                        LoadDataNhanVien();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xoá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xoá nhân viên này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    NhanVien nv = db.NhanViens.SingleOrDefault(n => n.MaNV.Equals(txtMaNV.Text.Trim()));
                    if (nv == null)
                    {
                        MessageBox.Show("Vui lòng chọn nhân viên cần xoá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        db.NhanViens.DeleteOnSubmit(nv);
                        db.SubmitChanges();
                        MessageBox.Show("Xoá thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearTxt();
                        LoadDataNhanVien();
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaNV.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (txtTenNV.Text.Trim().Length.Equals(0) || txtDiaChi.Text.Trim().Length.Equals(0) || txtSDT.Text.Trim().Length.Equals(0) || txtChucVu.Text.Trim().Length.Equals(0) || txtLuong.Text.Trim().Length.Equals(0))
                    {
                        MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (!Model.checkPhoneNumber(txtSDT.Text))
                    {
                        MessageBox.Show("Số điện thoại không đúng định dạng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        NhanVien nv = db.NhanViens.SingleOrDefault(n => n.MaNV.Equals(txtMaNV.Text.Trim()));
                        nv.HoTen = txtTenNV.Text.Trim();
                        nv.DiaChi = txtDiaChi.Text.Trim();
                        nv.NgaySinh = DateTime.Parse(dtNgaySinh.Value.ToShortDateString());
                        nv.SoDienThoai = txtSDT.Text.Trim();
                        nv.ChucVu = txtChucVu.Text.Trim();
                        nv.Luong = int.Parse(txtLuong.Text.Trim());
                        db.SubmitChanges();
                        MessageBox.Show("Sửa thông tin nhân viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearTxt();
                        LoadDataNhanVien();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void txtTenNV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            ClearTxt();
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

        private void dataNhanVien_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataNhanVien.SelectedRows)
                {
                    txtMaNV.Text = dr.Cells[0].Value.ToString();
                    txtTenNV.Text = dr.Cells[1].Value.ToString();
                    dtNgaySinh.Text = dr.Cells[2].Value.ToString();
                    txtSDT.Text = dr.Cells[6].Value.ToString();
                    txtDiaChi.Text = dr.Cells[3].Value.ToString();
                    txtChucVu.Text = dr.Cells[4].Value.ToString();
                    txtLuong.Text = dr.Cells[5].Value.ToString();
                }
                btnThem.Enabled = false;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}
