using filmarsiv.entity;

namespace filmarsiv.data.Abstract
{
    public interface ICategoryRepository: IRepository<Category>
    {  
        Category GetByIdWithMovies(int categoryId);
        void DeleteFromCategory(int movieId,int categoryId);
    }
}