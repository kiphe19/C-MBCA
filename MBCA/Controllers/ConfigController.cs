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
                .Field(new Field("tgl")
                    .Validator(Validation.DateFormat("MM/dd/yyyy"))
                    .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy"))
                )
                .Process(request)
                .Data();

                return Json(response);
            }
        }

        [Route("demob")]
        public ActionResult _ApiDemob()
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "demob_table")
                .Model<DemobModel>()
                .Field(new Field("tgl")
                    .Validator(Validation.DateFormat("MM/dd/yyyy"))
                    .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy"))
                )
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

            String query = "";

            switch (input["ation"])
            {
                case "create":
                    query = string.Format("insert into fuel_table ([tgl],[cost_usd], [cost_rp]) values('{0}', CAST('{1}' AS numeric(18,3)), CAST('{2}' AS numeric(18,3)) )", input["tgl"], hasilUSD.ToString(CultureInfo.InvariantCulture), hasilRP.ToString(CultureInfo.InvariantCulture));
                    break;
                case "update":
                    query = string.Format("update fuel_table set tgl='{0}', cost_usd={1}, cost_rp={2} where id={3}", input["tgl"], hasilUSD.ToString(CultureInfo.InvariantCulture), hasilRP.ToString(CultureInfo.InvariantCulture), input["id"]);
                    break;
                default:
                    break;
            }
            
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

            String query = "";
            switch (input["action"])
            {
                case "create":
                    query = String.Format("insert into hire_table ([tgl], [vessel], [cost_usd], [cost_rp]) values ('{0}', '{1}', {2}, {3})", input["tgl"], input["vessel"], hasilUSD.ToString(CultureInfo.InvariantCulture), hasilRP.ToString(CultureInfo.InvariantCulture));
                    break;
                case "update":
                    query = String.Format("update hire_table set tgl='{0}', vessel='{1}', cost_usd={2}, cost_rp={3} where id={4}",input["tgl"], input["vessel"], hasilUSD.ToString(CultureInfo.InvariantCulture), hasilRP.ToString(CultureInfo.InvariantCulture), input["id"]);
                    break;
                default:
                    break;
            }
            
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

        [Route("cs/demob")]
        public String _Demob(FormCollection input)
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

            String query = "";
            switch (input["action"])
            {
                case "create":
                    query = String.Format("insert into demob_table ([tgl], [vessel], [cost_usd], [cost_rp]) values ('{0}', '{1}', {2}, {3})", input["tgl"], input["vessel"], hasilUSD.ToString(CultureInfo.InvariantCulture), hasilRP.ToString(CultureInfo.InvariantCulture));
                    break;
                case "update":
                    query = String.Format("update demob_table set tgl='{0}', vessel='{1}', cost_usd={2}, cost_rp={3} where id={4}", input["tgl"], input["vessel"], hasilUSD.ToString(CultureInfo.InvariantCulture), hasilRP.ToString(CultureInfo.InvariantCulture), input["id"]);
                    break;
                default:
                    break;
            }

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
