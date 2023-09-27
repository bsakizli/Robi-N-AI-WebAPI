namespace Robi_N_WebAPI.Model.Request
{
    public class requestVoiceIVRApplication
    {
        public class newHolidayDay {
            public string? csq { get; set; }     
            public string? displayName { get; set; }     
            public string? holidayName { get; set; }     
            public string? description { get; set; }     
            public DateTime startDate { get; set; }
            public int years { get; set; }
            public DateTime endDate { get; set; }
            public bool active { get; set; }
        }
    }
}
