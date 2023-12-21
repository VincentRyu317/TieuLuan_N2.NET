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

namespace QLTN
{
    public partial class BatDauThi : Form
    {


        //Tạo biến thứ tự câu hỏi
        int Thutu = 1;

        public BatDauThi(string tk)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            lbl_MSSV.Text = tk;
        }

        private void BatDauThi_Load(object sender, EventArgs e)
        {
           
            //Đồng hồ
            Phut = 60;
            timer1.Start();
            //Load câu 1:
            CauHoi(Thutu);
            
        }
        private int _phut;

        public int Phut
        {
            get { return _phut; }
            set { _phut = value; }
        }
        public int giay = 1;
        private string _mathisinh;

        public string Mathisinh
        {
            get { return _mathisinh; }
            set { _mathisinh = value; }
        }
        private string _tenmonthi;

        public string Tenmonthi
        {
            get { return _tenmonthi; }
            set { _tenmonthi = value; }
        }
        private int _thoigianthi;

        public int Thoigianthi
        {
            get { return _thoigianthi; }
            set { _thoigianthi = value; }
        }
        private string _MAMONHOC;

        public string MAMONHOC
        {
            get { return _MAMONHOC; }
            set { _MAMONHOC = value; }
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
                string CauHoi = dta["CauHoi"].ToString();
                string CauA = dta["CauA"].ToString();
                string CauB = dta["CauB"].ToString();
                string CauC = dta["CauC"].ToString();
                string CauD = dta["CauD"].ToString();
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
                txt_CauHoi.Clear();
                txt_A.Clear();
                txt_B.Clear();
                txt_C.Clear();
                txt_D.Clear();
                if(Thutu <= 1)
                {
                    MessageBox.Show("Đây là câu đầu tiên");
                }
                else
                {
                    Thutu--;
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
                txt_CauHoi.Clear();
                txt_A.Clear();
                txt_B.Clear();
                txt_C.Clear();
                txt_D.Clear();
                if (Thutu > 40)
                {
                    MessageBox.Show("Đây là câu cuối cùng");
                }
                else
                {
                    Thutu++;
                    CauHoi(Thutu);
                }
            }
            catch
            { }
        }

        private void lbl_HoTen_Click(object sender, EventArgs e)
        {

        }

        private void lbl_MSSV_Click(object sender, EventArgs e)
        {

        }

        private void nopbai_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn nộp bài  không?", "Xác nhận", MessageBoxButtons.YesNo);
            {
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
    //
}

