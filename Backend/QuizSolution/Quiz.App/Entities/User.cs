using Microsoft.AspNetCore.Identity;

namespace Quiz.App.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}