using UrlShortener.Models;

namespace UrlShortener.Abstract
{
    public interface IJWTUserService
    {
        string Authenticate(UserModel user);
        UserModel GetById(int id);
    }
}
