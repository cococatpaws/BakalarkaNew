using webapi.Models;

namespace webapi.Service
{
    public interface IJwtService
    {
        public string CreateJwt(User user);
    }
}
