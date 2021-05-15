using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Quiz.App.Entities;

namespace Quiz.App.DbContext
{
    public class EfDbContextBase : IdentityDbContext<User,Role,int>
    {
        
    }
}