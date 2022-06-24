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
    public partial class FrmQLKhachHang : Form
    {

        DBQuanLyBanHangDataContext db = new DBQuanLyBanHangDataContext();

        AutoCompleteStringCollection collection = new AutoCompleteStringCollection();

        private void BindingData()
        {
            var listKH = db.KhachHangs.Where(n => n.Hoten.Contains(txtTimKiem.Text));
            foreach (var item in listKH)
            {
                collection.Add(item.Hoten);
            }
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiem.AutoCompleteCustomSource = collection;
        }


        private void fillGrid()
        {
            var load = from a in db.KhachHangs
                       where a.Hoten.Contains(txtTimKiem.Text)
                       select new
                       {
                           a.MaKH,
                           a.Hoten,
                           a.NgaySinh,
                           a.SoDienThoai,
                           a.DiaChi
                       };
            dataKhachHang.DataSource = load;
        }

        public FrmQLKhachHang()
        {
            InitializeComponent();
        }

        private void ClearTxt()
        {
            txtMaKH.Clear();
            txtTenKH.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            txtTenKH.Focus();
        }

        private void LoadDataKhachHang() {
            dataKhachHang.DataSource = from a in db.KhachHangs select new {
                a.MaKH,
                a.Hoten,
                a.NgaySinh,
                a.SoDienThoai,
                a.DiaChi
            };
        }

        private void FrmQLKhachHang_Load(object sender, EventArgs e)
        {
            LoadDataKhachHang();
            BindingData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTenKH.Text.Trim().Length.Equals(0) || txtDiaChi.Text.Trim().Length.Equals(0) || txtSDT.Text.Trim().Length.Equals(0) || txtSDT.Text.Length.Equals(1))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (!Model.checkIsLetter(txtTenKH.Text))
                    {
                        MessageBox.Show("Tên khách hàng không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (dtNgaySinh.Value.Date >= DateTime.Now.Date)
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
                        KhachHang kh = new KhachHang();
                        kh.Hoten = txtTenKH.Text.Trim();
                        kh.DiaChi = txtDiaChi.Text;
                        kh.NgaySinh = DateTime.Parse(dtNgaySinh.Value.ToShortDateString());
                        kh.SoDienThoai = txtSDT.Text;
                        db.KhachHangs.InsertOnSubmit(kh);
                        db.SubmitChanges();
                        MessageBox.Show("Thêm khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearTxt();
                        LoadDataKhachHang();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void txtTenKH_KeyPress(object sender, KeyPressEventArgs e)
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

        private decimal TotalOrder(string maKH)
        {
            decimal total = 0;
            var check = db.HoaDons.Where(n => n.MaKH.Equals(int.Parse(maKH.ToString())));
            if (check.Count() == 0)
            {
                return total;
            }
            else {
                total = (decimal)check.Sum(n => n.TongTien);
            }
            return total;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaKH.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xoá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xoá khách hàng này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH.Equals(int.Parse(txtMaKH.Text.Trim())));
                    if (kh == null)
                    {
                        MessageBox.Show("Vui lòng chọn khách hàng cần xoá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        db.KhachHangs.DeleteOnSubmit(kh);
                        db.SubmitChanges();
                        MessageBox.Show("Xoá thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearTxt();
                        LoadDataKhachHang();
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaKH.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng chọn khách hàng cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (txtTenKH.Text.Trim().Length.Equals(0) || txtDiaChi.Text.Trim().Length.Equals(0) || txtSDT.Text.Trim().Length.Equals(0))
                    {
                        MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (dtNgaySinh.Value.Date >= DateTime.Now.Date)
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
                        KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH.Equals(int.Parse(txtMaKH.Text.ToString())));
                        kh.Hoten = txtTenKH.Text.Trim();
                        kh.DiaChi = txtDiaChi.Text.Trim();
                        kh.NgaySinh = DateTime.Parse(dtNgaySinh.Value.ToShortDateString());
                        kh.SoDienThoai = txtSDT.Text.Trim();
                        db.SubmitChanges();
                        MessageBox.Show("Sửa thông tin khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearTxt();
                        LoadDataKhachHang();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            fillGrid();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearTxt();
        }

        private void iconPictureBox8_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmUser fuser = new FrmUser();
            fuser.ShowDialog();
        }

        private void iconPictureBox7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void dataKhachHang_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lbDHGanNhat.Text = "";
                foreach (DataGridViewRow dr in dataKhachHang.SelectedRows)
                {
                    txtMaKH.Text = dr.Cells[0].Value.ToString();
                    txtTenKH.Text = dr.Cells[1].Value.ToString();
                    dtNgaySinh.Text = dr.Cells[2].Value.ToString();
                    txtSDT.Text = dr.Cells[3].Value.ToString();
                    txtDiaChi.Text = dr.Cells[4].Value.ToString();
                }
                var countOrder = db.HoaDons.Where(n => n.MaKH.Equals(txtMaKH.Text)).Count();
                lbTongDonHang.Text = countOrder.ToString();
                var total = TotalOrder(txtMaKH.Text);
                lbTongThanhTien.Text = String.Format("{0:0,0}", total) + " VNĐ";
                var lastOrder = (db.HoaDons.Where(n => n.MaKH.Equals(int.Parse(txtMaKH.Text.ToString()))).OrderByDescending(n => n.NgayBan)).Take(1);
                foreach (var item in lastOrder)
                {
                    lbDHGanNhat.Text = item.NgayBan.Value.ToShortDateString();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}
