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
    public partial class FrmQLNhaSanXuat : Form
    {

        DBQuanLyBanHangDataContext db = new DBQuanLyBanHangDataContext();

        AutoCompleteStringCollection collection = new AutoCompleteStringCollection();

        private void BindingData()
        {
            var listNSX = db.NhaSanXuats.Where(n => n.TenNSX.Contains(txtTimKiem.Text));
            foreach (var item in listNSX)
            {
                collection.Add(item.TenNSX);
            }
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiem.AutoCompleteCustomSource = collection;
        }

        private void fillGrid()
        {
            var load = from a in db.NhaSanXuats
                       where a.TenNSX.Contains(txtTimKiem.Text)
                       select new
                       {
                           a.MaNSX,
                           a.TenNSX,
                           a.DiaChi,
                           a.SDT
                       };
            dataNhaSanXuat.DataSource = load;
        }

        public FrmQLNhaSanXuat()
        {
            InitializeComponent();
        }

        private void ClearTXT()
        {
            txtMaNSX.Clear();
            txtNSX.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            txtNSX.Focus();
        }

        private void LoadDataNSX() {
            dataNhaSanXuat.DataSource = from a in db.NhaSanXuats select new {
                a.MaNSX,
                a.TenNSX,
                a.DiaChi,
                a.SDT
            };
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                NhaSanXuat checkTenNSX = db.NhaSanXuats.SingleOrDefault(n => n.TenNSX.Equals(txtNSX.Text));
                if (txtNSX.Text.Trim().Length.Equals(0) || txtDiaChi.Text.Trim().Length.Equals(0) || txtSDT.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhà sản xuất", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!Model.checkIsLetter(txtNSX.Text))
                {
                    MessageBox.Show("Tên nhà sản xuất không hợp lệ. Vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNSX.Focus();
                }
                else if (!Model.checkPhoneNumber(txtSDT.Text.Trim()))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                }
                else if (checkTenNSX != null)
                {
                    MessageBox.Show("Đã tồn tại nhà sản xuất này trong danh mục. Vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    NhaSanXuat nsx = new NhaSanXuat();
                    nsx.TenNSX = txtNSX.Text.Trim();
                    nsx.DiaChi = txtDiaChi.Text.Trim();
                    nsx.SDT = txtSDT.Text.Trim();
                    db.NhaSanXuats.InsertOnSubmit(nsx);
                    db.SubmitChanges();
                    MessageBox.Show("Thêm nhà sản xuất thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataNSX();
                    ClearTXT();
                    txtNSX.Focus();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void FrmQLNhaSanXuat_Load(object sender, EventArgs e)
        {
            LoadDataNSX();
            this.ActiveControl = txtNSX;
            BindingData();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaNSX.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn nhà sản xuất cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xoá nhà sản xuất này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    NhaSanXuat nsx = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX.Equals(int.Parse(txtMaNSX.Text.ToString())));
                    if (nsx == null)
                    {
                        MessageBox.Show("Vui lòng chọn nhà sản xuất cần xoá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        db.NhaSanXuats.DeleteOnSubmit(nsx);
                        db.SubmitChanges();
                        MessageBox.Show("Xoá nhà sản xuất thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataNSX();
                        ClearTXT();
                        txtNSX.Focus();
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNSX.Text.Trim().Length.Equals(0) || txtDiaChi.Text.Trim().Length.Equals(0) || txtSDT.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhà sản xuất", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!Model.checkIsLetter(txtNSX.Text))
                {
                    MessageBox.Show("Tên nhà sản xuất không hợp lệ. Vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNSX.Focus();
                }
                else
                {
                    NhaSanXuat nsx = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX.Equals(int.Parse(txtMaNSX.Text.Trim())));
                    nsx.TenNSX = txtNSX.Text.Trim();
                    nsx.DiaChi = txtDiaChi.Text.Trim();
                    nsx.SDT = txtSDT.Text.Trim();
                    db.SubmitChanges();
                    MessageBox.Show("Sửa thông tin nhà sản xuất thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataNSX();
                    ClearTXT();
                    txtNSX.Focus();
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

        private void dataNhaSanXuat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataNhaSanXuat.SelectedRows)
                {
                    txtMaNSX.Text = dr.Cells[0].Value.ToString();
                    txtNSX.Text = dr.Cells[1].Value.ToString();
                    txtDiaChi.Text = dr.Cells[2].Value.ToString();
                    txtSDT.Text = dr.Cells[3].Value.ToString();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}
