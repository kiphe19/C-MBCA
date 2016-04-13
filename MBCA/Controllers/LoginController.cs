using System;
using System.Web.Mvc;

namespace chevron.Controllers
{
    public class LoginController : Controller
    {
        Connection con = new Connection();

        public ActionResult Index()
        {
            return View();
        }

        public void auth(FormCollection input)
        {
            var username = input["username"];
            var password = input["password"];

            var query = String.Format("select * from users_table where username='{0}' and password='{1}'", username, password);
            try
            {
                con.query(query);
                con.result.Read();

                if (con.result.HasRows)
                {
                    Session["logged"] = "1";
                    Session["userid"] = con.result["username"].ToString();
                    Session["level"] = con.result["tingkat"].ToString();

                    Response.Redirect(Url.Action("index", "home"), true);
                }
                else
                {
                    TempData["err_msg"] = "Username / password Salah";
                    Response.Redirect(Url.Action("index", "login"), true);
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
        }

        public void Out()
        {
            Session.Clear();
            Response.Redirect(Url.Action("index", "login"));
        }
    }
}
