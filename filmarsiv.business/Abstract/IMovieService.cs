using System.Collections.Generic;
using filmarsiv.entity;

namespace filmarsiv.business.Abstract
{
    public interface IMovieService:IValidator<Movie>
    {
         
        Movie GetById(int id);
        Movie GetByIdWithCategories(int id);

        Movie GetMovieDetails(string url);
        
        List<Movie> GetMoviesByCategory(string name,int page,int pageSize);
        List<Movie> GetAll();

        bool Create(Movie entity);

        void Update(Movie entity);
        void Delete(Movie entity);
        int GetCountByCategory(string category);
        List<Movie> GetHomePageMovies();

        List<Movie> GetSearchResult(string searchString);
        bool Update(Movie entity, int[] categoryIds);
    }
}