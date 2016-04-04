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

        //[Route("api/report")]
        [Route("api/report/{tg1}/{tg2}/{tipe}/{ves}")]
        //public string _reportSatu(FormCollection input)
        public string _reportSatu(string tg1, string tg2, string tipe, int ves)
        {
            //Response.Write("tg1 = " + tg1 + ", tg2 = " + tg2 + ", vesselnya " + ves.ToString()+", tipenya = "+tipe+"\n");
            DateTime dateFrom = Convert.ToDateTime(tg1);
            DateTime dateTo = Convert.ToDateTime(tg2);
            TimeSpan ambilTanggal = dateTo - dateFrom;
            var vesselname = "";
            con.select("vessel_table", "name", string.Format("id={0}", ves));
            while (con.result.Read())
            {
                vesselname = con.result["name"].ToString();
            }

            dynamic aa = new JObject();
            JArray all = new JArray();
            JArray unitall = new JArray();
            JArray unitnama = new JArray();
            JArray date = new JArray();
            JArray kolom = new JArray();
            JArray datay = new JArray();
            JArray isix = new JArray();

            kolom.Add("Date");
            kolom.Add("Vessel");

            var cr = string.Format("tgl > '{0}' and tgl <= '{1}' and id_vessel = {2}", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), ves);
            con.select("report_daily join unit_table on unit_table.id = report_daily.id_unit", "distinct(id_unit) id_unit,unit_table.name nama", cr);
            while (con.result.Read())
            {
                unitall.Add(con.result["id_unit"]);
                unitnama.Add(con.result["nama"]);
                kolom.Add(con.result["nama"]);
            }

            for (var i = 0; i <= ambilTanggal.TotalDays; i++)
            {
                dynamic ii = new JObject();
                JArray kk = new JArray();
                JArray isi = new JArray();
                ii.tg = dateFrom.AddDays(i).ToString("yyyy-MM-dd");
                ii.ves = vesselname;
                foreach (var u in unitall)
                {
                    ///*
                    var ww = string.Format("select round(fuel_litre,3) fuel_litre,round(fuel_price,3) fuel_price,fuel_curr,round(charter_price,3) charter_price,round(mob_price,3) mob_price,charter_curr from report_daily where tgl = '{0}' and id_unit = {1} and id_vessel = {2};", dateFrom.AddDays(i).ToString("yyyy-MM-dd"), u, ves);
                    //Response.Write(ww + "\n");
                    con.query(ww);
                    con.result.Read();
                    if (con.result.HasRows)
                    {
                        switch (tipe)
                        {
                            case "fl":
                                isi.Add(con.result["fuel_litre"]);
                                //isix.Add(con.result["fuel_litre"]);
                                break;
                            case "fc":
                                //isi.Add(con.result["fuel_price"]);
                                if (con.result["fuel_curr"].ToString() == "1") isi.Add("USD " + con.result["fuel_price"]);
                                else isi.Add("IDR " + con.result["fuel_price"]);
                                //isix.Add(con.result["fuel_price"]);
                                break;
                            case "ch":
                                //isi.Add(con.result["charter_price"]);
                                if (con.result["charter_curr"].ToString() == "1") isi.Add("USD " + con.result["charter_price"]);
                                else isi.Add("IDR " + con.result["charter_price"]);
                                //isix.Add(con.result["charter_price"]);
                                break;
                            case "mb":
                                //isi.Add(con.result["mob_price"]);
                                if (con.result["charter_curr"].ToString() == "1") isi.Add("USD " + con.result["mob_price"]);
                                else isi.Add("IDR " + con.result["mob_price"]);
                                //isix.Add(con.result["mob_price"]);
                                break;
                            default:
                                break;
                        }
                    }
                    else isi.Add(0);
                   // */
                }
                ii.datax = isi;
                all.Add(ii);
            }
            aa.data = all;
            aa.columns = kolom;
            Response.ContentType = "text/json";
            var json = JsonConvert.SerializeObject(aa);
            return json;
            //Response.Write(json);
            //return "success";
            ////var json = Newtonsoft.Json.JsonConvert.SerializeObject(aa);
            //*/
        }

        [Route("api/reportDC/{tg1}/{tg2}")]
        public string _reportDua(string tg1, string tg2)
        {
            DateTime dateFrom = Convert.ToDateTime(tg1);
            DateTime dateTo = Convert.ToDateTime(tg2);

            dynamic aa = new JObject();
            JArray zz = new JArray();
            //select max(unit_table.name)nama, sum(round(fuel_litre, 3)) litre, sum(fuel_price) fuel, sum(charter_price) chart, sum(mob_price) mob, sum(t_boat_h) boat from report_daily join unit_table on unit_table.id = report_daily.id_unit where id_mainunit = 1 and tgl > '2016-03-27' and tgl <= '2016-04-04' group by id_unit order by nama;

            string qq = string.Format("select max(unit_table.name) nama, sum(fuel_litre) litre, sum(fuel_price) fuel, sum(charter_price) charter, sum(mob_price) mob, sum(t_boat_h) boat from report_daily join unit_table on unit_table.id = report_daily.id_unit "+
                "where id_mainunit = 1 and tgl > '2016-03-27' and tgl <= '2016-04-04' group by id_unit order by nama " ,
                //" where id_mainunit = 1 and tgl > '{0}' and tgl <= '{1}' group by id_unit ;",
                dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"));

            //Response.Write(qq);
            con.query(qq);
            while (con.result.Read())
            {
                dynamic k = new JObject();
                JArray a = new JArray();
                //k.idunit = con.result["unit"];
                k.unit = con.result["nama"];
                k.litre = con.result["litre"];
                k.fuel = con.result["fuel"];
                k.charter = con.result["charter"];
                k.mob = con.result["mob"];
                k.boat = con.result["boat"];
                //a.Add(k);
                zz.Add(k);
            }
            con.Close();

            aa.data = zz;
            //aa.nama = "jono";
            Response.ContentType = "text/json";
            var json = JsonConvert.SerializeObject(aa);
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
                    Text = "",
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

            con.select("unit_table", "id");
            while (con.result.Read())
            {
                unit.Add(con.result["id"]);
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
                //cek jika vessel dan tgl exist
                String whrrr = String.Format("id_vessel = '{0}' and tgl = '{1}'", vsl, tgl);
                con.select("report_daily", "tgl", whrrr);

                if (con.result.HasRows)
                {
                    JArray data = new JArray();
                    foreach (var unt in unit)
                    {
                        var whrrrr = String.Format("id_vessel = '{0}' and tgl = '{1}' and id_unit = '{2}'", vsl, tgl, unt);
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



    }
}
