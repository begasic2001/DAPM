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
    public partial class FrmBanHang : Form
    {

        DBQuanLyBanHangDataContext db = new DBQuanLyBanHangDataContext();
        decimal thanhTien = 0;
        Receipt obj;

        public FrmBanHang()
        {
            InitializeComponent();
        }

        private void LoadComboMaKH()
        {
            cbMaKH.DataSource = from a in db.KhachHangs select a.MaKH;
            cbMaKH.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadComboSanPham()
        {
            cbTenSP.DataSource = from a in db.SanPhams select a.TenSP;
            cbTenSP.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void FrmBanHang_Load(object sender, EventArgs e)
        {
            LoadComboMaKH();
            LoadComboSanPham();
            receiptBindingSource.DataSource = new List<Receipt>(); 
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSoLuong.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Vui lòng nhập số lượng sản phẩm ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!Model.checkIsDigit(txtSoLuong.Text.Trim()))
                {
                    MessageBox.Show("Số lượng sản phẩm không hợp lệ. Vui lòng thử lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    var maSP = db.SanPhams.SingleOrDefault(n => n.TenSP.Equals(cbTenSP.Text)).MaSP;
                    var soLuongMua = int.Parse(txtSoLuong.Text.Trim());

                    var checkSL = db.SanPhams.SingleOrDefault(n => n.TenSP.Equals(cbTenSP.Text)).SoLuongTon;

                    if (soLuongMua <= 0 || soLuongMua > checkSL)
                    {
                        MessageBox.Show("Số lượng hàng trong kho không đủ, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        obj = new Receipt()
                        {
                            MaSP = maSP,
                            TenSP = cbTenSP.Text,
                            SoLuong = soLuongMua,
                            DonGia = decimal.Parse(txtDonGia.Text.Trim()),
                        };
                        receiptBindingSource.Add(obj);
                        thanhTien += obj.SoLuong * obj.DonGia;
                        total.Text = String.Format("{0:0,0}", thanhTien) + " VNĐ";
                        txtSoLuong.Clear();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void cbMaKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tenKH = db.KhachHangs.SingleOrDefault(n => n.MaKH.Equals(int.Parse(cbMaKH.Text))).Hoten;
            txtTenKH.Text = tenKH;
        }

        private void cbTenSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            var price = db.SanPhams.SingleOrDefault(n => n.TenSP.Equals(cbTenSP.Text)).DonGia;
            txtDonGia.Text = price.ToString();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            try
            {
                Receipt obj = receiptBindingSource.Current as Receipt;
                if (obj != null)
                {
                    thanhTien -= obj.DonGia * obj.SoLuong;
                    if (thanhTien.Equals(0))
                    {
                        total.Text = "";
                    }
                    else
                    {
                        total.Text = String.Format("{0:0,0}", thanhTien) + " VNĐ";
                    }
                }
                receiptBindingSource.RemoveCurrent();
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnHoanTat_Click(object sender, EventArgs e)
        {
            try
            {
                if (total.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Chưa có bất kì sản phẩm nào trong đơn hàng. Vui lòng kiểm tra lại ! ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (MessageBox.Show("Bạn đã in hoá đơn chưa. Chọn Yes nếu bạn đã in hoá đơn rồi!", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (cbMaKH.DataSource == null)
                        {
                            MessageBox.Show("Vui lòng thêm thông tin khách hàng trước khi thanh toán ! ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        else if (cbTenSP.DataSource == null)
                        {
                            MessageBox.Show("Kho đã hết hàng. Vui lòng kiểm tra lại kho ! ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        else
                        {
                            HoaDon hd = new HoaDon();
                            var letter = txtTenKH.Text.Trim().Substring(0, 1);
                            var sdtKH = db.KhachHangs.SingleOrDefault(n => n.Hoten.Equals(txtTenKH.Text)).SoDienThoai;
                            Random _r = new Random();
                            var temp_1 = _r.Next(0, 99999);
                            var temp_2 = "";
                            if (sdtKH.Trim().Length.Equals(10))
                            {
                                temp_2 += sdtKH.Trim().Substring(7, 3);
                            }
                            else
                            {
                                temp_2 += sdtKH.Trim().Substring(8, 3);
                            }
                            var maHD = letter + temp_1 + temp_2;
                            hd.MaHD = maHD;
                            hd.NgayBan = DateTime.Now;
                            hd.MaKH = int.Parse(cbMaKH.Text);
                            var lstNV = db.NhanViens;
                            if (lstNV.Count() == 0)
                            {
                                MessageBox.Show("Không có nhân viên nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                var ma = "";
                                foreach (var i in lstNV.Take(1))
                                {
                                    ma += i.MaNV;
                                }
                                hd.MaNV = String.IsNullOrEmpty(Model.maNV) ? ma : Model.maNV;
                                hd.TongTien = thanhTien;
                                db.HoaDons.InsertOnSubmit(hd);
                                db.SubmitChanges();
                                foreach (var item in receiptBindingSource.DataSource as List<Receipt>)
                                {
                                    ChiTietHoaDon cthd = new ChiTietHoaDon();
                                    cthd.MaHD = maHD;
                                    cthd.MaSP = item.MaSP;
                                    cthd.TenSP = item.TenSP;
                                    cthd.SoLuong = item.SoLuong;
                                    cthd.DonGia = item.DonGia;
                                    // Lấy sản phẩm trong database
                                    SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP.Equals(item.MaSP));
                                    // Cập nhật số lượng tồn trong database
                                    sp.SoLuongTon -= item.SoLuong;
                                    // Tăng số lần đã bán của sản phẩm
                                    sp.DaBan += item.SoLuong;
                                    db.ChiTietHoaDons.InsertOnSubmit(cthd);
                                }
                                db.SubmitChanges();
                                txtSoLuong.Clear();
                                total.Text = "";
                                thanhTien = 0;
                                MessageBox.Show("Giao dịch hoàn tất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                receiptBindingSource.Clear();
                            }

                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (total.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Chưa có bất kì sản phẩm nào trong đơn hàng. Vui lòng kiểm tra lại ! ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    using (PrintHoaDon hd = new PrintHoaDon(receiptBindingSource.DataSource as List<Receipt>, String.Format("{0:0,0}", thanhTien), DateTime.Now.ToString(), txtTenKH.Text))
                    {
                        hd.ShowDialog();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
    }
}
