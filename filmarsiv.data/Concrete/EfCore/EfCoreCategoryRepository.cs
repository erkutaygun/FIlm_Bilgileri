using System.Collections.Generic;
using System.Linq;
using filmarsiv.data.Abstract;
using filmarsiv.entity;
using Microsoft.EntityFrameworkCore;

namespace filmarsiv.data.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category>, ICategoryRepository
    {
        public EfCoreCategoryRepository(MovieContext context):base(context)
        {
            
        }
        private MovieContext MovieContext{
            get{return context as MovieContext;}
        }
        public void DeleteFromCategory(int movieId, int categoryId)
        {
            
            var cmd = "delete * from moviecategory where MovieId=@p0 and CategoryId=@p1";
            MovieContext.Database.ExecuteSqlRaw(cmd,movieId,categoryId);
            
        }

        public Category GetByIdWithMovies(int categoryId)
        {
             return MovieContext.Categories
                            .Where(i=>i.CategoryId==categoryId)
                            .Include(i=>i.MovieCategories)
                            .ThenInclude(i=>i.Movie)
                            .FirstOrDefault();
         
        }
    }
}