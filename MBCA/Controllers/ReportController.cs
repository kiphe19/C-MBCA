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

            //Response.Write("tg1x = " + dateFrom + ", tg2x = " + dateTo + ", timespanx = " + ambilTanggal.TotalDays + "\n");
            //return "sukses \n";
            //*
            //Response.Write("tipe reportnya "+ input["type"]+"\n");
            //DateTime dateFrom = (input["tgFrom"] == "") ? DateTime.Now.AddDays(-1) : Convert.ToDateTime(input["tgFrom"]);
            //DateTime dateTo = (input["tgTo"] == "") ? DateTime.Now : Convert.ToDateTime(input["tgTo"]);
            //TimeSpan ambilTanggal = dateTo - dateFrom;
            //var idvessel = (input["vesselId"] == "0") ? 0 : Convert.ToInt32(input["vesselId"]);
            var vesselname = "";
            //con.select("vessel_table", "name", string.Format("id={0}", idvessel));
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
           

            ///<summary>
            /// Create Array for Heading Table firstly
            /// columns : ["date","vessel","unitx#","unitx#"]
            /// </summary>

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

            ///<summary>
            ///end of create head table
            /// </summary>


            ///<summary>
            ///create data if each rows of tables
            /// </summary>
            /// 
            for (var i = 0; i <= ambilTanggal.TotalDays; i++)
            {
                dynamic ii = new JObject();
                JArray kk = new JArray();
                JArray isi = new JArray();

                //var tgl = dateFrom.AddDays(i).ToString("yyyy-MM-dd");
                ii.tg = dateFrom.AddDays(i).ToString("yyyy-MM-dd");
                ii.ves = vesselname;

                ////Response.Write(dateFrom.AddDays(i).ToString("yyyy-MM-dd") + "\n");


                //kk.Add(dateFrom.AddDays(i).ToString("yyyy-MM-dd"));
                //kk.Add(vesselname);
                //foreach (var ss in kk)
                //{
                //    isix.Add(kk);
                //}

                //kk.Clear();

                //datay.Add(dateFrom.AddDays(i).ToString("yyyy-MM-dd"));
                //datay.Add(vesselname);


                //isi

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
                        //isi.Add(con.result["fuel_litre"]);
                    }
                    else isi.Add(0);
                   // */
                }
                ii.datax = isi;
                all.Add(ii);
                //all.Add(ii["ves"]);
                //datay.Add(isi);
                //isix.Add(isi);
                //datay.Add(isix);


                //isi.Add(tgl);
                //isi.Add(vesselname);
                //isix.Add(isi);
                //isi.Clear();
            }

            //datay.Add(isi);

            //kolom.Add(unitnama);

            //aa.data = isix;
            aa.data = all;
            //aa.unit = unitnama;

            aa.columns = kolom;
            //aa.data = isix;
            Response.ContentType = "text/json";
            var json = JsonConvert.SerializeObject(aa);
            return json;
            //Response.Write(json);
            //return "success";
            ////var json = Newtonsoft.Json.JsonConvert.SerializeObject(aa);
            //*/
        }

        








        ////Response.Write(idvessel.ToString());
        //if (idvessel > 0)
        //{
        //    //cari_unit = string.Format("select id_unit from report_daily where tgl > '{0}' and tgl <= '{1}' and id_vessel = {2}", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), idvessel);
        //    cari_tg = string.Format("select distinct(tgl) from report_daily where tgl > '{0}' and tgl <= '{1}' and id_vessel = {2}", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), idvessel);

        //}
        //else
        //{
        //    //cari_unit = string.Format("select id_unit from report_daily where tgl > '{0}' and tgl <= '{1}'", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"));
        //    cari_tg = string.Format("select distinct(tgl) tgl from report_daily where tgl > '{0}' and tgl <= '{1}'", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"));

        //}
        ////Response.Write(cari_unit);
        //JArray unit = new JArray();
        //JArray date1 = new JArray();

        ////con.query(cari_unit);
        ////while (con.result.Read())
        ////{
        ////    unit.Add(con.result["id_unit"]);
        ////}

        ////con.Close();
        //con.query(cari_tg);
        //while (con.result.Read())
        //{
        //    date1.Add(con.result["tgl"]);
        //}
        //con.Close();
        //dynamic jj = new JObject();
        //JArray kk = new JArray();
        //JArray ll = new JArray();
        //JArray uni = new JArray();

        ////var k = 0;

        //foreach (var tg in date1)
        //{
        //    Response.Write("tanggal= "+tg + "\n");
        //    cari_unit = string.Format("select id_unit from report_daily where tgl = '{0}' and id_vessel = {1}", tg, idvessel);
        //    con.query(cari_unit);
        //    while (con.result.Read()) {
        //        //Response.Write(tg + " : vess : " +idvessel+" unit ==> " + con.result["id_unit"] + "\n");
        //        uni.Add(con.result["id_unit"]);

        //    }
        //    con.Close();

        //    foreach (var zz in uni)
        //    {
        //        //Response.Write(tg + " : vess : " + idvessel + " unit ==> " +zz + "\n");
        //        var ww = string.Format("select * from report_daily where tgl = '{0}' and id_vessel = {1} and id_unit = {2}", tg, idvessel, zz);
        //        Response.Write(ww + "\n");
        //        con.query(ww);
        //        while (con.result.Read())
        //        {
        //            Response.Write(con.result["fuel_litre"] + "--");
        //            kk.Add(con.result["fuel_litre"]);
        //            //uni.Add(con.result["name"]);
        //        }



        //    }
        //    jj.fuel = kk;
        //    ll.Add(jj);
        //    uni.Clear();
        //    kk.Clear();

        //foreach (var uuu in unit)
        //{
        //    Response.Write(tg+" : unit ==> "+uuu + "\n");

        //    //var ww = string.Format("select * from report_daily where tgl = '{0}' and id_vessel = {1} and id_unit = {2}", tg, idvessel, uuu);
        //    //Response.Write(ww + "\n");
        //    //con.query(ww);
        //    //while (con.result.Read())
        //    //{
        //    //    kk.Add(con.result["fuel_litre"]);
        //    //    uni.Add(con.result["name"]);
        //    //}

        //}
        //jj.unitx = kk;
        //jj.unitk = uni;


        //}
        //ll.Add(jj);
        //foreach( var ut in unit)
        //{
        //    JArray unun = new JArray();
        //    dynamic ee = new JObject();
        //    var vv = string.Format("select * from report_daily where tgl > '{0}' and tgl <= '{1}' and id_vessel = {2} and id_unit = {3}", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), idvessel, ut);
        //    //Response.Write(vv);
        //    con.query(vv);
        //    while (con.result.Read())
        //    {
        //        jj.tgl = con.result["tgl"];
        //        jj.ves = con.result["id_vessel"];
        //        jj.un = con.result["id_unit"];
        //        jj.fuel = con.result["fuel_tot"];
        //        //foreach (var uu in unit)
        //        //{
        //        //    ee.
        //        //}

        //    }
        //    kk.Add(jj);

        //}





        //JArray unit_id = new JArray();
        //JArray ves_id = new JArray();


        //var cari_isi = "";
        //dynamic cc = new JObject();
        //for (int i = 0; i <= ambilTanggal.TotalDays; i++)
        //{

        //    if (idvessel > 0)
        //    {
        //        cari_isi = string.Format("select id_unit,id_vessel, from report_daily where tgl > '{0}' and tgl <= '{1}' and id_vessel = {2}", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"), idvessel);
        //    }
        //    else
        //    {
        //        cari_unit = string.Format("select id_unit,id_vessel from report_daily where tgl > '{0}' and tgl <= '{1}'", dateFrom.ToString("yyyy-MM-dd"), dateTo.ToString("yyyy-MM-dd"));
        //    }
        //    con.query(cari_unit);
        //    while (con.result.Read())
        //    {
        //        unit_id.Add(con.result["id_unit"]);
        //        ves_id.Add(con.result["id_unit"]);

        //    }
        //    con.Close();



        //    cc.vessel = ves_id;
        //    cc.un = unit_id;
        //    cc.tgl = dateFrom.AddDays(i).ToString("yyyy-MM-dd");
        //    date.Add(cc);
        //}
        //aa.ddd = date;
        //aa.vessel = idvessel;
        //aa.data = ll;
        //aa.unit = unit;
        //Response.Write(ambilTanggal);
        //return Json(new { nama = "Jono" },
        //    JsonRequestBehavior.AllowGet);

        //return Json(aa, JsonRequestBehavior.AllowGet);
        //    Response.ContentType = "text/json";
        //    //var json = Newtonsoft.Json.JsonConvert.SerializeObject(aa);
        //    var json = JsonConvert.SerializeObject(aa);
        //    //*/
        //    //Response.Write(json);
        //    //return "success";
        //    return json;
        //}

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
