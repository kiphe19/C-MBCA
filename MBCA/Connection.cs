using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace chevron
{
    public class Connection
    {
        Properties.Settings sett = Properties.Settings.Default;
        
        private SqlCommand cmd;
        private static SqlConnection con;
        public string q;
        public SqlDataReader result;


        public void get()
        {
            con = new SqlConnection(sett.DbConnection);
        }

        public void select(String table, String column = "*", string where = "")
        {
            this.get();
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
            this.get();
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

        public void query(string query)
        {
            this.get();
            con.Open();
            try
            {
                cmd = new SqlCommand(query, con);
                result = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public void Close()
        {
            con.Close();
        }
    }
}