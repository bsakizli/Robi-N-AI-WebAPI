using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_SGK_HealthReports
    {
        [Key]
        public int Id { get; set; }
        public string? ISYERIADI { get; set; }
        public int ISYERIKODU { get; set; }
        public long TCKIMLIKNO { get; set; }
        public string? AD { get; set; }
        public string? SOYAD { get; set; }
        public long MEDULARAPORID { get; set; }
        public string? RAPORTAKIPNO { get; set; }
        public int RAPORSIRANO { get; set; }
        public DateTime POLIKLINIKTAR { get; set; }
        public DateTime YATRAPBASTAR { get; set; }
        public DateTime YATRAPBITTAR { get; set; }
        public DateTime ABASTAR { get; set; }
        public DateTime ABITTAR { get; set; }
        public DateTime ISBASKONTTAR { get; set; }
        public DateTime DOGUMONCBASTAR { get; set; }
        public DateTime ISKAZASITARIHI { get; set; }
        public int RAPORDURUMU { get; set; }
        public int VAKA { get; set; }
        public string? VAKAADI { get; set; }
        public int ARSIV { get; set; }
        public DateTime RAPORBITTAR { get; set; }
        public DateTime ISVERENEBILDIRILDIGITARIH { get; set; }
        public DateTime BASHEKIMONAYTARIHI { get; set; }
        public long TESISKODU { get; set; }
        public string? TESISADI { get; set; }
        public long? BildirimId { get; set; }
        public DateTime? OnaylamaTarihi { get; set; }
        public int? process { get; set; }
        public DateTime? addDate { get; set; }
        public Boolean? active { get; set; }
        public Boolean? mailSend { get; set; }
        public Boolean? Personel { get; set; }
        public int FirmCode { get; set; }



    }
}
