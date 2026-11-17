using UserManagementApi.Data;
using UserManagementApi.Entities;
using UserManagementApi.Interfaces;
using UserManagementApi.Enums;

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
        return _context.Users.ToList();
    }

    public IEnumerable<User> GetFiltered(DateFilterType dateFilterType, DateTime from, DateTime to)
    {
        var query = _context.Users.AsQueryable();

        if (dateFilterType == DateFilterType.Created)
        {
            return query.Where(u => u.CreatedAt >= from && u.CreatedAt <= to).ToList();
        }

        else if (dateFilterType == DateFilterType.Updated)
        {
            return query.Where(u => u.UpdatedAt >= from && u.UpdatedAt <= to).ToList();
        }

        throw new ArgumentException("Invalid date filter type", nameof(dateFilterType));
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