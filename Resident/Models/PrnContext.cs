using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<RegistrationApproval> RegistrationApprovals { get; set; }

    public virtual DbSet<RegistrationMember> RegistrationMembers { get; set; }

    public virtual DbSet<SeparationMember> SeparationMembers { get; set; }

    public virtual DbSet<User> Users { get; set; }

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
            entity.HasKey(e => e.AddressId).HasName("PK__Addresse__091C2A1BD72EDA75");

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
            entity.HasKey(e => e.AreaId).HasName("PK__Areas__70B820286F49B948");

            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.AreaName).HasMaxLength(255);
            entity.Property(e => e.PoliceId).HasColumnName("PoliceID");

            entity.HasOne(d => d.Police).WithMany(p => p.Areas)
                .HasForeignKey(d => d.PoliceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Areas__PoliceID__6E01572D");
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
            entity.HasKey(e => e.HeadOfHouseHoldId).HasName("PK__HeadOfHo__2D41F5BFF4460BEF");

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
            entity.HasKey(e => e.HouseholdId).HasName("PK__Househol__1453D6ECAD1F8B9E");

            entity.Property(e => e.HouseholdId).HasColumnName("HouseholdID");
            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Address).WithMany(p => p.Households)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__Addre__74AE54BC");
        });

        modelBuilder.Entity<HouseholdMember>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__Househol__0CF04B38F8ED890B");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.HouseholdId).HasColumnName("HouseholdID");
            entity.Property(e => e.Relationship)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Household).WithMany(p => p.HouseholdMembers)
                .HasForeignKey(d => d.HouseholdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__House__72C60C4A");

            entity.HasOne(d => d.User).WithMany(p => p.HouseholdMembers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__UserI__73BA3083");
        });

        modelBuilder.Entity<HouseholdSeparation>(entity =>
        {
            entity.HasKey(e => e.SeparationId).HasName("PK__Househol__3850A0BC3DC23E4B");

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
            entity.HasKey(e => e.TransferId).HasName("PK__Househol__95490171E2930332");

            entity.Property(e => e.TransferId).HasColumnName("TransferID");
            entity.Property(e => e.Comments).HasColumnType("text");
            entity.Property(e => e.FromAddressId).HasColumnName("FromAddressID");
            entity.Property(e => e.HouseholdId).HasColumnName("HouseholdID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.ToAddressId).HasColumnName("ToAddressID");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.HouseholdTransfers)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__Household__Appro__787EE5A0");

            entity.HasOne(d => d.FromAddress).WithMany(p => p.HouseholdTransferFromAddresses)
                .HasForeignKey(d => d.FromAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HouseholdTransfers_FromAddressID");

            entity.HasOne(d => d.Household).WithMany(p => p.HouseholdTransfers)
                .HasForeignKey(d => d.HouseholdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Household__House__797309D9");

            entity.HasOne(d => d.ToAddress).WithMany(p => p.HouseholdTransferToAddresses)
                .HasForeignKey(d => d.ToAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HouseholdTransfers_ToAddressID");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E328B491521");

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
            entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__6EF5883021091C95");

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
                .HasConstraintName("FK__Registrat__Addre__00200768");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.RegistrationApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__Registrat__Appro__01142BA1");

            entity.HasOne(d => d.User).WithMany(p => p.RegistrationUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__UserI__02084FDA");
        });

        modelBuilder.Entity<RegistrationApproval>(entity =>
        {
            entity.HasKey(e => e.ApprovalId).HasName("PK__Registra__328477D4571A37A2");

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
                .HasConstraintName("FK__Registrat__Appro__7D439ABD");

            entity.HasOne(d => d.Registration).WithMany(p => p.RegistrationApprovals)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Regis__7E37BEF6");
        });

        modelBuilder.Entity<RegistrationMember>(entity =>
        {
            entity.HasKey(e => e.RegistrationMemberId).HasName("PK__Registra__767EDBB2E9A93794");

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
                .HasConstraintName("FK__Registrat__Regis__7F2BE32F");
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
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC999C3A9C");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534D719E733").IsUnique();

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
