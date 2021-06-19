using System;
using filmarsiv.entity;
using Microsoft.EntityFrameworkCore;

namespace filmarsiv.data.Concrete.EfCore
{
    public class MovieContext:DbContext
    {
        public MovieContext(DbContextOptions options):base(options){
            
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Category> Categories { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
        //     optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=FilmDb");
        //     }
                       

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<MovieCategory>()
            .HasKey(c=>new {c.CategoryId,c.MovieId});
        }
    }
}