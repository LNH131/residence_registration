using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

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

    public virtual DbSet<HeadOfHouseHold> HeadOfHouseHolds { get; set; }

    public virtual DbSet<Household> Households { get; set; }

    public virtual DbSet<HouseholdMember> HouseholdMembers { get; set; }

    public virtual DbSet<HouseholdSeparation> HouseholdSeparations { get; set; }

    public virtual DbSet<HouseholdTransfer> HouseholdTransfers { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<RegistrationApproval> RegistrationApprovals { get; set; }

    public virtual DbSet<RegistrationMember> RegistrationMembers { get; set; }

    public virtual DbSet<SeparationMember> SeparationMembers { get; set; }

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
            entity.HasKey(e => e.AddressId).HasName("PK__Addresse__091C2A1B1279D83A");

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
            entity.HasKey(e => e.AreaId).HasName("PK__Areas__70B820288D0C96FE");

            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.AreaName).HasMaxLength(255);
            entity.Property(e => e.PoliceId).HasColumnName("PoliceID");

            entity.HasOne(d => d.Police).WithMany(p => p.Areas)
                .HasForeignKey(d => d.PoliceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Areas__PoliceID__72C60C4A");
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

        modelBuilder.Entity<HeadOfHouseHold>(entity =>
        {
            entity.HasKey(e => e.HeadOfHouseHoldId).HasName("PK__HeadOfHo__2D41F5BFBC67790B");

            entity.ToTable("HeadOfHouseHold");

            entity.HasIndex(e => e.HouseholdId, "UK_HeadOfHouseHold_HouseholdID").IsUnique();

            entity.Property(e => e.HeadOfHouseHoldId).HasColumnName("HeadOfHouseHoldID");
            entity.Property(e => e.HouseholdId).HasColumnName("HouseholdID");
            entity.Property(e => e.RegisteredDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Household).WithOne(p => p.HeadOfHouseHold)
                .HasForeignKey<HeadOfHouseHold>(d => d.HouseholdId)
                .HasConstraintName("FK_HeadOfHouseHold_Households");

            entity.HasOne(d => d.User).WithMany(p => p.HeadOfHouseHolds)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HeadOfHouseHold_Users");
        });

        modelBuilder.Entity<Household>(entity =>
        {
            entity.HasKey(e => e.HouseholdId).HasName("PK__Househol__1453D6EC6BE3D12B");

            entity.Property(e => e.HouseholdId).HasColumnName("HouseholdID");
            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Address).WithMany(p => p.Households)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__Addre__797309D9");
        });

        modelBuilder.Entity<HouseholdMember>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__Househol__0CF04B38A7E25A79");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.HouseholdId).HasColumnName("HouseholdID");
            entity.Property(e => e.Relationship)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Household).WithMany(p => p.HouseholdMembers)
                .HasForeignKey(d => d.HouseholdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__House__778AC167");

            entity.HasOne(d => d.User).WithMany(p => p.HouseholdMembers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__UserI__787EE5A0");
        });

        modelBuilder.Entity<HouseholdSeparation>(entity =>
        {
            entity.HasKey(e => e.SeparationId).HasName("PK__Househol__3850A0BC4C13F989");

            entity.Property(e => e.SeparationId).HasColumnName("SeparationID");
            entity.Property(e => e.ApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.Comments).HasColumnType("text");
            entity.Property(e => e.NewHouseholdId).HasColumnName("NewHouseholdID");
            entity.Property(e => e.OriginalHouseholdId).HasColumnName("OriginalHouseholdID");
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.HouseholdSeparations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK_HouseholdSeparations_ApprovedBy");

            entity.HasOne(d => d.NewHousehold).WithMany(p => p.HouseholdSeparationNewHouseholds)
                .HasForeignKey(d => d.NewHouseholdId)
                .HasConstraintName("FK_HouseholdSeparations_NewHousehold");

            entity.HasOne(d => d.OriginalHousehold).WithMany(p => p.HouseholdSeparationOriginalHouseholds)
                .HasForeignKey(d => d.OriginalHouseholdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HouseholdSeparations_OriginalHousehold");
        });

        modelBuilder.Entity<HouseholdTransfer>(entity =>
        {
            entity.HasKey(e => e.TransferId).HasName("PK__Househol__9549017123D50A8A");

            entity.Property(e => e.TransferId).HasColumnName("TransferID");
            entity.Property(e => e.Comments).HasColumnType("text");
            entity.Property(e => e.FromAreaId).HasColumnName("FromAreaID");
            entity.Property(e => e.HouseholdId).HasColumnName("HouseholdID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.ToAreaId).HasColumnName("ToAreaID");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.HouseholdTransfers)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__Household__Appro__7E37BEF6");

            entity.HasOne(d => d.FromArea).WithMany(p => p.HouseholdTransferFromAreas)
                .HasForeignKey(d => d.FromAreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__FromA__7F2BE32F");

            entity.HasOne(d => d.Household).WithMany(p => p.HouseholdTransfers)
                .HasForeignKey(d => d.HouseholdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__House__00200768");

            entity.HasOne(d => d.ToArea).WithMany(p => p.HouseholdTransferToAreas)
                .HasForeignKey(d => d.ToAreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__ToAre__01142BA1");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Logs__5E5499A86B49F00C");

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
                .HasConstraintName("FK__Logs__UserID__02084FDA");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32FAF46AA4");

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
            entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__6EF58830703B367B");

            entity.Property(e => e.RegistrationId).HasColumnName("RegistrationID");
            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.Comments).HasColumnType("text");
            entity.Property(e => e.RegistrationType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Address).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Addre__06CD04F7");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.RegistrationApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__Registrat__Appro__07C12930");

            entity.HasOne(d => d.User).WithMany(p => p.RegistrationUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__UserI__08B54D69");
        });

        modelBuilder.Entity<RegistrationApproval>(entity =>
        {
            entity.HasKey(e => e.ApprovalId).HasName("PK__Registra__328477D4F40E3795");

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
                .HasConstraintName("FK__Registrat__Appro__03F0984C");

            entity.HasOne(d => d.Registration).WithMany(p => p.RegistrationApprovals)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Regis__04E4BC85");
        });

        modelBuilder.Entity<RegistrationMember>(entity =>
        {
            entity.HasKey(e => e.RegistrationMemberId).HasName("PK__Registra__767EDBB23CFF7A86");

            entity.Property(e => e.RegistrationMemberId).HasColumnName("RegistrationMemberID");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IdentityCard)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationId).HasColumnName("RegistrationID");
            entity.Property(e => e.Relationship)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sex)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Registration).WithMany(p => p.RegistrationMembers)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Regis__05D8E0BE");
        });

        modelBuilder.Entity<SeparationMember>(entity =>
        {
            entity.Property(e => e.SeparationMemberId).HasColumnName("SeparationMemberID");
            entity.Property(e => e.NewRelationship)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SeparationId).HasColumnName("SeparationID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Separation).WithMany(p => p.SeparationMembers)
                .HasForeignKey(d => d.SeparationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeparationMembers_HouseholdSeparations");

            entity.HasOne(d => d.User).WithMany(p => p.SeparationMembers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeparationMembers_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC1D7AFD72");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534EEFD3838").IsUnique();

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
            entity.HasKey(e => e.ContactId).HasName("PK__UserCont__5C6625BBB4E9198F");

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
                .HasConstraintName("FK__UserConta__UserI__0B91BA14");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
