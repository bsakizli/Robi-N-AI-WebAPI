using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace EmptorUtility
{
    public class EmptorDbAction
    {

        private IConfiguration? _appConfig;

        public EmptorDbAction(IConfiguration appSettings)
        {
            _appConfig = appSettings;
        }




        public string getEmptorTicketId(int _Id)
        {
            var tt = _appConfig.GetConnectionString("EmptorConnection");

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(tt);
            SqlCommand cmd = new SqlCommand(String.Format(@"SELECT * FROM CRMTBL_TICKET WHERE 1 = 1 AND ID={0}", _Id), con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);

            con.Open();

            //using (var connection = new SqlConnection(tt))
            //{
            //    connection.Open();
            //    var record = connection.Query<ctyp>("SELECT MyID AS ID, Name FROM TABLE1 WHERE REC_ID = 1").FirstOrDefault();
            //}


            using (SqlDataReader rdr = cmd.ExecuteReader())
            {

                if (rdr.HasRows)
                {
                    while (rdr.Read()) {
                       
                        int Id = Convert.ToInt32(rdr["ID"]);
                        string name = rdr["NAME"].ToString();
                    }

                }
            }
            con.Close();

            return JsonConvert.SerializeObject(dt);


        }
    }
}