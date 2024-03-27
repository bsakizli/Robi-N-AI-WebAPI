using DocumentFormat.OpenXml.Spreadsheet;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using Hangfire;
using MailEntity;
using Microsoft.EntityFrameworkCore;
using Robi_N_WebAPI.Utility;
using Robi_N_WebAPI.Utility.Tables;
using SgkViziteReference;
using System.Data;
using System.Text.RegularExpressions;
using DateTime = System.DateTime;
using MailEntity.Models;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.Security;
using RobinCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Robi_N_WebAPI.Model.Request;
using System.Text;

namespace Robi_N_WebAPI.Services
{


    public class ViziteService
    {

        ViziteGonderClient _sgkClient = new ViziteGonderClient();
        MailService _MailService = new MailService();
        private readonly AIServiceDbContext _db;
        private readonly IWebHostEnvironment _appEnvironment;
        //private readonly IConfiguration _configuration;

        private HR_IAppSettings _appConfig;



        public ViziteService(IWebHostEnvironment appEnvironment, AIServiceDbContext db, HR_IAppSettings appConfig)
        {
            //_configuration = configuration;
            _appEnvironment = appEnvironment;
            _db = db;
            _appConfig = appConfig;
        }

        public class raporOnayResponseCustom
        {
            public raporOnayResponse? raporOnayResponse { get; set; }
            public string? bildirimId { get; set; }

        }

        public async Task<wsLoginResponse> getSGKToken(string username, string workplaceCode, string workplacePassword)
        {
            return await _sgkClient.wsLoginAsync(username, workplaceCode, workplacePassword);
        }

        public async Task<raporAramaTarihileResponse> getSGKraporAramaTarihileAsync(string username, string workplaceCode, string token, DateTime date)
        {
            return await _sgkClient.raporAramaTarihileAsync(username, workplaceCode, token, date.ToString("dd.MM.yyyy"));
        }

        public async Task<raporAramaKimlikNoResponse> getSGKraporAramaKimlikNoAsync(string username, string workplaceCode, string token, long kimlikNumarasi)
        {
            return await _sgkClient.raporAramaKimlikNoAsync(username, workplaceCode, token, kimlikNumarasi.ToString());
        }

        public async Task<raporOnayResponseCustom> getSGKraporOnay(string username, string workplaceCode, string token, long kimlikNumarasi, string vaka, long medulaRaporId, DateTime raporBitisTarihi, string nitelikDurumu = "0")
        {
            Regex rgx = new Regex("(?<=medulaRaporId: |bildirimId: )[0-9]+");


            var getService = await _sgkClient.raporOnayAsync(username, workplaceCode, token, kimlikNumarasi.ToString(), vaka.ToString(), medulaRaporId.ToString(), nitelikDurumu, raporBitisTarihi.ToString("dd.MM.yyyy"));
            string _BildirimId = rgx.Matches(getService.raporOnayReturn.sonucAciklama)[1].ToString();
            raporOnayResponseCustom raporOnayResponseCustom = new raporOnayResponseCustom
            {
                raporOnayResponse = getService,
                bildirimId = _BildirimId

            };
            //return await _sgkClient.raporOnayAsync(username,workplaceCode,token,kimlikNumarasi.ToString(),vaka.ToString(),medulaRaporId.ToString(),nitelikDurumu, raporBitisTarihi.ToString("dd.MM.yyyy"));
            return raporOnayResponseCustom;

        }

        public async Task<raporOkunduKapatResponse> getSGKraporOkunduKapatAsync(string username, string workplaceCode, string token, long medulaRaporId)
        {
            return await _sgkClient.raporOkunduKapatAsync(username, workplaceCode, token, medulaRaporId.ToString());
        }


        public async Task<personelimDegildirResponse> setPersonelimDegildirResponseAsync(string username, string workplaceCode, string token, long kimlikNumarasi, string vaka, long medulaRaporId)
        {
            return await _sgkClient.personelimDegildirAsync(username,workplaceCode,token,kimlikNumarasi.ToString(),vaka, medulaRaporId.ToString());
        }


