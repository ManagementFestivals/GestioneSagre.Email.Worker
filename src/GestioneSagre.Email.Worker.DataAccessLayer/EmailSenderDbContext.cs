using GestioneSagre.Email.Worker.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestioneSagre.Email.Worker.DataAccessLayer;

public class EmailSenderDbContext : DbContext
{
    public EmailSenderDbContext(DbContextOptions<EmailSenderDbContext> options) : base(options)
    {
    }

    public virtual DbSet<EmailMessage> EmailMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EmailMessage>(entity =>
        {
            entity.ToTable("EmailMessages");
            entity.Property(x => x.Status).HasConversion<string>();
        });
    }
}