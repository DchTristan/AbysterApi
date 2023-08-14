using AbysterApi.Data;
using AbysterApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AbysterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepenseController : ControllerBase
    {
        private readonly AbysterDbContext _context;

        public DepenseController(AbysterDbContext context)
        {
            _context = context;
        }

        [HttpPost("declare"), Authorize]
        public async Task<IActionResult> DeclareDepense([FromBody] DepenseDto depenseDto, [FromQuery] int categorieId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var utilisateur = await _context.Personnes.FindAsync(userId);
            var categorie = await _context.Categories.FindAsync(categorieId);

            if((utilisateur == null) || (categorie == null))
            {
                return NotFound("Utilisateur ou Categorie non trouvés");
            }

            var depense = new Operation
            {
                Montant = depenseDto.Montant,
                PersonneId = utilisateur.Id,
                CategorieId = categorieId
            };

            _context.Operations.Add(depense);
            await _context.SaveChangesAsync();

            return Ok("Depense enregistrée avec succès");
        }

        [HttpGet("operations-recentes"), Authorize]
        public async Task<ActionResult<List<Operation>>> GetRecentOperations()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            DateTime troisMoisAvant = DateTime.Now.AddMonths(-3);

            var operations = await _context.Operations
                .Where(o => o.PersonneId == userId && o.DateOperation >= troisMoisAvant)
                .ToListAsync();

            return Ok(operations);
        }
    }
}
