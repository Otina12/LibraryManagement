using Library.Model.Models;
using Library.Model.Models.Email;
using Library.Model.Models.Menu;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Library.Data;

public class ApplicationDbContext : IdentityDbContext<Employee>
{
    // access tables via context
    public DbSet<Author> Authors { get; set; }
    public DbSet<OriginalBook> OriginalBooks { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookAuthor> BookAuthor { get; set; }
    public DbSet<BookCopy> BookCopies { get; set; }
    public DbSet<BookGenre> BookGenre { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<ReservationCopy> ReservationCopies { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Shelf> Shelves { get; set; }

    // dynamic menu with role permissions
    public DbSet<RoleMenuPermission> RoleMenuPermission { get; set; }
    public DbSet<NavigationMenu> NavigationMenu { get; set; }
    
    // email templates
    public DbSet<EmailModel> EmailModels { get; set; }


    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // Library.Data.Configurations folder
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }
}
