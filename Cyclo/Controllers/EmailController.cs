using ActionMailer.Net.Mvc;
using Cyclo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cyclo.Controllers
{
    public class EmailController : MailerBase
    {
        public EmailResult NewJob(EmailModel model)
        {
            To.Add(model.To);

            From = model.From;

            Subject = "Новая задача \""+model.job.name+"\"";

            return Email("NewJob", model);
        }
    }
}