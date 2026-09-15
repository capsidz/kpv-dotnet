namespace UserManagementApi.Interfaces;

public interface IRepository<T> where T : class
{
    public T? GetById(int id);
    
    public IEnumerable<T> GetAll();
    
    public void Add(T entity);
    
    public void Update(T entity);
    
    public void Delete(T entity);
}