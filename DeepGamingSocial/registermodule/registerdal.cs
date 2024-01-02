using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepGamingSocial.registermodule
{
    internal class registerdal
    {
       dataconnection dc = new dataconnection();
        SqlCommand cmd;
        public bool registeraccount(useraccount t)
        {
            string sql = "insert into account values (@username, @passwords, @recoveryques, @recoverypass)";
            SqlConnection con = dc.getconnect();
            try
            {
                cmd = new SqlCommand(sql, con);
                con.Open();
                cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = t.username;
                cmd.Parameters.Add("@passwords", SqlDbType.NVarChar).Value = t.password;
                cmd.Parameters.Add("@recoveryques", SqlDbType.NVarChar).Value = t.recoveryques;
                cmd.Parameters.Add("@recoverypass", SqlDbType.NVarChar).Value = t.recoverypass;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;  
        }
    }   
}
