using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using chevron.Models;
using Newtonsoft.Json.Linq;
using System.Globalization;
using AttributeRouting.Web.Mvc;
using Newtonsoft.Json;

namespace chevron.Controllers
{
    public class ReportController : Controller
    {
        Connection con = new Connection();
        JArray vessel = new JArray();

        [Route("api/report")]
        //public String  _reportSatu()
        public string _reportSatu(FormCollection input)
        {

            dynamic aa = new JObject();
            //JArray tg = new JArray();

            DateTime dateFrom = (input["tgFrom"] == "") ? DateTime.Now.AddDays(-1) : DateTime.Parse(Convert.ToDateTime(input["tgFrom"]).ToString("yyyy-MM-dd"));
            DateTime dateTo = (input["tgTo"] == "") ? DateTime.Now : DateTime.Parse(Convert.ToDateTime(input["tgTo"]).ToString("yyyy-MM-dd"));
            TimeSpan ambilTanggal = dateTo - dateFrom;
            //return "success";
            //JArray unit = new JArray();
            JArray date = new JArray();

            //con.select("unit_table", "name");
            //while (con.result.Read())
            //{
            //    unit.Add(con.result["name"]);
            //}
            //con.Close();
            dynamic cc = new JObject();
            for (int i = 0; i <= ambilTanggal.TotalDays; i++)
            {
                //date.Add(dateFrom.AddDays(i).ToString("yyyy-MM-dd"));
                //date.Add(dateFrom.AddDays(i).ToString("yyyy-MM-dd"));

                cc.tgl = dateFrom.AddDays(i).ToString("yyyy-MM-dd");
                date.Add(cc);
            }
            aa.ddd = date;
            Response.Write(ambilTanggal);
            //return Json(new { nama = "Jono" },
            //    JsonRequestBehavior.AllowGet);

            //return Json(aa, JsonRequestBehavior.AllowGet);
            Response.ContentType = "text/json";
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(aa);
            var json = JsonConvert.SerializeObject(aa);

            //Response.Write(json);
            //return "success";
            return json;
        }

