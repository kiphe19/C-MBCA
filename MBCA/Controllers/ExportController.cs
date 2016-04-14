using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System;
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
            //pic.Border.LineStyle = eLineStyle.Solid;
            //pic.Border.Fill.Color = Color.DarkCyan;
            //pic.Fill.Style = eFillStyle.SolidFill;
            //pic.Fill.Color = Color.White;
            //pic.Fill.Transparancy = 50;
        }
        private void headerTbl(JArray judul, ExcelWorksheet ws)
        {
            int colidx = 1;
            int rowstart = 11;
            foreach (var tt in judul)
            {
                ws.Cells[rowstart, colidx].Value = tt;
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




            using (ExcelPackage pkg = new ExcelPackage())
            {
                ExcelWorksheet ws = pkg.Workbook.Worksheets.Add("coba");
                //buat gambar
                this.logoGb(ws);

                ws.Cells[7, 1].Value = "Month :"; ws.Cells[7, 3].Value = dateFrom.ToString("dd MMM") + " - " + dateTo.ToString("dd MMM yy"); 
                ws.Cells[8, 1].Value = "Boat Name :"; ws.Cells[8, 3].Value = vesselname;
                ws.Cells[9, 1].Value = "Boat Owner :";
                using (ExcelRange cel = ws.Cells["A7:D9"])
                {
                    cel.Style.Font.SetFromFont(new Font("Arial Narrow", 11, FontStyle.Bold));
                }

                //int colidx = 0, rowidx = 1;
                //int rowstart = 11;

                JArray header = new JArray();
                header.Add("Date");
                //header.Add("Vesse")
                var cr = string.Format("tgl >= '{0}' and tgl <= '{1}' and id_vessel = {2}", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), v);
                con.select("report_daily join unit_table on unit_table.id = report_daily.id_unit", "distinct(id_unit) id_unit,unit_table.name nama", cr);
                while (con.result.Read())
                {
                    //unitall.Add(con.result["id_unit"]);
                    //unitnama.Add(con.result["nama"]);
                    header.Add(con.result["nama"]);
                }

                //buat header judul
                this.headerTbl(header, ws);
                
                //foreach (var tt in header)
                //{
                //    ws.Cells[rowstart, colidx+1].Value = tt;
                //    colidx++;
                //}
                //for 


                //    using (ExcelRange cell = worksheet.Cells["A2:G2"])
                //{
                //    cell.Merge = true;
                //    cell.Style.Font.SetFromFont(new Font("Britannic Bold", 18, FontStyle.Italic));
                //    cell.Style.Font.Color.SetColor(Color.Black);
                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                //    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //    cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                //    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                //}

                //    ws.Cells[1, 1].Value = "halo ini satu satu";
                //ws.Cells[1, 2].Value = "halo ini satu dua";
                //ws.Cells[2, 1].Value = "halo ini dua satu";
                //ws.Cells[2, 2].Value = "halo ini dua dua";
                //ws.Cells["C5"].Value = tg1;
                //ws.Cells["D5"].Value = dateFrom.ToShortDateString();
                //ws.Cells["C7"].Value = tg2;
                //ws.Cells["C9"].Value = v;


                Byte[] fileBytes = pkg.GetAsByteArray();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=export"+tg1+".xlsx");
                Response.Charset = "";

                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                Response.BinaryWrite(fileBytes);
                Response.End();
            }
            //return RedirectToAction("Index");
        }
    }

}
