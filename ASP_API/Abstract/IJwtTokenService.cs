using ASP_API.Data.Entities.Identity;

namespace ASP_API.Abstract
{
    public interface IJwtTokenService
    {
        Task<string> CreateToken(UserEntity user);
    }
}
