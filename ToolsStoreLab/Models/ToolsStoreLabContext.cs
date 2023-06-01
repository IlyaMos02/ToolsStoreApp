using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ToolsStoreLab.Models;

public partial class ToolsStoreLabContext : DbContext
{
    private string connectionString = "Data Source=(local);Initial Catalog=ToolsStoreLab;Integrated Security=True;TrustServerCertificate=True;";

    public string ConnectionString
    {
        get { return connectionString; } 
        set { connectionString = value; }
    }

    public ToolsStoreLabContext()
    {
    }

    public ToolsStoreLabContext(DbContextOptions<ToolsStoreLabContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Characteristic> Characteristics { get; set; }

    public virtual DbSet<EnergySource> EnergySources { get; set; }

    public virtual DbSet<Kind> Kinds { get; set; }

    public virtual DbSet<KindTool> KindTools { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Query> Queries { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WorkTime> WorkTimes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Characteristic>(entity =>
        {
            entity.Property(e => e.CharacteristicId).HasColumnName("characteristic_id");
            entity.Property(e => e.KindToolId).HasColumnName("kind_tool_id");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.KindTool).WithMany(p => p.Characteristics)
                .HasForeignKey(d => d.KindToolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Characteristics_KindTools");
        });

        modelBuilder.Entity<EnergySource>(entity =>
        {
            entity.Property(e => e.EnergySourceId).HasColumnName("energy_source_id");
            entity.Property(e => e.Source)
                .HasMaxLength(50)
                .HasColumnName("source");
        });

        modelBuilder.Entity<Kind>(entity =>
        {
            entity.Property(e => e.KindId).HasColumnName("kind_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<KindTool>(entity =>
        {
            entity.Property(e => e.KindToolId).HasColumnName("kind_tool_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.EnergySourceId).HasColumnName("energy_source_id");
            entity.Property(e => e.Power).HasColumnName("power");

            entity.HasOne(d => d.Category).WithMany(p => p.KindTools)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KindTools_Categories");

            entity.HasOne(d => d.EnergySource).WithMany(p => p.KindTools)
                .HasForeignKey(d => d.EnergySourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KindTools_EnergySources");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CharacteristicId).HasColumnName("characteristic_id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity.Property(e => e.KindId).HasColumnName("kind_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");

            entity.HasOne(d => d.Characteristic).WithMany(p => p.Products)
                .HasForeignKey(d => d.CharacteristicId)
                .HasConstraintName("FK_Products_Characteristics");

            entity.HasOne(d => d.Kind).WithMany(p => p.Products)
                .HasForeignKey(d => d.KindId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Kinds");
        });

        modelBuilder.Entity<Query>(entity =>
        {
            entity.Property(e => e.QueryId).HasColumnName("query_id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.QueryText)
                .HasColumnType("text")
                .HasColumnName("query_text");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("user_email");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.Adress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("adress");
            entity.Property(e => e.WorkTimeId).HasColumnName("work_time_id");
            entity.Property(e => e.WorkWithoutElectricity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("work_without_electricity");

            entity.HasOne(d => d.WorkTime).WithMany(p => p.Stores)
                .HasForeignKey(d => d.WorkTimeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stores_WorkTime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Cart)
                .HasColumnType("text")
                .HasColumnName("cart");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .HasColumnName("surname");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<WorkTime>(entity =>
        {
            entity.HasKey(e => e.WorkTimeId).HasName("PK_WorkTime");

            entity.Property(e => e.WorkTimeId).HasColumnName("work_time_id");
            entity.Property(e => e.Time)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("time");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
