using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Db;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRHapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PRHapiController : ControllerBase
    {
        private readonly MyDbContext _context;

        public PRHapiController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("postal_codes/{postalCode}/companies")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompaniesByPostalCode(string postalCode)
        {
            var companies = await _context.Companies.Where(c => c.PostalCode == postalCode).ToListAsync();

            if (companies == null || companies.Count == 0)
            {
                return NotFound();
            }

            return companies;
        }
    }
}