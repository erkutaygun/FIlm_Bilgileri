using System.Collections.Generic;
using filmarsiv.business.Abstract;
using filmarsiv.data.Abstract;
using filmarsiv.data.Concrete.EfCore;
using filmarsiv.entity;

namespace filmarsiv.business.Concrete
{
    public class MovieManager : IMovieService
    {
        private IMovieRepository _movieRepository;
        public MovieManager(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public bool Create(Movie entity)
        {
            _movieRepository.Create(entity);
            return true;
        }

        public void Delete(Movie entity)
        {
            _movieRepository.Delete(entity);
        }

        public List<Movie> GetAll()
        {
            return _movieRepository.GetAll();
        }

        public Movie GetById(int id)
        {
            return _movieRepository.GetById(id);
        }

        public Movie GetByIdWithCategories(int id)
        {
            return _movieRepository.GetByIdWithCategories(id);
        }

        public int GetCountByCategory(string category)
        {
            return _movieRepository.GetCountByCategory(category);
        }

        public List<Movie> GetHomePageMovies()
        {
            return _movieRepository.GetHomePageMovies();
        }

        public Movie GetMovieDetails(string url)
        {
            return _movieRepository.GetMovieDetails(url);
        }

        public List<Movie> GetMoviesByCategory(string name,int page,int pageSize)
        {
            return _movieRepository.GetMoviesByCategory(name,page,pageSize);
        }

        public List<Movie> GetSearchResult(string searchString)
        {
            return _movieRepository.GetSearchResult(searchString);
        }

        public void Update(Movie entity)
        {
            _movieRepository.Update(entity);
        }

        public bool Update(Movie entity, int[] categoryIds)
        {
            if(Validation(entity)){
                if(categoryIds.Length==0){
                    ErroMessage += "Film icin en az bir kategori secmelisiniz.";
                    return false;
                }
                _movieRepository.Update(entity,categoryIds);
                return true;
            }
            return false;
        }
        public string ErroMessage { get; set; }
        public bool Validation(Movie entity)
        {
            var isValid = true;
            return isValid;
        }

       
        
    }
}