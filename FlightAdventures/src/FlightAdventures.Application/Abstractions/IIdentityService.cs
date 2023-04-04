namespace FlightAdventures.Application.Abstractions;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(int userId);

    Task<bool> IsInRoleAsync(int userId, string role);

    Task<bool> AuthorizeAsync(int userId, string policyName);

    Task<int> CreateUserAsync(string userName, string password);

    Task DeleteUserAsync(int userId);
}