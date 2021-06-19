namespace filmarsiv.business.Abstract
{
    public interface IValidator<T>
    {
         string ErroMessage { get; set; }
         bool Validation(T entity);
    }
}