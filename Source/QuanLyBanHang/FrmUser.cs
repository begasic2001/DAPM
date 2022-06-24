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
    public partial class FrmUser : Form
    {

        DBQuanLyBanHangDataContext db = new DBQuanLyBanHangDataContext();

        public FrmUser()
        {
            InitializeComponent();
        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmQLKhachHang qlkh = new FrmQLKhachHang();
            qlkh.ShowDialog();
        }

        private void iconPictureBox4_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmQLSanPham qlsp = new FrmQLSanPham();
            qlsp.ShowDialog();
        }

        private void iconPictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmQLLoaiSanPham qllsp = new FrmQLLoaiSanPham();
            qllsp.ShowDialog();
        }

        private void iconPictureBox5_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmQLNhaSanXuat qlnsx = new FrmQLNhaSanXuat();
            qlnsx.ShowDialog();
        }

        private void iconPictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmBanHang frmbanhang = new FrmBanHang();
            frmbanhang.ShowDialog();
        }

        private void iconPictureBox8_Click(object sender, EventArgs e)
        {
            try
            {
                if (Model.maNV != null)
                {
                    if (MessageBox.Show("Bạn có muốn đăng xuất không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        TaiKhoan tk = db.TaiKhoans.SingleOrDefault(n => n.MaNV.Equals(Model.maNV));
                        tk.TrangThai = "Offline";
                        Model.maNV = null;
                        db.SubmitChanges();
                        this.Hide();
                        FrmDangNhap frmlogin = new FrmDangNhap();
                        frmlogin.ShowDialog();
                    }
                }
                if (Model.maAM != null)
                {
                    this.Hide();
                    FrmThongKe frmtk = new FrmThongKe();
                    frmtk.ShowDialog();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Đã có lỗi: '" + ex.Message + "'. Vui lòng kiểm tra lại đi bạn !!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void iconPictureBox7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
