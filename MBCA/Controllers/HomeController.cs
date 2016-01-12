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

            con.select("unit_table", "name");
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
                .Where("tgl", DateTime.Now.ToString("yyyy-dd-MM"), "=")
                .Field(new Field("duration").Validator(Validation.Numeric()))
                .Field(new Field("tgl")
                    .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy"))
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
            var query = string.Format("insert into daily_activity ([tgl],[vessel],[activity],[duration], [unit], [fuel]) values ('{0}','{1}','{2}','{3}', '{4}', CAST('{5}' AS INT))", input["daily_date"], input["daily_vessel"], input["daily_activity"], input["daily_duration"].ToString(CultureInfo.InvariantCulture), input["daily_unit"], input["daily_fuel"]);
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
            var query = string.Format("update daily_activity set activity='{0}', unit='{1}', fuel={2}, duration={3} where id={4}", input["daily_activity"], input["daily_unit"], input["daily_fuel"], input["daily_duration"].ToString(CultureInfo.InvariantCulture), input["daily_type"]);
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
            List<DailyActivityModel> dataDaily = new List<DailyActivityModel>();

            var where = string.Format("tgl = '{0}'", DateTime.Now.ToString("yyyy-MM-dd"));

            con.select("daily_activity", "tgl, vessel, activity, duration, unit", where);

            while (con.result.Read())
            {
                dataDaily.Add(new DailyActivityModel
                {
                    activity = con.result["activity"].ToString(),
                    duration = (decimal)con.result["duration"],
                    tgl = DateTime.Parse(con.result["tgl"].ToString()).ToShortDateString(),
                    vessel = con.result["vessel"].ToString(),
                    unit = con.result["unit"].ToString()
                });
            }

            con.Close();

            foreach (DailyActivityModel daily in dataDaily)
            {
                var query = String.Format("insert into monthly_activity ([tgl], [vessel], [activity], [duration], [unit]) values('{0}', '{1}', '{2}', CAST('{3}' AS numeric(18,2)), '{4}')", daily.tgl, daily.vessel, daily.activity, daily.duration.ToString(CultureInfo.InvariantCulture), daily.unit);
                con.queryExec(query);
                Response.Write("true");
            }

            var kueri = string.Format("delete from daily_activity where tgl= '{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
            con.queryExec(kueri);
        }

    }
}
