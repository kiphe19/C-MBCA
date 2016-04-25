using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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


        //public ActionResult report(string tg1, string tg2, int v)
        //{
        //    DataTable dt = new DataTable();
        //    DateTime dateFrom = DateTime.ParseExact(tg1, "yyyyMMdd", CultureInfo.InvariantCulture);
        //    DateTime dateTo = DateTime.ParseExact(tg2, "yyyyMMdd", CultureInfo.InvariantCulture);
        //    TimeSpan ambilTanggal = dateTo - dateFrom;

        //    JArray unitid = new JArray();

        //    dt.Columns.Add("Date");
        //    var cr = string.Format("tgl >= '{0}' and tgl <= '{1}' and id_vessel = {2}", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), v);
        //    con.select("report_daily join unit_table on unit_table.id = report_daily.id_unit", "distinct(id_unit) id_unit,unit_table.name nama", cr);
        //    while (con.result.Read())
        //    {
        //        dt.Columns.Add(con.result["nama"].ToString());
        //        unitid.Add(con.result["id_unit"]);
        //    }
        //    dt.Columns.Add("Totalx",typeof(float));

        //    for (var i = 0; i <= ambilTanggal.TotalDays; i++)
        //    {
        //        DataRow brs = dt.NewRow();

        //        brs[0] = dateFrom.AddDays(i).ToString("yyyy-MM-dd");
        //        int k = 1;
        //        decimal tot_row = 0;
        //        foreach(var id in unitid)
        //        {
        //            var ww = string.Format("select round(fuel_litre,3) fuel_litre,round(fuel_price,3) fuel_price,fuel_curr,round(charter_price,3) charter_price,round(mob_price,3) mob_price,charter_curr from report_daily where tgl = '{0}' and id_unit = {1} and id_vessel = {2};", dateFrom.AddDays(i).ToString("yyyy-MM-dd"), id, v);
        //            con.query(ww);
        //            con.result.Read();
        //            if (con.result.HasRows)
        //                brs[k] = con.result["fuel_litre"];

        //            else
        //                brs[k] = 0;

        //            tot_row += Convert.ToDecimal(brs[k]);

        //            k++;
        //        }
        //        brs[k] = tot_row;
        //        dt.Rows.Add(brs);
        //    }

        //    var json = JsonConvert.SerializeObject(dt);
        //    //return Json(new { nama="jono"}, JsonRequestBehavior.AllowGet);
        //    return Json(json, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tg1"></param>
        /// <param name="tg2"></param>
        /// <param name="v"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public string laporan(string tg1, string tg2, int v, string t)
        {
            DataTable dt = DailyReport(tg1, tg2, v,t);

            var json = JsonConvert.SerializeObject(dt);
            //return Json(json,JsonRequestBehavior.AllowGet);
            return json;


        }

        private static DataTable DailyReport(string tg1, string tg2, int v, string t)
        {
            Connection con = new Connection();
            DataTable dt = new DataTable();
            DateTime dateFrom = DateTime.ParseExact(tg1, "yyyyMMdd", CultureInfo.InvariantCulture);
            DateTime dateTo = DateTime.ParseExact(tg2, "yyyyMMdd", CultureInfo.InvariantCulture);
            TimeSpan ambilTanggal = dateTo - dateFrom;

            JArray unitid = new JArray();

            dt.Columns.Add("Date");
            var cr = string.Format("tgl >= '{0}' and tgl <= '{1}' and id_vessel = {2}", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), v);
            con.select("report_daily join unit_table on unit_table.id = report_daily.id_unit", "distinct(id_unit) id_unit,unit_table.name nama", cr);
            while (con.result.Read())
            {
                dt.Columns.Add(con.result["nama"].ToString());
                unitid.Add(con.result["id_unit"]);
            }
            //dt.Columns.Add("Total", typeof(float));
            //dt.Columns.Add("Total");

            for (var i = 0; i <= ambilTanggal.TotalDays; i++)
            {
                DataRow brs = dt.NewRow();
                brs[0] = dateFrom.AddDays(i).ToString("dd-MMM-yy");
                int k = 1;
                //decimal tot_row = 0;
                foreach (var id in unitid)
                {
                    var ww = string.Format("select round(fuel_litre,3) fuel_litre,round(fuel_price,3) fuel_price,fuel_curr,round(charter_price,3) charter_price,round(mob_price,3) mob_price,charter_curr from report_daily where tgl = '{0}' and id_unit = {1} and id_vessel = {2};", dateFrom.AddDays(i).ToString("yyyy-MM-dd"), id, v);
                    con.query(ww);
                    con.result.Read();
                    if (con.result.HasRows)
                        switch (t)
                        {
                            case "fl":
                                brs[k] = con.result["fuel_litre"];
                                break;
                            case "fc":
                                brs[k] = con.result["fuel_price"];
                                break;
                            case "ch":
                                brs[k] = con.result["charter_price"];
                                break;
                            case "mb":
                                brs[k] = con.result["mob_price"];
                                break;
                            case "ap":
                                brs[k] = (decimal)con.result["fuel_price"] + (decimal)con.result["charter_price"]+(decimal)con.result["mob_price"];
                                break;
                            default:
                                break;
                        }
                    else
                        brs[k] = 0;
                    //tot_row += Convert.ToDecimal(brs[k]);
                    k++;
                }
                //brs[k] = tot_row;
                dt.Rows.Add(brs);
            }
            return dt;
        }
        
        
        public void r_daily(string tg1, string tg2, int v, string t)
        {
            DateTime dateFrom = DateTime.ParseExact(tg1, "yyyyMMdd", CultureInfo.InvariantCulture);
            DateTime dateTo = DateTime.ParseExact(tg2, "yyyyMMdd", CultureInfo.InvariantCulture);
            var vesselname = "";
            var prsh = "";
            con.select("vessel_table", "name,vs_owner", string.Format("id={0}", v));
            while (con.result.Read())
            {
                vesselname = con.result["name"].ToString();
                prsh = con.result["vs_owner"].ToString();
            }
            var periode = dateFrom.ToString("dd MMM") + " - " + dateTo.ToString("dd MMM yy");
            var period = dateFrom.ToString("dd MMM") + "_" + dateTo.ToString("dd MMM yy");

            using (ExcelPackage p = new ExcelPackage())
            {
                ExcelWorksheet ws = p.Workbook.Worksheets.Add("Daily Barge");

                this.logoGb(ws);

                ws.Cells[7, 1].Value = "Month :"; ws.Cells[7, 3].Value = periode;
                ws.Cells[8, 1].Value = "Boat Name :"; ws.Cells[8, 3].Value = vesselname;
                ws.Cells[9, 1].Value = "Boat Owner :"; ws.Cells[9, 3].Value = prsh;
                //using (ExcelRange cel = ws.Cells["A7:D9"])
                //{
                //    cel.Style.Font.SetFromFont(new Font("Arial Narrow", 11, FontStyle.Bold));
                //}

                DataTable dt = DailyReport(tg1, tg2, v,t);

                //Merging cells and create a center heading for out table
                ws.Cells[6, 1].Value = "Daily Boat Cost";
                ws.Cells[6, 1, 6, dt.Columns.Count].Merge = true;
                ws.Cells[6, 1, 6, dt.Columns.Count].Style.Font.Bold = true;
                //ws.Cells[6, 1, 6, dt.Columns.Count].Style.Font.SetFromFont(new Font("Arial Narrow"), 12, FontStyle.Bold);
                ws.Cells[6, 1, 6, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int colIndex = 1;
                int rowIndex = 12;

                foreach (DataColumn dc in dt.Columns) //Creating Headings
                {
                    var cell = ws.Cells[rowIndex, colIndex];

                    //Setting the background color of header cells to Gray
                    var fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.Aquamarine);


                    //Setting Top/left,right/bottom borders.
                    var bd1 = cell.Style.Border;
                    bd1.Bottom.Style =
                        bd1.Top.Style =
                        bd1.Left.Style =
                        bd1.Right.Style = ExcelBorderStyle.Medium;

                    //Setting Value in cell
                    cell.Value = dc.ColumnName;
                    //cell.AutoFitColumns();
                    
                    colIndex++;
                }

                foreach (DataRow dr in dt.Rows) // Adding Data into rows
                {
                    colIndex = 1;
                    rowIndex++;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        var cell = ws.Cells[rowIndex, colIndex];

                        //Setting Value in cell
                        if (colIndex > 1)
                        {
                            //cell.Style.Numberformat.Format = "0.00";
                            cell.Value = Convert.ToDecimal(dr[dc.ColumnName]);
                        }
                        //cell.Value = Convert.ToInt32(dr[dc.ColumnName]);

                        else cell.Value = dr[dc.ColumnName];
                        //Setting borders of cell
                        var bd2 = cell.Style.Border;
                        bd2.Left.Style =
                            bd2.Right.Style = ExcelBorderStyle.Thin;
                        colIndex++;
                    }
                }

                //hitung total per colom
                colIndex = 1;
                foreach (DataColumn dc in dt.Columns) //Creating Formula Sum
                {

                    var cell = ws.Cells[rowIndex + 1, colIndex];
                    if (colIndex == 1) cell.Value = "Total";
                    //Setting Sum Formula
                    else
                    {
                        cell.Formula = "Sum(" +
                                    ws.Cells[12 + 1, colIndex].Address +
                                    ":" +
                                    ws.Cells[rowIndex, colIndex].Address +
                                    ")";
                    }

                    var bd = cell.Style.Border;
                        bd.Top.Style = ExcelBorderStyle.Medium;
                        bd.Left.Style = ExcelBorderStyle.Thin;
                        bd.Right.Style = ExcelBorderStyle.Thin;
                        bd.Bottom.Style = ExcelBorderStyle.Double;
                    //Setting Background fill color to Gray
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(Color.Aquamarine);
                    colIndex++;
                }

                //colIndex = 1;
                //decimal total_r = 0;
                string a="";
                switch (t)
                {
                    case "fl":
                        a = "Fuel";

                        break;
                    case "fc":
                        a = "Fuel Cost";
                        break;
                    case "ch":
                        a = "Charter";
                        break;
                    case "mb":
                        a = "Mob/Demob";
                        break;
                    case "ap":
                        a = "All Cost";
                        break;
                    default: break;                            
                }


                int row_total = rowIndex + 4;
                int col_total = colIndex - 2;

                ws.Cells[row_total, col_total].Value = "Total "+a;
                var tt = ws.Cells[row_total, col_total+1];

                var border = tt.Style.Border;
                border.Left.Style = ExcelBorderStyle.Medium;
                border.Right.Style = ExcelBorderStyle.Medium;
                border.Bottom.Style = ExcelBorderStyle.Double;
                tt.Style.Fill.PatternType = ExcelFillStyle.Solid;
                tt.Style.Fill.BackgroundColor.SetColor(Color.Aquamarine);


                tt.Formula = "SUM("+
                            //ws.Cells[rowIndex + 1,2].Address+":"+
                            ws.Cells[row_total-3, 2].Address+":"+
                            ws.Cells[row_total - 3, col_total + 1].Address+")";

                colIndex = 1;
                foreach (DataColumn dc in dt.Columns)
                {
                    var cell_p = ws.Cells[rowIndex + 2, colIndex];
                    //if (colIndex == 1) cell_p.Value = "";
                    if (colIndex > 1) 
                    //else
                    {
                        cell_p.Style.Numberformat.Format = "0.00%";
                        cell_p.Formula = "+" + ws.Cells[rowIndex+1, colIndex].Address + "/" + tt.Address;
                        //cell_p.Value = "R" + (rowIndex + 2) + "C" + colIndex;
                    }
                    var bd = cell_p.Style.Border;
                    bd.Top.Style = ExcelBorderStyle.Medium;
                    bd.Left.Style = ExcelBorderStyle.Thin;
                    bd.Right.Style = ExcelBorderStyle.Thin;
                    bd.Bottom.Style = ExcelBorderStyle.Medium;
                    colIndex++;
                }
                int row_p = rowIndex + 3;
                var prs = ws.Cells[row_p, colIndex-1];

                var grs = prs.Style.Border;
                grs.Left.Style = ExcelBorderStyle.Medium;
                grs.Right.Style = ExcelBorderStyle.Medium;
                grs.Bottom.Style = ExcelBorderStyle.Medium;

                prs.Style.Numberformat.Format = "0.00%";
                prs.Formula = "SUM("+
                                ws.Cells[row_p-1,1].Address+":"+
                                ws.Cells[row_p - 1,colIndex-1].Address
                            +")";

                this.createxls(p, "daily_" + period);
            }
        }

        private static DataTable DailyDCReport(string tg1, string tg2, int u)
        {
            Connection con = new Connection();
            DataTable dt = new DataTable();
            DateTime dateFrom = DateTime.ParseExact(tg1, "yyyyMMdd", CultureInfo.InvariantCulture);
            DateTime dateTo = DateTime.ParseExact(tg2, "yyyyMMdd", CultureInfo.InvariantCulture);
            TimeSpan ambilTanggal = dateTo - dateFrom;

            string q = string.Format("select d.tgl tgl, well,afe, psc_no, t_start, t_end , durasi  , fuel_litre, fuel_price, charter_price, mob_price from drilling_table d " +
                " join report_daily r on r.id_unit = d.id_unit and d.tgl = r.tgl " +
                //" join unit_table u on u.Id = d.id_unit " +
                "where d.tgl > '{0}' and d.tgl <= '{1}' and d.id_unit = {2} ;", tg1, tg2, u);

            //JArray unitid = new JArray();

            dt.Columns.Add("date");
            dt.Columns.Add("well");
            dt.Columns.Add("afe");
            dt.Columns.Add("psc");
            dt.Columns.Add("start");
            dt.Columns.Add("end");
            dt.Columns.Add("durasi");
            dt.Columns.Add("litre");
            dt.Columns.Add("fuel");
            dt.Columns.Add("charter");
            dt.Columns.Add("mob");
            //dt.Columns.Add("charhour");
            //dt.Columns.Add("total");

            con.query(q);
            while (con.result.Read())
            {
                DataRow brs = dt.NewRow();
                brs["date"] = Convert.ToDateTime(con.result["tgl"]).ToString("dd-MMM-yyyy");
                brs["well"] = con.result["well"];
                brs["afe"] = con.result["afe"];
                brs["psc"] = con.result["psc_no"];
                brs["start"] = Convert.ToDateTime(con.result["t_start"]).ToString("HH:mm");
                brs["end"] = Convert.ToDateTime(con.result["t_end"]).ToString("HH:mm");
                brs["durasi"] = con.result["durasi"];
                brs["litre"] = con.result["fuel_litre"];
                brs["fuel"] = con.result["fuel_price"];
                brs["charter"] = con.result["charter_price"];
                brs["mob"] = con.result["mob_price"];

                dt.Rows.Add(brs);

                ////DataRow brs = dt.NewRow();
                //dt.Rows.Add(
                //    con.result["tgl"],
                //    con.result["well"],
                //    con.result["afe"],
                //    con.result["psc_no"],
                //    con.result["t_start"],
                //    con.result["t_end"],
                //    con.result["durasi"],
                //    con.result["fuel_litre"],
                //    con.result["fuel_price"],
                //    con.result["charter_price"],
                //    con.result["mob_price"]
                //);
            }

            return dt;
        }
        public string laporanDC(string tg1, string tg2, int u)
        {
            DataTable dt = DailyDCReport(tg1, tg2, u);

            dynamic kk = new JObject();
            kk.satu = tg1;
            kk.dua = tg2;
            kk.tiga = u;

            var json = JsonConvert.SerializeObject(dt);
            //return Json(new {a = "asdasd"}, JsonRequestBehavior.AllowGet);
            return json;
        }
        public void r_DC(string tg1, string tg2, int u)
        {
            DateTime dateFrom = DateTime.ParseExact(tg1, "yyyyMMdd", CultureInfo.InvariantCulture);
            DateTime dateTo = DateTime.ParseExact(tg2, "yyyyMMdd", CultureInfo.InvariantCulture);
            var unitname = "";
            con.select("unit_table", "name", string.Format("id={0}", u));
            while (con.result.Read())
            {
                unitname = con.result["name"].ToString();
            }
            var periode = dateFrom.ToString("dd MMM yy") + " - " + dateTo.ToString("dd MMM yy");
            var period = dateFrom.ToString("dd MMM yy") + "_" + dateTo.ToString("dd MMM yy");

            DataTable dt = DailyDCReport(tg1, tg2, u);

            using (ExcelPackage p = new ExcelPackage())
            {
                ExcelWorksheet ws = p.Workbook.Worksheets.Add("D&C "+unitname);
                this.logoGb(ws);
                int kol = dt.Columns.Count + 1;
                // buat Header
                ws.Cells[3, 1].Value = "Daily Boat Cost";
                ws.Cells[3, 1, 3, kol].Merge = true;
                ws.Cells[3, 1, 3, kol].Style.Font.Bold = true;
                ws.Cells[3, 1, 3, kol].Style.Font.Size = 14;
                ws.Cells[3, 1, 3, kol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[4, 1].Value = "Boat Cost Allocation to Wells";
                ws.Cells[4, 1, 4, kol].Merge = true;
                ws.Cells[4, 1, 4, kol].Style.Font.Bold = true;
                ws.Cells[4, 1, 4, kol].Style.Font.Size = 14;
                ws.Cells[4, 1, 4, kol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[5, 1].Value = periode;
                ws.Cells[5, 1, 5, kol].Merge = true;
                ws.Cells[5, 1, 5, kol].Style.Font.Bold = true;
                ws.Cells[5, 1, 5, kol].Style.Font.Size = 14;
                ws.Cells[5, 1, 5, kol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[6, 1].Value = unitname.ToUpper();
                ws.Cells[6, 1, 6, kol].Merge = true;
                ws.Cells[6, 1, 6, kol].Style.Font.Bold = true;
                ws.Cells[6, 1, 6, kol].Style.Font.Size = 18;
                ws.Cells[6, 1, 6, kol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                //buat header tabel

                ws.Cells[8,1].Value = "Date";
                ws.Cells[8,2].Value = "WELL";
                ws.Cells[8,3].Value = "AFE";
                ws.Cells[8,4].Value = "PSC NUMBER";
                ws.Cells[8,5].Value = "TIME COMMENCED";
                ws.Cells[8,6].Value = "TIME COMPLETED";
                ws.Cells[8,7].Value = "HOURS WORKED";
                ws.Cells[8, 1, 8, dt.Columns.Count].Style.WrapText = true;
                ws.Cells[8, 1, 8, dt.Columns.Count].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[8, 1, 8, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                int colIndex, rowIndex;

                rowIndex = 8;
                //isi table
                foreach (DataRow dr in dt.Rows) // Adding Data into rows
                {
                    colIndex = 1;
                    rowIndex++;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        

                        if(colIndex > 7)
                        {
                            var cell1 = ws.Cells[rowIndex, colIndex+1];
                            cell1.Value = dr[dc.ColumnName];
                        }
                        else
                        {
                            var cell = ws.Cells[rowIndex, colIndex];
                            cell.Value = dr[dc.ColumnName];
                        }

                        //if (colIndex == 1)
                        //{
                        //    cell.Style.Numberformat.Format = ""
                        //}
                        //Setting Value in cell
                        //if (colIndex > 1)
                        //{
                            //cell.Style.Numberformat.Format = "0.00";
                            //cell.Value = Convert.ToDecimal(dr[dc.ColumnName]);
                        //}
                        //cell.Value = Convert.ToInt32(dr[dc.ColumnName]);

                        //else 
                        //cell.Value = dr[dc.ColumnName];
                        //Setting borders of cell
                        //var bd2 = cell.Style.Border;
                        //bd2.Left.Style =
                        //    bd2.Right.Style = ExcelBorderStyle.Thin;
                        colIndex++;
                    }
                }


                //export to excel
                this.createxls(p, "DC_" + period);
            }
        }


    }

}
