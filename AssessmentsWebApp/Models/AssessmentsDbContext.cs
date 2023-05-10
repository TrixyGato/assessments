using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AssessmentsWebApp.Models;

public partial class AssessmentsDbContext : DbContext
{
    public AssessmentsDbContext()
    {
    }

    public AssessmentsDbContext(DbContextOptions<AssessmentsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Grading> Gradings { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:assessments-server.database.windows.net,1433;Initial Catalog=assessments-DB;Persist Security Info=False;User ID=assessments-dev;Password=SuperStr0ngPass;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Grading>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Grading__3214EC07A20E4EFB");

            entity.ToTable("Grading");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.StudentId)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Student).WithMany(p => p.Gradings)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Grading__Student__7B5B524B");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student__3214EC079344C9F1");

            entity.ToTable("Student");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teacher__3214EC0704882CAA");

            entity.ToTable("Teacher");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
