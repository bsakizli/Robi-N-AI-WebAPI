using EmptorUtility;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Robi_N_WebAPI.BackgroundJob.Schedules;
using Robi_N_WebAPI.Controllers;
using Robi_N_WebAPI.Model;
using Robi_N_WebAPI.Utility;
using Robi_N_WebAPI.Utility.Tables;

namespace Robi_N_WebAPI.Shecles
{
	public class AutomaticTicketClosed
	{
		private readonly IConfiguration _configuration;
		private readonly AIServiceDbContext _db;
		private readonly ILogger<IdentityCheckController> _logger;

		public AutomaticTicketClosed(AIServiceDbContext db, IConfiguration configuration, ILogger<IdentityCheckController> logger)
		{
			_configuration = configuration;
			_logger = logger;
			_db = db;
		}

		//public async Task Start()
		//{
		//	RecurringJobs.AutomaticTicketClosedOperation();
		//}
		[AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Delete)]
		public async Task Process()
		{
			EmptorDbAction emptorDbAction = new EmptorDbAction(_configuration);

			var Jobs = await _db.RBN_EMPTOR_AUTOTICKETCLOSEDScheduler.Where(x => x.active == true && x.process == 1).ToListAsync();
			if (Jobs.Count > 0)
			{
				foreach (var item in Jobs)
				{
					if (item.process == 1)
					{
						if (!String.IsNullOrEmpty(item.ticketQuery))
						{
							var Tickets = await emptorDbAction.getTicketClosedList(item.ticketQuery);

							if (Tickets.Count > 0)
							{
								//Tekrarlama Problemi Olmaması için process değerini 2 yap.
								item.process = 2;
								if (await _db.SaveChangesAsync() == 1)
								{
									_logger.LogInformation($@"Automatic Log Closing Process: 2 - AutoTicketClosedName: {item.name}");
									foreach (var ticket in Tickets)
									{
										//Kayıt Kapat
										if (await emptorDbAction.InventoryStillSaveClose(ticket))
										{

											var _ticketItem = new RBN_EMPTOR_ClosedTicketHistory()
											{
												active = true,
												TicketId = ticket,
												addDate = DateTime.Now,
												autoTicketId = item.Id,
												closedDate = DateTime.Now
											};
											var lastRecord = _db.RBN_EMPTOR_ClosedTicketHistory.Add(_ticketItem);
											if (await _db.SaveChangesAsync() == 1)
											{
												_logger.LogInformation($@"Automatic Log Closing TicketId: {ticket} - AutoTicketId:{item.Id} - AutoTicketName: {item.name} - LastRecordTicketId: {lastRecord}");
											}

											//Kayıt Kapandı Log At
										}
										else
										{
											//Kayıt Kapatılamadı
										}
									}
									item.process = 1;
									item.lastStartDate = DateTime.UtcNow;
									if (await _db.SaveChangesAsync() == 1)
									{
										//İş Tekrar 1e çekildi.
										_logger.LogInformation($@"Automatic Log Closing Process: 1 - AutoTicketClosedName: {item.name}");
									}
									else
									{
										//iş Tekrar Çekilirken Sorun Oluştu
										_logger.LogInformation($@"Automatic Log Closing Process: 1 - Error - AutoTicketClosedName: {item.name}");
									}
								}
							}
							else
							{
								//Kapatılacak Kayıt Yok
								_logger.LogInformation($@"Automatic Log Closing Record Not Found - AutoTicketClosedName: {item.name}");

							}
						}
					}

				}
			}
		}

	}
}
