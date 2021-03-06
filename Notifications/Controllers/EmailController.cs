﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notifications.Model;
using Notifications.Services;

namespace Notifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailController : ControllerBase
    {

        [HttpPost]
        public void SendMessage(EmailMessageRequest request)
        {
            EmailSender sender = new EmailSender();
            sender.SendNewUserEmail(request.EmailAdress);

        }

        [HttpPut]
        [AllowAnonymous]
        public IActionResult InvalidAction()
        {
            throw new InvalidOperationException("Symulowany problem z aplikacja (EmailController)");
        }
    }
}
