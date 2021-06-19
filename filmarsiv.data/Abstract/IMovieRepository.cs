using System.Collections.Generic;
using filmarsiv.entity;

namespace filmarsiv.data.Abstract
{
    public interface IMovieRepository:IRepository<Movie>
    {
        Movie GetMovieDetails(string Url);
        Movie GetByIdWithCategories(int id);
        List<Movie> GetMoviesByCategory (string name,int page,int pageSize);
        List<Movie> GetSearchResult(string searchString);
        int GetCountByCategory(string category);

        List<Movie> GetHomePageMovies();
        void Update(Movie entity, int[] categoryIds);
    }   
}