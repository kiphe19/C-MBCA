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

namespace chevron.Controllers
{
    [RouteArea("api")]
    public class HomeController : Controller
    {
        chevron.Properties.Settings setting = chevron.Properties.Settings.Default;

        Connection con = new Connection();

        public ActionResult Index()
        {
            ViewBag.daily_vessel = getListVessel();
            ViewBag.daily_activity = getListActivity();
            ViewBag.daily_unit = getUserUnit();
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

        private List<SelectListItem> getListActivity()
        {
            List<SelectListItem> activity = new List<SelectListItem>();

            con.select("activity_table", "name");
            while (con.result.Read())
            {
                activity.Add(new SelectListItem
                {
                    Text = con.result["name"].ToString(),
                    Value = con.result["name"].ToString()
                });
            }
            con.Close();

            var ActivitySorted = (from li in activity orderby li.Text select li).ToList();

            return ActivitySorted;
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

        /// <summary>
        /// END DROPDOWn
        /// </summary>
        /// <returns></returns>


        [Route("daily")]
        public ActionResult _ApiDaily()
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "daily_activity")
                .Model<DailyActivityModel>()
                .Where("tgl", DateTime.Now.ToString("yyyy-MM-dd"), "=")
                .Field(new Field("duration").Validator(Validation.Numeric()))
                .Field(new Field("tgl")
                    .GetFormatter(Format.DateTime("dd/MM/yyyy H:m:s", "MM/dd/yyyy"))
                    .Validator(Validation.NotEmpty())
                )
                .Process(request)
                .Data();

                return Json(response);
            }
        }

