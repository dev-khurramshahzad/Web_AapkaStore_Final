using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AapkaStore.Models;

public partial class DbAapkaStoreContext : DbContext
{
    public DbAapkaStoreContext()
    {
    }

    public DbAapkaStoreContext(DbContextOptions<DbAapkaStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CatId);

            entity.Property(e => e.CatId).HasColumnName("CatID");
            entity.Property(e => e.Details).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.Property(e => e.FeedBackId).HasColumnName("FeedBackID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.FeedbackType).HasMaxLength(50);
            entity.Property(e => e.Message).HasMaxLength(400);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserFid).HasColumnName("UserFID");

            entity.HasOne(d => d.UserF).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserFid)
                .HasConstraintName("FK_Feedbacks_Users");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.CatFid).HasColumnName("CatFID");
            entity.Property(e => e.Details).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.CatF).WithMany(p => p.Items)
                .HasForeignKey(d => d.CatFid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Items_Categories");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Details).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserFid).HasColumnName("UserFID");

            entity.HasOne(d => d.UserF).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserFid)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.ItemFid).HasColumnName("ItemFID");
            entity.Property(e => e.OrderFid).HasColumnName("OrderFID");

            entity.HasOne(d => d.ItemF).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ItemFid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_OrderDetails_Items");

            entity.HasOne(d => d.OrderF).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderFid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_OrderDetails_Orders");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Details).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
