using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Kiota.Abstractions;
using Robi_N_WebAPI.Controllers;
using Robi_N_WebAPI.Utility;
using Robi_N_WebAPI.Utility.Tables;
using SgkViziteReference;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace Robi_N_WebAPI.Services
{


    public  class   ViziteService
    {

        private readonly AIServiceDbContext _db;


        public ViziteService(AIServiceDbContext db)
        {
            _db = db;
        }


      

        public async Task RaporSorgulaOnay()
        {
            ViziteGonderClient _sgkClient = new ViziteGonderClient();

            //SGK Sicil Lokasyonları Listesi
            var firms = _db.RBN_SGK_VisitingIntroductionInformation.Where(x => x.active == true).ToList();
            if (firms.Count > 0)
            {
                foreach (var item in firms)
                {
                    //SGK Vizite İçin WSToken Almak Al.
                    var wsLogin = await _sgkClient.wsLoginAsync(item.username, item.workplaceCode, item.workplacePassword);
                    if (wsLogin.wsLoginReturn.sonucKod == 0)
                    {
                        if (String.IsNullOrEmpty(wsLogin.wsLoginReturn.wsToken))
                        {
                            //30dk geçerli token
                            string _token = wsLogin.wsLoginReturn.wsToken;

                            //Tarih ile Rapor Sorgulama
                            var reports = await _sgkClient.raporAramaTarihileAsync(item.username, item.workplaceCode, _token, DateTime.Now.ToString("dd.MM.yyyy"));
                            if (reports.raporAramaTarihileReturn.sonucKod == 0)
                            {
                                if (reports.raporAramaTarihileReturn.raporAramaTarihleBeanArray.Where(x => x.ARSIV == "0").Count() > 0)
                                {
                                    foreach (var report in reports.raporAramaTarihileReturn.raporAramaTarihleBeanArray.Where(x => x.ARSIV == "0"))
                                    {
                                        var reportCheck = await _db.RBN_SGK_HealthReports.Where(x => x.MEDULARAPORID == Convert.ToInt64(report.MEDULARAPORID)).FirstAsync();
                                        if (reportCheck != null)
                                        {
                                            var _record = new RBN_SGK_HealthReports()
                                            {
                                                ISYERIKODU = Convert.ToInt32(item.workplaceCode),
                                                TCKIMLIKNO = Convert.ToInt64(report.TCKIMLIKNO),
                                                AD = report.AD,
                                                SOYAD = report.SOYAD,
                                                MEDULARAPORID = Convert.ToInt64(report.MEDULARAPORID),
                                                RAPORTAKIPNO = report.RAPORTAKIPNO,
                                                RAPORSIRANO = Convert.ToInt32(report.RAPORSIRANO),
                                                POLIKLINIKTAR = Convert.ToDateTime(report.POLIKLINIKTAR),
                                                YATRAPBASTAR = Convert.ToDateTime(report.YATRAPBASTAR),
                                                YATRAPBITTAR = Convert.ToDateTime(report.YATRAPBITTAR),
                                                ABASTAR = Convert.ToDateTime(report.ABASTAR),
                                                ABITTAR = Convert.ToDateTime(report.ABITTAR),
                                                ISBASKONTTAR = Convert.ToDateTime(report.ISBASKONTTAR),
                                                DOGUMONCBASTAR = Convert.ToDateTime(report.DOGUMONCBASTAR),
                                                ISKAZASITARIHI = Convert.ToDateTime(report.ISKAZASITARIHI),
                                                RAPORDURUMU = Convert.ToInt32(report.RAPORDURUMU),
                                                VAKA = Convert.ToInt32(report.VAKA),
                                                VAKAADI = report.VAKAADI,
                                                ARSIV = Convert.ToInt32(report.ARSIV),
                                                process = 1
                                                
                                            };
                                            var lastRecord = _db.RBN_SGK_HealthReports.Add(_record);
                                            if (await _db.SaveChangesAsync() == 1)
                                            {
                                                //Detaylı Rapor Sorgulama
                                                var reportDetails = await _sgkClient.raporAramaKimlikNoAsync(item.username, item.workplaceCode, _token, _record.TCKIMLIKNO.ToString());
                                                if (reportDetails.raporAramaKimlikNoReturn.sonucKod == 0)
                                                {
                                                    if (reportDetails.raporAramaKimlikNoReturn.raporBeanArray.Count() > 0)
                                                    {
                                                        foreach (var reportDetail in reportDetails.raporAramaKimlikNoReturn.raporBeanArray)
                                                        {
                                                            _record.RAPORBITTAR = Convert.ToDateTime(reportDetail.RAPORBITTAR);
                                                            _record.ISVERENEBILDIRILDIGITARIH = Convert.ToDateTime(reportDetail.ISVERENEBILDIRILDIGITARIH);
                                                            _record.BASHEKIMONAYTARIHI = Convert.ToDateTime(reportDetail.BASHEKIMONAYTARIHI);
                                                            _record.TESISKODU = Convert.ToInt64(reportDetail.TESISKODU);
                                                            _record.TESISADI = reportDetail.TESISADI;
                                                        }
                                                    }
                                                }
                                            }

                                            if (await _db.SaveChangesAsync() == 1)
                                            {
                                                //Rapor Onay Bilgileri
                                                var reportConfirm = await _sgkClient.raporOnayAsync(
                                                    item.username,
                                                    item.workplaceCode,
                                                    _token,
                                                    _record.TCKIMLIKNO.ToString(),
                                                    _record.VAKA.ToString(),
                                                    _record.MEDULARAPORID.ToString(),
                                                    "0",
                                                    _record.RAPORBITTAR.ToString("dd.MM.yyyy"));
                                                if (reportConfirm.raporOnayReturn.sonucKod == 0)
                                                {
                                                    Regex rgx = new Regex("(?<=medulaRaporId: |bildirimId: )[0-9]+");

                                                    string _BildirimId = rgx.Matches(reportConfirm.raporOnayReturn.sonucAciklama)[1].ToString();
                                                    if(String.IsNullOrEmpty(_BildirimId))
                                                    {
                                                        //RaporBilgisi
                                                        _record.BildirimId = Convert.ToInt64(_BildirimId);
                                                        _record.OnaylamaTarihi = DateTime.Now;
                                                        _record.process = 0;
                                                        if(await _db.SaveChangesAsync() == 1)
                                                        {
                                                            //Rapor Okundu Kapat
                                                            var sgkRaporKapat = await _sgkClient.raporOkunduKapatAsync(item.username,item.workplaceCode,_token,_record.MEDULARAPORID.ToString());
                                                            if (sgkRaporKapat.raporOkunduKapatReturn.sonucKod == 0)
                                                            {
                                                                //Rapot Başarıyla Kapatıldı ve Onaylandı IK Ekiplerine Mail Gönder
                                                            }
                                                        }
                                                    } else
                                                    {
                                                        //Bildirim Numarası Boş
                                                    }
                                                } else
                                                {
                                                    //Rapor Onaylanmadı
                                                }
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
                }

            }
        }






    }
}
