using Quiz.App.Entities;

namespace Quiz.App.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user);
    }
}