using EmptorUtility.Models.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

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

        public async Task<List<r_GetWaitReasonsListFromTicketId>> GetWaitReasonsListFromTicketId(int TicketId)
        {
            var list = new List<r_GetWaitReasonsListFromTicketId>();

            var tt = _appConfig.GetConnectionString("EmptorConnection");

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(tt);
            SqlCommand cmd = new SqlCommand(String.Format(@"DECLARE @KAYIT_NUMARASI INT;
SET @KAYIT_NUMARASI = @TICKET_ID; --KAYIT NUMARASI  9113444

DECLARE @SERVIS_TIPI_ID INT;
SET @SERVIS_TIPI_ID =
(
    SELECT TICKETTYPEID
    FROM CRMTBL_TICKET
    WHERE 1 = 1
          AND ID = @KAYIT_NUMARASI
          AND ACTIVE = 1
);
DECLARE @ILGILI_FIRMA_ID INT;
SET @ILGILI_FIRMA_ID =
(
    SELECT C_CONCERNEDACCOUNTID
    FROM CRMTBL_TICKET
    WHERE 1 = 1
          AND ID = @KAYIT_NUMARASI
          AND ACTIVE = 1
);
DECLARE @PARENT_ID INT;
SET @PARENT_ID =
(
    SELECT DEFPARENTCUSTOMERID
    FROM CRMTBL_CUSTOMER
    WHERE 1 = 1
          AND DEFPARENTCUSTOMERID IS NOT NULL
          AND ID = @ILGILI_FIRMA_ID
          AND ACTIVE = 1
);
PRINT(@SERVIS_TIPI_ID);
PRINT(@ILGILI_FIRMA_ID);
PRINT(@PARENT_ID);


--------- ZENİA BEKLEME NEDENLERİ ---------
IF(@PARENT_ID IN(2677399))
    BEGIN
       SELECT CTWR.ID, 
               CTWR.DESCRIPTION_TR
        FROM CRMTBL_TICKETWAITINGREASON CTWR
        WHERE 1 = 1
              AND CTWR.ACTIVE = 1
              AND (@PARENT_ID = 2677399
                   AND CTWR.TICKETTYPEID IS NULL
                   --OR @PARENT_ID = 6136368
                   --AND CTWR.TICKETTYPEID = @SERVIS_TIPI_ID
                   AND CTWR.MAINCUSTOMERID = @PARENT_ID);
    END;

--------- MİLLİ PİYANGO BEKLEME NEDENLERİ ---------
	IF(@PARENT_ID IN(6136368))
    BEGIN
       SELECT CTWR.ID, 
               CTWR.DESCRIPTION_TR
        FROM CRMTBL_TICKETWAITINGREASON CTWR
        WHERE 1 = 1
              AND CTWR.ACTIVE = 1
              AND (
                   --AND CTWR.TICKETTYPEID IS NULL
                   @PARENT_ID = 6136368
                   AND CTWR.TICKETTYPEID = @SERVIS_TIPI_ID
                   AND CTWR.MAINCUSTOMERID = @PARENT_ID);
    END;


--------- FİNANSBANK BELİRLİ BEKLEME NEDENLERİ ---------
IF (@PARENT_ID IN(270855)) BEGIN


      SELECT CTWR.ID, 
               CTWR.DESCRIPTION_TR
        FROM CRMTBL_TICKETWAITINGREASON CTWR
        WHERE 1 = 1
              AND (@PARENT_ID = 270855
                   AND CTWR.ACTIVE = 1
                   AND CTWR.TICKETTYPEID IS NOT NULL
                   AND CTWR.MAINCUSTOMERID IS NOT NULL
                   --AND CTWR.TICKETTYPEID = @SERVIS_TIPI_ID
                   AND CTWR.ID IN(330, 331, 332, 333, 334, 335, 336, 337, 338, 339, 340, 341, 342))
        AND CTWR.MAINCUSTOMERID = @PARENT_ID;

    END;

   IF (@PARENT_ID NOT IN(270855,2677399,6136368)) BEGIN

---- BUNLARIN DIŞINDA NE GELİRSE GELSİN STANDART BEKLEME NEDENLERİ
        SELECT CTWR.ID, 
               CTWR.DESCRIPTION_TR
        FROM CRMTBL_TICKETWAITINGREASON CTWR
        WHERE 1 = 1
              AND CTWR.ID IN(3, 1, 7)
        AND CTWR.MAINCUSTOMERID IS NULL;
        --ELSE null  END)
        --AND (CTWR.MAINCUSTOMERID IS NULL OR CTWR.MAINCUSTOMERID NOT IN(2677399, 6136368, 270855))
        --AND CTWR.TICKETTYPEID = @SERVIS_TIPI_ID;
    END;"), con);
            cmd.Parameters.Add("@TICKET_ID", SqlDbType.Int).Value = TicketId;
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
                    while (rdr.Read())
                    {

                        int Id = Convert.ToInt32(rdr["ID"]);
                        string name = rdr["DESCRIPTION_TR"].ToString();
                        r_GetWaitReasonsListFromTicketId _item = new r_GetWaitReasonsListFromTicketId
                        {
                            Id = Id,
                            Name = name
                        };
                        list.Add(_item);
                    }
                }
            }
            con.Close();
            return list;
        }

    }
}