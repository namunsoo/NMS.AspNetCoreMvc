using Microsoft.EntityFrameworkCore;
using NMS.AspNetCoreMvc.Web.Models.DB;

namespace NMS.AspNetCoreMvc.Web.DataContext
{
    public class CommonDB : DbContext
    {
        public CommonDB(DbContextOptions<CommonDB> options)
            : base(options)
        {
        }

        public DbSet<QuestionCards> QuestionCards { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Board> Board { get; set; }
        public DbSet<BoardCategory> BoardCategory { get; set; }
    }
}
