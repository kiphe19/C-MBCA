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

            JArray unit = new JArray();
            JArray vessel = new JArray();
            JArray date = new JArray();

            con.select("unit_table", "name", "cat=1");
            while (con.result.Read())
            {
                unit.Add(con.result["name"]);
            }
            con.Close();

            con.select("report_table", "distinct(vessel_name)");
            while (con.result.Read())
            {
                vessel.Add(con.result["vessel_name"]);
            }
            con.Close();

            con.select("report_table", "distinct(date)");
            while (con.result.Read())
            {
                //date.Add(Convert.ToDateTime(con.result["date"]).ToShortDateString());
                date.Add(con.result["date"]);
            }
            con.Close();

            foreach (var vsl in vessel)
            {

                foreach (var tgl in date)
                {
                    dynamic c = new JObject(
                            new JProperty("vessel", vsl)
                        );

                    String whrrr = String.Format("vessel_name = '{0}' and date = '{1}'", vsl, Convert.ToDateTime(tgl.ToString()).ToString("yyyy-MM-dd"));
                    con.select("report_table", "date", whrrr);

                    if (con.result.HasRows)
                    {
                        JArray data = new JArray();
                        foreach (var unt in unit)
                        {
                            var whrrrr = String.Format("vessel_name = '{0}' and date = '{1}' and unit = '{2}'", vsl, Convert.ToDateTime(tgl.ToString()).ToString("yyyy-MM-dd"), unt);
                            con.select("report_table", "fuel_liter", whrrrr);
                            con.result.Read();
                            if (con.result.HasRows)
                            {
                                c.tgl = Convert.ToDateTime(tgl.ToString()).ToShortDateString();
                                data.Add(con.result["fuel_liter"]);
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

            a.data = b;
            a.unit = unit;

            Response.ContentType = "text/json";
            Response.Write(a);
        }

        public void test()
        {
            DateTime dateNow = DateTime.Now;
            DateTime date30 = DateTime.Now.AddMonths(-1);

            TimeSpan diff = dateNow - date30;

            int days = diff.Days;

            dynamic a = new JObject();
            JArray b = new JArray();

            JArray unit = new JArray();
            JArray vessel = new JArray();
            JArray date = new JArray();

            con.select("unit_table", "name", "cat=1");
            while (con.result.Read())
            {
                unit.Add(con.result["name"]);
            }
            con.Close();

            con.select("report_table", "distinct(vessel_name)");
            while (con.result.Read())
            {
                vessel.Add(con.result["vessel_name"]);
            }
            con.Close();

            con.select("report_table", "distinct(date)");
            while (con.result.Read())
            {
                date.Add(con.result["date"]);
            }
            con.Close();


            foreach (var vsl in vessel)
            {
                dynamic c = new JObject(
                    new JProperty("vessel", vsl)
                    );

                for (int i = 0; i < days; i++)
                {
                    JArray data = new JArray();
                    DateTime tgl = DateTime.Now.AddDays(-i);

                    var whrrr = String.Format("vessel_name = '{0}' and date ='{1}'", vsl, tgl.ToString("MM-dd-yyyy"));
                    con.select("report_table", "date", whrrr);
                    if (con.result.HasRows)
                    {

                        foreach (var unt in unit)
                        {
                            var whr = String.Format("vessel_name='{0}' and date='{2}' and unit='{1}'", vsl, unt, tgl.ToString("MM-dd-yyyy"));
                            con.select("report_table", "date, fuel_liter", whr);

                            con.result.Read();
                            if (con.result.HasRows)
                            {
                                c.tgl = Convert.ToDateTime(con.result["date"]).ToShortDateString();
                                data.Add(con.result["fuel_liter"]);
                            }
                            else
                            {
                                data.Add(0);
                            }
                        }
                        c.data = data;
                        b.Add(c);
                    }
                }

            }
            con.Close();


            a.data = b;
            a.unit = unit;

            Response.ContentType = "text/json";
            Response.Write(a);
        }
    }
}
