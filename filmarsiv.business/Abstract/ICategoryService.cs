using System.Collections.Generic;
using filmarsiv.entity;

namespace filmarsiv.business.Abstract
{
    public interface ICategoryService : IValidator<Category>
    {
        Category GetById(int id);
        Category GetByIdWithMovies(int categoryId);

        List<Category> GetAll();

        void Create(Category entity);

        void Update(Category entity);
        void Delete(Category entity);
        void DeleteFromCategory(int movieId,int categoryId);
    }
}