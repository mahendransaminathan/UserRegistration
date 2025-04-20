
public interface IUserService
{
    Task<UserReg> GetUserByEmail(string email);
    Task AddUser(UserReg user);
}