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
        private SqlConnection con;
        public SqlDataReader result;

        public void get()
        {
            con = new SqlConnection(sett.DbConnection);
        }

        public void select(String table, String column = "*", string where = "")
        {
            this.get();
            con.Open();
            var query = "";
            if (where == "")
            {
                query = string.Format("select {0} from {1}", column, table);
            }
            else
            {
                query = string.Format("select {0} from {1} where {2}", column, table, where);
            }
            try
            {
                cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 60;
                result = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                con.Dispose();
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                SqlConnection.ClearPool(con);
            }
        }

        public void queryExec(string query)
        {
            try
            {
                using (con = new SqlConnection(sett.DbConnection))
                {
                    con.Open();
                    using (cmd = con.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                SqlConnection.ClearPool(con);
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
            finally
            {
                SqlConnection.ClearPool(con);
            }
        }

        public void Close()
        {
            con.Close();
        }
    }
}