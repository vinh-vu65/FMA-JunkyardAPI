namespace JunkyardWebApp.API.Services;

public interface IService<T>
{
    Task<ICollection<T>> GetAll();
    Task<T?> GetById(int id);
    Task Update(T t);
    Task Add(T t);
    Task Delete(T t);
}