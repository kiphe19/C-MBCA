using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using chevron.Models;
using DataTables;
using AttributeRouting.Web.Mvc;
using AttributeRouting;
using System.Globalization;

namespace chevron.Controllers
{
    [RouteArea("api")]
    public class ConfigController : Controller
    {

        chevron.Properties.Settings setting = chevron.Properties.Settings.Default;
        Connection con = new Connection();

        public ActionResult Index()
        {
            ViewBag.currency_cat = _getCurrency();
            ViewBag.vessel = _getVessel1();
            return View();
        }

        [Route("vessel")]
        public ActionResult _ApiVessel()
        {
            var request = System.Web.HttpContext.Current.Request;

            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "vessel_table")
                .Model<VesselModel>()
                .Field(new Field("name")
                    .Validator(Validation.NotEmpty())
                )
                .Process(request)
                .Data();

                return Json(response);
            }
        }

        [Route("activity")]
        public ActionResult _ApiActivity()
        {
            var request = System.Web.HttpContext.Current.Request;

            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "activity_table")
                .Model<ActivityModel>()
                .Field(new Field("name")
                    .Validator(Validation.NotEmpty())
                )
                .Process(request)
                .Data();

                return Json(response);
            }
        }

        [Route("unit")]
        public ActionResult _ApiUserUnit()
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "unit_table")
                .Model<UnitModel>()
                .Field(new Field("name")
                    .Validator(Validation.NotEmpty())
                )
                .Field(new Field("distance")
                    .Validator(Validation.NotEmpty())
                    .Validator(Validation.Numeric())
                )
                .Process(request)
                .Data();

                return Json(response);
            }
        }

        [Route("distance")]
        public ActionResult _ApiDistance()
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "distance_table")
                .Model<DistanceModel>()
                .Field(new Field("name")
                    .Validator(Validation.NotEmpty())
                )
                .Field(new Field("distance")
                    .Validator(Validation.NotEmpty())
                    .Validator(Validation.Numeric())
                )
                .Process(request)
                .Data();

                return Json(response);
            }
        }

        [Route("fuel")]
        public ActionResult _ApiFuel()
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "fuel_table")
                .Model<FuelModel>()
                .Field(new Field("tgl")
                    .Validator(Validation.DateFormat("MM/dd/yyyy"))
                    .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy"))
                    )
                //.Field("")
                .Process(request)
                .Data();

                return Json(response);
            }
        }

        [Route("hire")]
        public ActionResult _ApiHire()
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "hire_table")
                .Model<HireModel>()
                .Process(request)
                .Data();

                return Json(response);
            }
        }

        [Route("currency")]
        public ActionResult _ApiCurrency()
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "currency_cat")
                .Model<CurrencyModel>()
                .Field(new Field("last_up").GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy")))
                .Process(request)
                .Data();

                return Json(response);
            }
        }

        [Route("distanceValue")]
        public ActionResult _distanceValue()
        {
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "distance_table")
                .Model<DistanceModel>()
                .Process(Request.Form)
                .Data();

                return Json(response);
            }
        }

        [Route("getvessel")]
        public ActionResult _getVessel()
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

        [Route("cs/currency")]
        public String _updateCurrency(FormCollection input)
        {
            var query = String.Format("update currency_cat set name='{0}', value={1}, last_up='{2}' where id={3}", input["currency_name"], input["currency_value"], DateTime.Today.ToShortDateString(), input["currency_id"]);
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

        private List<SelectListItem> _getCurrency()
        {
            List<SelectListItem> currency = new List<SelectListItem>();
            con.select("currency_cat", "name, id");

            while (con.result.Read())
            {
                currency.Add(new SelectListItem
                {
                    Text = con.result["name"].ToString(),
                    Value = con.result["id"].ToString()
                });
            }

            return currency;
        }

        private List<SelectListItem> _getVessel1()
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

            var VesselSorted = (from li in vessel orderby li.Text select li).ToList();
            return VesselSorted;
        }

        [Route("cs/fuel")]
        [HttpPost]
        public String _FuelCustom(FormCollection input)
        {
            List<CurrencyModel> curr = new List<CurrencyModel>();

            con.select("currency_cat", "*");

            while (con.result.Read())
            {
                curr.Add(new CurrencyModel
                {
                    id = int.Parse(con.result["id"].ToString()),
                    value = int.Parse(con.result["value"].ToString())
                });
            }
            con.Close();


            var a = int.Parse(input["currency_cat"]);
            var usd = (from curcat in curr where curcat.id == 1 select curcat.value).ToList();
            var rp = (from curcat in curr where curcat.id == 2 select curcat.value).ToList();
            Decimal hasilUSD = 0,
                hasilRP = 0,
                c = Decimal.Parse(input["cost"]);

            switch (a)
            {
                case 1:
                    hasilUSD = usd[0] * c;
                    hasilRP = rp[0] * c;
                    break;
                case 2:
                    hasilUSD = c / rp[0];
                    hasilRP = usd[0] * c;
                    break;
            }

            var query = string.Format("insert into fuel_table ([tgl],[cost_usd], [cost_rp]) values('{0}', CAST('{1}' AS numeric(18,3)), CAST('{2}' AS numeric(18,3)) )", input["tgl"], hasilUSD.ToString(CultureInfo.InvariantCulture), hasilRP.ToString(CultureInfo.InvariantCulture));
            try
            {
                con.queryExec(query);
                //return hasilUSD.ToString(CultureInfo.InvariantCulture) + " " + hasilRP.ToString(CultureInfo.InvariantCulture) + " " + c;
                return "true";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        [Route("cs/charter")]
        public String _Charter(FormCollection input)
        {
            List<CurrencyModel> curr = new List<CurrencyModel>();

            con.select("currency_cat", "*");

            while (con.result.Read())
            {
                curr.Add(new CurrencyModel
                {
                    id = int.Parse(con.result["id"].ToString()),
                    value = int.Parse(con.result["value"].ToString())
                });
            }
            con.Close();


            var a = int.Parse(input["currency_cat"]);
            var usd = (from curcat in curr where curcat.id == 1 select curcat.value).ToList();
            var rp = (from curcat in curr where curcat.id == 2 select curcat.value).ToList();

            Decimal hasilUSD = 0,
                hasilRP = 0,
                b = Decimal.Parse(input["cost"]);

            switch (a)
            {
                case 1:
                    hasilUSD = usd[0] * b;
                    hasilRP = rp[0] * b;
                    break;
                case 2:
                    hasilUSD = b / rp[0];
                    hasilRP = b * usd[0];
                    break;
                default:
                    break;
            }

            var query = String.Format("insert into hire_table ([vessel], [cost_usd], [cost_rp]) values ('{0}', {1}, {2})", input["vessel"], hasilUSD.ToString(CultureInfo.InvariantCulture), hasilRP.ToString(CultureInfo.InvariantCulture));
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
    }
}
