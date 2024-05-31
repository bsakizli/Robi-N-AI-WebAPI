namespace Robi_N_WebAPI.Model.Response
{
    public class DeviceInquiryResponse : GlobalResponse
    {
        public int Ref { get; set; }
        public string? Description { get; set; }
        public string? CargoCompany { get; set; }
        public ulong CargoTrackingNumber { get; set; }
        public ulong PhoneNo { get; set; }
        public decimal OfferPrice { get; set; }
        public string? ServiceName { get; set; }
        public bool? IsSendSms { get; set; }
        public long? BulkId { get; set; }
       
    }
}
