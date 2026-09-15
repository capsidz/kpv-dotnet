using UserManagementApi.Entities;

namespace UserManagementApi.Interfaces;

public interface IUserRepository : IRepository<User>
{
    public User? GetByLogin(string login);
}