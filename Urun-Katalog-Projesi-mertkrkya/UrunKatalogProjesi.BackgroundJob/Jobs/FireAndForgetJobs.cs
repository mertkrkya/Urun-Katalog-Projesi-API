using Hangfire;
using UrunKatalogProjesi.BackgroundJob.Services.Abstract;
using UrunKatalogProjesi.Data.Entities;
using UrunKatalogProjesi.Data.Models;

namespace UrunKatalogProjesi.BackgroundJob.Jobs
{
    public class FireAndForgetJobs
    {

        [AutomaticRetry(Attempts = 5,DelaysInSeconds = new int[] {60})]
        public static void EmailSendJob(EmailTypes emailType, string username, string email , object mailData = null)
        {
            Hangfire.BackgroundJob.Enqueue<IEmailSender>(x => x.SendEmail(emailType, username, email, mailData));
        }
    }
}
