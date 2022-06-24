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
    public partial class FrmQLLoaiSanPham : Form
    {

        DBQuanLyBanHangDataContext db = new DBQuanLyBanHangDataContext();

        AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
        private void BindingData()
        {
            var listLoaiSP = db.LoaiSanPhams.Where(n => n.TenLoaiSP.Contains(txtTimKiem.Text));
            foreach (var item in listLoaiSP)
            {
                collection.Add(item.TenLoaiSP);
            }
            txtTimKiem.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtTimKiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTimKiem.AutoCompleteCustomSource = collection;
        }

        private void fillGrid()
        {
            var load = from a in db.LoaiSanPhams
                       where a.TenLoaiSP.Contains(txtTimKiem.Text)
                       select new
                       {
                           a.MaLoaiSP,
                           a.TenLoaiSP
                       };
            dataLoaiSanPham.DataSource = load;
        }

        public FrmQLLoaiSanPham()
        {
            InitializeComponent();
        }

        private void LoadDataLoaiSP()
        {
            var load = from a in db.LoaiSanPhams
                       select new
                       {
                           a.MaLoaiSP,
                           a.TenLoaiSP
                       };
            dataLoaiSanPham.DataSource = load;
        }


        private void FrmQLLoaiSanPham_Load(object sender, EventArgs e)
        {
            LoadDataLoaiSP();
            BindingData();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            fillGrid();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTenLoai.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng nhập tên loại hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (!Model.checkIsLetter(txtTenLoai.Text))
                {
                    MessageBox.Show("Tên loại hàng không hợp lệ. Vui lòng nhập lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenLoai.Focus();
                }
                else
                {
                    LoaiSanPham checkTenL = db.LoaiSanPhams.SingleOrDefault(n => n.TenLoaiSP.Equals(txtTenLoai.Text));
                    if (checkTenL != null)
                    {
                        MessageBox.Show("Tên loại hàng này đã tồn tại trong danh mục. Vui lòng kiểm tra lại!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        LoaiSanPham l = new LoaiSanPham();
                        l.TenLoaiSP = txtTenLoai.Text.Trim();
                        db.LoaiSanPhams.InsertOnSubmit(l);
                        db.SubmitChanges();
                        MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataLoaiSP();
                        txtTenLoai.Text = "";
                        txtTenLoai.Focus();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaTL.Text.Trim().Length.Equals(0))
            {
                MessageBox.Show("Vui lòng chọn loại hàng cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xoá loại hàng này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    LoaiSanPham l = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP.Equals(txtMaTL.Text));
                    if (l == null)
                    {
                        MessageBox.Show("Vui lòng chọn loại hàng cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        db.LoaiSanPhams.DeleteOnSubmit(l);
                        db.SubmitChanges();
                        MessageBox.Show("Xoá thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataLoaiSP();
                        txtTenLoai.Text = "";
                        txtTenLoai.Focus();
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaTL.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng chọn tên thể loại cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (txtTenLoai.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng nhập tên loại hàng cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (!Model.checkIsLetter(txtTenLoai.Text))
                {
                    MessageBox.Show("Tên loại hàng không hợp lệ. Vui lòng nhập lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenLoai.Focus();
                }
                else
                {
                    LoaiSanPham l = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP.Equals(txtMaTL.Text));
                    if (l == null)
                    {
                        MessageBox.Show("Mã loại hàng không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        l.TenLoaiSP = txtTenLoai.Text.Trim();
                        db.SubmitChanges();
                        MessageBox.Show("Sửa thông tin loại hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataLoaiSP();
                        txtTenLoai.Text = "";
                        txtTenLoai.Focus();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void txtTenLoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
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

        private void dataLoaiSanPham_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataLoaiSanPham.SelectedRows)
                {
                    txtMaTL.Text = dr.Cells[0].Value.ToString();
                    txtTenLoai.Text = dr.Cells[1].Value.ToString();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}
