using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using chevron.Models;
using System.Dynamic;

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

        public void qu()
        {
            dynamic a = new JObject();
            dynamic b = new JObject();
            JArray c = new JArray();
            dynamic d = new JObject();

            JArray unit_dt = new JArray();
            JArray unit = new JArray();
            JArray vessel = new JArray();

            con.query("select name from unit_table where cat = 1");
            while (con.result.Read())
            {
                unit.Add(new JValue(con.result["name"]));
            }
            con.Close();

            con.query("select distinct(vessel_name) from report_table");
            while (con.result.Read())
            {
                vessel.Add(new JValue(con.result["vessel_name"]));
            }
            con.Close();

            foreach (var vs in vessel)
            {

                var query = String.Format("select distinct(date) from report_table where vessel_name='{0}'", vs);
                con.query(query);
                con.result.Read();

                c.Add(b);
                b.tgl = DateTime.Parse(con.result["date"].ToString()).ToString("MM/dd/yyyy");
                b.vessel = vs;


                foreach (var un in unit)
                {
                    var query2 = String.Format("select fuel_liter from report_table where vessel_name='{0}' and unit='{1}'", vs, un);
                    con.query(query2);
                    con.result.Read();

                    if (con.result.HasRows)
                    {
                        unit_dt.Add(con.result["fuel_liter"]);
                    }
                    else
                    {
                        unit_dt.Add(0);
                    }
                }
                b.data = unit_dt;
                //unit_dt.Clear();
            }

            a.data = c;
            a.unit = unit;
            Response.ContentType = "text/json";
            Response.Write(a);
        }

        public void vg()
        {

            dynamic a = new JObject();
            JArray b = new JArray();
            dynamic c = new JObject();
            
            JArray data = new JArray();
            var unit = new JArray();
            var vessel = new JArray();
            var date = new JArray();

            con.select("unit_table", "name", "cat =1");
            while (con.result.Read())
            {
                unit.Add(new JValue(con.result["name"]));
            }

            con.select("report_table", "distinct(vessel_name)");
            while (con.result.Read())
            {
                vessel.Add(new JValue(con.result["vessel_name"]));
            }

            con.select("report_table", "distinct(date)");
            while (con.result.Read())
            {
                date.Add(new JValue(con.result["date"]));
            }

            foreach (var vs in vessel)
            {
                c = new JObject(
                    new JProperty("vessel", vs)
                    );
                c.data = data;

                //data.Clear();
                //foreach (var unt in unit)
                //{
                //    var query = String.Format("vessel_name='{0}' and unit='{1}'", vs, unt);
                //    con.select("report_table", "distinct(date), fuel_liter", query);
                //    con.result.Read();

                //    if (con.result.HasRows)
                //    {
                //        c.tgl = Convert.ToDateTime(con.result["date"]).ToShortDateString();
                //        data.Add(new JValue(con.result["fuel_liter"]));
                //    }
                //    else
                //    {
                //        data.Add(0);
                //    }

                //}
                foreach (var tgl in date)
                {
                    data.Clear();
                    foreach (var unt in unit)
                    {
                        var query = String.Format("vessel_name='{0}' and unit='{1}' and date='{2}'", vs, unt, DateTime.Parse(tgl.ToString()).ToString("yyyy-MM-dd"));
                        con.select("report_table", "distinct(date), fuel_liter", query);
                        con.result.Read();

                        if (con.result.HasRows)
                        {
                            c.tgl = Convert.ToDateTime(con.result["date"]).ToShortDateString();
                            data.Add(new JValue(con.result["fuel_liter"]));
                        }
                        else
                        {
                            data.Add(0);
                        }

                    }
                }

                b.Add(c);
            }

            //for (int v = 0; v < vessel.Count; v++)
            //{
            //    c = new JObject(
            //        new JProperty("vessel", vessel[v])
            //        );
            //    b.Add(c);
            //    //c.data = data;

            //    data.Clear();

            //    for (int i = 0; i < unit.Count; i++)
            //    {
            //        var query = String.Format("vessel_name='{0}' and unit='{1}'", vessel[v], unit[i]);
            //        con.select("report_table", "distinct(date), fuel_liter", query);
            //        con.result.Read();

            //        if (con.result.HasRows)
            //        {
            //            c.tgl = Convert.ToDateTime(con.result["date"]).ToShortDateString();
            //            data.Add(new JValue(con.result["fuel_liter"]));
            //        }
            //        else
            //        {
            //            data.Add(0);
            //        }
            //    }
            //    c.data = data;
            //}

            a.data = b;
            a.unit = unit;

            Response.ContentType = "text/json";
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(a);
            //return json;
            Response.Write(a);
        }
    }
}
