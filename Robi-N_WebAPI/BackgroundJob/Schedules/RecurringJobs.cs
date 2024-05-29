using EmptorUtility;
using Google;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Robi_N_WebAPI.Controllers;
using Robi_N_WebAPI.Schedule;
using Robi_N_WebAPI.Services;
using Robi_N_WebAPI.Shecles;
using Robi_N_WebAPI.Utility;

namespace Robi_N_WebAPI.BackgroundJob.Schedules
{
	public class RecurringJobs
	{

		private readonly EmptorDbAction _emptorDbAction;
		private readonly AIServiceDbContext _db;
		private readonly ILogger<IdentityCheckController> _logger;

		public RecurringJobs(AIServiceDbContext db, ILogger<IdentityCheckController> logger, EmptorDbAction emptorDbAction)
		{
			_emptorDbAction = emptorDbAction;
			_logger = logger;
			_db = db;
		}


		[Obsolete]
		[AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Delete)]
		public async static void AutomaticTicketClosedOperation()
		{

			//IConfiguration configuration = new ConfigurationBuilder()
			//.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			//.Build();
			//var _database = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

			RecurringJob.RemoveIfExists(nameof(AutomaticTicketClosed));
			/*  RemoveIfExists yöntemini çağırarak var olan yinelenen bir işi kaldırabilirsiniz. 
				Böyle tekrar eden bir iş olmadığında bir istisna oluşturmaz */


			RecurringJob.AddOrUpdate<AutomaticTicketClosed>(nameof(AutomaticTicketClosed),
					job => job.Process(), "*/30 * * * *", TimeZoneInfo.Local);


			RecurringJob.AddOrUpdate<ViziteService>(nameof(ViziteService),
					job => job.RaporSorgulaOnay(), "0 10,17 * * *", TimeZoneInfo.Local);

            RecurringJob.RemoveIfExists(nameof(MissedCallsMessages));
			RecurringJob.AddOrUpdate<MissedCallsMessages>(nameof(MissedCallsMessages),
					job => job.MissedCallMessageService(), "* 8-19 * * *", TimeZoneInfo.Local);

			//var optionsBuilder = new DbContextOptionsBuilder<AIServiceDbContext>();
			//optionsBuilder.UseSqlServer(_database);
			//var _db = new AIServiceDbContext(optionsBuilder.Options);

			//var Jobs = await _db.RBN_EMPTOR_AUTOTICKETCLOSEDScheduler.Where(x => x.active == true).ToListAsync();
			//if (Jobs.Count > 0)
			//{
			//	foreach (var item in Jobs)
			//	{

			//	}
			//}

		}
	}
}
