using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using chevron.Models;

namespace chevron.Controllers
{
    public class ReportController : Controller
    {
        Connection con = new Connection();

        public ActionResult Index()
        {
            List<ReportModel> report = new List<ReportModel>();

            con.select("report_table", "*");
            while (con.result.Read())
            {
                report.Add(new ReportModel
                {
                    vessel_id = int.Parse(con.result["vessel_id"].ToString()),
                    vessel_name = con.result["vessel_name"].ToString(),
                    unit = con.result["unit"].ToString(),
                    date = DateTime.Parse(con.result["date"].ToString()).ToString("MM/dd/yyyy"),
                    fuel_liter = Decimal.Parse(con.result["fuel_liter"].ToString()),
                    fuel_usd = Decimal.Parse(con.result["fuel_usd"].ToString()),
                    fuel_rp = Decimal.Parse(con.result["fuel_rp"].ToString())
                });

            }

            ViewBag.report = report;
            return View();
        }

    }
}