        public async Task<Boolean> SuccessFactorsPersonnelControl(long tcKimlikNo)
        {
           try
            {
                requestSuccessFactorsPersonnelControl _req = new requestSuccessFactorsPersonnelControl
                {
                    ApiKey = "f53ecc86-4b8d-4b6c-ac96-c4e139e8fd2b",
                    TcKimlikNo = tcKimlikNo.ToString()
                };

                var json = JsonConvert.SerializeObject(_req);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync("http://10.254.46.69/BackOfficeApi/Personal/SearchPersonalInfosByTCKN", data);
                var result = await response.Content.ReadAsStringAsync();
                dynamic jsonResult = JsonConvert.DeserializeObject(result);
                if (jsonResult != null)
                {
                    return jsonResult[0].Aktif;
                }
                else
                {
                    return false;
                }
            } catch
            {
                return false;
            }
           
        }


        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task RaporSorgulaOnay()
        {

            AzureKeyVaultRobin AzureKeyVault = new AzureKeyVaultRobin(_appConfig);

            ViziteGonderClient _sgkClient = new ViziteGonderClient();

            //SGK Sicil Lokasyonları Listesi
            var firms = _db.RBN_SGK_VisitingIntroductionInformation.Where(x => x.active == true).ToList();
            if (firms.Count > 0)
            {
                string _azureServiceLink = String.Empty;

                #region Rapor Kontrol ve Onay 
                foreach (var item in firms)
                {
                    //Set Link
                    if(item.FirmCode == 1)
                    {
                        _azureServiceLink = "https://bdhhr.vault.azure.net/";
                    } else if (item.FirmCode == 2)
                    {
                        _azureServiceLink = "https://netashr.vault.azure.net";
                    }

                    #region KeyVaultGetPassword Start
                    var azureKey = await AzureKeyVault.RobinAzureKeyVault(item.value, _azureServiceLink);
                    #endregion

                    if (azureKey != null && !String.IsNullOrEmpty(azureKey.username) && !String.IsNullOrEmpty(azureKey.password) && !String.IsNullOrEmpty(azureKey.workcode))
                    {
                        #region Rapor KOntrol ve Otomatik Onay
                        var wsLogin = await getSGKToken(azureKey.username, azureKey.workcode, azureKey.password);
                        if (wsLogin.wsLoginReturn.sonucKod == 0)
                        {
                            if (!String.IsNullOrEmpty(wsLogin.wsLoginReturn.wsToken))
                            {
                                //30dk geçerli token
                                string _token = wsLogin.wsLoginReturn.wsToken;

                                //Tarih ile Rapor Sorgulama
                                var reports = await getSGKraporAramaTarihileAsync(azureKey.username, azureKey.workcode, _token, DateTime.Now);
                                if (reports.raporAramaTarihileReturn.sonucKod == 0)
                                {
                                    if (reports.raporAramaTarihileReturn.raporAramaTarihleBeanArray.Where(x => x.ARSIV == "0" && x.VAKA == "3").Count() > 0)
                                    {
                                        foreach (var report in reports.raporAramaTarihileReturn.raporAramaTarihleBeanArray.Where(x => x.ARSIV == "0" && x.VAKA == "3"))
                                        {
                                            var reportCheck = await _db.RBN_SGK_HealthReports.Where(x => x.MEDULARAPORID == Convert.ToInt64(report.MEDULARAPORID)).FirstOrDefaultAsync();
                                            if (reportCheck == null)
                                            {
                                                var _record = new RBN_SGK_HealthReports()
                                                {
                                                    ISYERIKODU = Convert.ToInt32(azureKey.workcode),
                                                    ISYERIADI = item.region.ToUpper(),
                                                    TCKIMLIKNO = Convert.ToInt64(report.TCKIMLIKNO),
                                                    AD = report.AD.TrimEnd(),
                                                    SOYAD = report.SOYAD.TrimEnd(),
                                                    MEDULARAPORID = Convert.ToInt64(report.MEDULARAPORID),
                                                    RAPORTAKIPNO = report.RAPORTAKIPNO,
                                                    RAPORSIRANO = Convert.ToInt32(report.RAPORSIRANO),
                                                    POLIKLINIKTAR = Convert.ToDateTime(report.POLIKLINIKTAR),
                                                    YATRAPBASTAR = Convert.ToDateTime(report.YATRAPBASTAR),
                                                    YATRAPBITTAR = Convert.ToDateTime(report.YATRAPBITTAR),
                                                    ABASTAR = Convert.ToDateTime(report.ABASTAR),
                                                    ABITTAR = Convert.ToDateTime(report.ABITTAR),
                                                    ISBASKONTTAR = Convert.ToDateTime(report.ISBASKONTTAR),
                                                    RAPORDURUMU = Convert.ToInt32(report.RAPORDURUMU),
                                                    VAKA = Convert.ToInt32(report.VAKA),
                                                    VAKAADI = report.VAKAADI.TrimEnd(),
                                                    ARSIV = Convert.ToInt32(report.ARSIV),
                                                    process = 1,
                                                    active = true,
                                                    FirmCode = item.FirmCode,
                                                    mailSend = false,
                                                    addDate = DateTime.Now

                                                };
                                                var lastRecord = _db.RBN_SGK_HealthReports.Add(_record);
                                                if (await _db.SaveChangesAsync() == 1)
                                                {
                                                    //Detaylı Rapor Sorgulama
                                                    var reportDetails = await getSGKraporAramaKimlikNoAsync(azureKey.username, azureKey.workcode, _token, _record.TCKIMLIKNO);
                                                    if (reportDetails.raporAramaKimlikNoReturn.sonucKod == 0)
                                                    {
                                                        if (reportDetails.raporAramaKimlikNoReturn.raporBeanArray.Where(x => x.MEDULARAPORID == report.MEDULARAPORID).Count() > 0)
                                                        {
                                                            foreach (var reportDetail in reportDetails.raporAramaKimlikNoReturn.raporBeanArray.Where(x => x.MEDULARAPORID == report.MEDULARAPORID))
                                                            {
                                                                if (reportDetail.DOGUMONCBASTAR != null)
                                                                {
                                                                    _record.DOGUMONCBASTAR = Convert.ToDateTime(reportDetail.DOGUMONCBASTAR);
                                                                }

                                                                if (!String.IsNullOrEmpty(reportDetail.ISKAZASITARIHI.ToString()) && reportDetail.ISKAZASITARIHI != "-")
                                                                {
                                                                    _record.ISKAZASITARIHI = Convert.ToDateTime(reportDetail.ISKAZASITARIHI);
                                                                }

                                                                _record.RAPORBITTAR = Convert.ToDateTime(reportDetail.RAPORBITTAR);
                                                                _record.ISVERENEBILDIRILDIGITARIH = Convert.ToDateTime(reportDetail.ISVERENEBILDIRILDIGITARIH);
                                                                _record.BASHEKIMONAYTARIHI = Convert.ToDateTime(reportDetail.BASHEKIMONAYTARIHI);
                                                                _record.TESISKODU = Convert.ToInt64(reportDetail.TESISKODU);
                                                                _record.TESISADI = reportDetail.TESISADI.TrimEnd();
                                                            }
                                                        }
                                                    }
                                                }

                                                if (await _db.SaveChangesAsync() == 1)
                                                {
                                                    if (_record.RAPORBITTAR <= DateTime.Now)
                                                    {
                                                        
                                                        #region Rapor Onaylama

                                                        if(await SuccessFactorsPersonnelControl(_record.TCKIMLIKNO))
                                                        {
                                                            var reportConfirm = await getSGKraporOnay(
                                                           azureKey.username,
                                                           azureKey.workcode,
                                                           _token,
                                                           _record.TCKIMLIKNO,
                                                           _record.VAKA.ToString(),
                                                           _record.MEDULARAPORID,
                                                           _record.RAPORBITTAR,
                                                           "0"
                                                          );

                                                            if (reportConfirm.raporOnayResponse != null)
                                                            {
                                                                if (reportConfirm.raporOnayResponse.raporOnayReturn.sonucKod == 0 && !String.IsNullOrEmpty(reportConfirm.bildirimId))
                                                                {
                                                                    //RaporBilgisi
                                                                    _record.BildirimId = Convert.ToInt64(reportConfirm.bildirimId);
                                                                    _record.OnaylamaTarihi = DateTime.Now;
                                                                    if (await _db.SaveChangesAsync() == 1)
                                                                    {
                                                                        //Rapor Okundu Kapat
                                                                        var sgkRaporKapat = await getSGKraporOkunduKapatAsync(azureKey.username, azureKey.workcode, _token, _record.MEDULARAPORID);
                                                                        if (sgkRaporKapat.raporOkunduKapatReturn.sonucKod == 0)
                                                                        {
                                                                            _record.process = 0;
                                                                            _record.Personel = true;
                                                                            _record.mailSend = true;
                                                                            
                                                                            if (await _db.SaveChangesAsync() == 1)
                                                                            {
                                                                                //Rapor Onaylanmıştır.
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //Rapor Kapatılmadı
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        //Veri Tabanına Onay Bilgileri Kayıt Edilemedi
                                                                    }
                                                                }
                                                            }
                                                        } else
                                                        {
                                                            var personelimDegildir = await setPersonelimDegildirResponseAsync(azureKey.username,azureKey.workcode,_token,_record.TCKIMLIKNO,_record.VAKA.ToString(),_record.MEDULARAPORID);
                                                            if(personelimDegildir.personelimDegildirReturn.sonucKod == 0)
                                                            {
                                                                _record.Personel = false;
                                                                await _db.SaveChangesAsync();
                                                                //Mail ve Gönderebilirsin.
                                                            } else
                                                            {
                                                                //Personelim Değildir Çalışmadı
                                                            }
                                                        }
                                                        #endregion Rapor Onaylama Bitiş
                                                    }
                                                }
                                                else
                                                {
                                                    //Rapor Veritabanıa kayıt edilemedi
                                                }
                                            }
                                            else
                                            {
                                                //Zaten Böyle vir rapor var.
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        //AzureKey Boş Geldi
                    }
                }
                #endregion


                #region Tarih Bazlı Rapor Onay
                foreach (var item in firms)
                {

                    #region KeyVaultGetPassword Start
                    var azureKey = await AzureKeyVault.RobinAzureKeyVault(item.value, _azureServiceLink);
                    #endregion

                    if (azureKey != null && !String.IsNullOrEmpty(azureKey.username) && !String.IsNullOrEmpty(azureKey.password) && !String.IsNullOrEmpty(azureKey.workcode))
                    {
                        #region Rapor Tarih Kontrol Onay & Otomatik Onay
                        var getDateReports = await _db.RBN_SGK_HealthReports.Where(x => x.process == 1 && x.BildirimId == null && x.RAPORBITTAR.Date  == DateTime.Now.Date && x.ISYERIKODU == Convert.ToInt32(azureKey.workcode)).ToListAsync();
                        if (getDateReports.Count() > 0)
                        {
                            foreach (var report in getDateReports)
                            {
                                var _sgkToken = await getSGKToken(azureKey.username, azureKey.workcode, azureKey.password);
                                if (_sgkToken != null && !String.IsNullOrEmpty(_sgkToken.wsLoginReturn.wsToken))
                                {
                                    string _token = _sgkToken.wsLoginReturn.wsToken;


                                    if (await SuccessFactorsPersonnelControl(report.TCKIMLIKNO))
                                    {
                                        //Rapor Onay
                                        var sgkConfirmReport = await getSGKraporOnay(azureKey.username, azureKey.workcode, _token, report.TCKIMLIKNO, report.VAKA.ToString(), report.MEDULARAPORID, report.RAPORBITTAR, "0");
                                        if (sgkConfirmReport != null && sgkConfirmReport.raporOnayResponse != null)
                                        {
                                            if (sgkConfirmReport.raporOnayResponse.raporOnayReturn.sonucKod == 0)
                                            {
                                                report.BildirimId = Convert.ToInt64(sgkConfirmReport.bildirimId);
                                                report.process = 0;
                                                report.OnaylamaTarihi = DateTime.Now;
                                                report.Personel = true;
                                                report.mailSend = true;
                                                if (await _db.SaveChangesAsync() == 1)
                                                {
                                                    var confirmOkunduKapat = await getSGKraporOkunduKapatAsync(azureKey.username, azureKey.workcode, _token, report.MEDULARAPORID);
                                                    if (confirmOkunduKapat != null && confirmOkunduKapat.raporOkunduKapatReturn.sonucKod == 0)
                                                    {
                                                        //Rapor Okundu ve Onaylandı.
                                                    }
                                                }
                                            }
                                        }

                                    } else
                                    {
                                        var personelimDegildir = await setPersonelimDegildirResponseAsync(azureKey.username, azureKey.workcode, _token, report.TCKIMLIKNO, report.VAKA.ToString(), report.MEDULARAPORID);
                                        if (personelimDegildir.personelimDegildirReturn.sonucKod == 0)
                                        {
                                            report.Personel = false;
                                            await _db.SaveChangesAsync();
                                            //Mail ve Gönderebilirsin.
                                        }
                                        else
                                        {
                                            //Personelim Değildir Çalışmadı
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }
                    else {
                    //Azure Key Boş Geldi
                    }
                }
                #endregion

            }

            #region Mail Gönderme ve Bilgilendirme
            //Mail Gönderme Süreçleri
            var _reports = await _db.RBN_SGK_HealthReports.Where(x => x.active == true && x.process == 0 && x.BildirimId != null && x.mailSend == true && x.active == true).ToListAsync();
            if (_reports.Count > 0)
            {
                //MemoryStream stream;
                string webRootPath = _appEnvironment.WebRootPath; // Get the path to the wwwroot folder
                WebReport webReport = new WebReport(); // Create a Web Report Object
                webReport.Report.Load(webRootPath + "/reports/SgkViziteOnayFormu.frx"); // Load the report into the WebReport object
                var dataSet = new DataSet();
                webReport.Report.RegisterData(_reports, "Reports"); // Register the data source in the report
                webReport.Report.GetDataSource("Reports").Enabled = true;

                webReport.Report.Prepare();

                Stream stream = new MemoryStream();
                webReport.Report.Export(new PDFSimpleExport(), stream);

                stream.Position = 0;


                PdfDocument document = PdfReader.Open(stream);
                PdfSecuritySettings securitySettings = document.SecuritySettings;

                securitySettings.UserPassword = await Helper.Helper.PdfGenerateCustomPassword(DateTime.Now);
                securitySettings.OwnerPassword = "Bdhpass!.";

                securitySettings.PermitAccessibilityExtractContent = false;
                securitySettings.PermitAnnotations = false;
                securitySettings.PermitAssembleDocument = false;
                securitySettings.PermitExtractContent = false;
                securitySettings.PermitFormsFill = true;
                securitySettings.PermitFullQualityPrint = false;
                securitySettings.PermitModifyDocument = false;
                securitySettings.PermitPrint = true;


                MemoryStream streamPdf = new MemoryStream();
                document.Save(streamPdf, false);
                byte[] bytes = streamPdf.ToArray();

                document.Save(stream, false);


                List<EmailReports> emailReports = new List<EmailReports>();
                foreach (var item in _reports)
                {
                    EmailReports _mailReport = new EmailReports
                    {
                        AdSoyad = item.AD + " " + item.SOYAD,
                        KimlikNumarasi = Helper.Helper.tcToMask(item.TCKIMLIKNO.ToString(), true),
                        MedulaRaporId = item.MEDULARAPORID,
                        RaporTakipNumarasi = item.RAPORTAKIPNO,
                        OnayReferansId = item.BildirimId,
                        RaporBaslamaTarihi = item.ABASTAR,
                        RaporBirisTarihi = item.RAPORBITTAR
                    };
                    emailReports.Add(_mailReport);
                }

                var _mailSend = _MailService.SGKOnayMailGonder(streamPdf, emailReports);
                if (_mailSend)
                {
                    _reports.ForEach(a => { a.mailSend = false; });
                    if (_db.SaveChanges() == 1)
                    {
                        //Tüm Süreç Tamamlandı
                    }
                }
                else
                {
                    //Mail Gönderilemedi
                }

            }
            #endregion



        }

    }
}