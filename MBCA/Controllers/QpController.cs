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

        public String Index()
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

            
            for (int v = 0; v < vessel.Count; v++)
            {
                for (int i = 0; i < unit.Count; i++)
                {
                   var query = String.Format("select * from report_table where vessel_name='{0}' and unit='{1}'", vessel[v].ToString(), unit[i].ToString());
                    
                    con.query(query);
                    while (con.result.Read())
                    {
                        b.date = con.result["date"].ToString();
                        b.vessel = con.result["vessel_name"].ToString();

                        c.Add(b);
                    }

                }
            }
            
            a.data = c;
            a.unit = unit;
            
            return Newtonsoft.Json.JsonConvert.SerializeObject(a);
        }

        public ActionResult qp()
        {
            return View();
        }

    }
}
