namespace NoteArchive.Data;
using NoteArchive.Models;   
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class NoteArchiveContext : IdentityDbContext<User>
{

    public NoteArchiveContext(DbContextOptions<NoteArchiveContext> options) : base(options)
    {
    }
    public DbSet<Note> Notes { get; set; }
    public DbSet<NoteImage> NoteImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Note>().ToTable("Note");
        modelBuilder.Entity<NoteImage>().ToTable("NoteImage");
    }

}