using Robi_N_WebAPI.Utility.Tables;

namespace Robi_N_WebAPI.Model.Response
{
    public class responseVoiceIVRApplication : GlobalResponse
    {
        public class GoogleCalender
        {
            public class Creator
            {
                public string? email { get; set; }
                public string? displayName { get; set; }
                public bool self { get; set; }
            }

            public class End
            {
                public string? date { get; set; }
            }

            public class Item
            {
                public string? kind { get; set; }
                public string? etag { get; set; }
                public string? id { get; set; }
                public string? status { get; set; }
                public string? htmlLink { get; set; }
                public DateTime created { get; set; }
                public DateTime updated { get; set; }
                public string? summary { get; set; }
                public string? description { get; set; }
                public Creator? creator { get; set; }
                public Organizer? organizer { get; set; }
                public Start? start { get; set; }
                public End? end { get; set; }
                public string? transparency { get; set; }
                public string? visibility { get; set; }
                public string? iCalUID { get; set; }
                public int sequence { get; set; }
                public string? eventType { get; set; }
            }

            public class Organizer
            {
                public string? email { get; set; }
                public string? displayName { get; set; }
                public bool? self { get; set; }
            }

            public class Root
            {
                public string? kind { get; set; }
                public string? etag { get; set; }
                public string? summary { get; set; }
                public string? description { get; set; }
                public DateTime updated { get; set; }
                public string? timeZone { get; set; }
                public string? accessRole { get; set; }
                public List<object>? defaultReminders { get; set; }
                public string? nextSyncToken { get; set; }
                public List<Item>? items { get; set; }
            }

            public class Start
            {
                public string? date { get; set; }
            }
        }

        public class getholidayDayList : GlobalResponse
        {
            public int totalcount { get; set; }
            public int activepage { get; set; }
            public int totalpagecount { get; set; }
            public int listcount { get; set; }
            public int pagesize { get; set; }
			public List<RBN_IVR_HOLIDAY_DAYS>? data { get; set; }
		}

        public class getholidayDay : GlobalResponse
        {
            public RBN_IVR_HOLIDAY_DAYS? data { get; set; }
        }
    }
}
