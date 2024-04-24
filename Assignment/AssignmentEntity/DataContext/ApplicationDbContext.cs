using System;
using System.Collections.Generic;
using AssignmentEntity.DataModels;
using Microsoft.EntityFrameworkCore;

namespace AssignmentEntity.DataContext;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<DataModels.Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID = postgres;Password=2829;Server=localhost;Port=5432;Database=Assignment;Integrated Security=true;Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Category_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Category_new_id_seq\"'::regclass)");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("City_pkey");
        });

        modelBuilder.Entity<DataModels.Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Task_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Task_new_id_seq\"'::regclass)");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Tasks).HasConstraintName("Category_Id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
