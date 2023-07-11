using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BaseProjectApp.Library.DbSpsResult;
using BaseProjectApp.Library.Templates.DTOs;
using BaseProjectApp.Library.Templates.Responses;
using BaseProjectApp.Library.Utility;

namespace BaseProjectApp.Library.DbModels
{
    public partial class BaseProjectDBContext : DbContext
    {
        public BaseProjectDBContext()
        {
        }

        public BaseProjectDBContext(DbContextOptions<BaseProjectDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!; 
        public virtual DbSet<CompanyLookup> CompanyLookups { get; set; } = null!;
        public virtual DbSet<CompanyLookupValue> CompanyLookupValues { get; set; } = null!; 
        public virtual DbSet<Dbexception> Dbexceptions { get; set; } = null!; 
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<LocationsBack> LocationsBacks { get; set; } = null!;
        public virtual DbSet<LocationsBackup> LocationsBackups { get; set; } = null!;
        public virtual DbSet<Lookup> Lookups { get; set; } = null!;
        public virtual DbSet<LookupValue> LookupValues { get; set; } = null!;
        public virtual DbSet<MediaFile> MediaFiles { get; set; } = null!;
        public virtual DbSet<MobileCustomMenu> MobileCustomMenus { get; set; } = null!; 
        public virtual DbSet<SystemParameter> SystemParameters { get; set; } = null!;
        public virtual DbSet<TextVar> TextVars { get; set; } = null!;
        public virtual DbSet<UserLog> UserLogs { get; set; } = null!;
        public virtual DbSet<UserPermission> UserPermissions { get; set; } = null!;
        public virtual DbSet<ViewArea> ViewAreas { get; set; } = null!;
        public virtual DbSet<ViewCity> ViewCities { get; set; } = null!;
        public virtual DbSet<ViewCountry> ViewCountries { get; set; } = null!; 
        public virtual DbSet<PropertySpRes> SP_Properties { get; set; }
        public virtual DbSet<PropertySpResCount> SP_PropertiesTotal { get; set; }
        public virtual DbSet<LocationsResponse> LocationsData { get; set; }
        public virtual DbSet<IsValidNewAd_FreeAds_Count> IsValidNewAd_FreeAds { get; set; }  



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string ConnectionString = Configuration.GetConfigurationString();
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<IsValidNewAd_FreeAds_Count>().HasNoKey();
            modelBuilder.Entity<LocationsResponse>().HasNoKey();   
            modelBuilder.Entity<PropertySpRes>().HasNoKey();
            modelBuilder.Entity<PropertySpResCount>().HasNoKey(); 

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.Code, "UQ__AspNetRo__A25C5AA7C4E368AD")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Code)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CssClassName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ModuleName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);

                entity.Property(e => e.SectionName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.Property(e => e.RoleId).HasMaxLength(128);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AspNetRol__RoleI__0697FACD");
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.ResetPasswordCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId").HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId").HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId").HasName("PK_dbo.AspNetUserRoles");

                            j.ToTable("AspNetUserRoles");

                            j.IndexerProperty<string>("UserId").HasMaxLength(128);

                            j.IndexerProperty<string>("RoleId").HasMaxLength(128);
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId })
                    .HasName("PK_dbo.AspNetUserLogins");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.Property(e => e.ProviderDisplayName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });     
            modelBuilder.Entity<CompanyLookup>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CompanyLookupValue>(entity =>
            {
                entity.Property(e => e.ValueAr)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ValueEn)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.CompanyLookup)
                    .WithMany(p => p.CompanyLookupValues)
                    .HasForeignKey(d => d.CompanyLookupId)
                    .HasConstraintName("FK_CompanyLookupsValues_CompanyLookups");
            });
              

            modelBuilder.Entity<Dbexception>(entity =>
            {
                entity.HasKey(e => e.ExceptionId)
                    .HasName("PK_Exceptions");

                entity.ToTable("DBExceptions");

                entity.Property(e => e.Details).HasColumnType("text");

                entity.Property(e => e.ExceptionDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Subject)
                    .HasMaxLength(4000)
                    .IsUnicode(false);
            });
              
            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Isocode3)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ISOCode3");

                entity.Property(e => e.NameAr)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LocationsBack>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LocationsBack");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Isocode3)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ISOCode3");

                entity.Property(e => e.NameAr)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LocationsBackup>(entity =>
            {
                entity.ToTable("LocationsBackup");

                entity.Property(e => e.Isocode3)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ISOCode3");

                entity.Property(e => e.NameAr)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Lookup>(entity =>
            {
                entity.Property(e => e.CanChange)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LookupValue>(entity =>
            {
                entity.Property(e => e.CanChange)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ValueAr)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ValueEn)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Visible).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Lookup)
                    .WithMany(p => p.LookupValues)
                    .HasForeignKey(d => d.LookupId)
                    .HasConstraintName("FK_LookupValues_LookUps");
            });

            modelBuilder.Entity<MediaFile>(entity =>
            {
                entity.Property(e => e.CaptionAr)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CaptionEn)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreationDate).HasColumnType("smalldatetime");

                entity.Property(e => e.FileName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FilePath).IsUnicode(false);

                entity.Property(e => e.LanguageId)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UserProfileId).HasMaxLength(128);

                entity.Property(e => e.YouTubePath).IsUnicode(false);
            });

            modelBuilder.Entity<MobileCustomMenu>(entity =>
            {
                entity.Property(e => e.Details).IsUnicode(false);

                entity.Property(e => e.DetailsAr).IsUnicode(false);

                entity.Property(e => e.IconUrl)
                    .IsUnicode(false)
                    .HasColumnName("IconURL");

                entity.Property(e => e.Label)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Link).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.NameAr).IsUnicode(false);

                entity.Property(e => e.Summary).IsUnicode(false);

                entity.Property(e => e.SummaryAr).IsUnicode(false);
            });
                    

            modelBuilder.Entity<SystemParameter>(entity =>
            {
                entity.Property(e => e.DateValue).HasColumnType("datetime");

                entity.Property(e => e.DecimalValue).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.Name).HasMaxLength(400);
            });

            modelBuilder.Entity<TextVar>(entity =>
            {
                entity.Property(e => e.DataAr).IsUnicode(false);

                entity.Property(e => e.DataEn).IsUnicode(false);

                entity.Property(e => e.LinkAr)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LinkEn)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TextKey)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserLog>(entity =>
            {
                entity.Property(e => e.Date)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.TableName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserLogs__UserId__2B947552");
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.Property(e => e.CanAdd).HasDefaultValueSql("((0))");

                entity.Property(e => e.CanDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.CanEdit).HasDefaultValueSql("((0))");

                entity.Property(e => e.CanView).HasDefaultValueSql("((0))");

                entity.Property(e => e.RoleId).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserPermissions)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__UserPermi__RoleI__2C88998B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPermissions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserPermi__UserI__2D7CBDC4");
            });

            modelBuilder.Entity<ViewArea>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_Areas");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.NameAr)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ViewCity>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_Cities");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.NameAr)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ViewCountry>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_Countries");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.NameAr)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
