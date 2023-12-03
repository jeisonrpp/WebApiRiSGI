using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApiRiSGI.Models;

public partial class SgiContext : DbContext
{
    public SgiContext()
    {
    }

    public SgiContext(DbContextOptions<SgiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activos> Activos { get; set; }

    public virtual DbSet<Areas> Areas { get; set; }

    public virtual DbSet<Asignaciones> Asignaciones { get; set; }

    public virtual DbSet<Departamentos> Departamentos { get; set; }

    public virtual DbSet<Descargos> Descargos { get; set; }

    public virtual DbSet<Localidades> Localidades { get; set; }

    public virtual DbSet<Marcas> Marcas { get; set; }

    public virtual DbSet<Modelos> Modelos { get; set; }

    public virtual DbSet<Movimientos> Movimientos { get; set; }

    public virtual DbSet<Organos> Organos { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<RolesUsuarios> RolesUsuarios { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }
    public virtual DbSet<TipoActivo> TipoActivo { get; set; }
    public virtual DbSet<MovimientoView> Movimientosview { get; set; }
    public virtual DbSet<DescargosView> DescargosView { get; set; }
    public virtual DbSet<ActivosView> ActivosView { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activos>(entity =>
        {
            entity.HasKey(e => e.ActivosId);

            entity.Property(e => e.ActivosId).HasColumnName("ActivosID");
            entity.Property(e => e.ActivoPrincipal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ActivoSecundario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MarcaActivo).HasColumnName("MarcaActivo");
            entity.Property(e => e.ModeloActivo).HasColumnName("ModeloActivo");
            entity.Property(e => e.Serial)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.TipoActivo).HasColumnName("TipoActivo");
            entity.Property(e => e.FechaAdquisicion).HasColumnType("datetime");
        });
        modelBuilder.Entity<Areas>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AreaId)
                .ValueGeneratedOnAdd()
                .HasColumnName("AreaID");
            entity.Property(e => e.AreaNombre)
              .HasMaxLength(150)
              .IsUnicode(false);
            entity.Property(e => e.DepartamentoId).HasColumnName("DepartamentoID");
            entity.Property(e => e.AreaEncargado)
         .HasMaxLength(50)
         .IsUnicode(false);
        });

        modelBuilder.Entity<Asignaciones>(entity =>
        {
            entity.HasKey(e => e.AsigId);

            entity.Property(e => e.ActivosId).HasColumnName("ActivosID");
            entity.Property(e => e.DomainUser)
    .HasMaxLength(50)
    .IsUnicode(false);
            entity.Property(e => e.DisplayName)
    .HasMaxLength(50)
    .IsUnicode(false);
            entity.Property(e => e.LocalidadId).HasColumnName("LocalidadId");
            entity.Property(e => e.OrganoID).HasColumnName("OrganoID");
            entity.Property(e => e.AreaId).HasColumnName("AreaId");

            entity.Property(e => e.FechaAsignacion).HasColumnType("datetime");
        });

        modelBuilder.Entity<Departamentos>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.DepartamentoNombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("DepartamentoNombre");
            entity.Property(e => e.DepartamentoId)
                .ValueGeneratedOnAdd()
                .HasColumnName("DepartamentoID");
            entity.Property(e => e.LocalidadId).HasColumnName("LocalidadID");
            entity.Property(e => e.OrganoId).HasColumnName("OrganoID");
            entity.Property(e => e.DepartamentoEncargado)
             .HasMaxLength(50)
             .IsUnicode(false)
             .HasColumnName("DepartamentoEncargado");
        });

        modelBuilder.Entity<Descargos>(entity =>
        {
            entity.HasKey(e => e.DescargoId);

            entity.Property(e => e.DescargoId).HasColumnName("DescargoID");
            entity.Property(e => e.Descargo)
           .HasMaxLength(15)
           .IsUnicode(false);
            entity.Property(e => e.ActivoId).HasColumnName("ActivoID");
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.LocalidadId).HasColumnName("LocalidadID");
            entity.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioRemitente)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Localidades>(entity =>
        {
            entity.HasKey(e => e.LocalidadId);

            entity.Property(e => e.Localidad)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.LocalidadId)
                .ValueGeneratedOnAdd()
                .HasColumnName("LocalidadID");
        });

        modelBuilder.Entity<Marcas>(entity =>
        {
            entity.HasKey(e => e.MarcaId);

            entity.Property(e => e.Marca1)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("Marca");
            entity.Property(e => e.MarcaId)
                .ValueGeneratedOnAdd()
                .HasColumnName("MarcaID");
        });

        modelBuilder.Entity<Modelos>(entity =>
        {
            entity.HasKey(e => e.ModeloId);

            entity.Property(e => e.MarcaId).HasColumnName("MarcaID");
            entity.Property(e => e.Modelo1)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("Modelo");
            entity.Property(e => e.ModeloId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ModeloID");
        });

        modelBuilder.Entity<Movimientos>(entity =>
        {
            entity.HasKey(e => e.MovimientoId);

            entity.Property(e => e.MovimientoId).HasColumnName("MovimientoID");
            entity.Property(e => e.Movimiento)
    .HasMaxLength(15)
    .IsUnicode(false);
            entity.Property(e => e.ActivoId).HasColumnName("ActivoID");
            entity.Property(e => e.LocalidadId_Destino).HasColumnName("LocalidadId_Destino");
            entity.Property(e => e.AreaId_Destino).HasColumnName("AreaId_Destino");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.LocalidadId_Remitente).HasColumnName("LocalidadId_Remitente");
            entity.Property(e => e.AreaId_Remitente).HasColumnName("AreaId_Remitente");
            entity.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MovimientoTipo)
               .HasMaxLength(50)
               .IsUnicode(false);
            entity.Property(e => e.UsuarioDestino)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioRemitente)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Organos>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Organo1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Organo");
            entity.Property(e => e.OrganoId)
                .ValueGeneratedOnAdd()
                .HasColumnName("OrganoID");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.RoliD);

            entity.Property(e => e.RolName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RolesUsuarios>(entity =>
        {
            entity.HasKey(e => e.UserRoliD).HasName("PK_UserRoles");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.UserLogin)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.UserPass)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoActivo>(entity =>
        {
            entity.HasKey(e => e.TipoId);

            entity.Property(e => e.TipoId)
                .ValueGeneratedOnAdd()
                .HasColumnName("TipoId");
            entity.Property(e => e.TipoNombre).HasColumnName("TipoNombre");
        });

       
        modelBuilder.Entity<ActivosView>().ToView(nameof(ActivosView)).HasNoKey(); 
        modelBuilder.Entity<MovimientoView>().ToView(nameof(Movimientosview)).HasNoKey(); 
        modelBuilder.Entity<DescargosView>().ToView(nameof(DescargosView)).HasNoKey(); 
           

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
