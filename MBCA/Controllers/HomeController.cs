using AttributeRouting;
using AttributeRouting.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables;
using chevron.Models;
using System.Globalization;
using AttributeRouting.Helpers;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Newtonsoft.Json;

namespace chevron.Controllers
{
    [RouteArea("api")]
    public class HomeController : Controller
    {
        Properties.Settings setting = chevron.Properties.Settings.Default;

        Connection con = new Connection();

        public ActionResult Index()
        {
            ViewBag.daily_vessel = getListVessel();
            ViewBag.daily_vesselid = getListVesselId();

            //ViewBag.daily_activity = getListActivity();
            ViewBag.daily_unit = getUserUnit();
            ViewBag.daily_unitid = getUserUnitId();
            return View();
        }

        /// <summary>
        /// Dropdown
        /// </summary>

        private List<SelectListItem> getListVessel()
        {
            List<SelectListItem> vessel = new List<SelectListItem>();

            con.select("vessel_table", "name");
            while (con.result.Read())
            {
                vessel.Add(new SelectListItem
                {
                    Text = con.result["name"].ToString(),
                    Value = con.result["name"].ToString()
                });
            }
            con.Close();

            var VesselSorted = (from li in vessel orderby li.Text select li).ToList();

            return VesselSorted;
        }
        private List<SelectListItem> getListVesselId()
        {
            List<SelectListItem> vessel = new List<SelectListItem>();

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


        private List<SelectListItem> getUserUnit()
        {
            List<SelectListItem> unit = new List<SelectListItem>();

            con.select("unit_table", "id, name");
            while (con.result.Read())
            {
                unit.Add(new SelectListItem
                {
                    Text = con.result["name"].ToString(),
                    Value = con.result["name"].ToString()
                });
            }
            con.Close();

            var unitSorted = (from li in unit orderby li.Text select li).ToList();

            return unitSorted;
        }

        private List<SelectListItem> getUserUnitId()
        {
            List<SelectListItem> unit_id = new List<SelectListItem>();

            con.select("unit_table", "id, name");
            while (con.result.Read())
            {
                unit_id.Add(new SelectListItem
                {
                    Text = con.result["name"].ToString(),
                    Value = con.result["id"].ToString()
                });
            }
            con.Close();
            //unit_id.Insert(0, "All");
            unit_id.Insert(0, (new SelectListItem { Text = "All Unit", Value = "0" }));
            var unitidSorted = (from li in unit_id orderby li.Text select li).ToList();

            return unitidSorted;
        }

        /// <summary>
        /// END DROPDOWn
        /// </summary>
        /// <returns></returns>


        [Route("daily/{id}")]
        public ActionResult _ApiDaily(int id)
        {


            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                if (id > 0)
                {
                    var response = new Editor(db, "detail_daily_table")
                    .Model<DailyUnitVesselModel>()
                    .LeftJoin("unit_table", "unit_table.id", "=", "detail_daily_table.id_unit")
                    .Where("detail_daily_table.id_daily_table", id, "=")
                    //.Where("date_input", DateTime.Today.ToString("yyyy-MM-dd"), "=")
                    .Process(request)
                    .Data();
                    return Json(response);
                }
                else
                {
                    var response = new Editor(db, "temp_daily")
                    .Model<TempDailyModel>()
                    .LeftJoin("unit_table", "unit_table.id", "=", "temp_daily.id_unit")
                    .Where("user_log", Session["userid"], "=")
                    .Where("date_input", DateTime.Today.ToString("yyyy-MM-dd"), "=")
                    .Process(request)
                    .Data();
                    return Json(response);
                }
            }
        }

        [Route("dailylog/{tg1}/{tg2}")]
        public ActionResult _LogDaily(string tg1, string tg2)
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "daily_table")
                    .Model<DailyTableModel>()
                    .Field(new Field("daily_table.tgl")
                        .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy"))
                        .Validator(Validation.NotEmpty())
                    )
                    //.LeftJoin("unit_table", "unit_table.id", "=", "daily_table.id_unit")
                    .LeftJoin("vessel_table", "vessel_table.id", "=", "daily_table.id_vessel")

                    //.Where("daily_table.tgl", tg1, ">")
                    //.Where("daily_table.tgl", tg2, "<=")
                    //.Where("date_input", DateTime.Today.ToString("yyyy-MM-dd"), "=")
                    //.Where("user_log", Session["userid"], "=")

