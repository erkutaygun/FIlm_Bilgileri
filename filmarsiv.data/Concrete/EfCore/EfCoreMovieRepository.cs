using System.Collections.Generic;
using System.Linq;
using filmarsiv.data.Abstract;
using filmarsiv.entity;
using Microsoft.EntityFrameworkCore;

namespace filmarsiv.data.Concrete.EfCore
{
    public class EfCoreMovieRepository : EfCoreGenericRepository<Movie>, IMovieRepository
    {
        public EfCoreMovieRepository(MovieContext context):base(context)
        {
            
        }

        private MovieContext MovieContext{
            get{return context as MovieContext;}
        }
        public Movie GetByIdWithCategories(int id)
        {
            return MovieContext.Movies
                            .Where(i=>i.MovieId == id)
                            .Include(i=>i.MovieCategories)
                            .ThenInclude(i=>i.Category)
                            .FirstOrDefault();
        }

        public int GetCountByCategory(string category)
        {
            
            var movies = MovieContext.Movies.Where(i=>i.IsApproved).AsQueryable();
            if(!string.IsNullOrEmpty(category)){
                movies = movies
                            .Include(i=>i.MovieCategories)
                            .ThenInclude(i=>i.Category)
                            .Where(i=>i.MovieCategories.Any(a=>a.Category.Url == category));
                                
            }
            return movies.Count();
        }

        public List<Movie> GetHomePageMovies()
        {
         
            return MovieContext.Movies.Where(i=>i.IsApproved && i.IsHome).ToList();
            
        }

        public List<Movie> GetMoviesByCategory(string name,int page,int pageSize)
        {
            var movies = MovieContext.Movies.Where(i=>i.IsApproved).AsQueryable();
            if(!string.IsNullOrEmpty(name)){
                movies = movies
                                .Include(i=>i.MovieCategories)
                                .ThenInclude(i=>i.Category)
                                .Where(i=>i.MovieCategories.Any(a=>a.Category.Url == name));
            }
            return movies.Skip((page-1)*pageSize).Take(pageSize).ToList();
            
        }

        public Movie GetMovieDetails(string url)
        {
            return MovieContext.Movies
                            .Where(i=>i.Url==url)
                            .Include(i=>i.MovieCategories)
                            .ThenInclude(i=>i.Category)
                            .FirstOrDefault();
        
        }

        public List<Movie> GetSearchResult(string searchString)
        {
         
            var movies = MovieContext
                        .Movies
                        .Where(i=>i.IsApproved && (i.Name.ToLower().Contains(searchString.ToLower())))
                        .AsQueryable();
            return movies.ToList();
           
        }

        public void Update(Movie entity, int[] categoryIds)
        {
            
            var movie = MovieContext.Movies
                        .Include(i=>i.MovieCategories)
                        .FirstOrDefault(i=>i.MovieId==entity.MovieId);

            if(movie!=null){
                movie.Name = entity.Name;
                movie.Url = entity.Url;
                movie.Years = entity.Years;
                movie.ImdbRating = entity.ImdbRating;
                movie.RunTime = entity.RunTime;
                movie.Language = entity.Language;
                movie.ImageUrl = entity.ImageUrl;
                movie.Details = entity.Details;
                movie.IsHome = entity.IsHome;
                movie.IsApproved = entity.IsApproved;
                
                movie.MovieCategories = categoryIds.Select(catid=>new MovieCategory()
                {
                    MovieId=entity.MovieId,
                    CategoryId = catid
                }).ToList();

                MovieContext.SaveChanges();
            }
            
        }
    }
}
