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
            JArray c = new JArray();
            JArray d = new JArray();

            //con.query("select * from activity_table");
            //while (con.result.Read())
            //{
            //    dynamic b = new JObject();
            //    b.a = con.result["name"].ToString();
            //    b.b = con.result["ket"].ToString();
            //    b.c = con.result["ket"].ToString();

            //    c.Add(b);
            //}

            //a.data = c;
            //Response.Write(a);
            con.select("unit_table", "name");
            while (con.result.Read())
            {
                d.Add(con.result["name"].ToString());
            }
            con.Close();


            con.select("report_table", "*");
            while (con.result.Read())
            {
                dynamic b = new JObject();

            }
        }

        public ActionResult qp()
        {
            return View();
        }

    }
}
