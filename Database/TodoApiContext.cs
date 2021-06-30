using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Database
{
    public class TodoApiContext : IdentityDbContext
    {
        public TodoApiContext(DbContextOptions<TodoApiContext> options) : base(options) { }

        public virtual DbSet<TodoItem> TodoItems { get; set; }
    }
}