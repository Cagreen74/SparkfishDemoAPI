using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SparkFishDemo.Resources.DB;

public partial class SparkFishDemoContext : DbContext
{
    public SparkFishDemoContext()
    {
    }

    public SparkFishDemoContext(DbContextOptions<SparkFishDemoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActiveStatus> ActiveStatuses { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<DefaultRange> DefaultRanges { get; set; }

    public virtual DbSet<RangeSearchHistory> RangeSearchHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:dynavisionweb.database.windows.net,1433;Initial Catalog=SparkFishDemo;Persist Security Info=False;User ID=SparkFishSqlUser;Password=T@rag0n@30;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActiveStatus>(entity =>
        {
            entity.HasKey(e => e.ActiveStatusId).HasName("PK__ActiveSt__14BE66DCE18F828C");

            entity.ToTable("ActiveStatus");

            entity.Property(e => e.ActiveStatusCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.ActiveStatusDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<DefaultRange>(entity =>
        {
            entity.HasKey(e => e.RangeId).HasName("PK__DefaultR__6899CA144D1C9D26");
        });

        modelBuilder.Entity<RangeSearchHistory>(entity =>
        {
            entity.HasKey(e => e.SearchHistoryId).HasName("PK__RangeSea__555F7F794873B415");

            entity.ToTable("RangeSearchHistory");

            entity.Property(e => e.SearchDate).HasColumnType("datetime");

            entity.HasOne(d => d.ActiveStatus).WithMany(p => p.RangeSearchHistories)
                .HasForeignKey(d => d.ActiveStatusId)
                .HasConstraintName("FK__RangeSear__Activ__619B8048");

            entity.HasOne(d => d.DefaultRange).WithMany(p => p.RangeSearchHistories)
                .HasForeignKey(d => d.DefaultRangeId)
                .HasConstraintName("FK__RangeSear__Defau__60A75C0F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
