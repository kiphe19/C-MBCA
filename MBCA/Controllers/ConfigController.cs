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
            ViewBag.vessel = _getVessel();
            ViewBag.distance = _getDistance();
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
                .Where("tgl", DateTime.Today.ToString("yyyy-MM-dd"), "<=")
                .Where("tgl", DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd"), ">=")
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

        [Route("users")]
        public ActionResult _ApiUsers()
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "users_table", "username")
                .Model<UsersModel>()
                .Field(new Field("tingkat").GetFormatter((val, data) => val.ToString() == "0" ? "Administrator" : "Operator"))
                .Process(request)
                .Data();

                return Json(response, JsonRequestBehavior.AllowGet);
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

        private List<SelectListItem> _getVessel()
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

        private List<SelectListItem> _getDistance()
        {
            List<SelectListItem> vessel = new List<SelectListItem>();
            con.select("distance_table", "name, distance");

            while (con.result.Read())
            {
                vessel.Add(new SelectListItem
                {
                    Text = con.result["name"].ToString(),
                    Value = con.result["distance"].ToString()
                });
            }

            var VesselSorted = (from li in vessel orderby li.Text select li).ToList();
            return VesselSorted;
        }

        [Route("cs/barge")]
        [HttpPost]
        public String _BargeCustom(FormCollection input)
        {
            String query = "";

            switch (input["action"])
            {
                case "create":
                    //query = String.Format("insert into unit_table ([name], [cat], [distance], [ket]) \n"+
                    //    "values ('{0}', '{1}', {2}, '{3}')",
                    //    input["unit_name"], input["unit_cat"], input["distance"], input["unit_desc"]
                    //    );
                    query = String.Format("insert into unit_table ([name], [distance], [ket]) \n" +
                        "values ('{0}', {1}, '{2}')",
                        input["unit_name"], input["distance"], input["unit_desc"]
                        );
                    break;
                case "update":
                    query = String.Format("update unit_table set name='{0}', distance={1}, ket='{2}' where Id={3}",
                            input["unit_name"], input["distance"], input["unit_desc"], input["id"]
                        );
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

        [Route("cs/fuel")]
        [HttpPost]
        public String _FuelCustom(FormCollection input)
        {
            DateTime tanggal1 = Convert.ToDateTime(input["tgl_from"]);
            DateTime tanggal2 = Convert.ToDateTime(input["tgl_to"]);
            TimeSpan ts = tanggal2.Subtract(tanggal1);
            var jml = (int)ts.TotalDays;



            //int hasil = int.Parse(ts.ToString());
            //Lblpesan.Text = "selisih " + ts.Days + " hari";


            /*
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
            */

            String query = "";

            //switch (input["ation"])
            //{
            //    case "create":
            //        for (int i = 0; i < jml; i++)
            //        {
            //            query = string.Format("insert into fuel_table ([tgl],[cost_usd], [currency_type]) values('{0}', CAST('{1}' AS numeric(18,3)), '{2}')", tanggal1.AddDays(i).ToString("yyyy-MM-dd"), input["cost"], input["currency_cat"]);
            //        }
            //        //query = string.Format("insert into fuel_table ([tgl],[cost_usd], [currency_type]) values('{0}', CAST('{1}' AS numeric(18,3)), '{2}')", input["tgl"], hasilUSD.ToString(CultureInfo.InvariantCulture), hasilRP.ToString(CultureInfo.InvariantCulture));
            //        break;
            //    case "update":
            //        //query = string.Format("update fuel_table set tgl='{0}', cost_usd={1}, cost_rp={2} where id={3}", input["tgl"], hasilUSD.ToString(CultureInfo.InvariantCulture), hasilRP.ToString(CultureInfo.InvariantCulture), input["id"]);
            //        //break;
            //    default:
            //        break;
            //}

            try
            {

                //if (input["ation"] == "create")
                //{
                

                for (int i = 0; i <= jml; i++)
                {
                    var tg = string.Format("tgl = '{0}'", tanggal1.AddDays(i).ToString("yyyy-MM-dd"));
                    con.select("fuel_table", "count(*)", tg);
                    con.result.Read();
                    if (con.result.HasRows)
                    {
                        query = string.Format("update fuel_table set cost_usd = {0},currency_type = {1} where tgl = '{2}' ", input["cost"], input["currency_cat"], tanggal1.AddDays(i).ToString("yyyy-MM-dd"));
                    }
                   
                    query = string.Format("insert into fuel_table ([tgl],[cost_usd], [currency_type]) values('{0}', CAST('{1}' AS numeric(18,3)), '{2}')", tanggal1.AddDays(i).ToString("yyyy-MM-dd"), input["cost"], input["currency_cat"]);
                   
                    con.queryExec(query);
                }
                
                return "success";
                //return tanggal1.AddDays(1).ToString("yyyy-MM-dd");
                //Response.Write(rp);

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

        [Route("cs/users")]
        public String _Users(FormCollection input)
        {
            String query = "";

            switch (input["action"])
            {
                case "create":
                    query = String.Format("insert into users_table \n" +
                        "values ('{0}', '{1}', '{2}')",
                        input["username"], input["password"], input["level"]
                        );
                    break;
                case "update":
                    query = String.Format("update users_table set username='{0}', password='{1}', tingkat={2} where username='{0}'",
                            input["username"], input["password"], input["level"]
                        );
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
