using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedComputing.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;


namespace DistributedComputing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PatientsController : ControllerBase
    {
        public readonly PrDataContext _context;
        public readonly ServiceBusSender _sender;

        public PatientsController(PrDataContext context, ServiceBusSender sender)
        {
            _context = context;
            _sender = sender;
        }

        //[HttpPut]
        //[AllowAnonymous]
        //public IActionResult InvalidAction()
        //{
            //throw new InvalidOperationException("Symulowany problem z aplikacja");
        //}

        [HttpGet]
        [AllowAnonymous]

        public IActionResult GetAll()
        {
            return Ok(_context.Patients.ToList());
        }

        [HttpPost]      
        public async Task<IActionResult>Add (Patient p)
        {
            _context.Patients.Add(p);
            _context.SaveChanges();

            await _sender.SendMessage(new MessagePayload()
            {
                EventName = "NewUserRegistered",
                EmailAddress = "lukaszfraniewskiwwsi@gmail.com",
                Title = "COVID19",
                Message = "Idziesz na kwarantanne!"

            });

            return Created("/api/patients/", p);

        }

    }
}
