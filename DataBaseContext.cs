using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Telegram_Bot_Project;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categories1> Categories1s { get; set; }

    public virtual DbSet<Categories2> Categories2s { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\vasil\\source\\repos\\Telegram_Bot_Project\\Telegram_Bot_Project\\Database.mdf;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categories1>(entity =>
        {
            entity.ToTable("Categories1");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("id");
            entity.Property(e => e.Categories).HasColumnType("text");
            entity.Property(e => e.LoginPassword).HasColumnType("text");
            entity.Property(e => e.Price).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.ProductName).HasColumnType("text");
        });

        modelBuilder.Entity<Categories2>(entity =>
        {
            entity.ToTable("Categories2");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("id");
            entity.Property(e => e.Categories).HasColumnType("text");
            entity.Property(e => e.LoginPassword).HasColumnType("text");
            entity.Property(e => e.Price).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.ProductName).HasColumnType("text");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("id");
            entity.Property(e => e.Money).HasColumnType("numeric(18, 0)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
