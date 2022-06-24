using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanHang
{
    public partial class Model
    {
        public static string maNV;

        public static string maAM;

        public static string path;

        public static string pathDefault = @"D:\workspace-hk2\Do_An_Phan_Mem\Quanlybanhang-main\Source\QuanLyBanHang\Resources\";

        public static string imgChoose;

        public static bool checkPhoneNumber(string phoneNumber)
        {
            var flag = false;
            if (phoneNumber != "")
            {
                var firstNumber = phoneNumber.Substring(0, 2);
                if (firstNumber == "09" || firstNumber == "08" || firstNumber == "07" || firstNumber == "06" || firstNumber == "05" || firstNumber == "04" || firstNumber == "03")
                {
                    if (phoneNumber.Length == 10)
                    {
                        flag = true;
                    }
                }
                else if (firstNumber == "01" || firstNumber == "02")
                {
                    if (phoneNumber.Length == 11)
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }


        public static bool checkIsLetter(string text)
        {
            var flag = false;
            foreach (char ch in text)
            {
                if (char.IsLetter(ch))
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }

        public static bool checkIsDigit(string text)
        {
            var flag = false;
            foreach (char ch in text)
            {
                if (char.IsDigit(ch))
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }
    }
}
