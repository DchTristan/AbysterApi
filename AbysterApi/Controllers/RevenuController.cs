using AbysterApi.Data;
using AbysterApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AbysterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenuController : ControllerBase
    {
        private readonly AbysterDbContext _context;


        [HttpPost("declare"), Authorize]
        public async Task<ActionResult> DeclareRevenu([FromBody] RevenuDto revenuDto, [FromQuery] int categorieId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var utilisateur = await _context.Personnes.FindAsync(userId);
            var categorie = await _context.Categories.FindAsync(categorieId);

            if (utilisateur == null || categorie == null)
            {
                return BadRequest("Utilisateur ou Categorie non trouvés");
            }

            var revenu = new Operation
            {
                Montant = revenuDto.Montant,
                PersonneId = utilisateur.Id,
                CategorieId = categorie.Id
            };

            _context.Operations.Add(revenu);
            await _context.SaveChangesAsync();

            return Ok("Revenu declaré avec succès");
        }


    }
}
