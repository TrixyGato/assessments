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

    public virtual DbSet<Stream> Streams { get; set; }

    public virtual DbSet<StreamStudent> StreamStudents { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:assessments-server.database.windows.net,1433;Initial Catalog=assessments-DB;Persist Security Info=False;User ID=assessments-dev;Password=SuperStr0ngPass;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Grading>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Grading__3214EC076B93F693");

            entity.ToTable("Grading");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Comment).HasMaxLength(255);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.StreamId).HasMaxLength(255);
            entity.Property(e => e.StudentId).HasMaxLength(255);
            entity.Property(e => e.SubjectId).HasMaxLength(255);

            entity.HasOne(d => d.Stream).WithMany(p => p.Gradings)
                .HasForeignKey(d => d.StreamId)
                .HasConstraintName("FK__Grading__StreamI__06CD04F7");

            entity.HasOne(d => d.Student).WithMany(p => p.Gradings)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Grading__Student__05D8E0BE");

            entity.HasOne(d => d.Subject).WithMany(p => p.Gradings)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK__Grading__Subject__07C12930");
        });

        modelBuilder.Entity<Stream>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stream__3214EC077009698C");

            entity.ToTable("Stream");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<StreamStudent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StreamSt__3214EC0789F6BCD5");

            entity.ToTable("StreamStudent");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.StreamId).HasMaxLength(255);
            entity.Property(e => e.StudentId).HasMaxLength(255);

            entity.HasOne(d => d.Stream).WithMany(p => p.StreamStudents)
                .HasForeignKey(d => d.StreamId)
                .HasConstraintName("FK__StreamStu__Strea__160F4887");

            entity.HasOne(d => d.Student).WithMany(p => p.StreamStudents)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__StreamStu__Stude__17036CC0");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student__3214EC078880D2B3");

            entity.ToTable("Student");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subject__3214EC0732C91315");

            entity.ToTable("Subject");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.TeacherId).HasMaxLength(255);

            entity.HasOne(d => d.Teacher).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__Subject__Teacher__08B54D69");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teacher__3214EC07CC564AD7");

            entity.ToTable("Teacher");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
