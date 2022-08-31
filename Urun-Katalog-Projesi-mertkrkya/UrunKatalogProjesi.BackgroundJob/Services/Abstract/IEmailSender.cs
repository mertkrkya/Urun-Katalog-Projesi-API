using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Entities;
using UrunKatalogProjesi.Data.Models;

namespace UrunKatalogProjesi.BackgroundJob.Services.Abstract
{
    public interface IEmailSender
    {
        Task SendEmail(EmailTypes emailType, string username, string email, object mailData = null);
    }
}
