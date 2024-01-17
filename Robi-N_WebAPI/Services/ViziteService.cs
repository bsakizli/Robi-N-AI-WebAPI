using Hangfire;
using Microsoft.EntityFrameworkCore;
using Robi_N_WebAPI.Utility;
using SgkViziteReference;
using System.Text.RegularExpressions;
using DateTime = System.DateTime;

namespace Robi_N_WebAPI.Services
{


    public class ViziteService
    {

        ViziteGonderClient _sgkClient = new ViziteGonderClient();

        private readonly AIServiceDbContext _db;

        public ViziteService(AIServiceDbContext db)
        {
            _db = db;
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


        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task RaporSorgulaOnay()
        {
            ViziteGonderClient _sgkClient = new ViziteGonderClient();

            //SGK Sicil Lokasyonları Listesi
            var firms = _db.RBN_SGK_VisitingIntroductionInformation.Where(x => x.active == true).ToList();
            if (firms.Count > 0)
            {
                #region Rapor Kontrol ve Onay 
                foreach (var item in firms)
                {
                    #region Rapor KOntrol ve Otomatik Onay
                    var wsLogin = await getSGKToken(item.username, item.workplaceCode, item.workplacePassword);
                    if (wsLogin.wsLoginReturn.sonucKod == 0)
                    {
                        if (!String.IsNullOrEmpty(wsLogin.wsLoginReturn.wsToken))
                        {
                            //30dk geçerli token
                            string _token = wsLogin.wsLoginReturn.wsToken;

                            //Tarih ile Rapor Sorgulama
                            var reports = await getSGKraporAramaTarihileAsync(item.username, item.workplaceCode, _token, DateTime.Now);
                            if (reports.raporAramaTarihileReturn.sonucKod == 0)
                            {
                                if (reports.raporAramaTarihileReturn.raporAramaTarihleBeanArray.Where(x => x.ARSIV == "0").Count() > 0)
                                {
                                    foreach (var report in reports.raporAramaTarihileReturn.raporAramaTarihleBeanArray.Where(x => x.ARSIV == "0"))
                                    {
                                        System.Threading.Thread.Sleep(2000);
                                        var reportCheck = await _db.RBN_SGK_HealthReports.Where(x => x.MEDULARAPORID == Convert.ToInt64(report.MEDULARAPORID)).FirstOrDefaultAsync();
                                        if (reportCheck == null)
                                        {

                                            var _record = new RBN_SGK_HealthReports()
                                            {
                                                ISYERIKODU = Convert.ToInt32(item.workplaceCode),
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
                                                mailSend = false,
                                                addDate = DateTime.Now

                                            };
                                            var lastRecord = _db.RBN_SGK_HealthReports.Add(_record);
                                            if (await _db.SaveChangesAsync() == 1)
                                            {
                                                //Detaylı Rapor Sorgulama
                                                var reportDetails = await getSGKraporAramaKimlikNoAsync(item.username, item.workplaceCode, _token, _record.TCKIMLIKNO);
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

                                                    var reportConfirm = await getSGKraporOnay(
                                                       item.username,
                                                       item.workplaceCode,
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
                                                                var sgkRaporKapat = await getSGKraporOkunduKapatAsync(item.username, item.workplaceCode, _token, _record.MEDULARAPORID);
                                                                if (sgkRaporKapat.raporOkunduKapatReturn.sonucKod == 0)
                                                                {
                                                                    _record.process = 0;
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
                #endregion


                #region Tarih Bazlı Rapor Onay
                foreach (var item in firms)
                {
                    #region Rapor Tarih Kontrol Onay & Otomatik Onay
                    var getDateReports = await _db.RBN_SGK_HealthReports.Where(x => x.process == 1 && x.BildirimId != null && x.RAPORBITTAR == DateTime.Now && x.ISYERIKODU == Convert.ToInt32(item.workplaceCode)).ToListAsync();
                    if (getDateReports.Count() > 0)
                    {
                        foreach (var report in getDateReports)
                        {
                            var _sgkToken = await getSGKToken(item.username, item.workplaceCode, item.workplacePassword);
                            if (_sgkToken != null && !String.IsNullOrEmpty(_sgkToken.wsLoginReturn.wsToken))
                            {
                                string _token = _sgkToken.wsLoginReturn.wsToken;

                                //Rapor Onay
                                var sgkConfirmReport = await getSGKraporOnay(item.username, item.workplaceCode, _token, report.TCKIMLIKNO, report.VAKA.ToString(), report.MEDULARAPORID, report.RAPORBITTAR, "0");
                                if (sgkConfirmReport != null && sgkConfirmReport.raporOnayResponse != null)
                                {
                                    if (sgkConfirmReport.raporOnayResponse.raporOnayReturn.sonucKod == 0)
                                    {
                                        report.BildirimId = Convert.ToInt64(sgkConfirmReport.bildirimId);
                                        report.process = 0;
                                        if (await _db.SaveChangesAsync() == 1)
                                        {
                                            var confirmOkunduKapat = await getSGKraporOkunduKapatAsync(item.username, item.workplaceCode, _token, report.MEDULARAPORID);
                                            if (confirmOkunduKapat != null && confirmOkunduKapat.raporOkunduKapatReturn.sonucKod == 0)
                                            {
                                                //Rapor Okundu ve Onaylandı.
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                #endregion

            }
        }

    }
}