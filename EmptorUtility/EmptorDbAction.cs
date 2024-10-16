﻿using EmptorUtility.Models;
using EmptorUtility.Models.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


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
                    while (rdr.Read())
                    {

                        int Id = Convert.ToInt32(rdr["ID"]);
                        string name = rdr["NAME"].ToString();
                    }

                }
            }
            con.Close();

            return JsonConvert.SerializeObject(dt);


        }

        public async Task<r_getCompanyFullName> getCompanyFullName(string TicketId)
        {
            var tt = _appConfig.GetConnectionString("EmptorConnection");
            r_getCompanyFullName _item = null;
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(tt);
            SqlCommand cmd = new SqlCommand(String.Format(@"SELECT CC.ID, CC.FULLNAME
            FROM CRMTBL_TICKET AS CT WITH(NOLOCK)
                 INNER JOIN CRMTBL_CUSTOMER AS CC WITH(NOLOCK) ON CC.ID = CT.C_CONCERNEDACCOUNTID
            WHERE 1 = 1
                  AND CT.IDDESC = @TICKET_ID
                  AND CT.ACTIVE = 1;"), con);
            cmd.Parameters.Add("@TICKET_ID", SqlDbType.VarChar).Value = TicketId;
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
                        string name = rdr["FULLNAME"].ToString();
                        _item = new r_getCompanyFullName
                        {
                            Id = Id,
                            Name = name
                        };

                    }
                }
            }
            con.Close();

            return _item;
        }

        public async Task<List<r_GetWaitReasonsListFromTicketId>> GetWaitReasonsListFromTicketId(string TicketId)
        {
            var list = new List<r_GetWaitReasonsListFromTicketId>();
            var tt = _appConfig.GetConnectionString("EmptorConnection");

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(tt);
            SqlCommand cmd = new SqlCommand(String.Format(@"DECLARE @KAYIT_NUMARASI_ID_DESC VARCHAR(12),@PARENT_ID INT, @ILGILI_FIRMA_ID INT,@ID_DESC INT,@SERVIS_TIPI_ID INT,@KAYIT_NUMARASI INT;

--SET @KAYIT_NUMARASI_ID_DESC = '252009194097';
SET @KAYIT_NUMARASI_ID_DESC = @TICKET_ID;

(SELECT
@ILGILI_FIRMA_ID=C_CONCERNEDACCOUNTID,
@SERVIS_TIPI_ID=TICKETTYPEID,
@KAYIT_NUMARASI =ID,
@SERVIS_TIPI_ID=TICKETTYPEID
    FROM CRMTBL_TICKET WITH (NOLOCK)
    WHERE 1 = 1
          AND IDDESC = @KAYIT_NUMARASI_ID_DESC
          AND ACTIVE = 1)

SET @PARENT_ID =
(
    SELECT MainCustomerId
    FROM CRMTBL_CUSTOMER WITH (NOLOCK)
    WHERE 1 = 1
          AND MainCustomerId IS NOT NULL
          AND ID = @ILGILI_FIRMA_ID
          AND ACTIVE = 1
);


--------- ZENİA BEKLEME NEDENLERİ ---------
IF(@PARENT_ID IN(2677399))
    BEGIN
        SELECT CTWR.ID, 
               CTWR.DESCRIPTION_TR
        FROM CRMTBL_TICKETWAITINGREASON CTWR WITH (NOLOCK)
        WHERE 1 = 1
              AND CTWR.ACTIVE = 1
              AND (@PARENT_ID = 2677399
                   AND CTWR.TICKETTYPEID IS NULL
                   --OR @PARENT_ID = 6136368
                   --AND CTWR.TICKETTYPEID = @SERVIS_TIPI_ID
                   AND CTWR.MAINCUSTOMERID = @PARENT_ID);
    END;

--------- TT_YHÇO BEKLEME NEDENLERİ ---------
IF(@PARENT_ID IN(363460))
    BEGIN
        SELECT CTWR.ID, 
               CTWR.DESCRIPTION_TR
        FROM CRMTBL_TICKETWAITINGREASON CTWR WITH (NOLOCK)
        WHERE 1 = 1
              AND CTWR.ACTIVE = 1
              AND CTWR.ID IN(249, 250, 3, 1, 7);
    END;

--------- MİLLİ PİYANGO BEKLEME NEDENLERİ ---------
IF(@PARENT_ID IN(6136368))
    BEGIN
        SELECT CTWR.ID, 
               CTWR.DESCRIPTION_TR
        FROM CRMTBL_TICKETWAITINGREASON CTWR WITH (NOLOCK)
        WHERE 1 = 1
              AND CTWR.ACTIVE = 1
              AND (
              --AND CTWR.TICKETTYPEID IS NULL
              @PARENT_ID = 6136368
              AND CTWR.TICKETTYPEID = @SERVIS_TIPI_ID
              AND CTWR.MAINCUSTOMERID = @PARENT_ID);
    END;

--------- FİNANSBANK BELİRLİ BEKLEME NEDENLERİ ---------
IF(@PARENT_ID IN(270855))
    BEGIN
        SELECT CTWR.ID, 
               CTWR.DESCRIPTION_TR
        FROM CRMTBL_TICKETWAITINGREASON CTWR WITH (NOLOCK)

        WHERE 1 = 1
              AND (@PARENT_ID = 270855
                   AND CTWR.ACTIVE = 1
                   AND CTWR.TICKETTYPEID IS NOT NULL
                   AND CTWR.MAINCUSTOMERID IS NOT NULL
                   --AND CTWR.TICKETTYPEID = @SERVIS_TIPI_ID
                   AND CTWR.ID IN(330, 331, 332, 333, 334, 335, 336, 337, 338, 339, 340, 341, 342))
        AND CTWR.MAINCUSTOMERID = @PARENT_ID;
    END;
IF(@PARENT_ID NOT IN(270855, 2677399, 6136368,363460))
    BEGIN

---- BUNLARIN DIŞINDA NE GELİRSE GELSİN STANDART BEKLEME NEDENLERİ
        SELECT CTWR.ID, 
               CTWR.DESCRIPTION_TR
        FROM CRMTBL_TICKETWAITINGREASON CTWR WITH (NOLOCK)
        WHERE 1 = 1
              AND CTWR.ID IN(3, 1, 7)
        AND CTWR.MAINCUSTOMERID IS NULL;
        --ELSE null  END)
        --AND (CTWR.MAINCUSTOMERID IS NULL OR CTWR.MAINCUSTOMERID NOT IN(2677399, 6136368, 270855))
        --AND CTWR.TICKETTYPEID = @SERVIS_TIPI_ID;
    END;"), con);
            cmd.Parameters.Add("@TICKET_ID", SqlDbType.VarChar).Value = TicketId;
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

        public async Task<r_TicketWaitingPreCheck> TicketWaitingPreCheck(string TicketId)
        {
            var tt = _appConfig.GetConnectionString("EmptorConnection");
            r_TicketWaitingPreCheck _response = null;
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(tt);
            SqlCommand cmd = new SqlCommand(String.Format(@"
DECLARE
@STATUS INT,
@KAYIT_NUMARASI_ID_DESC VARCHAR(12),
@ID_DESC INT,
@R_USERID INT,
@ILGILI_FIRMA_ID INT,
@PARENT_ID INT,
@KAYIT_STATU INT,
@KAYIT_NUMARASI INT,
@BEKLEME_AKTIVITE_SAYISI INT,
@BEKLEYE_ALINMA_SAYISI INT;



SET @R_USERID = 123; --TEKNİSYEN VEYA ANA SORUMLU EMPTOR_ID
SET @KAYIT_NUMARASI_ID_DESC = @TICKET_ID;

--SET @KAYIT_NUMARASI_ID_DESC = '252009212295';


(SELECT
@KAYIT_NUMARASI=ID,
@KAYIT_STATU = C_TICKETSTATUSSUBID,
@ILGILI_FIRMA_ID = C_CONCERNEDACCOUNTID
    FROM CRMTBL_TICKET WITH(NOLOCK)
    WHERE 1 = 1
          AND IDDESC = @KAYIT_NUMARASI_ID_DESC
          AND ACTIVE = 1)

SET @STATUS = (
SELECT 
COUNT(ID) AS STATUS
FROM CRMTBL_TICKET WITH(NOLOCK) WHERE 1 = 1 AND ID=@KAYIT_NUMARASI AND ACTIVE=1
)

SET @PARENT_ID =
(
    SELECT MainCustomerId
    FROM CRMTBL_CUSTOMER WITH(NOLOCK)
    WHERE 1 = 1
          AND ID = @ILGILI_FIRMA_ID
          AND ACTIVE = 1
);

SET @BEKLEME_AKTIVITE_SAYISI =
(
    SELECT COUNT(CA.ACTIVITYTYPEID) AS BeklemeAktiviteSayisi
    FROM CRMTBL_ACTIVITY AS CA WITH(NOLOCK)
    WHERE 1 = 1
          AND CA.TICKETID = @KAYIT_NUMARASI
          AND CA.ACTIVITYTYPEID = 122
          AND CA.ACTIVE = 1
);

SET @BEKLEYE_ALINMA_SAYISI =
(
    SELECT COUNT(SERVICEID) AS BeklemeyeAlinmaSayisi
    FROM PROTBL_SERVICESTATUSLOG AS PSL WITH(NOLOCK)
    WHERE 1 = 1
          AND PSL.SERVICEID = @KAYIT_NUMARASI
          AND PSL.STATUSID = 7
);

--IF(@PARENT_ID IN (2677399))
--  BEGIN
--	IF(@KAYIT_STATU = 8)
--	  BEGIN
--		 SELECT 199 AS StatusCode, 'Zenia Kaydı Yeni İstek Statüsnden beklemeye geçilmez. Kaydı işlemdeye alın.' AS StatusMessage;
--	  END
--	ELSE
--	  BEGIN
--		SELECT 'Kurallara devam';
--	  END
--  END


--IF(@PARENT_ID IN (2677399))
--  BEGIN
--	IF(@KAYIT_STATU = 8)
--	  BEGIN
--		 SELECT 199 AS StatusCode, 'Zenia Kaydı Yeni İstek Statüsnden beklemeye geçilmez. Kaydı işlemdeye alın.' AS StatusMessage;
--	  END
--	ELSE
--	  BEGIN
--		SELECT 'Kurallara devam';
--	  END
--  END

IF(@STATUS = 1)
  BEGIN
    IF(@KAYIT_STATU IN(1, 8, 7))
    BEGIN
       
	   --KAYIT BEKLEMEYE ALINMASI İÇİN DİĞER KURALLAR BURADA OLACAK
        --SELECT 201 AS StatusCode, 'Kayıt Beklemeye almak için Devam et' As StatusMessage;

		IF((SELECT TOP 1
			CASE
			WHEN 
			(DATEPART(WEEKDAY, CA.CREATE_USER_TIME) IN (2,3,4,5,6) 
			AND CAST(CONVERT(TIME, CA.CREATE_USER_TIME) AS DATETIME) >= '17:30:00' 
			OR CAST(CONVERT(TIME, CA.CREATE_USER_TIME) AS DATETIME) < '08:00:00')

			OR 
			(DATEPART(WEEKDAY, CA.CREATE_USER_TIME) IN (7,1) 
              AND CAST(CONVERT(TIME, CA.CREATE_USER_TIME) AS DATETIME) >= '00:00:00' 	  
			 )
			THEN 1
			ELSE 0
			END AS 'result'

			FROM CRMTBL_ACTIVITY AS CA
			WHERE 1 = 1
				  AND CA.ACTIVE = 1
				  AND CA.TICKETID = @KAYIT_NUMARASI
				  AND CA.ACTIVITYTYPEID=122
				  ORDER BY CA.ID DESC)=1)
		  BEGIN
		    IF(@BEKLEME_AKTIVITE_SAYISI <= 0)
            BEGIN
                SELECT 199 AS StatusCode, 
                       'Kayıt içinde bekleme aktivitesi yok. Lütfen Bekleme Aktivitesi giriniz.' AS StatusMessage;
            END;
            ELSE
            BEGIN
                IF(@BEKLEME_AKTIVITE_SAYISI <= @BEKLEYE_ALINMA_SAYISI)
                    BEGIN
                        SELECT 199 AS StatusCode, 
                               'Kayıt içinde bekleme aktivitesi yok. Lütfen ' + CONVERT(NVARCHAR(MAX), @BEKLEME_AKTIVITE_SAYISI + 1) + '. Lütfen Bekleme Aktivitesi giriniz.' AS StatusMessage;
                    END;
                    ELSE
                    BEGIN
                        SELECT 200 AS StatusCode, 
                               'Kayıt Beklemeye Alınabilir!' AS StatusMessage;
                    END;
            END;
		  END
		ELSE
		  BEGIN
			SELECT 199 AS StatusCode, 'Mesai dışında girilen bir bekleme aktivitesi yok.' AS StatusMessage;
		  END
    END;
    ELSE
    BEGIN
        SELECT 404 AS StatusCode, 
               'Kayıt statüsü uygun olmadığından dolayı kayıt beklemeye alınamaz.' AS StatusMessage;
    END;
  END
ELSE
  BEGIN
    SELECT 199 AS StatusCode, 
                       'Böyle bir kayıt numarası bulunamadı.' AS StatusMessage;
  END;
"), con);
            cmd.Parameters.Add("@TICKET_ID", SqlDbType.VarChar).Value = TicketId;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.SelectCommand.CommandTimeout = 500; // default is 30 seconds
            adapter.Fill(dt);


            await con.OpenAsync();

            using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {

                        int StatusCode = Convert.ToInt32(rdr["StatusCode"]);
                        string StatusMessage = rdr["StatusMessage"].ToString();
                        _response = new r_TicketWaitingPreCheck
                        {
                            StatusCode = StatusCode,
                            StatusMessage = StatusMessage
                        };

                    }
                }
            }
            await con.CloseAsync();
            return _response;

        }

        public async Task<Boolean> TicketWaitingHistoryAdded(string TicketIdDesc, int ReasonId, DateTime ReasonDate, int RequestingPerson)
        {
            try
            {
                var _config = _appConfig.GetConnectionString("EmptorConnection");
                using (SqlConnection cn = new SqlConnection(_config))
                using (SqlCommand cmd = new SqlCommand("INSERT INTO RBN_EMPTOR_WaitingTicketHistory(TicketId,MainUserId,TicketIdDesc,CallBackDate,TransactionDate,Active,UserId,WaitingReasonId)\r\nVALUES (@TicketId,@AnaSorumlu,@TicketIdDesc,@ReasonDate,@DateNow,1,@RequestingPerson,@ReasonId);", cn))
                {
                    // define parameters and their values
                    cmd.Parameters.Add("@TicketIdDesc", SqlDbType.VarChar).Value = TicketIdDesc;
                    cmd.Parameters.Add("@ReasonId", SqlDbType.Int).Value = ReasonId;
                    cmd.Parameters.Add("@ReasonDate", SqlDbType.DateTime).Value = ReasonDate;
                    cmd.Parameters.Add("@RequestingPerson", SqlDbType.Int).Value = RequestingPerson;
                    // open connection, execute INSERT, close connection
                    await cn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    await cn.CloseAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<Boolean> TicketWaiting(string TicketIdDesc, int ReasonId, DateTime ReasonDate, int RequestingPerson)
        {
            try
            {
                var tt = _appConfig.GetConnectionString("EmptorConnection");
                Boolean StatusCode = false;
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(tt);

                SqlCommand cmd = new SqlCommand(String.Format(@"
                BEGIN TRANSACTION t1;
                BEGIN TRY

                --DECLARE @RequestingPerson INT, @TicketIdDesc NVARCHAR(MAX), @ReasonId INT, @ReasonDate DATETIME;
                --SET @TicketIdDesc='252009195317';
                --SET @ReasonId=5;
                --SET @ReasonDate= GETDATE();
                --SET @RequestingPerson=1234;

                DECLARE @AnaSorumlu INT, @TicketId INT, @DateNow DATETIME, @UserId INT, @UserPositionId INT, @TICKETSTATUSSUBID INT, @NEWVALUE NVARCHAR(MAX);
                SET @UserId = 8624;
                SET @TICKETSTATUSSUBID = 7;
                SET @UserPositionId = 6484;
                SET @DateNow = GETDATE();

                SET @TicketId =
                (
                    SELECT ID
                    FROM CRMTBL_TICKET
                    WHERE 1 = 1
                          AND IDDESC = @TicketIdDesc
                          AND ACTIVE = 1
                );

                SET @AnaSorumlu = (SELECT RESPONSIBLEUSERID FROM CRMTBL_TICKET WITH(NOLOCK) WHERE ID=@TicketId);

                SET @NEWVALUE = CONVERT(NVARCHAR(10), 7) + '~~' + CONVERT(NVARCHAR(MAX), @DateNow, 121) + '~~' + CONVERT(NVARCHAR(10), @ReasonId) + '~~' + CONVERT(VARCHAR, @ReasonDate, 121);

                EXEC [dbo].[BIZSP_ADDLOG_QUICK]
                     @TABLENAME = 'CRMTBL_TICKET', 
                     @COLUMNNAMES = 'C_TICKETSTATUSSUBID,UPDATE_USER_TIME,C_WAITINGREASONID,C_BACKCALLDATE', 
                     @NEWVALUES = @NEWVALUE, 
                     @RECID = @TicketId, 
                     @USERID = @UserId, 
                     @POSITIONID = @UserPositionId;

                    EXEC SP_UPDATE_TICKET_TO_WAIT
                        @C_TICKETSTATUSSUBID = @TICKETSTATUSSUBID,
                        @UPDATE_USER_TIME = @DateNow,
                        @C_WAITINGREASONID = @ReasonId,
                        @UPDATE_USER_ID = @UserId,
                        @UPDATE_USER_POSITION_ID = @UserPositionId,
                        @C_BACKCALLDATE = @ReasonDate,
                        @IDDESC = @TicketIdDesc;

		                INSERT INTO RBN_EMPTOR_WaitingTicketHistory(TicketId,MainUserId,TicketIdDesc,CallBackDate,TransactionDate,Active,UserId,WaitingReasonId)
		                VALUES (@TicketId,@AnaSorumlu,@TicketIdDesc,@ReasonDate,@DateNow,1,@RequestingPerson,@ReasonId);


                     SELECT CONVERT(bit,1) AS Result;

                END TRY
                BEGIN CATCH

                SELECT CONVERT(bit,0) AS Result;

                IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION t1;
                END CATCH;
                IF @@TRANCOUNT > 0
                 COMMIT TRANSACTION t1  
                "), con);

                cmd.Parameters.Add("@TicketIdDesc", SqlDbType.VarChar).Value = TicketIdDesc;
                cmd.Parameters.Add("@ReasonId", SqlDbType.Int).Value = ReasonId;
                cmd.Parameters.Add("@RequestingPerson", SqlDbType.Int).Value = RequestingPerson;
                cmd.Parameters.Add("@ReasonDate", SqlDbType.DateTime).Value = ReasonDate;

                //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //adapter.Fill(dt);

                await con.OpenAsync();

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    StatusCode = Convert.ToBoolean(rdr["Result"]);
                }

                //using ()
                //{
                //    if (rdr.HasRows)
                //    {

                //    }
                //}
                await con.CloseAsync();
                return StatusCode;


                //var aa = dt.Rows.Count;

                //await con.CloseAsync();

                //return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<r_getMainResponsibleInfo> getMainResponsibleInfo(int UserId, string TicketId)
        {
            r_getMainResponsibleInfo _response = null;
            try
            {
                var tt = _appConfig.GetConnectionString("EmptorConnection");

                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(tt);
                SqlCommand cmd = new SqlCommand(String.Format(@"DECLARE @TICKET_ID VARCHAR(12), @ANASORUMLU_ID INT, @ALTSORUMLU_ID INT;
SET @TICKET_ID = @TicketId;

SET @ANASORUMLU_ID= (SELECT RESPONSIBLEUSERID FROM CRMTBL_TICKET WITH (NOLOCK) WHERE 1 = 1 AND ACTIVE=1 AND IDDESC=@TICKET_ID);
SET @ALTSORUMLU_ID = @UserId;
SELECT 
(SELECT ID FROM BIZTBL_USER WITH (NOLOCK) WHERE 1 = 1 AND ID=@ANASORUMLU_ID AND ACTIVE=1) AS MainResponsibleId,
(SELECT FULLNAME FROM BIZTBL_USER WITH (NOLOCK) WHERE 1 = 1 AND ID=@ANASORUMLU_ID AND ACTIVE=1) AS MainResponsibleFullName,
(SELECT EMAIL FROM BIZTBL_USER WITH (NOLOCK) WHERE 1 = 1 AND ID=@ANASORUMLU_ID AND ACTIVE=1) AS MainResponsibleEmail,
(SELECT FULLNAME FROM BIZTBL_USER WITH (NOLOCK) WHERE 1 = 1 AND ID=@ALTSORUMLU_ID AND ACTIVE=1) AS SubResponsibleFullName,
(SELECT EMAIL FROM BIZTBL_USER WITH (NOLOCK) WHERE 1 = 1 AND ID=@ALTSORUMLU_ID AND ACTIVE=1) AS SubResponsibleEmail
"), con);
                cmd.Parameters.Add("@TicketId", SqlDbType.VarChar).Value = TicketId;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                await con.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            string MainResponsibleFullName = rdr["MainResponsibleFullName"].ToString();
                            string MainResponsibleEmail = rdr["MainResponsibleEmail"].ToString();
                            string SubResponsibleFullName = rdr["SubResponsibleFullName"].ToString();
                            string SubResponsibleEmail = rdr["SubResponsibleEmail"].ToString();
                            string MainResponsibleFullNameId = rdr["MainResponsibleId"].ToString();

                            _response = new r_getMainResponsibleInfo
                            {
                                status = true,
                                MainResponsibleFullName = MainResponsibleFullName,
                                MainResponsibleEmail = MainResponsibleEmail,
                                SubResponsibleFullName = SubResponsibleFullName,
                                SubResponsibleEmail = SubResponsibleEmail,
                                MainResponsibleId = Convert.ToInt32(MainResponsibleFullNameId)
                            };
                        }
                    }
                    else
                    {
                        _response = new r_getMainResponsibleInfo
                        {
                            status = false
                        };
                    }
                }
                con.Close();

                return _response;

            }
            catch
            {
                _response = new r_getMainResponsibleInfo
                {
                    status = false
                };
                return _response;
            }
        }

        public async Task<Boolean> InventoryStillSaveClose(long TicketId)
        {
            Boolean _response = false;
            try
            {
                var tt = _appConfig.GetConnectionString("EmptorConnection");

                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(tt);
                SqlCommand cmd = new SqlCommand(String.Format(@"EXEC	[dbo].[ENVANTER_HAREKETSIZ_KAYITLARI_OTOMATIK_STATU_COZULDU_YAPMA]
		@TicketId = @_TicketId
"), con);
                cmd.Parameters.Add("@_TicketId", SqlDbType.BigInt).Value = TicketId;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                await con.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            string Result = rdr["Result"].ToString();

                            if (Convert.ToBoolean(Result))
                            {
                                _response = true;
                            }
                            else
                            {
                                _response = false;
                            }
                        }
                    }
                    else
                    {
                        _response = false;
                    }
                }
                con.Close();

                return _response;

            }
            catch
            {
                _response = false;
                return _response;
            }
        }

        public async Task<List<long>> getTicketClosedList(string SqlQuery)
        {
            List<long> ticketIds = new List<long>();
            try
            {

                var tt = _appConfig.GetConnectionString("EmptorConnection");

                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(tt);
                SqlCommand cmd = new SqlCommand(String.Format(SqlQuery), con);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                await con.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            string _TicketId = rdr["TicketId"].ToString();
                            ticketIds.Add((long)Convert.ToDouble(_TicketId));
                        }
                    }
                    else
                    {
                        return ticketIds;
                    }
                }
                con.Close();

                return ticketIds;

            }
            catch
            {
                return ticketIds;
            }
        }


        //public async Task<bool> addAttahmentTicket()
        //{
        //    try
        //    {
        //        var tt = _appConfig.GetConnectionString("EmptorBlobConnection");

        //        DataTable dt = new DataTable();
        //        SqlConnection con = new SqlConnection(tt);
        //        SqlCommand cmd = new SqlCommand(String.Format("SELECT TOP 5 * FROM BIZTBL_BLOB"), con);

        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        adapter.Fill(dt);

        //        await con.OpenAsync();

        //        using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
        //        {
        //            if (rdr.HasRows)
        //            {
        //                while (rdr.Read())
        //                {
        //                    string _TicketId = rdr["FILENAME"].ToString();
        //                }
        //            }
        //            else
        //            {
        //                return true;
        //            }
        //        }
        //        con.Close();

        //        return true;

        //    }
        //    catch
        //    {
        //        return true;
        //    }
        //}


        public async Task<bool> EmptorSaveFileToDatabase(EmptorFileUploadModel model)
        {
            var tt = _appConfig.GetConnectionString("EmptorBlobConnection");

            using (SqlConnection connection = new SqlConnection(tt))
            {
                connection.Open();

                string query = @"
            INSERT INTO [dbo].[BIZTBL_BLOB]
            (
                [FILENAME],
                [MIMETYPE],
                [DATASIZE],
                [SUMMARY],
                [CREATE_USER_TIME],
                [CREATE_USER_POSITION_ID],
                [CREATE_USER_ID],
                [UPDATE_USER_TIME],
                [UPDATE_USER_POSITION_ID],
                [UPDATE_USER_ID],
                [ACTIVE],
                [BLOBDATA],
                [C_BLOBTYPEID]
            )
            VALUES
            (
                @FILENAME,
                @MIMETYPE,
                @DATASIZE,
                @SUMMARY,
                @CREATE_USER_TIME,
                @CREATE_USER_POSITION_ID,
                @CREATE_USER_ID,
                @UPDATE_USER_TIME,
                @UPDATE_USER_POSITION_ID,
                @UPDATE_USER_ID,
                @ACTIVE,
                @BLOBDATA,
                @C_BLOBTYPEID
            )";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FILENAME", model.FileName);
                    command.Parameters.AddWithValue("@MIMETYPE", model.MimeType);
                    command.Parameters.AddWithValue("@DATASIZE", model.DataSize);
                    command.Parameters.AddWithValue("@SUMMARY", model.Summary);
                    command.Parameters.AddWithValue("@CREATE_USER_TIME", model.CreateUserTime);
                    command.Parameters.AddWithValue("@CREATE_USER_POSITION_ID", model.CreateUserPositionId);
                    command.Parameters.AddWithValue("@CREATE_USER_ID", model.CreateUserId);
                    command.Parameters.AddWithValue("@UPDATE_USER_TIME", model.UpdateUserTime);
                    command.Parameters.AddWithValue("@UPDATE_USER_POSITION_ID", model.UpdateUserPositionId);
                    command.Parameters.AddWithValue("@UPDATE_USER_ID", model.UpdateUserId);
                    command.Parameters.AddWithValue("@ACTIVE", model.Active);
                    //command.Parameters.AddWithValue("@FILEPATH", model.FilePath);
                    command.Parameters.AddWithValue("@BLOBDATA", model.BlobData);
                    command.Parameters.AddWithValue("@C_BLOBTYPEID", model.CBlobTypeId);
                    await command.ExecuteNonQueryAsync();
                }
            }

            return true;

        }
    }
}