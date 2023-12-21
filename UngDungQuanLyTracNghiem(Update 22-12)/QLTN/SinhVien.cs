using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace QLTN
{
    public partial class SinhVien : Form
    {
        //Tạo kết nối sql:
        SqlConnection connsql = Connect.Instance.GetConnection();
        public SinhVien()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
       

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void SinhVien_Load(object sender, EventArgs e)
        {
            loadData();
        }
        private void loadData()
        {
            // Connect to the database
            Connect.Instance.GetConnection();
            string sql = "SELECT * FROM NguoiDung";
            SqlCommand cmd = new SqlCommand(sql, connsql);

            // Execute the SELECT statement
            SqlDataReader reader = cmd.ExecuteReader();

            // Add data to the DataGridView
            while (reader.Read())
            {
                dataGridView1.Rows.Add(reader["username"].ToString(), reader["password"].ToString(), reader["HoTen"].ToString(), reader["Email"].ToString());
            }

            // Close the database connection and data reader
            Connect.Instance.CloseConnection();
            reader.Close();
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            string tk = txtCauA.Text;
            string mk = txtCauB.Text;
            string hoten = txtCauC.Text;
            string email = txtCauD.Text;
            // Check if the username or D is already in use
            string sql = "SELECT COUNT(*) FROM NguoiDung WHERE username = @tk";
            SqlCommand cmd = new SqlCommand(sql, connsql);
            cmd.Parameters.AddWithValue("@tk", tk);
            Connect.Instance.GetConnection();
            int count = (int)cmd.ExecuteScalar();
            Connect.Instance.CloseConnection();

            if (count > 0)
            {
                if (count == 1)
                {
                    MessageBox.Show("ID đã được sử dụng!", "Thông báo", MessageBoxButtons.OK);
                }
                return;
            }

            // Check if the password and password confirmation match

            // Proceed with registration
            sql = "INSERT INTO NguoiDung (username, password, HoTen, Email) VALUES (@tk, @mk, @hoten, @email);";
            cmd = new SqlCommand(sql, connsql);
            cmd.Parameters.AddWithValue("@tk", tk);
            cmd.Parameters.AddWithValue("@mk", mk);
            cmd.Parameters.AddWithValue("@hoten", hoten);
            cmd.Parameters.AddWithValue("@email", email);

            Connect.Instance.GetConnection();
            cmd.ExecuteNonQuery();
            dataGridView1.Rows.Add(txtCauA.Text, txtCauB.Text, txtCauC.Text, txtCauD.Text);
            txtCauA.Clear();
            txtCauB.Clear();
            txtCauC.Clear();
            txtCauD.Clear();
            Connect.Instance.CloseConnection();
            MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK);
        }

        private void txtCauA_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCauB_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCauC_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCauD_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string tk = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                string sql = "DELETE FROM NguoiDung WHERE username = @tk";
                SqlCommand cmd = new SqlCommand(sql, connsql);
                cmd.Parameters.AddWithValue("@tk", tk);

                Connect.Instance.GetConnection();
                cmd.ExecuteNonQuery();
                Connect.Instance.CloseConnection();

                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);

                MessageBox.Show("Xóa câu hỏi thành công!", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn câu hỏi cần xóa!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            QuanLy ql = new QuanLy();
            ql.ShowDialog();
        }
    }
}
