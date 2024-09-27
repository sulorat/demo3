using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace demo3.EntityModels;

public partial class ShaluhinContext : DbContext
{
    public ShaluhinContext()
    {
    }

    public ShaluhinContext(DbContextOptions<ShaluhinContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachedproduct> Attachedproducts { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Clientservice> Clientservices { get; set; }

    public virtual DbSet<Documentbyservice> Documentbyservices { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productphoto> Productphotos { get; set; }

    public virtual DbSet<Productsale> Productsales { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Servicephoto> Servicephotos { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Tagofclient> Tagofclients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Database = shaluhin; Username = shaluhin; Host = 45.67.56.214; Password = yniBvADq; Port = 5454");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachedproduct>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("attachedproduct", "demo3");

            entity.Property(e => e.Attachedproductid).HasColumnName("attachedproductid");
            entity.Property(e => e.Mainproductid).HasColumnName("mainproductid");

            entity.HasOne(d => d.AttachedproductNavigation).WithMany()
                .HasForeignKey(d => d.Attachedproductid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("attachedproduct_attachedproductid_fkey");

            entity.HasOne(d => d.Mainproduct).WithMany()
                .HasForeignKey(d => d.Mainproductid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("attachedproduct_mainproductid_fkey");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("client_pkey");

            entity.ToTable("client", "demo3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Gendercode)
                .HasMaxLength(1)
                .HasColumnName("gendercode");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(50)
                .HasColumnName("patronymic");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Photopath)
                .HasMaxLength(1000)
                .HasColumnName("photopath");
            entity.Property(e => e.Registrationdate).HasColumnName("registrationdate");

            entity.HasOne(d => d.GendercodeNavigation).WithMany(p => p.Clients)
                .HasForeignKey(d => d.Gendercode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("client_gendercode_fkey");
        });

        modelBuilder.Entity<Clientservice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clientservice_pkey");

            entity.ToTable("clientservice", "demo3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clientid).HasColumnName("clientid");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.Serviceid).HasColumnName("serviceid");
            entity.Property(e => e.Starttime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("starttime");

            entity.HasOne(d => d.Client).WithMany(p => p.Clientservices)
                .HasForeignKey(d => d.Clientid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("clientservice_clientid_fkey");

            entity.HasOne(d => d.Service).WithMany(p => p.Clientservices)
                .HasForeignKey(d => d.Serviceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("clientservice_serviceid_fkey");
        });

        modelBuilder.Entity<Documentbyservice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("documentbyservice_pkey");

            entity.ToTable("documentbyservice", "demo3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clientserviceid).HasColumnName("clientserviceid");
            entity.Property(e => e.Documentpath)
                .HasMaxLength(1000)
                .HasColumnName("documentpath");

            entity.HasOne(d => d.Clientservice).WithMany(p => p.Documentbyservices)
                .HasForeignKey(d => d.Clientserviceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("documentbyservice_clientserviceid_fkey");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("gender_pkey");

            entity.ToTable("gender", "demo3");

            entity.Property(e => e.Code)
                .HasMaxLength(1)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("manufacturer_pkey");

            entity.ToTable("manufacturer", "demo3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Startdate).HasColumnName("startdate");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");

            entity.ToTable("product", "demo3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Mainimagepath)
                .HasMaxLength(1000)
                .HasColumnName("mainimagepath");
            entity.Property(e => e.Manufacturerid).HasColumnName("manufacturerid");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.Manufacturerid)
                .HasConstraintName("product_manufacturerid_fkey");
        });

        modelBuilder.Entity<Productphoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("productphoto_pkey");

            entity.ToTable("productphoto", "demo3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Photopath)
                .HasMaxLength(1000)
                .HasColumnName("photopath");
            entity.Property(e => e.Productid).HasColumnName("productid");

            entity.HasOne(d => d.Product).WithMany(p => p.Productphotos)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productphoto_productid_fkey");
        });

        modelBuilder.Entity<Productsale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("productsale_pkey");

            entity.ToTable("productsale", "demo3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clientserviceid).HasColumnName("clientserviceid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Saledate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("saledate");

            entity.HasOne(d => d.Product).WithMany(p => p.Productsales)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productsale_productid_fkey");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("service_pkey");

            entity.ToTable("service", "demo3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.Durationinseconds).HasColumnName("durationinseconds");
            entity.Property(e => e.Mainimagepath)
                .HasMaxLength(1000)
                .HasColumnName("mainimagepath");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Servicephoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("servicephoto_pkey");

            entity.ToTable("servicephoto", "demo3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Photopath)
                .HasMaxLength(1000)
                .HasColumnName("photopath");
            entity.Property(e => e.Serviceid).HasColumnName("serviceid");

            entity.HasOne(d => d.Service).WithMany(p => p.Servicephotos)
                .HasForeignKey(d => d.Serviceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("servicephoto_serviceid_fkey");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tag_pkey");

            entity.ToTable("tag", "demo3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Color)
                .HasMaxLength(6)
                .HasColumnName("color");
            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Tagofclient>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tagofclient", "demo3");

            entity.Property(e => e.Clientid).HasColumnName("clientid");
            entity.Property(e => e.Tagid).HasColumnName("tagid");

            entity.HasOne(d => d.Client).WithMany()
                .HasForeignKey(d => d.Clientid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tagofclient_clientid_fkey");

            entity.HasOne(d => d.Tag).WithMany()
                .HasForeignKey(d => d.Tagid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tagofclient_tagid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
