using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Budget_Tracker_WebAPI.Models;

public partial class Db15765Context : DbContext
{
    public Db15765Context()
    {
    }

    public Db15765Context(DbContextOptions<Db15765Context> options)
        : base(options)
    {
    }

    public virtual DbSet<TransactionCategoryMaster> TransactionCategoryMasters { get; set; }

    public virtual DbSet<TransactionMaster> TransactionMasters { get; set; }

    public virtual DbSet<TransactionPaymentMode> TransactionPaymentModes { get; set; }

    public virtual DbSet<TransactionTypeMaster> TransactionTypeMasters { get; set; }

    public virtual DbSet<UserMaster> UserMasters { get; set; }

    public virtual DbSet<UserRoleMaster> UserRoleMasters { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<TransactionCategoryMaster>(entity =>
        {
            entity.HasKey(e => e.TransactionCategoryMasterId).HasName("PRIMARY");

            entity.ToTable("transaction_category_master");

            entity.HasIndex(e => e.UserMasterId, "user_master_id_idx");

            entity.Property(e => e.TransactionCategoryMasterId)
                .HasColumnType("int(11)")
                .HasColumnName("transaction_category_master_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)")
                .HasColumnName("is_active");
            entity.Property(e => e.TransactionCategoryName)
                .HasMaxLength(45)
                .HasColumnName("transaction_category_name");
            entity.Property(e => e.UserMasterId).HasColumnName("user_master_id");

            entity.HasOne(d => d.UserMaster).WithMany(p => p.TransactionCategoryMasters)
                .HasForeignKey(d => d.UserMasterId)
                .HasConstraintName("user_master_id");
        });

        modelBuilder.Entity<TransactionMaster>(entity =>
        {
            entity.HasKey(e => e.TransactionMasterId).HasName("PRIMARY");

            entity.ToTable("transaction_master");

            entity.HasIndex(e => e.TransactionCategoryMasterId, "transaction_category_master_idx");

            entity.HasIndex(e => e.TransactionPaymentmodeId, "transaction_paymernt_mode_idx");

            entity.HasIndex(e => e.TransactionTypeMasterId, "transaction_type_master_idx");

            entity.HasIndex(e => e.UserId, "user_master_idx");

            entity.Property(e => e.TransactionMasterId).HasColumnName("transaction_master_id");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasColumnType("tinyint(4)")
                .HasColumnName("isActive");
            entity.Property(e => e.TransactionAmount).HasColumnName("transaction_amount");
            entity.Property(e => e.TransactionCategoryMasterId)
                .HasColumnType("int(11)")
                .HasColumnName("transaction_category_master_id");
            entity.Property(e => e.TransactionDate)
                .HasColumnType("datetime")
                .HasColumnName("transaction_date");
            entity.Property(e => e.TransactionDescription)
                .HasMaxLength(145)
                .HasColumnName("transaction_description");
            entity.Property(e => e.TransactionNote).HasColumnName("transaction_note");
            entity.Property(e => e.TransactionPaymentmodeId)
                .HasColumnType("int(11)")
                .HasColumnName("transaction_paymentmodeId");
            entity.Property(e => e.TransactionTypeMasterId)
                .HasColumnType("int(11)")
                .HasColumnName("transaction_type_master_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.TransactionCategoryMaster).WithMany(p => p.TransactionMasters)
                .HasForeignKey(d => d.TransactionCategoryMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transaction_category_master");

            entity.HasOne(d => d.TransactionPaymentmode).WithMany(p => p.TransactionMasters)
                .HasForeignKey(d => d.TransactionPaymentmodeId)
                .HasConstraintName("transaction_paymernt_mode");

            entity.HasOne(d => d.TransactionTypeMaster).WithMany(p => p.TransactionMasters)
                .HasForeignKey(d => d.TransactionTypeMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transaction_type_master");

            entity.HasOne(d => d.User).WithMany(p => p.TransactionMasters)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_master");
        });

        modelBuilder.Entity<TransactionPaymentMode>(entity =>
        {
            entity.HasKey(e => e.PaymentModeId).HasName("PRIMARY");

            entity.ToTable("transaction_payment_mode");

            entity.HasIndex(e => e.UserMasterId, "usermasterID_FK_idx");

            entity.Property(e => e.PaymentModeId)
                .HasColumnType("int(11)")
                .HasColumnName("paymentModeId");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)");
            entity.Property(e => e.PaymentMode)
                .HasMaxLength(45)
                .HasColumnName("paymentMode");
            entity.Property(e => e.UserMasterId).HasColumnName("userMasterId");

            entity.HasOne(d => d.UserMaster).WithMany(p => p.TransactionPaymentModes)
                .HasForeignKey(d => d.UserMasterId)
                .HasConstraintName("usermasterID_FK");
        });

        modelBuilder.Entity<TransactionTypeMaster>(entity =>
        {
            entity.HasKey(e => e.TransactionTypeMasterId).HasName("PRIMARY");

            entity.ToTable("transaction_type_master");

            entity.HasIndex(e => e.UserMasterId, "user_master_id_idx");

            entity.Property(e => e.TransactionTypeMasterId)
                .HasColumnType("int(11)")
                .HasColumnName("transaction_type_master_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)")
                .HasColumnName("is_active");
            entity.Property(e => e.TransactionTypename)
                .HasMaxLength(45)
                .HasColumnName("transaction_typename");
            entity.Property(e => e.UserMasterId).HasColumnName("user_master_id");

            entity.HasOne(d => d.UserMaster).WithMany(p => p.TransactionTypeMasters)
                .HasForeignKey(d => d.UserMasterId)
                .HasConstraintName("user_master_id_fk");
        });

        modelBuilder.Entity<UserMaster>(entity =>
        {
            entity.HasKey(e => e.UserMasterId).HasName("PRIMARY");

            entity.ToTable("user_master");

            entity.HasIndex(e => e.UserRoleId, "RoleIdMaster_idx");

            entity.Property(e => e.UserMasterId).HasColumnName("UserMasterID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(145);
            entity.Property(e => e.IsActive).HasColumnType("tinyint(4)");
            entity.Property(e => e.LastName).HasMaxLength(145);
            entity.Property(e => e.UserEmail).HasMaxLength(255);
            entity.Property(e => e.UserMastercol).HasMaxLength(45);
            entity.Property(e => e.UserPassword).HasMaxLength(255);
            entity.Property(e => e.UserRoleId)
                .HasColumnType("int(11)")
                .HasColumnName("UserRoleID");

            entity.HasOne(d => d.UserRole).WithMany(p => p.UserMasters)
                .HasForeignKey(d => d.UserRoleId)
                .HasConstraintName("RoleIdMaster");
        });

        modelBuilder.Entity<UserRoleMaster>(entity =>
        {
            entity.HasKey(e => e.UserRoleMasterId).HasName("PRIMARY");

            entity.ToTable("user_role_master");

            entity.Property(e => e.UserRoleMasterId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("UserRoleMasterID");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)");
            entity.Property(e => e.UserRole).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
