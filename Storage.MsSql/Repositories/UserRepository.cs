using Business.Model.Entities.Users;
using Business.Application.Repositories;

namespace Storage.MsSql.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ArtBookingDbContextMsSql _dbContext;

    public UserRepository(ArtBookingDbContextMsSql dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }

    public User? GetUserByUserName(string userName) =>
        _dbContext.Users.FirstOrDefault(u => u.Username == userName);

    public User? GetUserByEmail(string email) =>
        _dbContext.Users.FirstOrDefault(u => u.Email == email);

    public bool HasUsers() => _dbContext.Users.Any();
}