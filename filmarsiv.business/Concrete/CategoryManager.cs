using System.Collections.Generic;
using filmarsiv.business.Abstract;
using filmarsiv.data.Abstract;
using filmarsiv.entity;

namespace filmarsiv.business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        public CategoryManager( ICategoryRepository categoryRepository)
        {
         _categoryRepository=categoryRepository;   
        }

        public string ErroMessage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void Create(Category entity)
        {   
            _categoryRepository.Create(entity);
        }

        public void Delete(Category entity)
        {
            _categoryRepository.Delete(entity);
        }

        public void DeleteFromCategory(int movieId, int categoryId)
        {
            _categoryRepository.DeleteFromCategory(movieId,categoryId);
        }

        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public Category GetById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public Category GetByIdWithMovies(int categoryId)
        {
            return _categoryRepository.GetByIdWithMovies(categoryId);
        }

        public void Update(Category entity)
        {
            _categoryRepository.Update(entity);
        }

        public bool Validation(Category entity)
        {
            throw new System.NotImplementedException();
        }
    }
}