                    //.Field(new Field("date_input")
                    //    .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy"))
                    //    .Validator(Validation.NotEmpty())
                    //)
                    .Process(request)
                    .Data();
                return Json(response);
            }
        }


        [Route("drill/{tg1}/{tg2}/{unitx}")]
        public ActionResult _dataDrillCompletion(string tg1, string tg2, int unitx)
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                if (unitx > 0)
                {
                    var response = new Editor(db, "drilling_table")
                        .Model<DrillModel>()
                        .Field(new Field("drilling_table.tgl")
                            .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy"))
                            .Validator(Validation.NotEmpty())
                        )
                        .Field(new Field("drilling_table.t_start")
                            .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "H:m"))
                            .Validator(Validation.NotEmpty())
                        )
                        .Field(new Field("drilling_table.t_end")
                            .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "H:m"))
                            .Validator(Validation.NotEmpty())
                        )
                        .LeftJoin("unit_table", "unit_table.id", "=", "drilling_table.id_unit")
                         //.LeftJoin("mainunit_table","")
                         .Where("drilling_table.tgl", tg1, ">")
                         .Where("drilling_table.tgl", tg2, "<=")
                         .Where("drilling_table.id_unit", unitx, "=")
                        .Process(request).Data();
                    return Json(response);
                }
                else
                {
                    var response = new Editor(db, "drilling_table")
                        .Model<DrillModel>()
                        .Field(new Field("drilling_table.tgl")
                            .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy"))
                            .Validator(Validation.NotEmpty())
                        )
                        .Field(new Field("drilling_table.t_start")
                            .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "H:m"))
                            .Validator(Validation.NotEmpty())
                        )
                        .Field(new Field("drilling_table.t_end")
                            .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "H:m"))
                            .Validator(Validation.NotEmpty())
                        )
                        .LeftJoin("unit_table", "unit_table.id", "=", "drilling_table.id_unit")
                        //.LeftJoin("mainunit_table","")
                        .Where("drilling_table.tgl", tg1, ">")
                        .Where("drilling_table.tgl", tg2, "<=")

                        .Process(request).Data();

                    //Response.Write(unitx);
                    return Json(response);
                }
                //var response1 =  new Editor(db.Select("drilling_table",))
            }
        }


        [Route("cs/daily")]
        [HttpPost]
        public String _dailyInsert(FormCollection input)
        {
            String query = "";
            var tanggal = (input["date_timeat"] == "") ? DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") : Convert.ToDateTime(input["date_timeat"]).ToString("yyyy-MM-dd");
            var cariunit = string.Format("id_unit = {0} and tgl = '{1}' and distance is not null and id_mainunit is not null", input["daily_unitid"], tanggal);
            con.select("unit_distance_table", "id_unit",cariunit);
            con.result.Read();
            //con.Close();
            if (con.result.HasRows) {
                switch (input["action"])
                {
                    case "create":
                        var val_unit = string.Format("date_input = '{0}' and user_log = '{1}' and id_unit = {2}", DateTime.Today.ToString("yyyy-MM-dd"), Session["userid"], input["daily_unitid"]);
                        con.select("temp_daily", "id_unit", val_unit);
                        con.result.Read();
                        if (con.result.HasRows)
                        {
                            query = string.Format("update temp_daily set duration = {0} where date_input = '{1}' and user_log = '{2}' and id_unit = {3}"
                                    , input["time_at_dur"].ToString(CultureInfo.InvariantCulture), DateTime.Today.ToString("yyyy-MM-dd"), Session["userid"], input["daily_unitid"]);
                        }
                        else
                        {
                            query = string.Format("insert into temp_daily (id_unit,duration,user_log,date_input) values ({0},{1},'{2}','{3}')"
                                    , input["daily_unitid"], input["time_at_dur"].ToString(CultureInfo.InvariantCulture), Session["userid"], DateTime.Today.ToString("yyyy-MM-dd"));
                        }
                        con.Close();
                        break;
                    case "update":
                        query = string.Format("update temp_daily set id_unit = {0}, duration = {1} where user_log = '{2}' and date_input = '{3}' and id = {4}",
                                input["daily_unitid"], input["time_at_dur"].ToString(CultureInfo.InvariantCulture), Session["userid"], DateTime.Today.ToString("yyyy-MM-dd"), input["id"]);
                        break;
                    default:
                        break;
                }
                con.queryExec(query);
                return "success";
                
            }
            else
            {
                return "Unit " + input["daily_unitid"] + " pada tanggal " + tanggal + " tidak ditemukan, hubungi Administrator";
            }
           
           
        }

        [Route("save/daily")]
        [HttpPost]
        public string _saveDailyData(FormCollection input)
        {
            var stb     = (input["standby"] == "") ? Convert.ToInt16(0) :  Convert.ToDecimal(input["standby"]);
            var load    = (input["load"] == "") ? Convert.ToInt16(0) : Convert.ToDecimal(input["load"]);
            var steam   = (input["steaming"] == "") ? Convert.ToInt16(0) : Convert.ToDecimal(input["steaming"]);
            var down    = (input["downtime"] == "") ? Convert.ToInt16(0) : Convert.ToDecimal(input["downtime"]);
            var fuel    = (input["daily_fuel"] == "") ? Convert.ToInt16(0) : Convert.ToInt32(input["daily_fuel"]);
            var tanggal = (input["daily_date"] == "") ? DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") : Convert.ToDateTime(input["daily_date"]).ToString("yyyy-MM-dd");
            var cekmob = (input["mob"] == "1") ? (int)1 : 0;
            //Response.Write("isi cek mob " + cekmob + "  ;");

            //save ke daily_table sebagai log .. divalidasi dahulu
            var q_save_act = "";
            var cek1 = string.Format("id_vessel = {0} and tgl = '{1}'", input["daily_vesselid"], tanggal);
            con.select("daily_table", "id", cek1);

            con.result.Read();
            if (con.result.HasRows)
            {
                //Response.Write("ada isinya ");
                q_save_act = string.Format("update daily_table set standby = {0}, loading = {1}, steaming= {2}, downtime = {3}, fuel_tot ={4}, user_log = '{5}', date_input = '{6}', mob_status= {7} "
                               + "where tgl = '{8}'  and id_vessel = {9}",
                               stb, load, steam, down, fuel, Session["userid"], DateTime.Now.ToString("yyyy-MM-dd"), cekmob, tanggal, input["daily_vesselid"]);
            }
            else {
                //Response.Write("ga ada isinya");
                q_save_act = string.Format("insert into daily_table (tgl,id_vessel,standby,loading,steaming,downtime,fuel_tot,user_log,date_input,mob_status) values "
                               + " ('{0}',{1},{2},{3},{4},{5},{6},'{7}','{8}',{9} ) ",
                               tanggal, input["daily_vesselid"], stb, load, steam, down, fuel, Session["userid"], DateTime.Now.ToString("yyyy-MM-dd"), cekmob);

            }
            con.Close();
            //Response.Write(q_save_act);
            con.queryExec(q_save_act);

            //Response.Write(tanggal);
            var skr = DateTime.Now.ToString("yyyy-MM-dd");

            //cari id daily_table yang baru saja di input;
            var id_daily = 0;
            //string q_detail = null;
            var cari_input = string.Format("tgl = '{0}' and id_vessel = {1} and date_input = '{2}'", tanggal, input["daily_vesselid"],skr);
            con.select("daily_table", "id", cari_input);
            con.result.Read();
            if (con.result.HasRows) id_daily = (int)con.result["id"];
            con.Close();
            //Response.Write("id dailynya adh "+ id_daily+" ; ");

            //ambil data table time at table temp_daily 
            List<DailyUnitActivityModel> usernit = new List<DailyUnitActivityModel>();
            var qunit = string.Format("select td.id_unit,td.duration,dt.distance, dt.id_mainunit from temp_daily td "
                + " inner join unit_distance_table dt on dt.id_unit = td.id_unit "
                + " where td.user_log = '{0}' and td.date_input = '{1}' and dt.tgl = '{2}' ", Session["userid"], skr, tanggal);

            //Response.Write(qunit);
            con.query(qunit);
            while (con.result.Read())
            {
                //Response.Write("isinya " + con.result["id_unit"] + ", jarak " + con.result["distance"] + ", durasi " + con.result["duration"] + " , mainunit " + con.result["id_mainunit"]+"\n");
                usernit.Add(new DailyUnitActivityModel
                {
                    id_mainunit = (int)con.result["id_mainunit"],
                    id_unit = (int)con.result["id_unit"],
                    durasi = (decimal)con.result["duration"],
                    jarak = (int)con.result["distance"],
                    hit = Convert.ToInt16(1)
                });
            }
            con.Close();

            //input ke detail_daily_table
            decimal total_jarak = 0, tot_hit = 0;
            var q_detail = "";

            var c = string.Format("id_daily_table = {0}", id_daily);
            con.select("detail_daily_table", "id", c);
            con.result.Read();
            if (con.result.HasRows) {
                //Response.Write("detail daily isi");
                var del_det = string.Format("delete from detail_daily_table where id = {0}",id_daily);
                con.queryExec(del_det);
                foreach (DailyUnitActivityModel unit in usernit)
                {
                    total_jarak += unit.jarak;
                    tot_hit += unit.hit;
                    q_detail = string.Format("insert into detail_daily_table (id_daily_table,id_unit,id_mainunit,distance,duration) values ({0},{1},{2},{3},{4})",
                                   id_daily, unit.id_unit, unit.id_mainunit, unit.jarak, unit.durasi);
                    //Response.Write(q_detail);
                    con.queryExec(q_detail);
                }
            }
            else
            {
                //Response.Write("detail daily kosong");
                foreach (DailyUnitActivityModel unit in usernit)
                {
                    total_jarak += unit.jarak;
                    tot_hit += unit.hit;
                    q_detail = string.Format("insert into detail_daily_table (id_daily_table,id_unit,id_mainunit,distance,duration) values ({0},{1},{2},{3},{4})",
                                   id_daily, unit.id_unit, unit.id_mainunit, unit.jarak, unit.durasi);
                    //Response.Write(q_detail);
                    con.queryExec(q_detail);
                }
            }
            con.Close();
            //Response.Write(" total jarak : " + total_jarak + " dan hitnya : " + tot_hit + " ==> ");

            //buat hitungan data daily_report

            //buat variabel harga fuel per hari di input
            decimal harga, curr_harga;
            var cari_fuel = string.Format("tgl = '{0}'", tanggal);
            con.select("fuel_table", "cost_usd, currency_type", cari_fuel);
            con.result.Read();
            if (con.result.HasRows)
            {
                harga = Convert.ToDecimal(con.result["cost_usd"]);
                curr_harga = Convert.ToDecimal(con.result["currency_type"]);
            }
            else
            {
                harga = 0;
                curr_harga = 0;
            }
            con.Close();

            //buat variable charter_rate
            decimal charter_rate, mob_demob_rate, curr_charter, mob_cost, period;
            var cari_charter = string.Format("tgl_start <= '{0}' and tgl_end >= '{0}' and id_vessel = {1}", tanggal, input["daily_vesselid"]);
            con.select("hire_table", "cost_usd, mob_cost,curency_cat, mob_rate, periode", cari_charter);
            con.result.Read();
            if (con.result.HasRows)
            {
                charter_rate = (decimal)con.result["cost_usd"];
                mob_cost = (cekmob > 0) ? (decimal)con.result["mob_cost"] : 0;
                mob_demob_rate = (cekmob > 0) ? (decimal)con.result["mob_rate"] : 0;
                curr_charter = (int)con.result["curency_cat"];
                period = (int)con.result["periode"];
            }
            else {
                charter_rate = 0; mob_cost = 0; mob_demob_rate = 0; period = 0; curr_charter = 0;
            }
            con.Close();

            Response.Write("charter_rate = " + charter_rate + ", mob_cost = " + mob_cost + ", mob_demob_rate = " + mob_demob_rate + ", period = " + period + ", curr_charter = " + curr_charter);

            //buat variabel yang digunakan untuk save ke report;
            decimal t_standby = 0, t_load = 0, t_steam = 0, t_all = 0, t_boat_h = 0, fuel_l = 0, fuel_price = 0, charter_price = 0, t_stb_mob = 0, t_all_mob = 0, mob_price = 0;

            //simpan ke daily_report

            var d = string.Format("id_daily_table = {0}", id_daily);
            con.select("report_daily", "id", d);
            con.result.Read();
            if (con.result.HasRows)
            {
                Response.Write("ada isinya di report daily\n") ;
                var del_rep = string.Format("delete from report_daily where id_daily_table = {0}", id_daily);
                con.queryExec(del_rep);
                foreach (DailyUnitActivityModel isiunit in usernit)
                {
                    t_standby = stb / tot_hit;
                    t_load = load / tot_hit;
                    t_steam = steam * (isiunit.jarak / total_jarak);
                    t_all = t_standby + t_load + t_steam + isiunit.durasi; //t active hours
                    t_boat_h = stb * (t_all / 24) + t_all; 
                    fuel_l = (t_all / (24 - down)) * fuel;
                    fuel_price = harga * fuel_l;
                    charter_price = t_all * charter_rate / 24;
                    t_stb_mob = stb * (t_load + t_steam + isiunit.durasi);
                    t_all_mob = t_stb_mob + t_load + t_steam + isiunit.durasi;
                    mob_price = t_all_mob * (mob_demob_rate / 24);

                    //simpan ke report_daily untuk Reporting
                    var q_rpt1 = string.Format("insert into report_daily (tgl,id_vessel,id_unit,t_standby,t_load,t_steam,t_down,t_durasi,t_all,fuel_litre,fuel_price,fuel_curr,charter_price,charter_curr,mob_price,id_mainunit,id_daily_table,t_boat_h) "
                            + " values ('{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}); ",
                            tanggal, input["daily_vesselid"], isiunit.id_unit, t_standby, t_load, t_steam, down, isiunit.durasi, t_all, fuel_l, fuel_price, curr_harga, charter_price, curr_charter, mob_price, isiunit.id_mainunit, id_daily, t_boat_h);
                    Response.Write(q_rpt1);
                    con.queryExec(q_rpt1);
                }
               


            }
            else {
                Response.Write("isnya masih kosong bro\n");
                foreach (DailyUnitActivityModel isiunit in usernit)
                {
                    t_standby = stb / tot_hit;
                    t_load = load / tot_hit;
                    t_steam = steam * (isiunit.jarak / total_jarak);
                    t_all = t_standby + t_load + t_steam + isiunit.durasi;
                    t_boat_h = stb * (t_all / 24) + t_all;
                    fuel_l = (t_all / (24 - down)) * fuel;
                    fuel_price = harga * fuel_l;
                    charter_price = t_all * charter_rate / 24;
                    t_stb_mob = stb * (t_load + t_steam + isiunit.durasi);
                    t_all_mob = t_stb_mob + t_load + t_steam + isiunit.durasi;
                    mob_price = t_all_mob * (mob_demob_rate / 24);

                    //simpan ke report_daily untuk Reporting
                    var q_rpt1 = string.Format("insert into report_daily (tgl,id_vessel,id_unit,t_standby,t_load,t_steam,t_down,t_durasi,t_all,fuel_litre,fuel_price,fuel_curr,charter_price,charter_curr,mob_price,id_mainunit,id_daily_table,t_boat_h) "
                            + " values ('{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}); ",
                            tanggal, input["daily_vesselid"], isiunit.id_unit, t_standby, t_load, t_steam, down, isiunit.durasi, t_all, fuel_l, fuel_price, curr_harga, charter_price, curr_charter, mob_price, isiunit.id_mainunit,id_daily,t_boat_h);
                    Response.Write(q_rpt1);
                    con.queryExec(q_rpt1);
                }
            }

            
            //delete setelah input
            var qdelete = string.Format("delete temp_daily where date_input = '{0}' and user_log = '{1}'", skr, Session["userid"]);
            Response.Write(qdelete);
            con.queryExec(qdelete);

            return "success";
        }


        [Route("filter/monthly")]
        [HttpPost]
        public ActionResult _MonthlyGrid(FormCollection input)
        {
            List<MonthlyActivityModel> data = new List<MonthlyActivityModel>();
            var tgl_e = input["tgl_e"];
            var tgl_f = input["tgl_eop"];
            var nowDate = DateTime.Today.ToString("MM/dd/yyyy");

            if (string.IsNullOrEmpty(tgl_e) && string.IsNullOrEmpty(tgl_f))
            {
                tgl_e = nowDate;
                tgl_f = nowDate;
            }
            var query = String.Format("select * from monthly_activity where tgl between '{0}' and '{1}'", tgl_e, tgl_f);
            con.query(query);
            while (con.result.Read())
            {
                data.Add(new MonthlyActivityModel
                {
                    tgl = DateTime.Parse(con.result["tgl"].ToString()).ToString("MM/dd/yyyy"),
                    activity = con.result["activity"].ToString(),
                    duration = Decimal.Parse(con.result["duration"].ToString()),
                    unit = con.result["unit"].ToString(),
                    vessel = con.result["vessel"].ToString()
                });
            }
            //ViewBag.data = data;
            return PartialView("_MonthlyGrid", data);
        }

        [Route("save/drill")]
        [HttpPost]
        public string _saveDailyDrill(FormCollection input)
        {
            var awal = Convert.ToDateTime(input["t_start"]);
            var akhir = Convert.ToDateTime(input["t_end"]);
            TimeSpan dur = akhir - awal;
            string qq = null;
            //Response.Write(input["action"]);

            if (input["action"] == "create")
            {
                qq = string.Format("insert into drilling_table (id_unit,well,afe,psc_no,tgl,t_start,t_end,durasi) values ({0},'{1}','{2}','{3}','{4}','{5}','{6}',{7})", input["daily_unitid"], input["well"], input["afe"], input["psc"], input["drill_date"], input["t_start"], input["t_end"], dur.TotalHours);

            }
            else if (input["action"] == "update") {
                qq = string.Format("update drilling_table set id_unit={0}, well='{1}', afe = '{2}',psc_no='{3}',tgl = '{4}',t_start = '{5}',t_end='{6}',durasi={7} where id = {8}", input["daily_unitid"], input["well"], input["afe"], input["psc"], input["drill_date"], input["t_start"], input["t_end"], dur.TotalHours,input["id"]);

            }
            //Response.Write(input["mob"]);
            //Response.Write(qq);
            con.queryExec(qq);
            return "success";
        }

        //public ActionResult BuatJson() {

        //    //select * from daily_table join unit_table on unit_table.Id = daily_table.id_unit
        //    dynamic un = new ExpandoObject();
        //    JArray unitx = new JArray();
        //    JArray durx = new JArray();
        //    JArray isinya = new JArray();
        //    //List<string> unitk = new List<string>();
        //    var qry = string.Format("select * from daily_table join unit_table on unit_table.Id = daily_table.id_unit");
        //    con.select("daily_table join unit_table on unit_table.Id = daily_table.id_unit","*");
        //    //con.
        //    //con.queryExec(qry);
        //    while (con.result.Read())
        //    {
        //        unitx.Add(con.result["name"]);
        //        durx.Add(con.result["duration"]);

        //        //unitk.Add( new 
        //        //{
        //        //     = con.result["name"]
        //        //});
        //        foreach (var isik in unitx)
        //        {
        //            un.Nama = isik;
        //        }
        //        foreach (var lama in durx)
        //        {
        //            un.Durasi = lama;
        //        }
        //        isinya.Add(un);
        //    }
        //    con.Close();

        //    dynamic kk = new ExpandoObject();
        //    kk.auuu = isinya;
            



        //    //var qunit = string.Format("select td.id_unit,td.duration,dt.distance, dt.id_mainunit "
        //    //    + " from temp_daily td inner join unit_distance_table dt on dt.id_unit = td.id_unit "
        //    //    + " where td.user_log = '{0}' and td.date_input = '{1}' and dt.tgl = '{2}' ", Session["userid"], skr, tanggal);

        //    ////Response.Write(qunit);

        //    ////*
        //    //con.query(qunit);
        //    //while (con.result.Read())
        //    //{
        //    //    usernit.Add(new DailyUnitActivityModel
        //    //    {
        //    //        id_mainunit = (int)con.result["id_mainunit"],
        //    //        id_unit = (int)con.result["id_unit"],
        //    //        durasi = (decimal)con.result["duration"],
        //    //        jarak = (int)con.result["distance"],
        //    //        hit = Convert.ToInt16(1)
        //    //    });
        //    //}
        //    //con.Close();

        //    //con.select()






        //    //dynamic b = new JObject();
        //    dynamic aa = new ExpandoObject();
        //    aa.Nama = "jono";
        //    aa.Alamat = "Amerika";
        //    //b.Nama = "Jono";
        //    //b.Alamat = "Amerika";

        //    string[] hari = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        //    //hari.a
        //    aa.Minggu = hari;
            

        //    List<string> list = new List<string>();
        //    //list.Add("one");
        //    //list.Add("two");
        //    //list.Add("three");

        //    for (int i = 0; i < 10; i++)
        //    {
        //        list.Add("isi ke-" + i);
        //    }

        //    string[] itung = list.ToArray();
        //    int u = 10;
        //    string[] isi = new string[u];
        //    for (int j = 0; j < u; j++)
        //    {
        //        isi[j] = "haloo ke - " + j;
        //    }

        //    aa.Hitung = itung;
        //    aa.Isi = isi;
        //    //string a = "jono";
        //    //return Json(new { nama = a },
        //    //return Json(aa, JsonRequestBehavior.AllowGet);
        //    //Response.ContentType = "text/json";
        //    //var json = Newtonsoft.Json.JsonConvert.SerializeObject(b);
        //    //return Response.Write();

        //    //var workbook = new ExcelFile();

        //    //var json = JsonConvert.SerializeObject(aa);
        //    var json = JsonConvert.SerializeObject(kk);

        //    return Content(json, "application/json");
        //}
    }
}
