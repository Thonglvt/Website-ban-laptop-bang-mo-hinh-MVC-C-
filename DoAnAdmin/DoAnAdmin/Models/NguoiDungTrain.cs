using LinqToExcel.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnAdmin.Models
{
    public class NguoiDungTrain
    {
        public double distance;//Khoảng cách

        [ExcelColumn("Tuoi")]
        public double Tuoi { get; set; }//Tọa độ 
        [ExcelColumn("MucDich")]
        public double MucDich { get; set; }//Tọa độ 
        [ExcelColumn("MucGia")]
        public double MucGia { get; set; }//Tọa độ 
        [ExcelColumn("ThuongHieu")]
        public int ThuongHieu { get; set; }//Class của point

        public NguoiDungTrain()
        {
            this.ThuongHieu = 1;
            this.Tuoi = 1;
            this.MucDich = 1;
            this.MucGia = 1;
        }

        public NguoiDungTrain(double Tuoi, double MucDich, double MucGia, int ThuongHieu)
        {
            this.ThuongHieu = ThuongHieu;
            this.Tuoi = Tuoi;
            this.MucDich = MucDich;
            this.MucGia = MucGia;
        }
        public NguoiDungTrain(double Tuoi, double MucDich, double MucGia)
        {
            this.Tuoi = Tuoi;
            this.MucDich = MucDich;
            this.MucGia = MucGia;
        }
    }
}