        public ActionResult Index()
        {
            this.getVessel();
            List<SelectListItem> filterByVessel = new List<SelectListItem>();
            foreach (var item in vessel)
            {
                filterByVessel.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Value = item.ToString()

                });
            }
            filterByVessel.Insert(0, (new SelectListItem { Text = "-- Select Vessel -- ", Value = "0" }));
            ViewBag.vessel = filterByVessel;
            ViewBag.vesselId = getListVesselId();
            return View();
        }
        private List<SelectListItem> getListVesselId()
        {
            List<SelectListItem> vessel = new List<SelectListItem>();
            vessel.Insert(0, (new SelectListItem
                {
                    Text = "All",
                    Value = "0",
                    Selected = true
                }
            ));
            con.select("vessel_table", "id,name");
            while (con.result.Read())
            {
                vessel.Add(new SelectListItem
                {
                    Text = con.result["name"].ToString(),
                    Value = con.result["id"].ToString()
                });
            }
            con.Close();
            var VesselSorted = (from li in vessel orderby li.Text select li).ToList();

            return VesselSorted;
        }

        private void getVessel()
        {
            con.select("report_daily", "distinct(id_vessel)");
            while (con.result.Read())
            {
                vessel.Add(con.result["id_vessel"]);
            }
            con.Close();
        }
        public void Reporting(FormCollection input)
        {
            String column = "",
                   type = input["type"],
                   vsl = input["vessel"];

            DateTime dateFrom = (input["dateFrom"] == "") ? DateTime.Now.AddDays(-1) : DateTime.Parse(Convert.ToDateTime(input["dateFrom"]).ToString("yyyy-MM-dd"));
            DateTime dateTo = (input["dateTo"] == "") ? DateTime.Now : DateTime.Parse(Convert.ToDateTime(input["dateTo"]).ToString("yyyy-MM-dd"));
            TimeSpan ambilTanggal = dateTo - dateFrom;

            switch (type)
            {
                case "fl":
                    column = "fuel_litre";
                    break;
                case "fc":
                    column = "fuel_price, fuel_curr";
                    break;
                case "ch":
                    column = "charter_price, charter_curr";
                    break;
                case "mb":
                    column = "mob_price, charter_curr";
                    break;
                default:
                    column = "fuel_litre";
                    break;
            }

            dynamic a = new JObject();
            JArray b = new JArray();

            JArray unit = new JArray();
            JArray date = new JArray();

            con.select("unit_table", "name");
            while (con.result.Read())
            {
                unit.Add(con.result["name"]);
            }
            con.Close();

            for (int i = 0; i <= ambilTanggal.TotalDays; i++)
            {
                date.Add(dateFrom.AddDays(i).ToString("yyyy-MM-dd"));
            }

            foreach (var tgl in date)
            {
                dynamic c = new JObject(
                        new JProperty("vessel", vsl)
                    );

                String whrrr = String.Format("vessel = '{0}' and tgl = '{1}'", vsl, tgl);
                con.select("report_daily", "tgl", whrrr);

                if (con.result.HasRows)
                {
                    JArray data = new JArray();
                    foreach (var unt in unit)
                    {
                        var whrrrr = String.Format("vessel = '{0}' and tgl = '{1}' and user_unit = '{2}'", vsl, tgl, unt);
                        con.select("report_daily", column, whrrrr);
                        con.result.Read();
                        if (con.result.HasRows)
                        {
                            c.tgl = Convert.ToDateTime(tgl.ToString()).ToShortDateString();

                            switch (type)
                            {
                                case "fl":
                                    data.Add(con.result["fuel_litre"]);
                                    break;
                                case "fc":
                                    if (con.result["fuel_curr"].ToString() == "1")
                                    {
                                        data.Add(Convert.ToDecimal(con.result["fuel_price"]).ToString("c0", CultureInfo.CreateSpecificCulture("en-US")));
                                    }
                                    else
                                    {
                                        data.Add(Convert.ToDecimal(con.result["fuel_price"]).ToString("c0", CultureInfo.CreateSpecificCulture("id-ID")));
                                    }
                                    break;
                                case "ch":
                                    if (con.result["charter_curr"].ToString() == "1")
                                    {
                                        data.Add(Convert.ToDecimal(con.result["charter_price"]).ToString("c0", CultureInfo.CreateSpecificCulture("en-US")));
                                    }
                                    else
                                    {
                                        data.Add(Convert.ToDecimal(con.result["charter_price"]).ToString("c0", CultureInfo.CreateSpecificCulture("id-ID")));
                                    }
                                    break;
                                case "mb":
                                    if (con.result["charter_curr"].ToString() == "1")
                                    {
                                        data.Add(Convert.ToDecimal(con.result["mob_price"]).ToString("c0", CultureInfo.CreateSpecificCulture("en-US")));
                                    }
                                    else
                                    {
                                        data.Add(Convert.ToDecimal(con.result["mob_price"]).ToString("c0", CultureInfo.CreateSpecificCulture("id-ID")));
                                    }
                                    break;
                                default:
                                    data.Add(con.result["fuel_litre"]);
                                    break;
                            }
                        }
                        else
                        {
                            data.Add(0);
                        }
                        c.data = data;
                    }
                    b.Add(c);
                }

            }
            con.Close();

            a.data = b;
            a.unit = unit;

            Response.ContentType = "text/json";
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(a);
            Response.Write(json);

        }

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


        //List<ReportDailyModel> reportDaily = new List<ReportDailyModel>();

        //con.select("report_daily", "*");
        //while (con.result.Read())
        //{
        //    reportDaily.Add(new ReportDailyModel
        //    {
        //        tanggal = Convert.ToDateTime(con.result["tgl"]).ToString("dd/MM/yyyy"),
        //        vessel  = con.result["vessel"].ToString(),
        //        user_unit   = con.result["user_unit"].ToString(),
        //        fuel_litre  =  decimal.Round(Convert.ToDecimal(con.result["fuel_litre"]),2) ,
        //        fuel_price  = decimal.Round(Convert.ToDecimal(con.result["fuel_price"]),2),
        //        fuel_curr   = Convert.ToInt16(con.result["fuel_curr"]),
        //        charter_price = decimal.Round(Convert.ToDecimal(con.result["charter_price"]),2),
        //        mob_price   = decimal.Round(Convert.ToDecimal(con.result["mob_price"]),2),
        //        charter_curr = Convert.ToInt16(con.result["charter_curr"])



        //    });
        //}
        //con.Close();

        //ViewBag.reportd = reportDaily;
        //    return View();
        //}

       


    }
}
