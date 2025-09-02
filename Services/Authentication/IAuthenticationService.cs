namespace Services.Authentication;

public interface IAuthenticationService
{
    Task<Response<string>> GenerateTokenByUserIdAsync(string userId);
}