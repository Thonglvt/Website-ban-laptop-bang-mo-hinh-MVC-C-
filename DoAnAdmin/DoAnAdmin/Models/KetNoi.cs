using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace DoAnAdmin.Models
{
    public class KetNoi
    {
        //Khai báo chuỗi kết nối
        private static string serverName = @"MMT-LEVANTHONG\SQLEXPRESS";
        private static string database = "QL_Laptop";
        private static string id = "sa";
        private static string pass = "123";

        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }
        public string Database
        {
            get { return database; }
            set { database = value; }
        }
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Pass
        {
            get { return pass; }
            set { pass = value; }
        }


        SqlConnection Connect = new SqlConnection(@"Data Source=" + serverName + ";"
                                                  + "Initial Catalog=" + database + ";"
                                                  + "User ID =" + id + ";"
                                                  + "Password =" + pass);
        //SqlConnection Connect = new SqlConnection(@"Data Source=" + serverName + ";"
        //                                         + "Initial Catalog=" + database + ";"
        //                                         + "Integrated Security = True");
        public void open()
        {
            if (Connect.State == ConnectionState.Closed)
            {
                 Connect.Open();
            }
        }
        public void close()
        {
            if (Connect.State == ConnectionState.Open)
            {
                Connect.Close();
            }
        }
        //Hàm load dữ liệu từ CSDL
        public DataTable Select_Data(string sql)
        {
            open();
            SqlCommand cmd = new SqlCommand(sql, Connect);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();//Chứa dữ liệu bảng
            ad.Fill(dt);
            close();
            return dt;
        }
        //Hàm load dữ liệu từ CSDL có sử dụng procedure (tên procedure là tham số truyền vào)
        public DataTable Select_DataUseProcedure(string sql)
        {
            open();
            SqlCommand cmd = new SqlCommand(sql, Connect);
            cmd.CommandType = CommandType.StoredProcedure;//Gọi procedure
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();//Chứa dữ liệu bảng
            ad.Fill(dt);
            close();
            return dt;
        }

        //Hàm load dữ liệu từ CSDL với điều kiện
        //->sql: câu lệnh query,
        //->name: mảng chuỗi gồm tên cột,
        //->values: điều kiện của từng tên cột,
        //->parameter: số lượng parameter (số cột điều kiện)
        public DataTable Select_DataWithParameter(string sql, string[] name, object[] values, int parameter)
        {
            open();
            SqlCommand cmd = new SqlCommand(sql, Connect);
            //Add từng values của điều kiện cột (name[i]) tương ứng
            for (int i = 0; i < parameter; i++)
            {
                cmd.Parameters.AddWithValue(name[i], values[i]);
            }
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            close();
            return dt;
        }

        //Hàm load dữ liệu từ CSDL với điều kiện có sử dụng procedure
        public DataTable Select_DataWithParameterUseProcedure(string sql, string[] name, object[] values, int parameter)
        {
            open();
            SqlCommand cmd = new SqlCommand(sql, Connect);
            cmd.CommandType = CommandType.StoredProcedure;//Gọi procedure
            //Add từng values của điều kiện cột (name[i]) tương ứng
            for (int i = 0; i < parameter; i++)
            {
                cmd.Parameters.AddWithValue(name[i], values[i]);
            }
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            close();
            return dt;
        }
        //Hàm Excute để insert, Update, Delete
        //-> sql: câu query
        //-> name[]: Chuỗi parameter tương ứng khi insert, update, delete
        public int Execute(string sql, string[] name, object[] values, int parameter)
        {
            try
            {
                open();
                SqlCommand cmd = new SqlCommand(sql, Connect);
                for (int i = 0; i < parameter; i++)
                {
                    cmd.Parameters.AddWithValue(name[i], values[i]);
                }
                int kq = cmd.ExecuteNonQuery();
                close();
                return kq;
            }
            catch
            {
                //Thêm thất bại
                close();
                return -1;
            }
        }
        //Hàm Excute để insert, Update, Delete sử dụng procedure
        public int ExecuteUsProcedure(string sql, string[] name, object[] values, int parameter)
        {
            try
            {
                open();
                SqlCommand cmd = new SqlCommand(sql, Connect);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < parameter; i++)
                {
                    cmd.Parameters.AddWithValue(name[i], values[i]);
                }
                int kq = cmd.ExecuteNonQuery();
                close();
                return kq;
            }
            catch
            {
                //Thêm thất bại
                close();
                return -1;
            }
        }
        //Hàm trả về 1 giá trị kiểu int từ CSDL
        public int ReturnInteger(string sql)
        {
            open();
            SqlCommand cmd = new SqlCommand(sql, Connect);
            int ValuesInteger = int.Parse(cmd.ExecuteScalar().ToString());
            close();
            return ValuesInteger;
        }
        //Hàm trả về 1 giá trị kiểu int từ CSDL có sử dụng procedure
        public int ReturnIntegerUseProcedure(string sql)
        {
            open();
            SqlCommand cmd = new SqlCommand(sql, Connect);
            cmd.CommandType = CommandType.StoredProcedure;
            int ValuesInteger = int.Parse(cmd.ExecuteScalar().ToString());
            close();
            return ValuesInteger;
        }
        //Hàm trả về 1 giá trị kiểu int từ CSDL có parameter
        public int ReturnIntegerWithParameter(string sql, string[] name, object[] values, int parameter)
        {
            open();
            SqlCommand cmd = new SqlCommand(sql, Connect);
            for (int i = 0; i < parameter; i++)
            {
                cmd.Parameters.AddWithValue(name[i], values[i]);
            }
            int ValuesInteger = int.Parse(cmd.ExecuteScalar().ToString());
            close();
            return ValuesInteger;
        }
        //Hàm trả về 1 giá trị kiểu int từ CSDL có parameter sử dụng Procedure
        public int ReturnIntegerWithParameterUserProcedure(string sql, string[] name, object[] values, int parameter)
        {
            open();
            SqlCommand cmd = new SqlCommand(sql, Connect);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < parameter; i++)
            {
                cmd.Parameters.AddWithValue(name[i], values[i]);
            }
            int ValuesInteger = int.Parse(cmd.ExecuteScalar().ToString());
            close();
            return ValuesInteger;
        }
        //Hàm trả về 1 giá trị kiểu double từ CSDL
        public double ReturnDouble(string sql)
        {
            open();
            SqlCommand cmd = new SqlCommand(sql, Connect);
            double ValuesDouble = double.Parse(cmd.ExecuteScalar().ToString());
            close();
            return ValuesDouble;
        }
        //Hàm trả về 1 giá trị kiểu double từ CSDL có sử dụng procedure
        public double ReturnDoubleUseProcedure(string sql)
        {
            open();
            SqlCommand cmd = new SqlCommand(sql, Connect);
            cmd.CommandType = CommandType.StoredProcedure;
            Double ValuesDouble = Double.Parse(cmd.ExecuteScalar().ToString());
            close();
            return ValuesDouble;
        }
        //Hàm trả về 1 giá trị kiểu string từ CSDL
        public string ReturnString(string sql)
        {
            open();
            SqlCommand cmd = new SqlCommand(sql, Connect);
            string ValuesString = cmd.ExecuteScalar().ToString();
            close();
            return ValuesString;
        }
        //Hàm trả về 1 giá trị kiểu string từ CSDL có sử dụng procedure
        public string ReturnStringUseProcedure(string sql)
        {
            open();
            SqlCommand cmd = new SqlCommand(sql, Connect);
            cmd.CommandType = CommandType.StoredProcedure;
            string ValuesString = cmd.ExecuteScalar().ToString();
            close();
            return ValuesString;
        }
        //Hàm trả về 1 giá trị kiểu string từ CSDL có sử dụng procedure có tham số output
        public string ReturnStringUseProcedureOut(string sql, string output, int size)
        {
            open();
            SqlCommand cmd = new SqlCommand(sql, Connect);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter para = new SqlParameter(output, SqlDbType.VarChar, size);//output: tham số out của procedure
                                                                                //SqlDbType.VarChar: Kiểu dữ liệu
                                                                                //size:  kích thước output
            //Thiết lập direction output cho para
            cmd.Parameters.Add(para).Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
            //Đọc kết quả trả về
            string strKQ = para.Value.ToString();
            close();
            return strKQ;
        }
        //---------------------------------------------Dùng cho giá trị trả về là Image-----------------
        //Hàm trả về 1 giá trị kiểu byte[] từ CSDL
        public byte[] ReturnByte(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Connect);
            byte[] ValuesByte = (byte[])cmd.ExecuteScalar();
            return ValuesByte;
        }
        //Hàm trả về 1 giá trị kiểu string từ CSDL có sử dụng procedure
        public byte[] ReturnByteUserProcedure(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Connect);
            cmd.CommandType = CommandType.StoredProcedure;
            byte[] ValuesByte = (byte[])cmd.ExecuteScalar();
            return ValuesByte;
        }
    }
}