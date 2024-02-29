using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ApiNetDani.Models;

public partial class IesdawdaniContext : DbContext
{
    public IesdawdaniContext()
    {
    }

    public IesdawdaniContext(DbContextOptions<IesdawdaniContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Equipo> Equipo { get; set; }

    public virtual DbSet<Jugador> Jugador { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("equipo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fundacion).HasColumnName("fundacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Jugador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("jugador");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bandera).HasColumnName("Bandera");
            entity.Property(e => e.Banderablob)
                .HasMaxLength(255)
                .HasColumnName("Banderablob");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.IdEquipo).HasColumnName("idEquipo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.HasOne(d => d.oEquipo).WithMany(p => p.Jugador)
                .HasForeignKey(d => d.IdEquipo)
                .HasConstraintName("FK_IdEquipo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