        [Route("monthly")]
        public ActionResult _ApiMonthly()
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "monthly_activity")
                .Model<MonthlyActivityModel>()
                .Field(new Field("tgl")
                    .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy"))
                    .Validator(Validation.NotEmpty())
                )
                .Field(new Field("duration").Validator(Validation.Numeric()))
                .Process(request)
                .Data();

                return Json(response);
            }
        }

        [Route("dataa")]
        public ActionResult _dataActivity()
        {
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "activity_table")
                .Model<ActivityModel>()
                .Process(Request.Form)
                .Data();

                return Json(response);
            }
        }

        [Route("datau")]
        public ActionResult _dataUnit()
        {
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "unit_table")
                .Model<UnitModel>()
                .Process(Request.Form)
                .Data();

                return Json(response);
            }
        }

        [Route("datav")]
        public ActionResult _dataVessel()
        {
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "vessel_table")
                .Model<VesselModel>()
                .Process(Request.Form)
                .Data();

                return Json(response);
            }
        }

        [Route("cs/daily")]
        [HttpPost]
        public String _dailyInsert(FormCollection input)
        {
            var q = string.Format("name = '{0}'", input["daily_unit"]);
            con.select("unit_table", "cat", q);
            con.result.Read();
            q = con.result["cat"].ToString();
            var query = string.Format("insert into daily_activity ([tgl],[vessel],[activity],[duration], [unit], [fuel], [unit_cat]) values (CAST('{0}' AS DATE),'{1}','{2}','{3}', '{4}', CAST('{5}' AS INT), {6})", input["daily_date"], input["daily_vessel"], input["daily_activity"], input["daily_duration"].ToString(CultureInfo.InvariantCulture), input["daily_unit"], input["daily_fuel"], q);
            try
            {
                con.queryExec(query);
                return "success";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        [Route("cs/daily/up")]
        [HttpPost]
        public String _dailyUpdate(FormCollection input)
        {
            var q = string.Format("name = '{0}'", input["daily_unit"]);

            con.select("unit_table", "cat", q);
            con.result.Read();
            q = con.result["cat"].ToString();

            var query = string.Format("update daily_activity set activity='{0}', unit='{1}', fuel={2}, duration={3}, unit_cat={5} where id={4}", input["daily_activity"], input["daily_unit"], input["daily_fuel"], input["daily_duration"].ToString(CultureInfo.InvariantCulture), input["daily_type"], q);
            try
            {
                con.queryExec(query);
                return "success";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        [Route("save/daily")]
        [HttpPost]
        public void saveDailytoMonthly()
        {
            var nowDate = DateTime.Now.ToString("yyyy-MM-dd");
            var kueriDelete = string.Format("delete from daily_activity where tgl= '{0}'", nowDate);
            List<DailyActivityModel> dataDaily = new List<DailyActivityModel>();

            var where = string.Format("tgl = '{0}'", nowDate);
            con.select("daily_activity", "tgl, vessel, activity, duration, unit", where);

            while (con.result.Read())
            {
                dataDaily.Add(new DailyActivityModel
                {
                    activity = con.result["activity"].ToString(),
                    duration = (decimal)con.result["duration"],
                    tgl = DateTime.Parse(con.result["tgl"].ToString()).ToString("MM/dd/yyy"),
                    vessel = con.result["vessel"].ToString(),
                    unit = con.result["unit"].ToString(),
                });
            }

            con.Close();

            foreach (DailyActivityModel daily in dataDaily)
            {
                var Query = String.Format("insert into monthly_activity ([tgl], [vessel], [activity], [duration], [unit]) \n" +
                        "values('{0}', '{1}', '{2}', {3}, '{4}')",
                        daily.tgl, daily.vessel, daily.activity, daily.duration.ToString(CultureInfo.InvariantCulture), daily.unit
                    );
                con.queryExec(Query);
            }

            List<ReportModelCS> ya = new List<ReportModelCS>();
            List<ReportModelCS> tidak = new List<ReportModelCS>();
            Decimal standby = 0, load = 0, steaming = 0, countDistance = 0,downTime = 0;
            Decimal rupiah = 0, dollar = 0, totalFuel = 0;

            var query = "select vt.id as vessel_id, da.vessel, da.unit, da.duration, da.fuel, ut.distance, (select sum(unit_cat) from daily_activity) as jml \n" +
                                       "from daily_activity da \n" +
                                       "inner join vessel_table vt \n" +
                                       "on vt.name = da.vessel \n" +
                                       "inner join unit_table ut \n" +
                                       "on da.unit = ut.name \n" +
                                       "where da.unit_cat=1 and da.tgl = '" + nowDate + "'";

            var query2 = "select vt.id as vessel_id, da.vessel, da.activity, da.duration, da.fuel from daily_activity da \n" +
                                       "inner join vessel_table vt \n" +
                                       "on vt.name = da.vessel \n" +
                                       "inner join unit_table ut \n" +
                                       "on da.unit = ut.name \n" +
                                       "where da.unit_cat!=1";

            var query3 = "select top 1 cost_usd, cost_rp from fuel_table";

            try
            {
                con.query(query);
                while (con.result.Read())
                {
                    ya.Add(new ReportModelCS
                    {
                        vessel_id = int.Parse(con.result["vessel_id"].ToString()),
                        vessel_name = con.result["vessel"].ToString(),
                        duration = Decimal.Parse(con.result["duration"].ToString()),
                        distance = int.Parse(con.result["distance"].ToString()),
                        fuel = int.Parse(con.result["fuel"].ToString()),
                        jml = int.Parse(con.result["jml"].ToString()),
                        unit = con.result["unit"].ToString()
                    });
                }
                con.Close();

                con.query(query2);
                while (con.result.Read())
                {
                    tidak.Add(new ReportModelCS
                    {
                        vessel_id = int.Parse(con.result["vessel_id"].ToString()),
                        vessel_name = con.result["vessel"].ToString(),
                        duration = Decimal.Parse(con.result["duration"].ToString()),
                        activity = con.result["activity"].ToString(),
                    });
                }
                con.Close();

                con.query(query3);
                while (con.result.Read())
                {
                    dollar = Decimal.Parse(con.result["cost_usd"].ToString());
                    rupiah = Decimal.Parse(con.result["cost_rp"].ToString());
                }
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
            }
            finally
            {
                con.Close();
            }


            foreach (var item in ya)
            {
                countDistance += item.distance; //Hitung jumlah jarak 
            }

            foreach (var item in ya)
            {
                for (int i = 0; i < tidak.Count; i++)
                {
                    switch (tidak[i].activity)
                    {
                        case "Standby":
                            standby = tidak[i].duration / item.jml;
                            break;
                        case "Load":
                            load = tidak[i].duration / item.jml;
                            break;
                        case "Steaming Time":
                            steaming = tidak[i].duration * (item.distance / countDistance);
                            break;
                        case "Downtime":
                            downTime = tidak[i].duration;
                            break;
                        default:
                            break;
                    }
                }

                //Response.Write("Standby = " + standby.ToString("f2") + " Load = " + load.ToString("f2") + " Steaming = " + steaming.ToString("f2") + "<br />");
                Decimal hasil = standby + load + steaming + item.duration;
                //hasil = hasil;

                //Response.Write("Hasilnya adalah == " + hasil.ToString("f3") + "<br />");
                //Response.Write("ini downtime : " + downTime + "<br/>");

                totalFuel = item.fuel / (24 - downTime);

                Decimal duit1 = hasil * dollar * totalFuel,
                        duit2 = hasil * rupiah * totalFuel;

                var qp = String.Format(
                    "insert into report_table ([vessel_id], [vessel_name], [unit], [date], [fuel_liter], [fuel_usd], [fuel_rp], [standby_time], [load_time], [steaming_time], [down_time]) \n" +
                    "VALUES ({0}, '{1}', '{2}', '{3}', {4}, {5}, {6}, {7}, {8}, {9}, {10})",
                     item.vessel_id, item.vessel_name, item.unit, nowDate, totalFuel.ToString("n3", CultureInfo.InvariantCulture), duit1.ToString("f3", CultureInfo.InvariantCulture), duit2.ToString("f3", CultureInfo.InvariantCulture), standby.ToString(CultureInfo.InvariantCulture), load.ToString(CultureInfo.InvariantCulture), steaming.ToString(CultureInfo.InvariantCulture), downTime.ToString(CultureInfo.InvariantCulture)
                    );
                con.queryExec(qp);
                //Response.Write(qp + "<br><hr>");
            }

            //Response.Write("<hr><hr> $" + dollar + "<br>Rp" + rupiah);
            con.queryExec(kueriDelete);
            Response.Write("true");
        }
    }
}
