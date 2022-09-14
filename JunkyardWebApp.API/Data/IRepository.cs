namespace JunkyardWebApp.API.Data;

public interface IRepository<T>
{
    Task<ICollection<T>> Get();
    Task<T>? GetById(int id);
    Task Add(T t);
    Task Update(T t);
    Task Delete(T t);
}