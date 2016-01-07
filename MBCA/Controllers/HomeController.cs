using AttributeRouting;
using AttributeRouting.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables;
using chevron.Models;

namespace chevron.Controllers
{
    [RouteArea("api")]
    public class HomeController : Controller
    {
        chevron.Properties.Settings setting = chevron.Properties.Settings.Default;
        
        Connection con = new Connection();

        public ActionResult Index()
        {
            return View();
        }

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
                .Field(new Field("tgl").GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "dd/MM/yyyy")))
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
        public void _dailyInsert(FormCollection input)
        {
            var query = string.Format("insert into daily_activity ([tgl],[vessel],[activity],[duration]) values ('{0}','{1}','{2}','{3}')", input["daily_date"], input["daily_vessel"], input["daily_activity"], input["daily_duration"]);
            con.queryExec(query);
            Response.Write("true");
        }

    }
}
