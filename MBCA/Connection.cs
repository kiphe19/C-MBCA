using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace chevron
{
    public class Connection
    {
        private SqlCommand cmd;
        private SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=MBCA;Integrated Security=True;User Id=Syahrul;Password=qpse");

        public SqlDataReader result;

        public void get()
        {
            con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=MBCA;Integrated Security=True;User Id=Syahrul;Password=qpse");
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public void select(String table, String column = "*", string where = "")
        {
            var query = "";
            if (where == "")
            {
                query = string.Format("select {0} from {1}", column, table);
            }
            else
            {
                query = string.Format("select {0} from {1} where {2}", column, table, where);
            }
            con.Open();
            try
            {
                cmd = new SqlCommand(query, con);
                result = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
        }

        public void queryExec(string query)
        {
            con.Open();
            try
            {
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public void Close()
        {
            con.Close();
        }
    }
}