using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace QLTN
{
    public partial class BatDauThi : Form
    {


        //Tạo biến thứ tự câu hỏi
        int Thutu = 1;
        public int Phut;
        public int giay = 1;
        private int SoLuongCauHoi;
        private int score = 0;
        private void HienThiSoCauHoi()
        {
            int soLuongCauHoi = DemSoCauHoi();
            MessageBox.Show($"Số câu hỏi trong database: {soLuongCauHoi}", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public BatDauThi(string tk)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            lbl_MSSV.Text = tk;
        }

        private void BatDauThi_Load(object sender, EventArgs e)
        {
            SoLuongCauHoi = DemSoCauHoi();
            //HienThiSoCauHoi();
            ReadOnly();
            //Đồng hồ
            Phut = 60;
            timer1.Start();
            //Load câu 1:
            CauHoi(Thutu);
            
        }
        //Đếm số câu hỏi:
        public int DemSoCauHoi()
        {
            int soLuongCauHoi = 0;
            string sql = "SELECT COUNT(*) FROM cauhoi";

            SqlConnection connsql = Connect.Instance.GetConnection(); 
                try
                {
                    using (SqlCommand command = new SqlCommand(sql, connsql))
                    {
                        soLuongCauHoi = (int)command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            Connect.Instance.CloseConnection();
            return soLuongCauHoi;
        }
        private void ReadOnly()
        {
            txt_A.ReadOnly = true;
            txt_B.ReadOnly = true;
            txt_C.ReadOnly = true;
            txt_D.ReadOnly = true;
            txt_CauHoi.ReadOnly = true;
        }
        //Đồng hồ
        private void timer1_Tick(object sender, EventArgs e)
        {
            giay = giay - 1;
            lbl_Giay.Text = giay.ToString();
            if (giay == 0)
            {
                Phut = Phut - 1;
                lbl_Phut.Text = Phut.ToString();
                giay = 60;
            }
            if (Phut == 0)
            {
                timer1.Stop();
                MessageBox.Show("Hết thời gian làm bài.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                lbl_Phut.Text = "00";
                lbl_Giay.Text = "00";
                btn_Nop_Click(sender, e);
            }
        }
        //Load câu hỏi
        private void CauHoi(int i)
        {
            SqlConnection connsql = Connect.Instance.GetConnection(); 
            string sql = "SELECT * from cauhoi WHERE ID = " + i;
            SqlCommand cmd = new SqlCommand(sql, connsql);
            SqlDataReader dta = cmd.ExecuteReader();
            if (dta.HasRows)
            {
                dta.Read();
                string Id = dta["ID"].ToString();
                string CauHoi = dta["CauHoi"].ToString();
                string CauA = dta["CauA"].ToString();
                string CauB = dta["CauB"].ToString();
                string CauC = dta["CauC"].ToString();
                string CauD = dta["CauD"].ToString();
                lbl_ID.Text = Id;
                txt_CauHoi.Text = CauHoi;
                txt_A.Text = CauA;
                txt_B.Text = CauB;
                txt_C.Text = CauC;
                txt_D.Text = CauD;
                dta.Close();
            }
            Connect.Instance.CloseConnection();
        }
        private void btn_Nop_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_Truoc_Click(object sender, EventArgs e)
        {
            try
            {
                if(Thutu <= 1)
                {
                    MessageBox.Show("Đây là câu đầu tiên");
                }
                else
                {
                    Thutu--;
                    ClearRadioButtons();
                    CauHoi(Thutu);
                }
            }
            catch
            { }
        }

        private void btn_Sau_Click(object sender, EventArgs e)
        {
            try
            {
                if (Thutu > SoLuongCauHoi - 1)
                {
                    MessageBox.Show("Đây là câu cuối cùng");
                }
                else
                {
                    
                    ClearRadioButtons();
                    Thutu++;
                    CauHoi(Thutu);
                }
            }
            catch
            { }
        }
        //Chấm điểm:
        private List<string> dapAnNguoiDung = new List<string>();
        private void LuuDapAnNguoiDung()
        {
            string dapAn = GetUserAnswer();
            dapAnNguoiDung.Add(dapAn);
            dapAnNguoiDung.RemoveAll(answer => string.IsNullOrWhiteSpace(answer));
        }
        private void rbtn_A_CheckedChanged(object sender, EventArgs e)
        {
            LuuDapAnNguoiDung();
        }

        private void rbtn_B_CheckedChanged(object sender, EventArgs e)
        {
            LuuDapAnNguoiDung();
        }

        private void rbtn_C_CheckedChanged(object sender, EventArgs e)
        {
            LuuDapAnNguoiDung();
        }

        private void rbtn_D_CheckedChanged(object sender, EventArgs e)
        {
            LuuDapAnNguoiDung();
        }
        private List<string> LayDapAnDungTuCSDL()
        {
            List<string> dapAnDung = new List<string>();

            try 
            {
                SqlConnection connection = Connect.Instance.GetConnection();
                {
                    // Thực hiện truy vấn SQL để lấy đáp án đúng từ cơ sở dữ liệu
                    string query = "SELECT DA FROM cauhoi";
                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Lấy giá trị đáp án từ cột DapAn trong CSDL
                            string dapAn = reader["DA"].ToString();
                            dapAnDung.Add(dapAn);
                        }
                    }
                }
                Connect.Instance.CloseConnection();
            }
            catch (Exception ex)
            {
                // Xử lý exception nếu có
                MessageBox.Show($"Lỗi khi lấy đáp án từ CSDL: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dapAnDung;
        }
        private void TinhDiem()
        {
            List<string> dapAnDung = LayDapAnDungTuCSDL();

            // Kiểm tra từng câu hỏi
            for (int i = 0; i <= dapAnNguoiDung.Count && i < dapAnDung.Count; i++)
            {
                if (dapAnNguoiDung[i] == dapAnDung[i])
                {
                    score++; // Tăng điểm nếu đúng
                }
            }
        }

        private string GetUserAnswer()
        {
            if (rbtn_A.Checked) return "A";
            if (rbtn_B.Checked) return "B";
            if (rbtn_C.Checked) return "C";
            if (rbtn_D.Checked) return "D";
            return "";
        }
        private void ClearRadioButtons()
        {
            rbtn_A.Checked = false;
            rbtn_B.Checked = false;
            rbtn_C.Checked = false;
            rbtn_D.Checked = false;
        }
        private void HienThiThongBaoDiem()
        {
            List<string> dapAnDung = LayDapAnDungTuCSDL();

            MessageBox.Show($"Điểm của bạn là: {score}/{SoLuongCauHoi}", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            string selectedAnswerString = string.Join(", ", dapAnNguoiDung);
            string selectedAnswerString2 = string.Join(", ", dapAnDung);
            // Hiển thị thông báo sử dụng MessageBox.
            MessageBox.Show("Các đáp án đã chọn: " + selectedAnswerString + "\n Các đáp án đúng: " + selectedAnswerString2, "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            

        }
        private void nopbai_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn nộp bài  không?", "Xác nhận", MessageBoxButtons.YesNo);
            {
                TinhDiem();
                HienThiThongBaoDiem();
                this.Close();
            }
        }

        private void cbb_DsCauHoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbb_DsCauHoi.Items.Clear();
            string[] dt = { "Đã trả lời", "Chưa trả lời", "Tất cả câu" };
            foreach (string s in dt)
            {
                cbb_DsCauHoi.Items.Add(s);
            }
        }

    }
}

