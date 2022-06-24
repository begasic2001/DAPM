using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang
{
    public partial class FrmQLSanPham : Form
    {

        DBQuanLyBanHangDataContext db = new DBQuanLyBanHangDataContext();
        Bitmap imgDefault = Properties.Resources._default;
        AutoCompleteStringCollection collection = new AutoCompleteStringCollection();

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
            dataSanPham.DataSource = load;
        }
        public FrmQLSanPham()
        {
            InitializeComponent();
        }

        private void LoadDataSanPham()
        {
            dataSanPham.DataSource = from a in db.SanPhams
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
        private void FrmQLSanPham_Load(object sender, EventArgs e)
        {
            LoadDataSanPham();
            LoadComboTenLoai();
            LoadComboNSX();
            cbTenLoai.DropDownStyle = ComboBoxStyle.DropDownList;
            cbNSX.DropDownStyle = ComboBoxStyle.DropDownList;
            pictureBox1.Image = imgDefault;
            BindingData();
        }

        private void LoadComboTenLoai()
        {
            cbTenLoai.DataSource = from a in db.LoaiSanPhams select a.TenLoaiSP;
        }

        private void LoadComboNSX()
        {
            cbNSX.DataSource = from a in db.NhaSanXuats select a.TenNSX;
        }

        private void btnChon_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Model.imgChoose = "";
                    Model.path = dlg.FileName;
                    pictureBox1.Image = Image.FromFile(Model.path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void ClearTXT()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtDonGia.Clear();
            txtSLT.Clear();
            txtDaBan.Clear();
            txtTenSP.Focus();
            Model.path = "";
            Model.imgChoose = "";
            pictureBox1.Image = imgDefault;
        }




        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTenSP.Text.Trim().Length.Equals(0) || txtDonGia.Text.Trim().Length.Equals(0) || txtSLT.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (!Model.checkIsLetter(txtTenSP.Text.Trim()))
                    {
                        MessageBox.Show("Tên hàng không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTenSP.Focus();
                    }
                    else if (!Model.checkIsDigit(txtDonGia.Text))
                    {
                        MessageBox.Show("Đơn giá không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDonGia.Focus();
                    }
                    else if (decimal.Parse(txtDonGia.Text.ToString()) <= 0)
                    {
                        MessageBox.Show("Đơn giá không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDonGia.Focus();
                    }
                    else if (!Model.checkIsDigit(txtSLT.Text))
                    {
                        MessageBox.Show("Số lượng tồn không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSLT.Focus();
                    }
                    else if (int.Parse(txtSLT.Text.ToString()) <= 0)
                    {
                        MessageBox.Show("Số lượng tồn không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSLT.Focus();
                    }
                    else if (pictureBox1.Image == imgDefault)
                    {
                        MessageBox.Show("Chưa chọn hình ảnh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        SanPham checkTenSP = db.SanPhams.SingleOrDefault(n => n.TenSP.Equals(txtTenSP.Text.Trim()));
                        if (checkTenSP != null)
                        {
                            MessageBox.Show("Đã có sản phẩm " + txtTenSP.Text.ToString().ToLower() + " trong kho. Vui lòng chọn sản phẩm khác!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            SanPham sp = new SanPham();
                            sp.TenSP = txtTenSP.Text.Trim();
                            sp.DonGia = decimal.Parse(txtDonGia.Text.Trim().ToString());
                            if (System.IO.File.Exists(Model.pathDefault + Path.GetFileName(Model.path)) || System.IO.File.Exists(Model.imgChoose))
                            {
                                MessageBox.Show("Đã có hình ảnh này trong danh mục rồi. Vui lòng chọn hình ảnh khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                File.Copy(Model.path, Path.Combine(Model.pathDefault, Path.GetFileName(Model.path)), true);
                                sp.HinhAnh = Model.pathDefault + Path.GetFileName(Model.path);
                                sp.SoLuongTon = int.Parse(txtSLT.Text.Trim().ToString());
                                sp.DaBan = 0;
                                var query1 = db.NhaSanXuats.SingleOrDefault(n => n.TenNSX.Equals(cbNSX.Text)).MaNSX;
                                sp.MaNSX = query1;
                                var query2 = db.LoaiSanPhams.SingleOrDefault(n => n.TenLoaiSP.Equals(cbTenLoai.Text)).MaLoaiSP;
                                sp.MaLoaiSP = query2;
                                db.SanPhams.InsertOnSubmit(sp);
                                db.SubmitChanges();
                                MessageBox.Show("Thêm sản phẩm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDataSanPham();
                                ClearTXT();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearTXT();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaSP.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (MessageBox.Show("Bạn có chắc chắn muốn xoá sản phẩm này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP.Equals(int.Parse(txtMaSP.Text)));
                        if (sp == null)
                        {
                            MessageBox.Show("Vui lòng chọn sản phẩm cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            var ctpn = db.ChiTietPhieuNhaps.Where(n => n.MaSP.Equals(int.Parse(txtMaSP.Text)));
                            foreach (var item in ctpn)
                            {
                                db.ChiTietPhieuNhaps.DeleteOnSubmit(item);
                                db.SubmitChanges();
                            }
                            db.SubmitChanges();
                            db.SanPhams.DeleteOnSubmit(sp);
                            db.SubmitChanges();
                            MessageBox.Show("Đã xoá 1 sản phẩm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearTXT();
                            LoadDataSanPham();
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaSP.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (txtTenSP.Text.Trim().Length.Equals(0) || txtDonGia.Text.Trim().Length.Equals(0) || txtSLT.Text.Trim().Length.Equals(0))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (!Model.checkIsLetter(txtTenSP.Text.Trim()))
                    {
                        MessageBox.Show("Tên hàng không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTenSP.Focus();
                    }
                    else if (!Model.checkIsDigit(txtDonGia.Text))
                    {
                        MessageBox.Show("Đơn giá không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDonGia.Focus();
                    }
                    else if (decimal.Parse(txtDonGia.Text.ToString()) <= 0)
                    {
                        MessageBox.Show("Đơn giá không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDonGia.Focus();
                    }
                    else if (!Model.checkIsDigit(txtSLT.Text))
                    {
                        MessageBox.Show("Số lượng tồn không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSLT.Focus();
                    }
                    else if (int.Parse(txtSLT.Text.ToString()) <= 0)
                    {
                        MessageBox.Show("Số lượng tồn không hợp lệ. Vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSLT.Focus();
                    }
                    else if (pictureBox1.Image == imgDefault)
                    {
                        MessageBox.Show("Chưa chọn hình ảnh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP.Equals(txtMaSP.Text.Trim()));
                        sp.TenSP = txtTenSP.Text.Trim();
                        sp.DonGia = decimal.Parse(txtDonGia.Text.Trim().ToString());
                        if (!String.IsNullOrEmpty(Model.path))
                        {
                            File.Copy(Model.path, Path.Combine(Model.pathDefault, Path.GetFileName(Model.path)), true);
                            sp.HinhAnh = Model.pathDefault + Path.GetFileName(Model.path);
                            sp.SoLuongTon = int.Parse(txtSLT.Text.Trim().ToString());
                            sp.DaBan = int.Parse(txtDaBan.Text.Trim());
                            var query1 = db.NhaSanXuats.SingleOrDefault(n => n.TenNSX.Equals(cbNSX.Text)).MaNSX;
                            sp.MaNSX = query1;
                            var query2 = db.LoaiSanPhams.SingleOrDefault(n => n.TenLoaiSP.Equals(cbTenLoai.Text)).MaLoaiSP;
                            sp.MaLoaiSP = query2;
                            db.SubmitChanges();
                            MessageBox.Show("Sửa thông tin sản phẩm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataSanPham();
                            ClearTXT();
                        }
                        else
                        {
                            sp.SoLuongTon = int.Parse(txtSLT.Text.Trim().ToString());
                            sp.DaBan = int.Parse(txtDaBan.Text.Trim());
                            var query1 = db.NhaSanXuats.SingleOrDefault(n => n.TenNSX.Equals(cbNSX.Text)).MaNSX;
                            sp.MaNSX = query1;
                            var query2 = db.LoaiSanPhams.SingleOrDefault(n => n.TenLoaiSP.Equals(cbTenLoai.Text)).MaLoaiSP;
                            sp.MaLoaiSP = query2;
                            db.SubmitChanges();
                            MessageBox.Show("Sửa thông tin sản phẩm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataSanPham();
                            ClearTXT();
                        }
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

        private void dataSanPham_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataSanPham.SelectedRows)
                {
                    txtMaSP.Text = dr.Cells[0].Value.ToString();
                    txtTenSP.Text = dr.Cells[1].Value.ToString();
                    var img = db.SanPhams.SingleOrDefault(n => n.MaSP.Equals(txtMaSP.Text.Trim())).HinhAnh;
                    Model.imgChoose = img;
                    pictureBox1.Image = Image.FromFile(img);
                    txtDonGia.Text = dr.Cells[2].Value.ToString();
                    txtSLT.Text = dr.Cells[3].Value.ToString();
                    txtDaBan.Text = dr.Cells[4].Value.ToString();
                    var tennsx = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX.Equals(dr.Cells[5].Value.ToString())).TenNSX;
                    cbNSX.Text = tennsx.ToString();
                    var tenloai = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP.Equals(dr.Cells[6].Value.ToString())).TenLoaiSP;
                    cbTenLoai.Text = tenloai.ToString();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '"+ ex.Message + "'","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
        }

        private void dataSanPham_KeyUp_1(object sender, KeyEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataSanPham.SelectedRows)
                {
                    txtMaSP.Text = dr.Cells[0].Value.ToString();
                    txtTenSP.Text = dr.Cells[1].Value.ToString();
                    var img = db.SanPhams.SingleOrDefault(n => n.MaSP.Equals(txtMaSP.Text.Trim())).HinhAnh;
                    Model.imgChoose = img;
                    pictureBox1.Image = Image.FromFile(img);
                    txtDonGia.Text = dr.Cells[2].Value.ToString();
                    txtSLT.Text = dr.Cells[3].Value.ToString();
                    txtDaBan.Text = dr.Cells[4].Value.ToString();
                    var tennsx = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX.Equals(dr.Cells[5].Value.ToString())).TenNSX;
                    cbNSX.Text = tennsx.ToString();
                    var tenloai = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP.Equals(dr.Cells[6].Value.ToString())).TenLoaiSP;
                    cbTenLoai.Text = tenloai.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void dataSanPham_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataSanPham.SelectedRows)
                {
                    txtMaSP.Text = dr.Cells[0].Value.ToString();
                    txtTenSP.Text = dr.Cells[1].Value.ToString();
                    var img = db.SanPhams.SingleOrDefault(n => n.MaSP.Equals(txtMaSP.Text.Trim())).HinhAnh;
                    Model.imgChoose = img;
                    pictureBox1.Image = Image.FromFile(img);
                    txtDonGia.Text = dr.Cells[2].Value.ToString();
                    txtSLT.Text = dr.Cells[3].Value.ToString();
                    txtDaBan.Text = dr.Cells[4].Value.ToString();
                    var tennsx = db.NhaSanXuats.SingleOrDefault(n => n.MaNSX.Equals(dr.Cells[5].Value.ToString())).TenNSX;
                    cbNSX.Text = tennsx.ToString();
                    var tenloai = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP.Equals(dr.Cells[6].Value.ToString())).TenLoaiSP;
                    cbTenLoai.Text = tenloai.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
