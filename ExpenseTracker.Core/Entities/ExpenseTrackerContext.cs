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

    public virtual DbSet<EmailCopy> EmailCopies { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<ExpenseCategory> ExpenseCategories { get; set; }

    public virtual DbSet<ExpenseItem> ExpenseItems { get; set; }

    public virtual DbSet<ExpenseType> ExpenseTypes { get; set; }

    public virtual DbSet<Family> Families { get; set; }

    public virtual DbSet<FamilyMemberRequest> FamilyMemberRequests { get; set; }

    public virtual DbSet<UserBudget> UserBudgets { get; set; }

    public virtual DbSet<UserIncome> UserIncomes { get; set; }

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

        modelBuilder.Entity<EmailCopy>(entity =>
        {
            entity.HasKey(e => e.EmailCopyId).HasName("PK_EmailCopy_EmailCopyId");

            entity.ToTable("EmailCopy");

            entity.Property(e => e.EmailFrom)
                .HasMaxLength(100)
                .HasDefaultValue("learnsmartcoding@gmail.com");
            entity.Property(e => e.EmailMessage).HasMaxLength(2000);
            entity.Property(e => e.EmailSubject).HasMaxLength(100);
            entity.Property(e => e.EmailTo).HasMaxLength(100);
            entity.Property(e => e.SentDate).HasDefaultValueSql("(getdate())");
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

        modelBuilder.Entity<ExpenseItem>(entity =>
        {
            entity.HasKey(e => e.ExpenseItemId).HasName("PK_ExpenseItem_ExpenseId");

            entity.ToTable("ExpenseItem");

            entity.Property(e => e.ExpenseAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExpenseDescription).HasMaxLength(500);

            entity.HasOne(d => d.CreditCard).WithMany(p => p.ExpenseItems)
                .HasForeignKey(d => d.CreditCardId)
                .HasConstraintName("FK_ExpenseItem_CreditCard");

            entity.HasOne(d => d.ExpenseCategory).WithMany(p => p.ExpenseItems)
                .HasForeignKey(d => d.ExpenseCategoryId)
                .HasConstraintName("FK_ExpenseItem_Category");

            entity.HasOne(d => d.Expense).WithMany(p => p.ExpenseItems)
                .HasForeignKey(d => d.ExpenseId)
                .HasConstraintName("FK_ExpenseItem_Expense");

            entity.HasOne(d => d.ExpenseType).WithMany(p => p.ExpenseItems)
                .HasForeignKey(d => d.ExpenseTypeId)
                .HasConstraintName("FK_ExpenseItem_Type");

            entity.HasOne(d => d.User).WithMany(p => p.ExpenseItems)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ExpenseItem_User");
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

        modelBuilder.Entity<FamilyMemberRequest>(entity =>
        {
            entity.HasKey(e => e.FamilyMemberRequestId).HasName("PK_FamilyMemberRequest_FamilyMemberRequestId");

            entity.ToTable("FamilyMemberRequest");

            entity.Property(e => e.FamilyEmailIds).HasMaxLength(100);
            entity.Property(e => e.UserMessage).HasMaxLength(500);

            entity.HasOne(d => d.RequestedUser).WithMany(p => p.FamilyMemberRequests)
                .HasForeignKey(d => d.RequestedUserId)
                .HasConstraintName("FK_FamilyMemberRequest_UserProfile");
        });

        modelBuilder.Entity<UserBudget>(entity =>
        {
            entity.HasKey(e => e.UserBudgetId).HasName("PK_UserBudget_UserBudgetId");

            entity.ToTable("UserBudget");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BudgetDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ItemDescription).HasMaxLength(500);
            entity.Property(e => e.ItemName).HasMaxLength(100);

            entity.HasOne(d => d.User).WithMany(p => p.UserBudgets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserBudget_UserProfile");
        });

        modelBuilder.Entity<UserIncome>(entity =>
        {
            entity.HasKey(e => e.UserIncomeId).HasName("PK_UserIncome_UserIncomeId");

            entity.ToTable("UserIncome");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IncomeDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IncomeDescription).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.UserIncomes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserIncome_UserProfile");
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
