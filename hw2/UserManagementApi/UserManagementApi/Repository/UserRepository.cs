using UserManagementApi.Data;
using UserManagementApi.Entities;
using UserManagementApi.Interfaces;

namespace UserManagementApi.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public User? GetById(int id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }
    
    public User? GetByLogin(string login)
    {
        return _context.Users.FirstOrDefault(u => u.Login == login);
    }

    public IEnumerable<User> GetAll()
    {
        throw new NotImplementedException();
    }

    public void Add(User entity)
    {
        _context.Users.Add(entity);
        _context.SaveChanges();
    }

    public void Update(User entity)
    {
        _context.Users.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(User entity)
    {
        _context.Users.Remove(entity);
        _context.SaveChanges();
    }
}