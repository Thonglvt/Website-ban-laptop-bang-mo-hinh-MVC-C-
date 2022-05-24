using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using LinqToExcel;
using System.Data;

namespace DoAnAdmin.Models
{
    public class K_NNAlgorithm
    {
        public List<LaptopDouble> lstLaptop = new List<LaptopDouble>();
        public List<LaptopDouble> lstTrainByK = new List<LaptopDouble>();
        List<Label> lstLabel = new List<Label>();
        Random r = new Random();


        public void docFileModel()
        {
            string pathModel = @"E:\Cong nghe web\9_WebsiteBanLapTop\DoAnAdmin\DoAnAdmin\Assets\Mining\LaptopTraining.xlsx";
            string ext = Path.GetExtension(pathModel);
            if (ext.ToLower() == ".xls" || ext.ToLower() == ".xlsx")
            {
                var excel = new ExcelQueryFactory(pathModel);
                var laptop = from t in excel.Worksheet<LaptopStr>("LaptopTraining")
                             select t;
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("Company");
                dt2.Columns.Add("TypeName");
                dt2.Columns.Add("Inches");
                dt2.Columns.Add("ScreenResolution");
                dt2.Columns.Add("Cpu");
                dt2.Columns.Add("Ram");
                dt2.Columns.Add("Memory");
                dt2.Columns.Add("Gpu");
                dt2.Columns.Add("OpSys");
                dt2.Columns.Add("Weight");
                dt2.Columns.Add("Price_euros");
                foreach (var item in laptop)
                {
                    int Company = IntToString(removeSpecialChar(item.Company));
                    int TypeName = IntToString(removeSpecialChar(item.TypeName));
                    int Inches = IntToString(removeSpecialChar(item.Inches));
                    int ScreenResolution = IntToString(removeSpecialChar(item.ScreenResolution));
                    int Cpu = IntToString(removeSpecialChar(item.Cpu));
                    int Ram = IntToString(removeSpecialChar(item.Ram));
                    int Memory = IntToString(removeSpecialChar(item.Memory));
                    int Gpu = IntToString(removeSpecialChar(item.Gpu));
                    int OpSys = IntToString(removeSpecialChar(item.OpSys));
                    int Weight = IntToString(removeSpecialChar(item.Weight));
                    int Price_euros = IntToString(removeSpecialChar(item.Price_euros));

                    dt2.Rows.Add(Company, TypeName, item.Inches, ScreenResolution, Cpu, Ram, Memory, Gpu, OpSys, item.Weight, item.Price_euros);
                    LaptopDouble lapInt = new LaptopDouble();
                    lapInt.Company = Company;
                    lapInt.TypeName = TypeName;
                    lapInt.Inches = Inches;
                    lapInt.ScreenResolution = ScreenResolution;
                    lapInt.Cpu = Cpu;
                    lapInt.Ram = Ram;
                    lapInt.Memory = Memory;
                    lapInt.Gpu = Cpu;
                    lapInt.OpSys = OpSys;
                    lapInt.Weight = Weight;
                    lapInt.Price_euros = Price_euros;
                    lstLaptop.Add(lapInt);
                    Label l = new Label();
                    l._Label = item.Company;
                    l.ID = Company;
                    Label l3 = new Label();
                    l3._Label = item.TypeName;
                    l3.ID = TypeName;
                    Label l4 = new Label();
                    l4._Label = item.Inches;
                    l4.ID = Inches;
                    Label l5 = new Label();
                    l5._Label = item.ScreenResolution;
                    l5.ID = ScreenResolution;
                    Label l6 = new Label();
                    l6._Label = item.Cpu;
                    l6.ID = Cpu;
                    Label l7 = new Label();
                    l7._Label = item.Ram;
                    l7.ID = Ram;
                    Label l8 = new Label();
                    l8._Label = item.Memory;
                    l8.ID = Memory;
                    Label l9 = new Label();
                    l9._Label = item.Gpu;
                    l9.ID = Gpu;
                    Label l10 = new Label();
                    l10._Label = item.OpSys;
                    l10.ID = OpSys;
                    Label l11 = new Label();
                    l11._Label = item.Weight;
                    l11.ID = Weight;
                    Label l12 = new Label();
                    l12._Label = item.Price_euros;
                    l12.ID = Price_euros;

                    if ((l._Label.ToLower() == "apple"
                        || l._Label.ToLower() == "hp" || l._Label.ToLower() == "acer" || l._Label.ToLower() == "asus"
                        || l._Label.ToLower() == "dell" || l._Label.ToLower() == "lenovo" || l._Label.ToLower() == "msi"
                        || l._Label.ToLower() == "lg" || l._Label.ToLower() == "gigabyte"))
                    {
                        if (checkLabelTrung(lstLabel, l.ID))
                            lstLabel.Add(l);
                        if (checkLabelTrung(lstLabel, l3.ID))
                            lstLabel.Add(l3);
                        if (checkLabelTrung(lstLabel, l4.ID))
                            lstLabel.Add(l4);
                        if (checkLabelTrung(lstLabel, l5.ID))
                            lstLabel.Add(l5);
                        if (checkLabelTrung(lstLabel, l6.ID))
                            lstLabel.Add(l6);
                        if (checkLabelTrung(lstLabel, l7.ID))
                            lstLabel.Add(l7);
                        if (checkLabelTrung(lstLabel, l8.ID))
                            lstLabel.Add(l8);
                        if (checkLabelTrung(lstLabel, l9.ID))
                            lstLabel.Add(l9);
                        if (checkLabelTrung(lstLabel, l10.ID))
                            lstLabel.Add(l10);
                        if (checkLabelTrung(lstLabel, l11.ID))
                            lstLabel.Add(l11);
                        if (checkLabelTrung(lstLabel, l12.ID))
                            lstLabel.Add(l12);
                    }
                }
            }
        }
        public int IntToString(string s)
        {
            int kq = 0;
            foreach (char c in s)
            {
                kq += Convert.ToInt32(c);
            }
            return kq;
        }
        public bool checkLabelTrung(List<Label> lstLbl, int id)
        {
            foreach (Label l in lstLbl)
            {
                if (l.ID == id)
                {
                    return false;//Trùng
                }
            }
            return true;
        }
        public int thuatToan(LaptopDouble newTrain, int k)
        {
            docFileModel();
            foreach (LaptopDouble p in lstLaptop)
            {
                p.Distance = TinhKhoangCachTrainToTrain(p, newTrain);
            }
            //Tìm k điểm gần nhất
            lstTrainByK = kNearestNeighbor(lstLaptop, k);
            return timKQXuatHienNN(lstTrainByK);
        }
        //Hàm tính khoảng cách
        public double TinhKhoangCachTrainToTrain(LaptopDouble p1, LaptopDouble p2)
        {
            double bp = Math.Pow(p1.TypeName - p2.TypeName, 2)
                //+ Math.Pow(p1.Product - p2.Product, 2)
                        + Math.Pow(p1.Inches - p2.Inches, 2)
                        + Math.Pow(p1.ScreenResolution - p2.ScreenResolution, 2)
                        + Math.Pow(p1.Cpu - p2.Cpu, 2)
                        + Math.Pow(p1.Ram - p2.Ram, 2)
                        + Math.Pow(p1.Memory - p2.Memory, 2)
                        + Math.Pow(p1.Gpu - p2.Gpu, 2)
                        + Math.Pow(p1.OpSys - p2.OpSys, 2)
                        + Math.Pow(p1.Weight - p2.Weight, 2)
                        + Math.Pow(p1.Price_euros - p2.Price_euros, 2);
            return Math.Sqrt(bp);
        }
        //hàm tìm k điểm dữ liệu gần nhất:
        public List<LaptopDouble> kNearestNeighbor(List<LaptopDouble> lstTr, int k)
        {
            return lstTr.OrderBy(t => t.Distance).Take(k).ToList();
        }
        //tìm ketQua xuất hiện nhiều nhất trong k điểm tìm được:
        public int timKQXuatHienNN(List<LaptopDouble> lstTr)
        {
            int max = 1;
            int KQ = 152;
            List<int> lstI = new List<int>();
            lstI.Add(152); lstI.Add(385); lstI.Add(147); lstI.Add(489); lstI.Add(489);
            lstI.Add(379); lstI.Add(412); lstI.Add(379); lstI.Add(233); lstI.Add(627);
            foreach (int it in lstI)
            {
                int num = lstTr.Count(t => (int)t.Company == it);
                if (num > max)
                {
                    max = num;
                    KQ = it;
                }
            }
            return KQ;
        }
        //Xóa các kí tự đặc biệt
        public string removeSpecialChar(string str)
        {
            string s="";
            str = new string((from c in str
                  where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c)
                  select c).ToArray());
            return str;
        }

    }
}