using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace pizzaweb.Models.Tables
{
    public partial class pizzawebContext : DbContext
    {
        public pizzawebContext()
        {
        }

        public pizzawebContext(DbContextOptions<pizzawebContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCategory> TblCategories { get; set; } = null!;
        public virtual DbSet<TblContact> TblContacts { get; set; } = null!;
        public virtual DbSet<TblCustomer> TblCustomers { get; set; } = null!;
        public virtual DbSet<TblOrder> TblOrders { get; set; } = null!;
        public virtual DbSet<TblPaymentOder> TblPaymentOders { get; set; } = null!;
        public virtual DbSet<TblProduct> TblProducts { get; set; } = null!;
        public virtual DbSet<TblProductOrder> TblProductOrders { get; set; } = null!;
        public virtual DbSet<TblUser> TblUsers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;database=pizzaweb;user=root;password=080602;allow user variables=True", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.ToTable("tbl_category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<TblContact>(entity =>
            {
                entity.ToTable("tbl_contact");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .HasColumnName("full_name");

                entity.Property(e => e.Message)
                    .HasMaxLength(200)
                    .HasColumnName("message");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .HasColumnName("phone_number");
            });

            modelBuilder.Entity<TblCustomer>(entity =>
            {
                entity.ToTable("tbl_customer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(45)
                    .HasColumnName("email")
                    .UseCollation("utf8mb4_bin");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .HasColumnName("password")
                    .UseCollation("utf8mb4_bin");

                entity.Property(e => e.Phone)
                    .HasMaxLength(100)
                    .HasColumnName("phone")
                    .UseCollation("utf8mb4_bin");
            });

            modelBuilder.Entity<TblOrder>(entity =>
            {
                entity.ToTable("tbl_order");

                entity.HasIndex(e => e.Payment, "payment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.CustomerAddress)
                    .HasMaxLength(100)
                    .HasColumnName("customer_address")
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(100)
                    .HasColumnName("customer_name")
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.Property(e => e.CustomerPhone)
                    .HasMaxLength(100)
                    .HasColumnName("customer_phone")
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.Property(e => e.CutomerEmail)
                    .HasMaxLength(100)
                    .HasColumnName("cutomer_email")
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.Property(e => e.Payment).HasColumnName("payment");

                entity.Property(e => e.TotalPrice)
                    .HasPrecision(13, 2)
                    .HasColumnName("total_price");

                entity.HasOne(d => d.PaymentNavigation)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.Payment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_order_ibfk_1");
            });

            modelBuilder.Entity<TblPaymentOder>(entity =>
            {
                entity.ToTable("tbl_payment_oder");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.ToTable("tbl_products");

                entity.HasIndex(e => e.CategoryId, "category_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(200)
                    .HasColumnName("avatar")
                    .UseCollation("utf8mb4_bin");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.DetailDescription)
                    .HasColumnName("detail_description")
                    .UseCollation("utf8mb4_bin");

                entity.Property(e => e.Price)
                    .HasPrecision(13, 2)
                    .HasColumnName("price");

                entity.Property(e => e.PriceSale)
                    .HasPrecision(13, 2)
                    .HasColumnName("price_sale");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TblProducts)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("tbl_products_ibfk_1");
            });

            modelBuilder.Entity<TblProductOrder>(entity =>
            {
                entity.ToTable("tbl_product_order");

                entity.HasIndex(e => e.OrderId, "order_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Price)
                    .HasPrecision(13, 2)
                    .HasColumnName("price");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TblProductOrders)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_product_order_ibfk_1");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.ToTable("tbl_user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(45)
                    .HasColumnName("email")
                    .UseCollation("utf8mb4_bin");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .HasColumnName("password")
                    .UseCollation("utf8mb4_bin");

                entity.Property(e => e.Phone)
                    .HasMaxLength(100)
                    .HasColumnName("phone")
                    .UseCollation("utf8mb4_bin");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Username)
                    .HasMaxLength(45)
                    .HasColumnName("username")
                    .UseCollation("utf8mb4_bin");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
