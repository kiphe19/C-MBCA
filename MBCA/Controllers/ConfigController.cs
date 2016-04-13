using System;
using System.Collections.Generic;
using System.Linq;
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
            //ViewBag.vessel = _getVessel();
            ViewBag.vesselid = _getVesselId();
            ViewBag.distance = _getDistance();
            ViewBag.userunit = _getUserUnit();
            ViewBag.mainunit = _getMainUnit();

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


        [Route("unit/{tg}")]
        public ActionResult _ApiUserUnitParam(string tg)
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "unit_table")
                .Model<UnitDistanceDailyModel>()
                .Field(new Field("unit_table.name")
                    .Validator(Validation.NotEmpty())
                )
                .Field(new Field("unit_distance_table.distance")
                    .Validator(Validation.NotEmpty())
                    .Validator(Validation.Numeric())
                )
                .Field(new Field("unit_distance_table.tgl")
                    .Validator(Validation.DateFormat("MM/dd/yyyy"))
                    .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy"))
                    )
                .LeftJoin("unit_distance_table", "unit_distance_table.id_unit", "=", "unit_table.id")
                .LeftJoin("mainunit_table", "mainunit_table.id","=","unit_distance_table.id_mainunit")
                .Where("unit_distance_table.tgl", tg, "=")
                .Process(request)
                .Data();

                //return Json(response);
                return Json(response, JsonRequestBehavior.AllowGet);

            }
            //return Json(new { a }, JsonRequestBehavior.AllowGet);
            //return Json(new { date = tgl.ToString() },
            //            JsonRequestBehavior.AllowGet);
        }

        [Route("unit")]
        public ActionResult _ApiUserUnit()
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "unit_table")
                .Model<UnitDistanceDailyModel>()
                .Field(new Field("name")
                    .Validator(Validation.NotEmpty())
                )
                //.Field(new Field("distance")
                //    .Validator(Validation.NotEmpty())
                //    .Validator(Validation.Numeric())
                //)
                //.Field(new Field("tgl")
                //    .Validator(Validation.DateFormat("MM/dd/yyyy"))
                //    .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy"))
                //    )
                .Process(request)
                .Data();

                return Json(response);
            }
        }

        [Route("mainunit")]
        public ActionResult _ApiMainUnit()
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "mainunit_table")
                .Model<MainUnitModel>()
                .Field(new Field("nama")
                    .Validator(Validation.NotEmpty())
                )
                //.Field(new Field("distance")
                //    .Validator(Validation.NotEmpty())
                //    .Validator(Validation.Numeric())
                //)
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

        [Route("fuel/{tg1}/{tg2}")]
        public ActionResult _ApiFuel(string tg1, string tg2)
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "fuel_table")
                .Model<FuelModel>()
                .Where("tgl", tg1, ">=")
                .Where("tgl", tg2, "<=")
                .Field(new Field("tgl")
                    .Validator(Validation.DateFormat("MM/dd/yyyy"))
                    .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy"))
                    )
                .Process(request)
                .Data();

                return Json(response);
            }
        }

        [Route("charter")]
        public ActionResult _ApiHire()
        {
            var request = System.Web.HttpContext.Current.Request;
            using (var db = new Database(setting.DbType, setting.DbConnection))
            {
                var response = new Editor(db, "hire_table")
                .Model<CharterModel>()
                .Field(new Field("hire_table.tgl_start")
                    .Validator(Validation.DateFormat("MM/dd/yyyy"))
                    .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy"))
                )
                .Field(new Field("hire_table.tgl_end")
                    .Validator(Validation.DateFormat("MM/dd/yyyy"))
                    .GetFormatter(Format.DateTime("MM/dd/yyyy H:m:s", "MM/dd/yyyy"))
                )
                .LeftJoin("vessel_table", "vessel_table.id","=","hire_table.id_vessel")
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
                var response = new Editor(db, "users_table")
                .Model<UsersModel>()
                //.Field(new Field("tingkat").GetFormatter((val, data) => (val.ToString() == "0") ? "Administrator" : "Operator"))
                .Process(request)
                .Data();

                //return Json(response, JsonRequestBehavior.AllowGet);
                return Json(response);

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
            con.Close();
            return currency;
        }

        private List<SelectListItem> _getVesselId()
        {
            List<SelectListItem> vessel_id = new List<SelectListItem>();
            con.select("vessel_table", "id,name");

            while (con.result.Read())
            {
                vessel_id.Add(new SelectListItem
                {
                    Text = con.result["name"].ToString(),
                    Value = con.result["id"].ToString()
                });
            }
            con.Close();
            var VesselSorted = (from li in vessel_id orderby li.Text select li).ToList();
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
            con.Close();
            var VesselSorted = (from li in vessel orderby li.Text select li).ToList();
            return VesselSorted;
        }

        private List<SelectListItem> _getUserUnit()
        {
            List<SelectListItem> unit = new List<SelectListItem>();

            con.select("unit_table", "id, name");
            while (con.result.Read())
            {
                unit.Add(new SelectListItem
                {
                    Text = con.result["name"].ToString(),
                    Value = con.result["id"].ToString()
                });
            }
            con.Close();

            var unitSorted = (from li in unit orderby li.Text select li).ToList();

            return unitSorted;
        }

        private List<SelectListItem> _getMainUnit()
        {
            List<SelectListItem> mainunit = new List<SelectListItem>();

            con.select("mainunit_table", "id, nama");
            while (con.result.Read())
            {
                mainunit.Add(new SelectListItem
                {
                    Text = con.result["nama"].ToString(),
                    Value = con.result["id"].ToString()
                });
            }
            con.Close();
            var mainunitSorted = (from li in mainunit orderby li.Text select li).ToList();
            return mainunitSorted;
        }

        [Route("cs/mainunit")]
        [HttpPost]
        public string _MainUnitCreate(FormCollection input)
        {
            String query = "";
            try
            {
                switch (input["action"])
                {
                    case "create":
                        query = string.Format("insert into mainunit_table (nama,description) values ('{0}','{1}')", input["mainunit_name"], input["mainunit_desc"]);
                        break;
                    case "update":
                        query = string.Format("update mainunit_table set nama = '{0}', description='{1}' where id = {2}", input["mainunit_name"], input["mainunit_desc"], input["id"]);
                        break;
                    default:
                        break;
                }

                //Response.Write(query);
                con.queryExec(query);
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [Route("cs/unit")]
        [HttpPost]
        public string _BargeCreate(FormCollection input)
        {
            String query = "";
            try
            {
                switch (input["action"])
                {
                    case "create":
                        query = string.Format("insert into unit_table(name, ket) values('{0}','{1}')", input["unit_name"], input["unit_desc"]);
                        break;
                    case "update":
                        query = string.Format("update unit_table set name='{0}', ket='{1}' where id = {2}", input["unit_name"], input["unit_desc"], input["id"]);
                        break;
                    default:
                        break;
                }
                con.queryExec(query);
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [Route("cs/unitdist")]
        [HttpPost]
        public string _BargeCustom(FormCollection input)
        {
            String query = "";
            DateTime tanggal1 = Convert.ToDateTime(input["tgl_from"]);
            DateTime tanggal2 = Convert.ToDateTime(input["tgl_to"]);
            TimeSpan ts = tanggal2.Subtract(tanggal1);
            var jml = (int)ts.TotalDays;

            //Response.Write(jml);
            try
            {

                for (int i = 0; i <= jml; i++)
                {
                    //var cari = string.Format("id_unit={0} and tgl = '{1}'", input["userunit"],  tanggal1.AddDays(i).ToString("yyyy-MM-dd"));
                    var cari = string.Format("id_unit={0} and tgl = '{1}' and id_mainunit = '{2}'", input["userunit"], tanggal1.AddDays(i).ToString("yyyy-MM-dd"), input["mainunit"]);

                    con.select("unit_distance_table", "*", cari);
                    con.result.Read();
                    if (con.result.HasRows)
                    {
                        query = string.Format("update unit_distance_table set distance= {0} where id_unit = {1} and tgl = '{2}' and id_mainunit = {3}", input["distance"], input["userunit"], tanggal1.AddDays(i).ToString("yyyy-MM-dd"), input["mainunit"]);
                    }
                    else
                    {
                        query = string.Format("insert into unit_distance_table (id_unit,distance,tgl,id_mainunit) values({0},{1},'{2}',{3})", input["userunit"], input["distance"], tanggal1.AddDays(i).ToString("yyyy-MM-dd"), input["mainunit"]);
                    }
                    con.Close();
                    //Response.Write(query);
                    con.queryExec(query);
                }

               //Response.Write(input);
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [Route("cs/unit_mainunit")]
        [HttpPost]
        public string _UnittoMainUnitCustom(FormCollection input)
        {
            String query = "";
            DateTime tanggal1 = Convert.ToDateTime(input["tgl_from"]);
            DateTime tanggal2 = Convert.ToDateTime(input["tgl_to"]);
            TimeSpan ts = tanggal2.Subtract(tanggal1);
            var jml = (int)ts.TotalDays;

            //Response.Write(jml);
            try
            {

                for (int i = 0; i <= jml; i++)
                {
                    var cari = string.Format("id_unit={0} and tgl = '{1}' and id_mainunit = '{2}'", input["userunit"], tanggal1.AddDays(i).ToString("yyyy-MM-dd"), input["mainunit"]);
                    con.select("unit_distance_table", "*", cari);
                    con.result.Read();
                    if (con.result.HasRows)
                        continue;
                    else
                        query = string.Format("insert into unit_distance_table (id_unit,id_mainunit,tgl) values({0},{1},'{2}')", input["userunit"], input["mainunit"], tanggal1.AddDays(i).ToString("yyyy-MM-dd"));
                    con.Close();
                    //Response.Write(query);
                    con.queryExec(query);
                }
                //Response.Write(input);
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [Route("cs/mainunit")]
        [HttpPost]
        public String _MainUnitCustom(FormCollection input)
        {
            String query="";
            Response.Write(input["action"]);
            try
            {
                switch (input["action"])
                {
                    case "create":
                        query = string.Format("insert into mainunit_table (nama,description) values('{0}','{1}')", input["mainunit_name"], input["mainunit_desc"]);
                        break;
                    default:
                        break;
                }
                Response.Write(query);
                return "success";
            }
            catch(Exception e)
            {
                return e.Message;
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
            string query = "";

            try
            {
                for (int i = 0; i <= jml; i++)
                {
                    var tg = string.Format("tgl = '{0}'", tanggal1.AddDays(i).ToString("yyyy-MM-dd"));
                    //con.select("temp_daily", "*", tg);

                    con.select("fuel_table", "*", tg);
                    con.result.Read();
                    if (con.result.HasRows)
                    {
                        query = string.Format("update fuel_table set cost_usd = {0},currency_type = {1} where tgl = '{2}' ", input["cost"], input["currency_cat"], tanggal1.AddDays(i).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        query = string.Format("insert into fuel_table (tgl,cost_usd, currency_type) values('{0}', CAST('{1}' AS numeric(18,3)), '{2}')", tanggal1.AddDays(i).ToString("yyyy-MM-dd"), input["cost"], input["currency_cat"]);
                    }
                    con.Close();

                    //Response.Write(query);
                    con.queryExec(query);
                    //return query;

                    con.select("report_daily", "fuel_litre,fuel_curr", tg);
                    while (con.result.Read())
                    {
                        var f_harga = Convert.ToDecimal(con.result["fuel_litre"]) * Convert.ToDecimal(input["cost"]);
                        string q_updfuel = string.Format("update report_daily set fuel_price = {0}, fuel_curr = {1} where tgl = '{2}'", f_harga, input["currency_cat"], tanggal1.AddDays(i).ToString("yyyy-MM-dd"));
                        //Response.Write("query => "+ q_updfuel+"\n");
                        con.queryExec(q_updfuel);
                    }
                    con.Close();
                }

                //return jml.ToString();
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
            //Double mob_rate,charter_rate;
            DateTime tg1 = Convert.ToDateTime(input["tgl_start"]);
            DateTime tg2 = Convert.ToDateTime(input["tgl_end"]);
            TimeSpan lama = tg2.Subtract(tg1);
            decimal jml = (decimal)lama.TotalDays + 1;

            var charter_rate = (input["charter_cost"] == "") ? Convert.ToInt16(0) : Convert.ToDecimal(input["charter_cost"]);
            var mob_cost = (input["mob_cost"] == "") ? Convert.ToInt16(0) : Convert.ToDecimal(input["mob_cost"]);
            var mob_rate = Convert.ToDecimal(mob_cost) / Convert.ToDecimal(jml);

            //Response.Write(mob_rate +" "+charter_rate);
           
            String query = "";
            switch (input["action"])
            {
                case "create":
                    //query = string.format("insert into hire_table ([tgl], [vessel], [cost_usd], [cost_rp]) values ('{0}', '{1}', {2}, {3})", input["tgl"], input["vessel"], hasilusd.tostring(cultureinfo.invariantculture), hasilrp.tostring(cultureinfo.invariantculture));
                    query = string.Format("insert into hire_table (tgl_start,tgl_end,id_vessel,cost_usd,mob_cost,mob_rate,curency_cat,periode) " +
                                        "values ('{0}','{1}',{2},{3},{4},{5},{6},{7})",
                                        Convert.ToDateTime(input["tgl_start"]).ToString("yyyy-MM-dd"),
                                        Convert.ToDateTime(input["tgl_end"]).ToString("yyyy-MM-dd"),
                                        input["vesselid"],
                                        charter_rate,
                                        mob_cost,
                                        mob_rate,
                                        input["currency_cat"],
                                        jml);
                    break;
                case "update":
                    //query = string.format("update hire_table set tgl='{0}', vessel='{1}', cost_usd={2}, cost_rp={3} where id={4}", input["tgl"], input["vessel"], hasilusd.tostring(cultureinfo.invariantculture), hasilrp.tostring(cultureinfo.invariantculture), input["id"]);
                    query = string.Format("update hire_table set tgl_start = '{0}', tgl_end='{1}', id_vessel = {2}, cost_usd = {3}, mob_cost={4}, mob_rate={5},curency_cat={6}, periode = {7} where id={8}",
                                        input["tgl_start"], 
                                        input["tgl_end"], 
                                        input["vesselid"], 
                                        charter_rate, 
                                        mob_cost, 
                                        mob_rate, 
                                        input["currency_cat"], 
                                        jml, 
                                        input["id"]);
                    break;
                default:
                    break;
            }
            try
            {
                //Response.Write(query);
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
                    query = String.Format("insert into users_table values ('{0}', '{1}', {2})",input["username"], input["password"], input["level"]);
                    break;
                case "update":
                    query = String.Format("update users_table set username='{0}', password='{1}', tingkat={2} where id={3}",input["username"], input["password"], input["level"],input["id"]);
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
