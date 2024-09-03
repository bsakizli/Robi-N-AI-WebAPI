using Microsoft.EntityFrameworkCore;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Utility.Tables;

namespace Robi_N_WebAPI.Utility
{
    public class AIServiceDbContext : DbContext
    {
        //public AIServiceDbContext() : base() { }
		public AIServiceDbContext(DbContextOptions<AIServiceDbContext> options) : base(options) { }
		public DbSet<ApiUsers> RBN_AI_SERVICE_USERS { get; set; }  
        public DbSet<RBN_AI_SERVICE_ROLES_MAP> RBN_AI_SERVICE_ROLES_MAP { get; set; }  
        public DbSet<RBN_AI_SERVICE_ROLE> RBN_AI_SERVICE_ROLE { get; set; }  
        public DbSet<RBN_IVR_HOLIDAY_DAYS> RBN_IVR_HOLIDAY_DAYS { get; set; }  
        public DbSet<RBN_IVR_LOGS> RBN_IVR_LOGS { get; set; }  
        public DbSet<RBN_VOICE_SOUNDS> RBN_VOICE_SOUNDS { get; set; }  
        public DbSet<RBN_SMS_TEMPLATES> RBN_SMS_TEMPLATES { get; set; }  
        public DbSet<RBN_WAITING_TIMES> RBN_WAITING_TIMES { get; set; }  
        public DbSet<RBN_EMPTOR_WaitingTicketHistory> RBN_EMPTOR_WaitingTicketHistory { get; set; }  
        public DbSet<RBN_CARGO_COMPANY_LIST> RBN_CARGO_COMPANY_LIST { get; set; }  
        public DbSet<RBN_EMPTOR_ClosedTicketHistory> RBN_EMPTOR_ClosedTicketHistory { get; set; }  
        public DbSet<RBN_EMPTOR_AUTOTICKETCLOSEDScheduler> RBN_EMPTOR_AUTOTICKETCLOSEDScheduler { get; set; }  
        public DbSet<RBN_EMPTOR_AUTOCLOSEDTICKET> RBN_EMPTOR_AUTOCLOSEDTICKET { get; set; }  
        public DbSet<RNB_IVR_WORKING_HOURS> RNB_IVR_WORKING_HOURS { get; set; }  
        public DbSet<RBN_SGK_VisitingIntroductionInformation> RBN_SGK_VisitingIntroductionInformation { get; set; }  

        //public DbSet<RBN_SGK_Settings> RBN_SGK_Settings { get; set; }  
        public DbSet<RBN_SGK_HealthReports> RBN_SGK_HealthReports { get; set; }  
        public DbSet<RBN_WhatsAppMessageTemplate> RBN_WhatsAppMessageTemplate { get; set; }  
        public DbSet<RBN_UnansweredCalls> RBN_UnansweredCalls { get; set; }  
        public DbSet<RBN_ANKET_CEVAP> RBN_ANKET_CEVAP { get; set; }  
        public DbSet<RBN_RequestACallBack> RBN_RequestACallBack { get; set; }  
    }
}
