using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using chevron.Models;
using System.Dynamic;
using System.Globalization;

namespace chevron.Controllers
{
    public class QpController : Controller
    {
        //
        // GET: /Qp/

        Connection con = new Connection();
        JArray vessel = new JArray();

        public ActionResult Index()
        {
            this.getVessel();
            List<SelectListItem> filterByVessel = new List<SelectListItem>();
            foreach (var item in vessel)
            {
                filterByVessel.Add(new SelectListItem{
                    Text = item.ToString(),
                    Value = item.ToString()
                });
            }
            ViewBag.vessel = filterByVessel;
            return View();
        }

        public void json(FormCollection input)
        {
            String column = "",
                   type = Request["type"];

            switch (type)
            {
                case "fl":
                    column = "fuel_litre";
                    break;
                case "fc":
                    column = "fuel_price, fuel_curr";
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

            this.getVessel();

            con.select("report_daily", "distinct(tgl)");
            while (con.result.Read())
            {
                 date.Add(con.result["tgl"]);
            }
            con.Close();

            foreach (var vsl in vessel)
            {

                foreach (var tgl in date)
                {
                    dynamic c = new JObject(
                            new JProperty("vessel", vsl)
                        );

                    String whrrr = String.Format("vessel = '{0}' and tgl = '{1}'", vsl, Convert.ToDateTime(tgl.ToString()).ToString("yyyy-MM-dd"));
                    con.select("report_daily", "tgl", whrrr);

                    if (con.result.HasRows)
                    {
                        JArray data = new JArray();
                        foreach (var unt in unit)
                        {
                            var whrrrr = String.Format("vessel = '{0}' and tgl = '{1}' and user_unit = '{2}'", vsl, Convert.ToDateTime(tgl.ToString()).ToString("yyyy-MM-dd"), unt);
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
            }
            con.Close();

            a.data = b;
            a.unit = unit;

            Response.ContentType = "text/json";
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(a);

            Response.Write(json);
        }

        private void getVessel()
        {
            con.select("report_daily", "distinct(vessel)");
            while (con.result.Read())
            {
                vessel.Add(con.result["vessel"]);
            }
            con.Close();
        }
    }
}
