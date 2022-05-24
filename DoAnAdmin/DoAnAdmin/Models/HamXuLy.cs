using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnAdmin.Models
{
    public class HamXuLy
    {
        KetNoi conn = new KetNoi();
        //dbQuanLyBanLaptopDataContext db = new dbQuanLyBanLaptopDataContext();
        QL_LaptopEntities db = new QL_LaptopEntities();
        //Tạo mã sản phẩm
        public string createIDProducts()
        {
            string prodIDLast;
            try
            {
                //Lấy mã cuối cùng
                prodIDLast = db.Products.OrderByDescending(p => p.id).Select(p => p.id).First().ToString();
            }
            catch (Exception)
            {
                return "LAP000000";
            }
            //Ép thành Int
            int IDInt = int.Parse(prodIDLast.Substring(3, 6));
            //Thực hiện tăng dần
            string ID = "LAP";
            if (IDInt >= 0 && IDInt < 9)
                ID += "00000" + (IDInt + 1);
            else if (IDInt >= 9 && IDInt < 99)
                ID += "0000" + (IDInt + 1);
            else if (IDInt >= 99 && IDInt < 999)
                ID += "000" + (IDInt + 1);
            else if (IDInt >= 999 && IDInt < 9999)
                ID += "00" + (IDInt + 1);
            else if (IDInt >= 9999 && IDInt < 99999)
                ID += "0" + (IDInt + 1);

            return ID;
        }
        //Tạo mã Phiếu nhập
        public string createIDPhieuNhap()
        {
            string PNIDLast;
            string dateNow = DateTime.Now.ToShortDateString(); //lấy theo định dạng DD/MM/YYYY
            dateNow = dateNow.Replace("/", "");
            string ID = "";
            try
            {
                //Lấy mã cuối cùng
                PNIDLast = db.PhieuNhaps.OrderByDescending(p => p.id.Substring(10,6)).Select(p => p.id).First().ToString();
                //Ép thành Int
                int IDInt = int.Parse(PNIDLast.Substring(10, 6));
                //Thực hiện tăng dần
                ID = dateNow + "PN";
                if (IDInt >= 0 && IDInt < 9)
                    ID += "00000" + (IDInt + 1);
                else if (IDInt >= 9 && IDInt < 99)
                    ID += "0000" + (IDInt + 1);
                else if (IDInt >= 99 && IDInt < 999)
                    ID += "000" + (IDInt + 1);
                else if (IDInt >= 999 && IDInt < 9999)
                    ID += "00" + (IDInt + 1);
                else if (IDInt >= 9999 && IDInt < 99999)
                    ID += "0" + (IDInt + 1);

            }
            catch (Exception)
            {
                return dateNow + "PN000000";
            }
           
            return ID;
        }
        //Tạo mã Phiếu xuất
        public string createIDPhieuXuat()
        {
            string PXIDLast;
            string dateNow = DateTime.Now.ToShortDateString(); //lấy theo định dạng DD/MM/YYYY
            dateNow = dateNow.Replace("/", "");
            string ID = "";
            try
            {
                //Lấy mã cuối cùng
                PXIDLast = db.PhieuXuats.OrderByDescending(p => p.id.Substring(11, 6)).Select(p => p.id).First().ToString();
                //Ép thành Int
                int IDInt = int.Parse(PXIDLast.Substring(10, 6));
                //Thực hiện tăng dần
                ID = dateNow + "PX";
                if (IDInt >= 0 && IDInt < 9)
                    ID += "00000" + (IDInt + 1);
                else if (IDInt >= 9 && IDInt < 99)
                    ID += "0000" + (IDInt + 1);
                else if (IDInt >= 99 && IDInt < 999)
                    ID += "000" + (IDInt + 1);
                else if (IDInt >= 999 && IDInt < 9999)
                    ID += "00" + (IDInt + 1);
                else if (IDInt >= 9999 && IDInt < 99999)
                    ID += "0" + (IDInt + 1);

            }
            catch (Exception)
            {
                return dateNow + "PX000000";
            }

            return ID;
        }
        //Tạo mã Khách hàng
        public string createIDKH()
        {
            string IDLast;
            string ID = "";
            try
            {
                //Lấy mã cuối cùng
                IDLast = db.Customers.OrderByDescending(p => p.id.Substring(2, 10)).Select(p => p.id).First().ToString();
                //Ép thành Int
                int IDInt = int.Parse(IDLast.Substring(2, 10));
                //Thực hiện tăng dần
                ID = "KH";
                if (IDInt >= 0 && IDInt < 9)
                    ID += "000000000" + (IDInt + 1);
                else if (IDInt >= 9 && IDInt < 99)
                    ID += "00000000" + (IDInt + 1);
                else if (IDInt >= 99 && IDInt < 999)
                    ID += "0000000" + (IDInt + 1);
                else if (IDInt >= 999 && IDInt < 9999)
                    ID += "000000" + (IDInt + 1);
                else if (IDInt >= 9999 && IDInt < 99999)
                    ID += "00000" + (IDInt + 1);
                else if (IDInt >= 99999 && IDInt < 999999)
                    ID += "0000" + (IDInt + 1);
                else if (IDInt >= 99999 && IDInt < 999999)
                    ID += "000" + (IDInt + 1);
                else if (IDInt >= 99999 && IDInt < 999999)
                    ID += "00" + (IDInt + 1);
                else if (IDInt >= 99999 && IDInt < 999999)
                    ID += "0" + (IDInt + 1);

            }
            catch (Exception)
            {
                return "KH0000000000";
            }
            return ID;
        }
        public bool kiemTraKhoaChinhOnCTPN(string idPN, string prod_id)
        {
            ChiTietPhieuNhap ctpn;
            try
            {
                ctpn = db.ChiTietPhieuNhaps.Single(t => t.PhieuNhap_id == idPN && t.product_id == prod_id);
            }
            catch (Exception)
            {
                ctpn = null;
            }

            return (ctpn != null) ? true : false;// true: đã tồn tại, false else
        }
        public bool kiemTraKhoaChinhOnImage(string ProdID, string url)
        {
            Image img;
            try
            {
                img = db.Images.Single(t => t.product_id == ProdID && t.image_ == url);
            }
            catch (Exception)
            {
                img = null;
            }

            return (img != null) ? true : false;// true: đã tồn tại, false else
        }


        //Kiểm tra ngày 2 phải lớn hơn ngày 1=> true: s2>s1 ,
        public bool checkDate1AndDate2(string s1, string s2)//Input: MM/DD/YYYY
        {
            string[] date1 = s1.Split('/');
            int nam1 = Convert.ToInt32(date1[2]);
            int thang1 = Convert.ToInt32(date1[0]);
            int ngay1 = Convert.ToInt32(date1[1]);

            string[] date2 = s2.Split('/');
            int nam2 = Convert.ToInt32(date2[2]);
            int thang2 = Convert.ToInt32(date2[0]);
            int ngay2 = Convert.ToInt32(date2[1]);

            if (nam2 > nam1)
            {
                return true;
            }
            else if (nam2 < nam1)
            {
                return false;
            }
            else
            {
                if (thang2 > thang1)
                {
                    return true;
                }
                else if (thang2 < thang1)
                {
                    return false;
                }
                else
                {
                    if (ngay2 >= ngay1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        //Kiểm tra ngày hợp lệ

        public bool kTraNamNhuan(int nYear)
        {
           return ((nYear % 4 == 0 && nYear % 100 != 0) || nYear % 400 == 0);
        }

        // Hàm trả về số ngày trong tháng thuộc năm cho trước
        public int tinhSoNgayTrongThang(int nMonth, int nYear)
        {
            int nNumOfDays = 0;
            switch (nMonth)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    nNumOfDays = 31;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    nNumOfDays = 30;
                    break;
                case 2:
                    if (kTraNamNhuan(nYear))
                    {
                        nNumOfDays = 29;
                    }
                    else
                    {
                        nNumOfDays = 28;
                    }
                    break;
            }

            return nNumOfDays;
        }

        // Hàm kiểm tra ngày dd/mm/yyyy cho trước có phải là ngày hợp lệ
        public bool kTraNgayHopLe(int nDay, int nMonth, int nYear)
        {
            // Kiểm tra năm
            if (nYear < 0)
            {
                return false; // Ngày không còn hợp lệ nữa!
            }

            // Kiểm tra tháng
            if (nMonth < 1 || nMonth > 12)
            {
                return false; // Ngày không còn hợp lệ nữa!
            }

            // Kiểm tra ngày
            if (nDay < 1 || nDay > tinhSoNgayTrongThang(nMonth, nYear))
            {
                return false; // Ngày không còn hợp lệ nữa!
            }

            return true; // Trả về trạng thái cuối cùng hợp lệ...
        }

        public bool isNumber(string strNumber)
        {
            return strNumber.All(char.IsDigit); 
        }
    }
}