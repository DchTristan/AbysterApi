using AbysterApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AbysterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImpressionController : ControllerBase
    {
        private readonly AbysterDbContext _context;

        public ImpressionController(AbysterDbContext context)
        {
            _context = context;
        }

        /*[HttpGet("imprimer-operations"), Authorize]
        public async Task<IActionResult> ImprimerOperations()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var operations = await _context.Operations
                .Where(o => o.PersonneId == userId)
                .ToListAsync();
        }
        */
    }
}
