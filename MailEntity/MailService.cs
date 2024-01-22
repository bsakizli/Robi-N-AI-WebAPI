using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using EmptorUtility.Models.Response;
using System.IO;
using MailEntity.Models;

namespace MailEntity
{
    public class MailService
    {
        //private readonly IConfiguration _configuration;

        //public MailService(AIServiceDbContext db, IConfiguration configuration)
        //{
        //    _db = db;
        //    _configuration = configuration;
        //}

        //_configuration.GetValue<string>("IronBarCode.LicenseKey");


        public bool WaitingEmptorSendMail(string TicketId, r_getMainResponsibleInfo _request, string CompanyName, DateTime WaitingDate, string ReasonName)
        {
            try
            {
                var message = new MimeMessage();
                message.To.Add(MailboxAddress.Parse(_request.SubResponsibleEmail));
                message.Cc.Add(MailboxAddress.Parse(_request.MainResponsibleEmail));
                message.Bcc.Add(MailboxAddress.Parse("baris.sakizli@bdh.com.tr"));
                //message.Cc.Add(MailboxAddress.Parse("kemal.yurdakul@bdh.com.tr"));
                //message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

                message.From.Add(MailboxAddress.Parse("robin@bdh.com.tr"));

                message.Subject = String.Format("📣📣 {0} numaralı kayıt beklemeye alınmıştır... ✅", TicketId);


                //We will say we are sending HTML. But there are options for plaintext etc. 
                message.Body = new TextPart(TextFormat.Html)
                {

                    Text = String.Format($@"<div style=""background-color:#eee !important;font-family:'system-ui' , '-apple-system' , 'blinkmacsystemfont' , 'segoe ui' , 'roboto' , 'oxygen' , 'ubuntu' , 'cantarell' , 'open sans' , 'helvetica neue' , sans-serif;margin:0"">
    <table border='0' style='border-collapse:collapse;margin:20px auto 20px auto;max-width:750px;width:100%'>
      <tbody>
        <tr style='background-color:#009ca6;height:105px;width:100%'>
          <td>
            <table style='color:#fff;width:100%'>
              <tbody>
                <tr>
                  <td style='padding-left:30px;width:170px'>
                    <a href='https://www.bdh.com/' data-link-id='96' target='_blank' rel='noopener noreferrer'>
                      <img alt='bdh-logo' src='https://www.bdh.com.tr/wp-content/uploads/2019/08/BDHLogo2019_160w.png' style='max-width:170px'>
                    </a>
                  </td>
                  <td style='padding-right:30px;text-align:right'>Kaydın Ana Sorumlusu : <b>{_request.MainResponsibleFullName}</b>
                  </td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
        <tr style='background-color:#fff;width:100%'>
          <td style='padding:1rem'>
            <table style='color:#000;padding-bottom:0px;width:100%'>
              <tbody>
                <tr>
                  <td style='font-size:21px;padding-bottom:1rem;text-align:left;width:100%'> Sayın; <b>{_request.SubResponsibleFullName}</b>
                  </td>
                </tr>
              </tbody>
            </table>
            <table style='color:#000;font-size:18px;margin-top:10px;padding-bottom:25px;text-align:left'>
              <tbody>
                <tr>
                  <td>

<b>{TicketId}</b> numaralı kayıt <b>{DateTime.Now.ToString("dd.MM.yyyy HH:mm")}</b> itibari ile <b>{WaitingDate.ToString("dd.MM.yyyy HH:mm")}</b> kadar kadar ürün parça bekliyor istemiyle beklemeye alınmıştır.  <br> <br> Teşekkürler,<br> <b>Profosyonel Hizmetler</b>
                  </td>
                </tr>
              </tbody>
            </table>
            <table style='border:0;border-collapse:collapse;margin-bottom:10px;margin-top:10px;text-align:center;width:100%'>
              <thead style='background-color:#009ca6;color:#fff;font-weight:normal;width:100%'>
                <tr style='font-weight:normal'>
                  <th style='border:1px solid #009ca6;font-weight:normal;padding:0.5rem;text-align:center;'> Kayıt Numarası </th>
                  <th style='border:1px solid #009ca6;font-weight:normal;padding:0.5rem;text-align:center;width:40%'> İlgili Firma </th>
                  <th style='border:1px solid #009ca6;font-weight:normal;padding:0.5rem;text-align:center;width:30%'> Bekleme Çıkış Zamanı </th>
                 
                </tr>
              </thead>
              <tbody style='color:#505050;width:100%'>
                <tr style='color:#505050'>
                  <td style='border:1px solid #505050;padding:0.4rem 0.6rem 0.4rem 0.6rem;text-align:center;color:red;'> <b>{TicketId}</b> </td>
                  <td style='border:1px solid #505050;margin-left:5px;padding:0.4rem 0.7rem 0.4rem 0.7rem;text-align:center'> {CompanyName} </td>
                  <td style='border:1px solid #505050;padding:0.4rem 0.6rem 0.4rem 0.6rem;text-align:center'> {WaitingDate} </td>
                </tr>
              </tbody>
            </table>
            <table style='display: none;'>
              <thead>
                <tr>
                  <th style='padding:1rem'> &nbsp; </th>
                </tr>
              </thead>
            </table>
            <table style='display:none;background-color:#f3f3f3;color:#000;font-size:15px;width:750px;' >
              <tbody>
                <tr style='background-color:#f3f3f3;width:400px'>
                  <td style='float:left;padding-top:4px;width:30%'>
                    <b>Sipariş Döviz Cinsi</b>
                    <span style='float:right'>:</span>
                  </td>
                  <td style='float:left;padding-top:4px;text-align:left;width:30%'> USD </td>
                  <td style='float:left;padding-top:4px;text-align:left;width:30%'> &nbsp; </td>
                </tr>
                <tr style='background-color:#f3f3f3;width:400px'>
                  <td style='float:left;padding-top:4px;width:30%'>
                    <b>Döviz Kuru</b>
                    <span style='float:right'>:</span>
                  </td>
                  <td style='float:left;padding-top:4px;text-align:left;width:30%'> 1 USD : 28,67 TL </td>
                  <td style='float:left;padding-top:4px;text-align:left;width:30%'> &nbsp; </td>
                </tr>
                <tr style='background-color:#f3f3f3;width:400px'>
                  <td style='float:left;padding-top:4px;width:30%'>
                    <b>Ödeme Tipi</b>
                    <span style='float:right'>:</span>
                  </td>
                  <td style='float:left;padding-top:4px;text-align:left;width:30%'> Havale </td>
                  <td style='float:left;padding-top:4px;text-align:left;width:30%'> &nbsp; </td>
                </tr>
                <tr style='background-color:#f3f3f3;width:400px'>
                  <td style='float:left;padding-top:4px;width:30%'>
                    <b>Teslimat Tipi</b>
                    <span style='float:right'>:</span>
                  </td>
                  <td style='float:left;padding-top:4px;text-align:left;width:30%'> Karşı Ödemeli Aras Kargo </td>
                  <td style='float:left;padding-top:4px;text-align:left;width:30%'> &nbsp; </td>
                </tr>
              </tbody>
              <tbody style='margin-bottom:20px;min-width:100%'></tbody>
            </table>
            <table style='display: none;border:0;border-collapse:collapse;margin-bottom:10px;margin-top:10px;width:100%'>
              <thead style='background-color:#009ca6;border:1px solid #009ca6;width:100%'>
                <tr>
                  <th style='color:#fff;font-weight:bolder;padding:1rem;text-transform:uppercase;width:50%'></th>
                  <th style='color:#fff;font-weight:bolder;padding:1rem;text-transform:uppercase;width:50%'></th>
                </tr>
              </thead>
              <tbody style='margin-bottom:10px;width:100%'>
                <tr style='margin-bottom:10px;text-align:center'>
                  <td style='border:1px solid #2c2c2c;padding:1rem 1rem 0.3rem 1rem'>
                    <table>
                      <tbody>
                        <tr style='width:100%'>
                          <td style='text-align:left;width:100%'></td>
                        </tr>
                        <tr style='text-align:left;width:100%'>
                          <td style='width:100%'>
                            <span style='float:left;font-weight:700;min-width:10%'>Tel <span style='float:right;margin-left:auto'>:</span>
                            </span>
                            <span style='margin-left:10px'>
                              <span class='wmi-callto'></span>
                            </span>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </td>
                  <td style='border:1px solid #2c2c2c;padding:1rem'>
                    <table>
                      <tbody>
                        <tr style='width:100%'>
                          <td style='text-align:left;width:100%'></td>
                        </tr>
                        <tr style='text-align:left;width:100%'>
                          <td style='width:100%'>
                            <span style='float:left;font-weight:700;min-width:10%'>Tel <span style='float:right;margin-left:auto'>:</span>
                            </span>
                            <span style='margin-left:10px'>
                              <span class='wmi-callto'></span>
                            </span>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </td>
                </tr>
              </tbody>
            </table>
            
            
            
            <table style='margin:10px 0px 10px 0px'>
              <tbody>
                <tr>
                  <td style='text-align:center'> Bu e-posta kişiye özel olup <span style='color:#009ca6'>
                      <b> Profosyonel Hizmetler</b>
                    </span> tarafın'dan gönderilmiştir. Bir yanlışlık olduğunu düşünüyorsanız lütfen bu e-postayı dikkate almayınız. </td>
                </tr>
              </tbody>
            </table>
            <table style='background-color:#e2e2e2;margin:0 auto 0 auto;max-width:750px;padding:5px 0 5px 0'>
              <thead>
                <tr>
                  <th style='border-right-color:black;border-right-width:1px;font-size:0.8rem;padding:0'>BDH Bilişim Destek Hizmetleri A.Ş</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td style='border-right-color:black;border-right-width:1px;font-size:0.7rem;font-weight:600;padding:0 20px 0 20px;text-align:center'> Bağlarbaşı Mahallesi, Cemal Bey Caddesi
                    No:110, 34844 Maltepe/İstanbul <span class='wmi-callto'>(0212) 500 17 00</span> (Pbx) - <a href='mailto:info@bdh.com.tr' target='_blank' rel='noopener noreferrer'>info@bdh.com.tr</a>
                  </td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
      </tbody>
    </table>
  </div>", _request.SubResponsibleFullName, TicketId, CompanyName, WaitingDate)
                };

                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                using (var emailClient = new SmtpClient())
                {
                    //The last parameter here is to use SSL (Which you should!)
                    emailClient.Connect("smtp-mail.outlook.com", 587, false);

                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate("robin@bdh.com.tr", "ea3zCPD998");

                    var tt = emailClient.Send(message);

                    emailClient.Disconnect(true);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool SGKOnayMailGonder(Stream ms, List<EmailReports> _reports)
        {
            try
            {
                string _htmlRaw = String.Empty;
                string _htmlRawTable = String.Empty;
                string _htmlRawTableRecord = String.Empty;
                var message = new MimeMessage();
                message.To.Add(MailboxAddress.Parse("baris.sakizli@bdh.com.tr"));
                message.Cc.Add(MailboxAddress.Parse("hakan.dansik@bdh.com.tr"));
                message.Cc.Add(MailboxAddress.Parse("kaya.aslan@bdh.com.tr"));
                message.Cc.Add(MailboxAddress.Parse("gamze.ozen@bdh.com.tr"));
                message.Cc.Add(MailboxAddress.Parse("mehmet.adiyaman@bdh.com.tr"));
                //message.Bcc.Add(MailboxAddress.Parse("baris.sakizli@bdh.com.tr"));
                //message.Cc.Add(MailboxAddress.Parse("kemal.yurdakul@bdh.com.tr"));
                //message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

                message.From.Add(MailboxAddress.Parse("robin@bdh.com.tr"));

                message.Subject = String.Format($"📣📣 {DateTime.Now.ToString("dd/MM/yyyy")} SGK Vizite Onay Bildirimi ✅");
                //We will say we are sending HTML. But there are options for plaintext etc. 

                if (_reports.Count > 0)
                {
                    _htmlRaw = @"<table style='border:0;border-collapse:collapse;margin-bottom:10px;margin-top:10px;width:100%'>
                    <thead style='color:#fff;font-weight:normal;width:100%'>
                      <tr style='font-weight:normal'>
                        <th  colspan=""5"" style='background-color:#009ca6;font-weight:normal;padding:0.5rem;'> <b>SGK Vizite Rapor Onay Bilgileri</b></th>
                      </tr>
                    </thead>
                    <thead style='color:#fff;font-weight:normal;width:100%'>
                      <tr style='font-weight:normal'>
                        <th  style='background-color:#009ca6;border:1px solid white;font-weight:normal;padding:0.5rem;'> <b>Kimlik Numarası</b></th>
                        <th  style='background-color:#009ca6;border:1px solid white;font-weight:normal;padding:0.5rem;'> <b>Ad Soyad</b></th>
                        <th  style='background-color:#009ca6;border:1px solid white;font-weight:normal;padding:0.5rem;'> <b>Medula Rapor ID</b></th>
                        <th  style='background-color:#009ca6;border:1px solid white;font-weight:normal;padding:0.5rem;'> <b>Rapor Takip Numarası</b></th>
                        <th  style='background-color:#009ca6;border:1px solid white;font-weight:normal;padding:0.5rem;'> <b>Onay Referans Numarası</b></th>
                      </tr>
                    </thead>
                    <tbody style='color:#505050;width:100%'>
              
                     {REPORTS}

                    </tbody>
                  </table>";

                    foreach (var report in _reports)
                    {
                        _htmlRawTableRecord = _htmlRawTableRecord + @$"<tr style='color:#505050;'>
                        <td style='width: 20%;border:1px solid black;padding:0.4rem 0.6rem 0.4rem 0.6rem;text-align:center;color:black;'> <b>{report.KimlikNumarasi}</b> </td>
                        <td style='width:20%;text-align:center;border:1px solid #505050;margin-left:5px;padding:0.4rem 0.7rem 0.4rem 0.7rem;'> {report.AdSoyad} </td>
                        <td style='width:20%;text-align:center;border:1px solid #505050;margin-left:5px;padding:0.4rem 0.7rem 0.4rem 0.7rem;'> {report.MedulaRaporId} </td>
                        <td style='width:20%;text-align:center;border:1px solid #505050;margin-left:5px;padding:0.4rem 0.7rem 0.4rem 0.7rem;'>{report.RaporTakipNumarasi} </td>
                        <td style='width:20%;text-align:center;border:1px solid #505050;margin-left:5px;padding:0.4rem 0.7rem 0.4rem 0.7rem;'> {report.OnayReferansId} </td>
                      </tr>";
                    }

                    _htmlRaw = _htmlRaw.Replace("{REPORTS}", _htmlRawTableRecord);
                }

                string _body = @"<div style=""background-color:#eee !important;font-family:'system-ui' , '-apple-system' , 'blinkmacsystemfont' , 'segoe ui' , 'roboto' , 'oxygen' , 'ubuntu' , 'cantarell' , 'open sans' , 'helvetica neue' , sans-serif;margin:0"""">
                  <table border='0' style='border-collapse:collapse;margin:20px auto 20px auto;max-width:750px;width:100%'>
                    <tbody>
                      <tr style='background-color:#009ca6;height:105px;width:100%'>
                        <td>
                          <table style='color:#fff;width:100%'>
                            <tbody>
                              <tr>
                                <td style='padding-left:30px;width:170px'>
                                  <a href='https://www.bdh.com/' data-link-id='96' target='_blank' rel='noopener noreferrer'>
                                    <img alt='bdh-logo' src='https://www.bdh.com.tr/wp-content/uploads/2019/08/BDHLogo2019_160w.png' style='max-width:170px'>
                                  </a>
                                </td>
                                <td style='padding-right:30px;text-align:right'>SGK Vizite Otomatik Onay Bilgilendirme</b>
                                </td>
                              </tr>
                            </tbody>
                          </table>
                        </td>
                      </tr>
                      <tr style='background-color:#fff;width:100%'>
                        <td style='padding:1rem'>
                          <table style='color:#000;padding-bottom:0px;width:100%'>
                            <tbody>
                              <tr>
                                <td style='font-size:21px;padding-bottom:1rem;text-align:left;width:100%'> Sayın; <b>Yetkili,</b>
                                </td>
                              </tr>
                            </tbody>
                          </table>
                          <table style='color:#000;font-size:18px;margin-top:10px;padding-bottom:25px;text-align:left'>
                            <tbody>
                              <br>
                                <br>

                <b>{Tarih}</b> tarihinde yapılan kontrollerde <b>SGK Vizite uygulamasında</b> toplam <b style=""color:red"">{RaporSayisi} adet</b> rapor onayı verilmiştir. </br></br> Raporu onaylanan çalışanların rapoy bilgisini alt taraftaki tabloda bulabilir veya ekte yer alan rapor onay bildirim formu ile personel özlük dosyasına ekleyebilirsiniz. </br></br> İyi Çalışmalar.
                </td>
                              </tr>
                            </tbody>
                          </table>
          
                          {RAPOR_LISTESI}

                          <table style='display: none;'>
                            <thead>
                              <tr>
                                <th style='padding:1rem'> &nbsp; </th>
                              </tr>
                            </thead>
                          </table>
                          <table style='display:none;background-color:#f3f3f3;color:#000;font-size:15px;width:750px;' >
                            <tbody>
                              <tr style='background-color:#f3f3f3;width:400px'>
                                <td style='float:left;padding-top:4px;width:30%'>
                                  <b>Sipariş Döviz Cinsi</b>
                                  <span style='float:right'>:</span>
                                </td>
                                <td style='float:left;padding-top:4px;text-align:left;width:30%'> USD </td>
                                <td style='float:left;padding-top:4px;text-align:left;width:30%'> &nbsp; </td>
                              </tr>
                              <tr style='background-color:#f3f3f3;width:400px'>
                                <td style='float:left;padding-top:4px;width:30%'>
                                  <b>Döviz Kuru</b>
                                  <span style='float:right'>:</span>
                                </td>
                                <td style='float:left;padding-top:4px;text-align:left;width:30%'> 1 USD : 28,67 TL </td>
                                <td style='float:left;padding-top:4px;text-align:left;width:30%'> &nbsp; </td>
                              </tr>
                              <tr style='background-color:#f3f3f3;width:400px'>
                                <td style='float:left;padding-top:4px;width:30%'>
                                  <b>Ödeme Tipi</b>
                                  <span style='float:right'>:</span>
                                </td>
                                <td style='float:left;padding-top:4px;text-align:left;width:30%'> Havale </td>
                                <td style='float:left;padding-top:4px;text-align:left;width:30%'> &nbsp; </td>
                              </tr>
                              <tr style='background-color:#f3f3f3;width:400px'>
                                <td style='float:left;padding-top:4px;width:30%'>
                                  <b>Teslimat Tipi</b>
                                  <span style='float:right'>:</span>
                                </td>
                                <td style='float:left;padding-top:4px;text-align:left;width:30%'> Karşı Ödemeli Aras Kargo </td>
                                <td style='float:left;padding-top:4px;text-align:left;width:30%'> &nbsp; </td>
                              </tr>
                            </tbody>
                            <tbody style='margin-bottom:20px;min-width:100%'></tbody>
                          </table>
                          <table style='display: none;border:0;border-collapse:collapse;margin-bottom:10px;margin-top:10px;width:100%'>
                            <thead style='background-color:#009ca6;border:1px solid #009ca6;width:100%'>
                              <tr>
                                <th style='color:#fff;font-weight:bolder;padding:1rem;text-transform:uppercase;width:50%'></th>
                                <th style='color:#fff;font-weight:bolder;padding:1rem;text-transform:uppercase;width:50%'></th>
                              </tr>
                            </thead>
                            <tbody style='margin-bottom:10px;width:100%'>
                              <tr style='margin-bottom:10px;text-align:center'>
                                <td style='border:1px solid #2c2c2c;padding:1rem 1rem 0.3rem 1rem'>
                                  <table>
                                    <tbody>
                                      <tr style='width:100%'>
                                        <td style='text-align:left;width:100%'></td>
                                      </tr>
                                      <tr style='text-align:left;width:100%'>
                                        <td style='width:100%'>
                                          <span style='float:left;font-weight:700;min-width:10%'>Tel <span style='float:right;margin-left:auto'>:</span>
                                          </span>
                                          <span style='margin-left:10px'>
                                            <span class='wmi-callto'></span>
                                          </span>
                                        </td>
                                      </tr>
                                    </tbody>
                                  </table>
                                </td>
                                <td style='border:1px solid #2c2c2c;padding:1rem'>
                                  <table>
                                    <tbody>
                                      <tr style='width:100%'>
                                        <td style='text-align:left;width:100%'></td>
                                      </tr>
                                      <tr style='text-align:left;width:100%'>
                                        <td style='width:100%'>
                                          <span style='float:left;font-weight:700;min-width:10%'>Tel <span style='float:right;margin-left:auto'>:</span>
                                          </span>
                                          <span style='margin-left:10px'>
                                            <span class='wmi-callto'></span>
                                          </span>
                                        </td>
                                      </tr>
                                    </tbody>
                                  </table>
                                </td>
                              </tr>
                            </tbody>
                          </table>
          
                          <table style='margin:10px 0px 10px 0px'>
                            <tbody>
                              <tr>
                                <td style='text-align:center'> Bu e-posta kişiye özel olup <span style='color:#009ca6'>
                                    <b> Profosyonel Hizmetler</b>
                                  </span> tarafından gönderilmiştir. Bir yanlışlık olduğunu düşünüyorsanız lütfen bu e-postayı dikkate almayınız. </td>
                              </tr>
                            </tbody>
                          </table>
                          <table style='background-color:#e2e2e2;margin:0 auto 0 auto;max-width:750px;padding:5px 0 5px 0'>
                            <thead>
                              <tr>
                                <th style='border-right-color:black;border-right-width:1px;font-size:0.8rem;padding:0'>BDH Bilişim Destek Hizmetleri A.Ş</th>
                              </tr>
                            </thead>
                            <tbody>
                              <tr>
                                <td style='border-right-color:black;border-right-width:1px;font-size:0.7rem;font-weight:600;padding:0 20px 0 20px;text-align:center'> Bağlarbaşı Mahallesi, Cemal Bey Caddesi
                                  No:110, 34844 Maltepe/İstanbul <span class='wmi-callto'>(0212) 500 17 00</span> (Pbx) - <a href='mailto:info@bdh.com.tr' target='_blank' rel='noopener noreferrer'>info@bdh.com.tr</a>
                                </td>
                              </tr>
                            </tbody>
                          </table>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>";
                _body = _body.Replace("{Tarih}", DateTime.Now.ToString("dd/MM/yyyy"));
                _body = _body.Replace("{RaporSayisi}", _reports.Count().ToString());
                _body = _body.Replace("{RAPOR_LISTESI}", _htmlRaw);

                var body = new TextPart(TextFormat.Html)
                {
                    Text = _body
                };


                var attachment = new MimePart("application/pdf")
                {
                    Content = new MimeContent(ms),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = $"SGK Vizite Onay Bildirim Formu - {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}.pdf"
                };

                var multipart = new Multipart("mixed");
                multipart.Add(body);
                multipart.Add(attachment);

                // now set the multipart/mixed as the message body
                message.Body = multipart;

                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                using (var emailClient = new SmtpClient())
                {
                    //The last parameter here is to use SSL (Which you should!)
                    emailClient.Connect("smtp-mail.outlook.com", 587, false);

                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate("robin@bdh.com.tr", "ea3zCPD998");

                    var tt = emailClient.Send(message);

                    emailClient.Disconnect(true);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendMail()
        {
            try
            {

                var message = new MimeMessage();
                message.To.Add(MailboxAddress.Parse("baris.sakizli@bdh.com.tr"));
                message.To.Add(MailboxAddress.Parse("hakan.dansik@bdh.com.tr"));
                //message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

                message.From.Add(MailboxAddress.Parse("robin@bdh.com.tr"));

                message.Subject = "Deneme Mail";


                //We will say we are sending HTML. But there are options for plaintext etc. 
                message.Body = new TextPart(TextFormat.Html)
                {

                    Text = @"<b>Deneme İçerik</b> <img src='https://www.bdh.com.tr/wp-content/uploads/2019/08/BDHLogo2019_160.png'>"
                };

                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                using (var emailClient = new SmtpClient())
                {
                    //The last parameter here is to use SSL (Which you should!)
                    emailClient.Connect("smtp-mail.outlook.com", 587, false);

                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate("robin@bdh.com.tr", "ea3zCPD998");

                    var tt = emailClient.Send(message);

                    emailClient.Disconnect(true);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }


        }


        public bool SendMailHtml(string html, string subject)
        {
            try
            {
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = html;
                var message = new MimeMessage();

                //message.To.Add(MailboxAddress.Parse("bdh_dl@bdh.com.tr"));

                message.To.Add(MailboxAddress.Parse("baris.sakizli@bdh.com.tr"));
                message.To.Add(MailboxAddress.Parse("aysenur.kocar@bdh.com.tr"));
                message.To.Add(MailboxAddress.Parse("hakan.dansik@bdh.com.tr"));

                //message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

                message.From.Add(MailboxAddress.Parse("ik@bdh.com.tr"));

                message.Subject = subject;
                //We will say we are sending HTML. But there are options for plaintext etc. 
                message.Body = bodyBuilder.ToMessageBody();

                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                using (var emailClient = new SmtpClient())
                {
                    //The last parameter here is to use SSL (Which you should!)
                    emailClient.Connect("smtp-mail.outlook.com", 587, false);

                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate("ik@bdh.com.tr", "BdhHR2023");


                    var tt = emailClient.Send(message);

                    emailClient.Disconnect(true);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }


        }




    }
}