using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using chevron.Models;

namespace chevron.Controllers
{
    public class QpController : Controller
    {
        //
        // GET: /Qp/

        Connection con = new Connection();

        public void Index()
        {
            dynamic a = new JObject();
            dynamic b = new JObject();
            JArray c = new JArray();
            JArray unit = new JArray();
            JArray unit_data = new JArray();
            JArray vessel = new JArray();

            // Prosess Ngambil Unit

            con.query("select name from unit_table where cat = 1");
            while (con.result.Read())
            {
                unit.Add(con.result["name"].ToString());
            }
            con.Close();

            // End Pross

            // Proses ngambil Vessel -> Di distinct biar ngga duplicat
            con.query("select distinct(vessel_name) from report_table");
            while (con.result.Read())
            {
                vessel.Add(con.result["vessel_name"].ToString());
            }
            con.Close();
            // End Proses

            for (int i = 0; i < vessel.Count; i++)
            {
                c.Add(b);
                b.data = unit_data;

                var query = String.Format("select distinct(date) from report_table where vessel_name='{0}'", vessel[i]);
                con.query(query);

                while (con.result.Read())
                {
                    b.date = DateTime.Parse(con.result["date"].ToString()).ToString("MM/dd/yyyy");
                    b.vessel = vessel[i];

                    for (int u = 0; u < unit.Count; u++)
                    {
                        var query2 = String.Format("select fuel_liter from report_table where vessel_name='{0}' and unit='{1}'", vessel[i].ToString(), unit[u].ToString());
                        con.query(query2);
                        if (con.result.HasRows)
                        {
                            con.result.Read();
                            unit_data.Add(con.result["fuel_liter"]);
                        }
                        else
                        {
                            unit_data.Add(0);
                        }
                    }
                }
            }


            a.data = c;
            a.unit = unit;
            Response.ContentType = "text/json";
            Response.Write(a);
        }

        public ActionResult qp()
        {
            return View();
        }

    }
}
