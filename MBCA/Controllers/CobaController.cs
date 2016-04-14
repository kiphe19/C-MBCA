using OfficeOpenXml;
using System;
using System.IO;
using System.Web.Mvc;

namespace chevron.Controllers
{
    public class CobaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //public void kirimExcel()
        //{
        //    Workbook wb = new Workbook();
        //    Worksheet ws = wb.Worksheets.Add("contoh");
        //    WorksheetRow row = ws.Table.Rows.Add();
        //    row.Cells.Add("Haloo Dunia");
        //    wb.Save(@"d:\testing.xls");
        //    Response.End();
        //}
        public ActionResult excel(string dt1, string dt2)
        {
            using (ExcelPackage pkg = new ExcelPackage())
            {
                ExcelWorksheet ws = pkg.Workbook.Worksheets.Add("coba");
                ws.Cells[1, 1].Value = "halo ini satu satu";
                ws.Cells[1, 2].Value = "halo ini satu dua";
                ws.Cells[2, 1].Value = "halo ini dua satu";
                ws.Cells[2, 2].Value = "halo ini dua dua";
                ws.Cells["C5"].Value = dt1;
                ws.Cells["C7"].Value = dt2;


                Byte[] fileBytes = pkg.GetAsByteArray();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=DataTable.xlsx");
                Response.Charset = "";

                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                Response.BinaryWrite(fileBytes);
                Response.End();
            }

            return RedirectToAction("Index");
        }
    }
}
