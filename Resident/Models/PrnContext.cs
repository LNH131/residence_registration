using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Resident.Enums;

namespace Resident.Models;

public partial class PrnContext : DbContext
{
    public PrnContext()
    {
    }

    public PrnContext(DbContextOptions<PrnContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<HeadOfHouseHold> HeadOfHouseHolds { get; set; }

    public virtual DbSet<Household> Households { get; set; }

    public virtual DbSet<HouseholdMember> HouseholdMembers { get; set; }

    public virtual DbSet<HouseholdTransfer> HouseholdTransfers { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<RegistrationApproval> RegistrationApprovals { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserContact> UserContacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder(); //Microsoft.Extensions...
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        var configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Addresse__091C2A1B72CD08DE");

            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.District)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Street)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Ward)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ZipCode)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK__Areas__70B82028FA02F815");

            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.AreaName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PoliceId).HasColumnName("PoliceID");

            entity.HasOne(d => d.Police).WithMany(p => p.Areas)
                .HasForeignKey(d => d.PoliceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Areas__PoliceID__5CD6CB2B");
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId);

            entity.ToTable("ChatMessage");

            entity.Property(e => e.Content).HasMaxLength(500);
            entity.Property(e => e.SentDate).HasColumnType("datetime");

            entity.HasOne(d => d.FromUser).WithMany(p => p.ChatMessageFromUsers)
                .HasForeignKey(d => d.FromUserId)
                .HasConstraintName("FK_ChatMessage_Users");

            entity.HasOne(d => d.ToUser).WithMany(p => p.ChatMessageToUsers)
                .HasForeignKey(d => d.ToUserId)
                .HasConstraintName("FK_ChatMessage_Users1");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__Document__1ABEEF6F080406AB");

            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.FilePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationId).HasColumnName("RegistrationID");
            entity.Property(e => e.UploadDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Documents__Docum__5FB337D6");

            entity.HasOne(d => d.Registration).WithMany(p => p.Documents)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Documents__Regis__60A75C0F");
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.HasKey(e => e.DocumentTypeId).HasName("PK__Document__DBA390E1D699A8D3");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HeadOfHouseHold>(entity =>
        {
            entity.HasKey(e => e.HeadOfHouseHoldId).HasName("PK__HeadOfHo__2D41F5BF599904B0");

            entity.ToTable("HeadOfHouseHold");

            entity.Property(e => e.HeadOfHouseHoldId).HasColumnName("HeadOfHouseHoldID");
            entity.Property(e => e.HouseholdId).HasColumnName("HouseholdID");
            entity.Property(e => e.RegisteredDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Household).WithMany(p => p.HeadOfHouseHolds)
                .HasForeignKey(d => d.HouseholdId)
                .HasConstraintName("FK_HeadOfHouseHold_Households");

            entity.HasOne(d => d.User).WithMany(p => p.HeadOfHouseHolds)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HeadOfHouseHold_Users");
        });

        modelBuilder.Entity<Household>(entity =>
        {
            entity.HasKey(e => e.HouseholdId).HasName("PK__Househol__1453D6EC24FCA4A3");

            entity.Property(e => e.HouseholdId).HasColumnName("HouseholdID");
            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.HeadId).HasColumnName("HeadID");

            entity.HasOne(d => d.Address).WithMany(p => p.Households)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__Addre__656C112C");

            entity.HasOne(d => d.Head).WithMany(p => p.Households)
                .HasForeignKey(d => d.HeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Households_HeadOfHouseHold");
        });

        modelBuilder.Entity<HouseholdMember>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__Househol__0CF04B38C8CC06E9");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.HouseholdId).HasColumnName("HouseholdID");
            entity.Property(e => e.Relationship)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Household).WithMany(p => p.HouseholdMembers)
                .HasForeignKey(d => d.HouseholdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__House__6383C8BA");

            entity.HasOne(d => d.User).WithMany(p => p.HouseholdMembers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__UserI__6477ECF3");
        });

        modelBuilder.Entity<HouseholdTransfer>(entity =>
        {
            entity.HasKey(e => e.TransferId).HasName("PK__Househol__95490171516D209F");

            entity.Property(e => e.TransferId).HasColumnName("TransferID");
            entity.Property(e => e.Comments).HasColumnType("text");
            entity.Property(e => e.FromAreaId).HasColumnName("FromAreaID");
            entity.Property(e => e.HouseholdId).HasColumnName("HouseholdID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue(Status.Pending);
            entity.Property(e => e.ToAreaId).HasColumnName("ToAreaID");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.HouseholdTransfers)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__Household__Appro__6754599E");

            entity.HasOne(d => d.FromArea).WithMany(p => p.HouseholdTransferFromAreas)
                .HasForeignKey(d => d.FromAreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__FromA__68487DD7");

            entity.HasOne(d => d.Household).WithMany(p => p.HouseholdTransfers)
                .HasForeignKey(d => d.HouseholdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__House__693CA210");

            entity.HasOne(d => d.ToArea).WithMany(p => p.HouseholdTransferToAreas)
                .HasForeignKey(d => d.ToAreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__ToAre__6A30C649");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Logs__5E5499A8F7F9081F");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.EntityType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Logs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Logs__UserID__6B24EA82");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E326EC11185");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Message).HasColumnType("text");
            entity.Property(e => e.SentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notifications_Users");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__6EF58830852E35DA");

            entity.Property(e => e.RegistrationId).HasColumnName("RegistrationID");
            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.Comments).HasColumnType("text");
            entity.Property(e => e.RegistrationType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue(Status.Pending);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Address).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Addre__6EF57B66");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.RegistrationApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__Registrat__Appro__6FE99F9F");

            entity.HasOne(d => d.User).WithMany(p => p.RegistrationUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__UserI__70DDC3D8");
        });

        modelBuilder.Entity<RegistrationApproval>(entity =>
        {
            entity.HasKey(e => e.ApprovalId).HasName("PK__Registra__328477D471BF78C6");

            entity.Property(e => e.ApprovalId).HasColumnName("ApprovalID");
            entity.Property(e => e.ApprovalDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ApprovalStep)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApproverId).HasColumnName("ApproverID");
            entity.Property(e => e.Comments).HasColumnType("text");
            entity.Property(e => e.RegistrationId).HasColumnName("RegistrationID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Approver).WithMany(p => p.RegistrationApprovals)
                .HasForeignKey(d => d.ApproverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Appro__6D0D32F4");

            entity.HasOne(d => d.Registration).WithMany(p => p.RegistrationApprovals)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Regis__6E01572D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACDB7CED6D");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534D981A92B").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.CurrentAddressId).HasColumnName("CurrentAddressID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IdentityCard)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sex)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Area).WithMany(p => p.Users)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK_Users_Areas");

            entity.HasOne(d => d.CurrentAddress).WithMany(p => p.Users)
                .HasForeignKey(d => d.CurrentAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Addresses");
        });

        modelBuilder.Entity<UserContact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__UserCont__5C6625BBB41FE34A");

            entity.Property(e => e.ContactId).HasColumnName("ContactID");
            entity.Property(e => e.ContactType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContactValue)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.UserContacts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserConta__UserI__71D1E811");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
