// using UserRegistration.Models.Entities;

public interface IUserRepository
{
    Task<UserReg> GetUserByEmail(string email);
    
    Task AddUser(UserReg user);

    
}