using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTN
{
    internal class Connect
    {
        public static string TenMay = "LAPTOP-H34EM8I7\\SQLEXPRESS";
        public static string TenCSDL = "QLTN";
        public static string connectstring = $@"Data Source={TenMay};Initial Catalog={TenCSDL};Integrated Security=True";
        public SqlConnection connsql;
        public static Connect instance;
        public Connect()
        {

            connsql = new SqlConnection(connectstring);
        }
        public Connect(string strcm)
        {
            connsql = new SqlConnection(strcm);
        }

        public static Connect Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Connect();
                }
                return instance;
            }
        }
        public SqlConnection GetConnection()
        {
            // Mở kết nối trước khi trả về
            if (connsql.State == System.Data.ConnectionState.Closed)
            {
                connsql.Open();
            }
            return connsql;
        }

        public void CloseConnection()
        {
            // Đóng kết nối khi không sử dụng nữa
            if (connsql.State == System.Data.ConnectionState.Open)
            {
                connsql.Close();
            }
        }
    }
}
