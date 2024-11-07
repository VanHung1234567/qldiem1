using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace qlDiem
{
    public partial class TrangChu : Form
    {
        public TrangChu()
        {
            InitializeComponent();
        }

        private Form currentFormChild;

        private void OpenChildForm(Form childForm, string title)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }
            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel3.Controls.Add(childForm);
            panel3.Tag = childForm;
            panel3.AutoScroll = true;

            childForm.BringToFront();
            childForm.Show();

            lblTittle.Text = title;
        }

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }
            lblTittle.Text = "Trang Chủ";
        }

        private void btnSinhVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(new SinhVien(), "Sinh Viên");

        }

        private void btnMonHoc_Click(object sender, EventArgs e)
        {
            OpenChildForm(new MonHoc(), "Môn Học");
        }

        private void btnDauDiem_Click(object sender, EventArgs e)
        {

        }

        private void btnLopHoc_Click(object sender, EventArgs e)
        {

        }

        private void btnDiemSV_Click(object sender, EventArgs e)
        {

        }

        private void btnDieuKien_Click(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TrangChu_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn muốn đóng giao diện không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
