using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbysterApi.Models
{
    public class Operation
    {
        public int Id { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal Montant { get; set; }

        public int PersonneId { get; set; }
        public Personne? personne { get; set; }
        public int CategorieId { get; set; }
        public Categorie? categorie { get; set; }
    }
}
