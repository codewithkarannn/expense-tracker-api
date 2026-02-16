using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Budget_Tracker_WebAPI.Models;

public partial class DbExpenseTrackerContext : DbContext
{
    public DbExpenseTrackerContext()
    {
    }

    public DbExpenseTrackerContext(DbContextOptions<DbExpenseTrackerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BudgetMaster> BudgetMasters { get; set; }

    public virtual DbSet<CurrencyMaster> CurrencyMasters { get; set; }

    public virtual DbSet<FixedTransactionMaster> FixedTransactionMasters { get; set; }

    public virtual DbSet<TransactionCategoryMaster> TransactionCategoryMasters { get; set; }

    public virtual DbSet<TransactionMaster> TransactionMasters { get; set; }

    public virtual DbSet<TransactionPaymentMode> TransactionPaymentModes { get; set; }

    public virtual DbSet<TransactionTypeMaster> TransactionTypeMasters { get; set; }

    public virtual DbSet<UserMaster> UserMasters { get; set; }

    public virtual DbSet<UserRoleMaster> UserRoleMasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=db_expense_tracker;user=root;password=Admin@1234", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.5.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<BudgetMaster>(entity =>
        {
            entity.HasKey(e => e.BudgetMasterId).HasName("PRIMARY");

            entity.UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.TransactionCategoryMasterId, "IX_BudgetMasters_TransactionCategoryMasterId");

            entity.HasIndex(e => e.UserMasterId, "IX_BudgetMasters_UserMasterId");

            entity.Property(e => e.BudgetMasterId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.CreatedAt).HasMaxLength(6);
            entity.Property(e => e.DeletedAt).HasMaxLength(6);
            entity.Property(e => e.ModifiedAt).HasMaxLength(6);
            entity.Property(e => e.UserId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.UserMasterId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.TransactionCategoryMaster).WithMany(p => p.BudgetMasters)
                .HasForeignKey(d => d.TransactionCategoryMasterId)
                .HasConstraintName("FK_BudgetMasters_transaction_category_master_TransactionCategor~");

            entity.HasOne(d => d.UserMaster).WithMany(p => p.BudgetMasters).HasForeignKey(d => d.UserMasterId);
        });

        modelBuilder.Entity<CurrencyMaster>(entity =>
        {
            entity.HasKey(e => e.CurrencyMasterId).HasName("PRIMARY");

            entity.UseCollation("utf8mb4_general_ci");
        });

        modelBuilder.Entity<FixedTransactionMaster>(entity =>
        {
            entity.HasKey(e => e.FixedTransactionMasterId).HasName("PRIMARY");

            entity
                .ToTable("fixed_transaction_master")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.TransactionPaymentmodeId, "TransactioPayment_FK_idx");

            entity.HasIndex(e => e.TransactionCategoryMasterId, "TransactionCat_FK_idx");

            entity.HasIndex(e => e.TransactionTypeMasterId, "TransactionType_FK_idx");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValueSql("'1'");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionAmount).HasPrecision(18, 2);
            entity.Property(e => e.TransactionDescription).HasMaxLength(255);
            entity.Property(e => e.TransactionNote).HasMaxLength(500);
            entity.Property(e => e.UserMasterId).HasColumnName("UserMasterID");

            entity.HasOne(d => d.TransactionCategoryMaster).WithMany(p => p.FixedTransactionMasters)
                .HasForeignKey(d => d.TransactionCategoryMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TransactionCat_FK");

            entity.HasOne(d => d.TransactionPaymentmode).WithMany(p => p.FixedTransactionMasters)
                .HasForeignKey(d => d.TransactionPaymentmodeId)
                .HasConstraintName("TransactioPayment_FK");

            entity.HasOne(d => d.TransactionTypeMaster).WithMany(p => p.FixedTransactionMasters)
                .HasForeignKey(d => d.TransactionTypeMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TransactionType_FK");
        });

        modelBuilder.Entity<TransactionCategoryMaster>(entity =>
        {
            entity.HasKey(e => e.TransactionCategoryMasterId).HasName("PRIMARY");

            entity
                .ToTable("transaction_category_master")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.TransactionTypeMasterId, "IX_transaction_category_master_TransactionTypeMasterId");

            entity.HasIndex(e => e.UserMasterId, "user_master_id_idx");

            entity.Property(e => e.TransactionCategoryMasterId).HasColumnName("transaction_category_master_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.TransactionCategoryName)
                .HasMaxLength(45)
                .HasColumnName("transaction_category_name");
            entity.Property(e => e.UserMasterId)
                .HasColumnName("user_master_id")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.TransactionTypeMaster).WithMany(p => p.TransactionCategoryMasters)
                .HasForeignKey(d => d.TransactionTypeMasterId)
                .HasConstraintName("FK_transaction_category_master_transaction_type_master_Transact~");

            entity.HasOne(d => d.UserMaster).WithMany(p => p.TransactionCategoryMasters)
                .HasForeignKey(d => d.UserMasterId)
                .HasConstraintName("user_master_id");
        });

        modelBuilder.Entity<TransactionMaster>(entity =>
        {
            entity.HasKey(e => e.TransactionMasterId).HasName("PRIMARY");

            entity
                .ToTable("transaction_master")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.TransactionCategoryMasterId, "transaction_category_master_idx");

            entity.HasIndex(e => e.TransactionPaymentmodeId, "transaction_paymernt_mode_idx");

            entity.HasIndex(e => e.TransactionTypeMasterId, "transaction_type_master_idx");

            entity.HasIndex(e => e.UserId, "user_master_idx");

            entity.Property(e => e.TransactionMasterId)
                .HasColumnName("transaction_master_id")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.TransactionAmount).HasColumnName("transaction_amount");
            entity.Property(e => e.TransactionCategoryMasterId).HasColumnName("transaction_category_master_id");
            entity.Property(e => e.TransactionDate)
                .HasColumnType("datetime")
                .HasColumnName("transaction_date");
            entity.Property(e => e.TransactionDescription)
                .HasMaxLength(145)
                .HasColumnName("transaction_description");
            entity.Property(e => e.TransactionNote).HasColumnName("transaction_note");
            entity.Property(e => e.TransactionPaymentmodeId).HasColumnName("transaction_paymentmodeId");
            entity.Property(e => e.TransactionTypeMasterId).HasColumnName("transaction_type_master_id");
            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

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

            entity
                .ToTable("transaction_payment_mode")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.UserMasterId, "usermasterID_FK_idx");

            entity.Property(e => e.PaymentModeId).HasColumnName("paymentModeId");
            entity.Property(e => e.IsActive).HasDefaultValueSql("'1'");
            entity.Property(e => e.PaymentMode)
                .HasMaxLength(45)
                .HasColumnName("paymentMode");
            entity.Property(e => e.UserMasterId)
                .HasColumnName("userMasterId")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.UserMaster).WithMany(p => p.TransactionPaymentModes)
                .HasForeignKey(d => d.UserMasterId)
                .HasConstraintName("usermasterID_FK");
        });

        modelBuilder.Entity<TransactionTypeMaster>(entity =>
        {
            entity.HasKey(e => e.TransactionTypeMasterId).HasName("PRIMARY");

            entity
                .ToTable("transaction_type_master")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.UserMasterId, "user_master_id_idx1");

            entity.Property(e => e.TransactionTypeMasterId).HasColumnName("transaction_type_master_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.TransactionTypename)
                .HasMaxLength(45)
                .HasColumnName("transaction_typename");
            entity.Property(e => e.UserMasterId)
                .HasColumnName("user_master_id")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.UserMaster).WithMany(p => p.TransactionTypeMasters)
                .HasForeignKey(d => d.UserMasterId)
                .HasConstraintName("user_master_id_fk");
        });

        modelBuilder.Entity<UserMaster>(entity =>
        {
            entity.HasKey(e => e.UserMasterId).HasName("PRIMARY");

            entity
                .ToTable("user_master")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.CurrencyMasterId, "IX_user_master_CurrencyMasterId");

            entity.HasIndex(e => e.UserRoleId, "RoleIdMaster_idx");

            entity.Property(e => e.UserMasterId)
                .HasColumnName("UserMasterID")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(145);
            entity.Property(e => e.LastName).HasMaxLength(145);
            entity.Property(e => e.UserEmail).HasMaxLength(255);
            entity.Property(e => e.UserPassword).HasMaxLength(255);
            entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

            entity.HasOne(d => d.CurrencyMaster).WithMany(p => p.UserMasters).HasForeignKey(d => d.CurrencyMasterId);

            entity.HasOne(d => d.UserRole).WithMany(p => p.UserMasters)
                .HasForeignKey(d => d.UserRoleId)
                .HasConstraintName("RoleIdMaster");
        });

        modelBuilder.Entity<UserRoleMaster>(entity =>
        {
            entity.HasKey(e => e.UserRoleMasterId).HasName("PRIMARY");

            entity
                .ToTable("user_role_master")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.UserRoleMasterId)
                .ValueGeneratedNever()
                .HasColumnName("UserRoleMasterID");
            entity.Property(e => e.IsActive).HasDefaultValueSql("'1'");
            entity.Property(e => e.UserRole).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
