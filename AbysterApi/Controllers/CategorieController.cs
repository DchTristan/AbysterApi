using AbysterApi.Data;
using AbysterApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AbysterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        private readonly AbysterDbContext _context;

        public CategorieController(AbysterDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")] // Ajoutez l'attribut Authorize avec les rôles appropriés
        public async Task<ActionResult> CreateCategorie(CategorieDto categorieDto)
        {
            var categorie = new Categorie
            {
                Libelle = categorieDto.Libelle,
                Type = categorieDto.Type,
                CreatedDate = DateTime.Now
            };

            _context.Categories.Add(categorie);
            await _context.SaveChangesAsync();

            return Ok("Catégorie créée avec succès");
        }
    }
}