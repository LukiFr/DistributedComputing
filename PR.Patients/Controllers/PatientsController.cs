using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedComputing.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistributedComputing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        public readonly PrDataContext _context;

        public PatientsController(PrDataContext context)
        {
            _context = context;
        }

        public IActionResult GetAll()
        {
            return Ok(_context.Patients.ToList());
        }


    }
}
