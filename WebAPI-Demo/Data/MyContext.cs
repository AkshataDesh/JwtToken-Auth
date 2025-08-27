using Microsoft.EntityFrameworkCore;
using WebAPI_Demo.Entities;

namespace WebAPI_Demo.Data
{
    public class MyContext:DbContext
    {
      public  MyContext(DbContextOptions<MyContext> op) : base(op)
        {

        }

    public DbSet<User> Users { get; set; }

    }
}
