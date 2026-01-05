using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace OnePWA.Models.Entities;

public partial class OnecgdbContext : DbContext
{
    public OnecgdbContext()
    {
    }

    public OnecgdbContext(DbContextOptions<OnecgdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cards> Cards { get; set; }

    public virtual DbSet<PushSusbcrption> PushSusbcrption { get; set; }

    public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Cards>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cards");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Color)
                .HasMaxLength(45)
                .HasColumnName("color");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Special).HasColumnName("special");
        });

        modelBuilder.Entity<PushSusbcrption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("push_susbcrption");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Auth)
                .HasMaxLength(100)
                .HasColumnName("auth");
            entity.Property(e => e.Endpoint)
                .HasMaxLength(500)
                .HasColumnName("endpoint");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("timestamp")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaUltimaNotificacion)
                .HasColumnType("timestamp")
                .HasColumnName("fecha_ultima_notificacion");
            entity.Property(e => e.P256dh)
                .HasMaxLength(200)
                .HasColumnName("p256dh");
            entity.Property(e => e.UserAgent)
                .HasMaxLength(500)
                .HasColumnName("user_agent");
        });

        modelBuilder.Entity<RefreshTokens>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("refresh_tokens");

            entity.HasIndex(e => e.IdUsuario, "FK1_idusuario");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Creado)
                .HasColumnType("datetime")
                .HasColumnName("creado");
            entity.Property(e => e.Expiracion)
                .HasColumnType("datetime")
                .HasColumnName("expiracion");
            entity.Property(e => e.IdUsuario)
                .HasColumnType("int(11)")
                .HasColumnName("id_usuario");
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .HasColumnName("token");
            entity.Property(e => e.Usado)
                .HasColumnType("tinyint(2)")
                .HasColumnName("usado");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK1_idusuario");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .HasColumnName("password");
            entity.Property(e => e.ProfilePictures)
                .HasMaxLength(50)
                .HasColumnName("profile_Pictures");
            entity.Property(e => e.WonGames)
                .HasColumnType("int(11)")
                .HasColumnName("wonGames");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
