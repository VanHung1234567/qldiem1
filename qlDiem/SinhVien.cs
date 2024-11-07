using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace qlDiem
{
    public partial class SinhVien : Form
    {
        private string connString = "Data Source=.;Initial Catalog=qlDiem1;Integrated Security=True";
        private DataTable dt;

        public SinhVien()
        {
            InitializeComponent();
            dt = new DataTable();
            cbxGender.SelectedIndex = 0;
        }

        private void LoadData(string searchCondition = "")
        {
            dataGridView1.AutoGenerateColumns = false;
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                string sql = "SELECT * FROM Students WHERE IsDeleted = 0" + searchCondition;
                SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlConn);
                dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private DataTable GetStudentData(string query)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConn);
                adapter.Fill(dataTable);
            }
            return dataTable;
        }


        void LoadMajor()
        {
            string query = "SELECT DISTINCT MajorID FROM Majors"; 
            DataTable dtMajors = GetStudentData(query);
            cbxMajorID.DataSource = dtMajors;
            cbxMajorID.ValueMember = "MajorID";
            cbxMajorID.DisplayMember = "MajorID";
        }



        private string GenerateStudentCode()
        {
            Random random = new Random();
            string studentCode = "SV" + random.Next(100000, 999999).ToString();
            return studentCode;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtStudentCode.Text = GenerateStudentCode();

            if (ValidateFields())
            {
                try
                {
                    using (SqlConnection sqlConn = new SqlConnection(connString))
                    {
                        sqlConn.Open();
                        string gender = cbxGender.Text == "Nam" ? "M" : "F";
                        string sql = "INSERT INTO Students (StudentID, FirstName, LastName, StudentCode, Email, Phone, MajorID, Gender, Address, IdentityNumber, Cohort, DOB, Note, IsDeleted) " +
                                     "VALUES (@StudentID, @FirstName, @LastName, @StudentCode, @Email, @Phone, @MajorID, @Gender, @Address, @IdentityNumber, @Cohort, @DOB, @Note, 0)";
                        SqlCommand cmd = new SqlCommand(sql, sqlConn);
                        cmd.Parameters.AddWithValue("@StudentID", txtStudentID.Text);
                        cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@StudentCode", txtStudentCode.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                        cmd.Parameters.AddWithValue("@MajorID", cbxMajorID.SelectedValue);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@IdentityNumber", txtIdentityNumber.Text);
                        cmd.Parameters.AddWithValue("@Cohort", txtCohort.Text);
                        cmd.Parameters.AddWithValue("@DOB", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@Note", txtNote.Text);
                        cmd.ExecuteNonQuery();
                        LoadData();
                        MessageBox.Show("Thêm sinh viên thành công!");
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                try
                {
                    using (SqlConnection sqlConn = new SqlConnection(connString))
                    {
                        sqlConn.Open();
                        string gender = cbxGender.Text == "Nam" ? "M" : "F";
                        string sql = "UPDATE Students SET FirstName = @FirstName, LastName = @LastName, StudentCode = @StudentCode, " +
                                     "Email = @Email, Phone = @Phone, MajorID = @MajorID, Gender = @Gender, Address = @Address, " +
                                     "IdentityNumber = @IdentityNumber, Cohort = @Cohort, DOB = @DOB, Note = @Note " +
                                     "WHERE StudentID = @StudentID";
                        SqlCommand cmd = new SqlCommand(sql, sqlConn);
                        cmd.Parameters.AddWithValue("@StudentID", txtStudentID.Text);
                        cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@StudentCode", txtStudentCode.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                        cmd.Parameters.AddWithValue("@MajorID", cbxMajorID.SelectedValue);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@IdentityNumber", txtIdentityNumber.Text);
                        cmd.Parameters.AddWithValue("@Cohort", txtCohort.Text);
                        cmd.Parameters.AddWithValue("@DOB", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@Note", txtNote.Text);
                        cmd.ExecuteNonQuery();
                        LoadData();
                        MessageBox.Show("Cập nhật sinh viên thành công!");
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtStudentID.Text))
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection sqlConn = new SqlConnection(connString))
                    {
                        sqlConn.Open();
                        string sql = "UPDATE Students SET IsDeleted = 1 WHERE StudentID = @StudentID";
                        SqlCommand cmd = new SqlCommand(sql, sqlConn);
                        cmd.Parameters.AddWithValue("@StudentID", txtStudentID.Text);
                        cmd.ExecuteNonQuery();
                        LoadData();
                        MessageBox.Show("Xóa sinh viên thành công!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên để xóa.");
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string searchCondition = "";
            if (!string.IsNullOrEmpty(txtStudentID.Text))
            {
                searchCondition = $" AND StudentID = '{txtStudentID.Text}'";
            }
            LoadData(searchCondition);
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(txtStudentID.Text) || string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(txtLastName.Text) || string.IsNullOrEmpty(txtPhone.Text) ||
                string.IsNullOrEmpty(txtAddress.Text) || string.IsNullOrEmpty(txtEmail.Text) || cbxMajorID.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show("Email không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtPhone.Text.Length != 10 || !txtPhone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại phải là 10 chữ số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtIdentityNumber.Text.Length != 12 || !txtIdentityNumber.Text.All(char.IsDigit))
            {
                MessageBox.Show("CCCD phải là chuỗi số 12 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        private void SinhVien_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadMajor();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtStudentID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txtFirstName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtLastName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                txtStudentCode.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                txtEmail.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                txtPhone.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                cbxMajorID.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                

                string gender = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                cbxGender.SelectedItem = gender == "M" ? "Nam" : "Nữ";  

                txtAddress.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                txtIdentityNumber.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString();
                txtCohort.Text = dataGridView1.CurrentRow.Cells[10].Value.ToString();
                var dateCellValue = dataGridView1.CurrentRow.Cells[11].Value;
                if (dateCellValue != null && dateCellValue != DBNull.Value)
                {
                    dateTimePicker1.Value = Convert.ToDateTime(dateCellValue);
                }
                else
                {
                    dateTimePicker1.Value = DateTime.Now;  
                }
                txtNote.Text = dataGridView1.CurrentRow.Cells[12].Value.ToString();
            }
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
