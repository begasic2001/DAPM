using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanHang
{
    class Connection
    {
        public static SqlConnection connect = 
            new SqlConnection(@"Data Source=DESKTOP-IVSSD64\MINHTHANH;Initial Catalog=PTPM_QLBanHang;Integrated Security=True");
    }
}
