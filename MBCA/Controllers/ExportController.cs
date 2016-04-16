using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web.Mvc;

namespace chevron.Controllers
{
    public class ExportController : Controller
    {
        Properties.Settings setting = chevron.Properties.Settings.Default;
        Connection con = new Connection();
        public ActionResult Index()
        {
            return View();
        }
        private void logoGb(ExcelWorksheet ws) 
        {
            Image logo = Image.FromFile(Server.MapPath("~/images/chevron.jpg"));
            ExcelPicture pic = ws.Drawings.AddPicture("logo", logo);
            pic.SetPosition(5, 5);
            pic.SetSize(90, 90);
        }
        private void headerTbl(JArray judul, ExcelWorksheet ws)
        {
            int colidx = 1;
            int rowstart = 11;
            foreach (var tt in judul)
            {
                var isi = ws.Cells[rowstart, colidx];
                isi.Value = tt;
                isi.Style.Font.Bold = true;
                isi.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                isi.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                isi.AutoFitColumns();
                colidx++;
            }
        }
        public void r_dailyUnit(string tg1,string tg2, int v)
        {
            DateTime dateFrom = DateTime.ParseExact(tg1, "yyyyMMdd", CultureInfo.InvariantCulture);
            DateTime dateTo = DateTime.ParseExact(tg2,"yyyyMMdd",CultureInfo.InvariantCulture);
            TimeSpan ambilTanggal = dateTo - dateFrom;
            var vesselname = "";
            con.select("vessel_table", "name", string.Format("id={0}", v));
            while (con.result.Read())
            {
                vesselname = con.result["name"].ToString();
            }


            var periode = dateFrom.ToString("dd MMM") + " - " + dateTo.ToString("dd MMM yy");
            var period = dateFrom.ToString("dd MMM") + "_" + dateTo.ToString("dd MMM yy");

            JArray header = new JArray();
            JArray unitid = new JArray();
            JArray unitnama = new JArray();
            var dt = new DataTable();
            header.Add("Date");
            //header.Add("Vesse")
            var cr = string.Format("tgl >= '{0}' and tgl <= '{1}' and id_vessel = {2}", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), v);
            con.select("report_daily join unit_table on unit_table.id = report_daily.id_unit", "distinct(id_unit) id_unit,unit_table.name nama", cr);
            while (con.result.Read())
            {
                unitid.Add(con.result["id_unit"]);
                unitnama.Add(con.result["nama"]);
                header.Add(con.result["nama"]);


            }

            dt.Columns.Add("date");

            for (var i = 0; i <= ambilTanggal.TotalDays; i++)
            {
                //dt.NewRow();
                var baris = dt.NewRow();

                baris["date"] = dateFrom.AddDays(i).ToString("yyyy-MM-dd");

                //foreach (var u in unitid)
                //{

                //}
            }




            using (ExcelPackage pkg = new ExcelPackage())
            {
                ExcelWorksheet ws = pkg.Workbook.Worksheets.Add("coba");
                //buat gambar
                this.logoGb(ws);
                
                ws.Cells[7, 1].Value = "Month :"; ws.Cells[7, 3].Value = periode; 
                ws.Cells[8, 1].Value = "Boat Name :"; ws.Cells[8, 3].Value = vesselname;
                ws.Cells[9, 1].Value = "Boat Owner :";
                using (ExcelRange cel = ws.Cells["A7:D9"])
                {
                    cel.Style.Font.SetFromFont(new Font("Arial Narrow", 11, FontStyle.Bold));
                }


                    //buat header judul
                this.headerTbl(header, ws);

                // isi tanggal
                var mulai = 11;

                for (var i = 0; i <= ambilTanggal.TotalDays; i++)
                {
                    //dt.NewRow();
                    //var baris = dt.NewRow();

                    //baris["date"] = dateFrom.AddDays(i).ToString("yyyy-MM-dd");
                    var br = ws.Cells[mulai + 1 + i, 1];
                    br.Value = dateFrom.AddDays(i).ToString("dd-MMM-yyyy");
                    br.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    br.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    br.AutoFitColumns();

                    //foreach (var u in unitid)
                    //{

                    //}
                }




                //create excel
                this.createxls(pkg, "daily_" + period);

            }
            //return RedirectToAction("Index");
        }

        private void createxls (ExcelPackage pkg, string filenama)
        {
            Byte[] fileBytes = pkg.GetAsByteArray();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + filenama + ".xlsx");
            Response.Charset = "";

            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            Response.BinaryWrite(fileBytes);
            Response.End();
        }

        public ActionResult coba()
        {
            //this.getData();
            var json = JsonConvert.SerializeObject(this.getData());

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public DataTable getData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("UserId", typeof(Int32));
            dt.Columns.Add("UserName", typeof(string));
            dt.Columns.Add("Education", typeof(string));
            dt.Columns.Add("Location", typeof(string));
            dt.Rows.Add(1, "Satinder Singh", "Bsc Com Sci", "Mumbai");
            dt.Rows.Add(2, "Amit Sarna", "Mstr Com Sci", "Mumbai");
            dt.Rows.Add(3, "Andrea Ely", "Bsc Bio-Chemistry", "Queensland");
            dt.Rows.Add(4, "Leslie Mac", "MSC", "Town-ville");
            dt.Rows.Add(5, "Vaibhav Adhyapak", "MBA", "New Delhi");
            return dt;
        }

        
    }

}
