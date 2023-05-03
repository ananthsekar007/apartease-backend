using apartease_backend.Dao;

namespace apartease_backend.Services.JwtService
{
    public interface IJwtService
    {
        string CreateToken(AppUser user);
    }
}
