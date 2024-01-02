using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepGamingSocial.registermodule
{
    public class dataconnection
    {
        public string constr;
        public dataconnection()
        {
            constr = "Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = useraccount; Integrated Security=true; TrustServerCertificate=True";
        }
        public SqlConnection getconnect()
        {
                return new SqlConnection(constr);   
        }
    }
}
