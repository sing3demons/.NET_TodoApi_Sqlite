using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Database
{
    public class TodoApiContext : DbContext
    {
        public TodoApiContext(DbContextOptions<TodoApiContext> options) : base(options) { }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}