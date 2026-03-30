using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ShoesShop.Models;

public partial class DbShoesContext : DbContext
{
    private static DbShoesContext _context;
    public static DbShoesContext Context
    {
        get
        {
            if (_context == null)
            {
                _context = new DbShoesContext();
            }
            return _context;
        }
    }
    public DbShoesContext()
    {
    }

    public DbShoesContext(DbContextOptions<DbShoesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacture> Manufactures { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<PickupPoint> PickupPoints { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Tovar> Tovars { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=1234;database=db_shoes", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Manufacture>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId).HasName("PRIMARY");

            entity.ToTable("manufactures");

            entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
            entity.Property(e => e.ManufacturerName)
                .HasMaxLength(100)
                .HasColumnName("manufacturer_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("orders");

            entity.HasIndex(e => e.ClientId, "client_id_fk_idx");

            entity.HasIndex(e => e.PickupPointId, "pickup_point_id_fk_idx");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.CodeToReceive)
                .HasMaxLength(10)
                .HasColumnName("code_to_receive");
            entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
            entity.Property(e => e.OrderStatus)
                .HasDefaultValueSql("'Новый'")
                .HasColumnType("enum('Новый','Завершен')")
                .HasColumnName("order_status");
            entity.Property(e => e.PickupPointId).HasColumnName("pickup_point_id");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("client_id_fk");

            entity.HasOne(d => d.PickupPoint).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PickupPointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pickup_point_id_fk");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PRIMARY");

            entity.ToTable("order_items");

            entity.HasIndex(e => e.OrderId, "order_id_fk1_idx");

            entity.HasIndex(e => e.TovarArticle, "tovar_article_fk1_idx");

            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.TovarArticle)
                .HasMaxLength(6)
                .HasColumnName("tovar_article");
            entity.Property(e => e.TovarQuantity).HasColumnName("tovar_quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_id_fk1");

            entity.HasOne(d => d.TovarArticleNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.TovarArticle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tovar_article_fk1");
        });

        modelBuilder.Entity<PickupPoint>(entity =>
        {
            entity.HasKey(e => e.PickupPointId).HasName("PRIMARY");

            entity.ToTable("pickup_points");

            entity.Property(e => e.PickupPointId).HasColumnName("pickup_point_id");
            entity.Property(e => e.PickupPointCity)
                .HasMaxLength(45)
                .HasColumnName("pickup_point_city");
            entity.Property(e => e.PickupPointHouse)
                .HasMaxLength(45)
                .HasColumnName("pickup_point_house");
            entity.Property(e => e.PickupPointIndex)
                .HasMaxLength(6)
                .HasColumnName("pickup_point_index");
            entity.Property(e => e.PickupPointStreet)
                .HasMaxLength(45)
                .HasColumnName("pickup_point_street");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PRIMARY");

            entity.ToTable("suppliers");

            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(100)
                .HasColumnName("supplier_name");
        });

        modelBuilder.Entity<Tovar>(entity =>
        {
            entity.HasKey(e => e.TovarArticle).HasName("PRIMARY");

            entity.ToTable("tovars");

            entity.HasIndex(e => e.CategoryId, "category_id_fk2_idx");

            entity.HasIndex(e => e.ManufacturerId, "manufacturer_id_fk2_idx");

            entity.HasIndex(e => e.SupplierId, "supplier_id_fk2_idx");

            entity.Property(e => e.TovarArticle)
                .HasMaxLength(6)
                .HasColumnName("tovar_article");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CurrentDiscount)
                .HasPrecision(8, 2)
                .HasColumnName("current_discount");
            entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
            entity.Property(e => e.Photo)
                .HasMaxLength(255)
                .HasColumnName("photo");
            entity.Property(e => e.QuantityInStock).HasColumnName("quantity_in_stock");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.TovarDescription)
                .HasColumnType("text")
                .HasColumnName("tovar_description");
            entity.Property(e => e.TovarName)
                .HasMaxLength(255)
                .HasColumnName("tovar_name");
            entity.Property(e => e.TovarPrice)
                .HasPrecision(8, 2)
                .HasColumnName("tovar_price");
            entity.Property(e => e.UnitOfMeasurement)
                .HasMaxLength(45)
                .HasColumnName("unit_of_measurement");

            entity.HasOne(d => d.Category).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("category_id_fk2");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.ManufacturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("manufacturer_id_fk2");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("supplier_id_fk2");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .HasColumnName("user_email");
            entity.Property(e => e.UserFirstname)
                .HasMaxLength(100)
                .HasColumnName("user_firstname");
            entity.Property(e => e.UserLastname)
                .HasMaxLength(100)
                .HasColumnName("user_lastname");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .HasColumnName("user_password");
            entity.Property(e => e.UserRole)
                .HasDefaultValueSql("'Авторизированный клиент'")
                .HasColumnType("enum('Авторизированный клиент','Администратор','Менеджер')")
                .HasColumnName("user_role");
            entity.Property(e => e.UserSurname)
                .HasMaxLength(100)
                .HasColumnName("user_surname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
