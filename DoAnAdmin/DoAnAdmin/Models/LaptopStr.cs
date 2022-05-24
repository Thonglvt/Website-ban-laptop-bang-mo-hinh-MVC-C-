using LinqToExcel.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnAdmin.Models
{
    public class LaptopStr
    {
        //[ExcelColumn("laptop_ID")]
        //public string laptop_ID { get; set; }

        [ExcelColumn("Company")]
        public string Company { get; set; }

        [ExcelColumn("Product")]
        public string Product { get; set; }

        [ExcelColumn("TypeName")]
        public string TypeName { get; set; }

        [ExcelColumn("Inches")]
        public string Inches { get; set; }

        [ExcelColumn("ScreenResolution")]
        public string ScreenResolution { get; set; }

        [ExcelColumn("Cpu")]
        public string Cpu { get; set; }

        [ExcelColumn("Ram")]
        public string Ram { get; set; }

        [ExcelColumn("Memory")]
        public string Memory { get; set; }

        [ExcelColumn("Gpu")]
        public string Gpu { get; set; }

        [ExcelColumn("OpSys")]
        public string OpSys { get; set; }

        [ExcelColumn("Weight")]
        public string Weight { get; set; }

        [ExcelColumn("Price_euros")]
        public string Price_euros { get; set; }
    }
}