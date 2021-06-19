using System.Linq;
using filmarsiv.entity;
using Microsoft.EntityFrameworkCore;

namespace filmarsiv.data.Concrete.EfCore
{
    public static class SeedDatabase
    {
         public static void Seed(){
        //     var context = new MovieContext();

        //     if(context.Database.GetPendingMigrations().Count() == 0){
        //         if(context.Categories.Count()==0){
        //             context.Categories.AddRange(Categories);
        //         }
        //         if(context.Movies.Count() == 0){
        //             context.Movies.AddRange(Movies);
        //             context.AddRange(MovieCategories);
        //         }
        //     context.SaveChanges();
        // }
    }
        private static Category[] Categories={
            new Category(){Name="Aksiyon",Url="aksiyon"},
            new Category(){Name="Bilim Kurgu",Url="bilim-kurgu"},
            new Category(){Name="Belgesel",Url="belgesel"}
        };
        private static Movie[] Movies = {
            new Movie(){Name="The Imitation Game",Years="2014",ImdbRating="8/10",ImageUrl="1.jpg"}
        };
      
      private static MovieCategory[] MovieCategories={
          new MovieCategory(){Movie = Movies[0],Category = Categories[0]}
      };
    }
}