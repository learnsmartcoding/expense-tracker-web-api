using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Core.Entities;

public partial class ExpenseTrackerDbContext : DbContext
{
    public ExpenseTrackerDbContext()
    {
    }

    public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CreditCard> CreditCards { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<ExpenseCategory> ExpenseCategories { get; set; }

    public virtual DbSet<ExpenseType> ExpenseTypes { get; set; }

    public virtual DbSet<Family> Families { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CreditCard>(entity =>
        {
            entity.HasKey(e => e.CreditCardId).HasName("PK_CreditCard_CreditCardId");

            entity.ToTable("CreditCard");

            entity.Property(e => e.CardLastFourDigit).HasMaxLength(4);
            entity.Property(e => e.CreditCardName).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.CreditCards)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_CreditCard_UserProfile");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId).HasName("PK_Expense_ExpenseId");

            entity.ToTable("Expense");

            entity.HasIndex(e => e.UserId, "IDX_Expense_UserId");

            entity.Property(e => e.ExpenseAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExpenseDescription).HasMaxLength(500);

            entity.HasOne(d => d.CreditCard).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.CreditCardId)
                .HasConstraintName("FK_Expense_CreditCard");

            entity.HasOne(d => d.ExpenseCategory).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.ExpenseCategoryId)
                .HasConstraintName("FK_Expense_Category");

            entity.HasOne(d => d.ExpenseType).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.ExpenseTypeId)
                .HasConstraintName("FK_Expense_Type");

            entity.HasOne(d => d.User).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Expense_User");
        });

        modelBuilder.Entity<ExpenseCategory>(entity =>
        {
            entity.HasKey(e => e.ExpenseCategoryId).HasName("PK_ExpenseCategory_ExpenseCategoryId");

            entity.ToTable("ExpenseCategory");

            entity.Property(e => e.ExpenseCategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<ExpenseType>(entity =>
        {
            entity.HasKey(e => e.ExpenseTypeId).HasName("PK_ExpenseType_ExpenseTypeId");

            entity.ToTable("ExpenseType");

            entity.Property(e => e.ExpenseTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Family>(entity =>
        {
            entity.HasKey(e => e.FamilyId).HasName("PK_Family_FamilyId");

            entity.ToTable("Family");

            entity.Property(e => e.FamilyName).HasMaxLength(100);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_UserProfile_UserId");

            entity.ToTable("UserProfile");

            entity.HasIndex(e => e.FamilyId, "IDX_Expense_FamilyId");

            entity.Property(e => e.AdObjId).HasMaxLength(128);
            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .HasDefaultValue("Guest");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.HasOne(d => d.Family).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.FamilyId)
                .HasConstraintName("FK_UserProfile_Family");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
