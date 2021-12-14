using Microsoft.EntityFrameworkCore;
using Projet2.Models.Compte;

namespace Projet2.Models
{
    public class BddContext : DbContext
    {



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user id=root;password=Isika_22;database=amaporte");
        }

      
    }
}
