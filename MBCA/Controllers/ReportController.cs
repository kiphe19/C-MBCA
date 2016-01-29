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
            //List<ReportModel> report = new List<ReportModel>();

            //con.select("report_table", "*");
            //while (con.result.Read())
            //{
            //    report.Add(new ReportModel
            //    {
            //        vessel_id = int.Parse(con.result["vessel_id"].ToString()),
            //        vessel_name = con.result["vessel_name"].ToString(),
            //        unit = con.result["unit"].ToString(),
            //        date = DateTime.Parse(con.result["date"].ToString()).ToString("MM/dd/yyyy"),
            //        fuel_liter = Decimal.Parse(con.result["fuel_liter"].ToString()),
            //        fuel_usd = Decimal.Parse(con.result["fuel_usd"].ToString()),
            //        fuel_rp = Decimal.Parse(con.result["fuel_rp"].ToString()),
            //        standby_time = Decimal.Parse(con.result["standby_time"].ToString()),
            //        load_time = Decimal.Parse(con.result["load_time"].ToString()),
            //        steaming_time = Decimal.Parse(con.result["steaming_time"].ToString()),
            //        down_time = Decimal.Parse(con.result["down_time"].ToString()),
            //        charter_usd = Decimal.Parse(con.result["charter_usd"].ToString()),
            //        charter_rp = Decimal.Parse(con.result["charter_rp"].ToString()),
            //        demob_rp = Decimal.Parse(con.result["demob_rp"].ToString()),
            //        demob_usd = Decimal.Parse(con.result["demob_usd"].ToString())
            //    });

            //}


            List<ReportDailyModel> reportDaily = new List<ReportDailyModel>();

            con.select("report_daily", "*");
            while (con.result.Read())
            {
                reportDaily.Add(new ReportDailyModel
                {
                    tanggal = Convert.ToDateTime(con.result["tgl"]).ToString("dd/MM/yyyy"),
                    vessel  = con.result["vessel"].ToString(),
                    user_unit   = con.result["user_unit"].ToString(),
                    fuel_litre  =  decimal.Round(Convert.ToDecimal(con.result["fuel_litre"]),2) ,
                    fuel_price  = decimal.Round(Convert.ToDecimal(con.result["fuel_price"]),2),
                    fuel_curr   = Convert.ToInt16(con.result["fuel_curr"]),
                    charter_price = decimal.Round(Convert.ToDecimal(con.result["charter_price"]),2),
                    mob_price   = decimal.Round(Convert.ToDecimal(con.result["mob_price"]),2),
                    charter_curr = Convert.ToInt16(con.result["charter_curr"])


                    
                });
            }
            con.Close();

            ViewBag.reportd = reportDaily;
            return View();
        }

        private void getVessel()
        {
            //con.select("report_daily", "distinct(vessel)");
            //while (con.result.Read())
            //{
            //    //vessel.Add(con.result["vessel"]);
            //}
            //con.Close();
        }

    }
}
