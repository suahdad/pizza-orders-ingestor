using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace pizza_orders_ingestor;

public partial class PizzaPlaceDbContext : DbContext
{
    public PizzaPlaceDbContext()
    {
    }

    public PizzaPlaceDbContext(DbContextOptions<PizzaPlaceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Pizzadetail> Pizzadetails { get; set; }

    public virtual DbSet<Pizzaingredient> Pizzaingredients { get; set; }

    public virtual DbSet<Pizzaorder> Pizzaorders { get; set; }

    public virtual DbSet<Pizzaprice> Pizzaprices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("Name=ConnectionStrings:PizzaPlaceDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("orderdetails");

            entity.Property(e => e.Id)
                .HasColumnType("mediumint unsigned")
                .HasColumnName("id");
            entity.Property(e => e.OrderId)
                .HasColumnType("mediumint unsigned")
                .HasColumnName("order_id");
            entity.Property(e => e.PizzaId)
                .HasMaxLength(20)
                .HasColumnName("pizza_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
        });

        modelBuilder.Entity<Pizzadetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("pizzadetails");

            entity.Property(e => e.Category)
                .HasMaxLength(20)
                .HasColumnName("category");
            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Pizzaingredient>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("pizzaingredients");

            entity.Property(e => e.Ingredients)
                .HasMaxLength(100)
                .HasColumnName("ingredients");
            entity.Property(e => e.PizzaId)
                .HasMaxLength(20)
                .HasColumnName("pizza_id");
        });

        modelBuilder.Entity<Pizzaorder>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("pizzaorders");

            entity.Property(e => e.Datetime)
                .HasColumnType("datetime")
                .HasColumnName("datetime");
            entity.Property(e => e.Id)
                .HasColumnType("mediumint unsigned")
                .HasColumnName("id");
        });

        modelBuilder.Entity<Pizzaprice>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("pizzaprices");

            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .HasColumnName("id");
            entity.Property(e => e.PizzaId)
                .HasMaxLength(20)
                .HasColumnName("pizza_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Size)
                .HasMaxLength(3)
                .HasColumnName("size");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
