using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Threading.Tasks;

namespace UrlShortener.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UrlModel> Urls { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            //bool exists = (Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();
            //if(exists)
            //{
            //    Database.EnsureDeleted();
            //}
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserModel>().HasData(new UserModel { Login = "pupkinV", Password = "CustomPass1", Id = 1 },
                new UserModel { Login = "PetrovK", Password = "CustomPass2", Id = 2 },
                new UserModel { Login = "PavelM", Password = "CustomPass3", Id = 3 },
                new UserModel { Login = "KonradA", Password = "CustomPass4", Id = 4 });

            builder.Entity<UrlModel>().HasData(new UrlModel { UrlOriginal = new Uri("https://www.songsterr.com/a/wsa/pt-adamczyk-olga-jankowska-cyberpunk-2077-never-fade-away-samurai-cover-guitar-solo-tab-s476473"), UrlOriginalHost = "", UrlOriginalPath = "", UrlModded = "https://localhost:44367/test1", UrlModdedPath = "test1", Id = 1, UserId = 1 });
        }
    }
}
