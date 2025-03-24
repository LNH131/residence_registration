using Microsoft.EntityFrameworkCore;
using Resident.Enums;
using Resident.Models;
using Resident.Service;
using System.Windows;
using System.Windows.Input;

public class HouseholdSeparationDetailsViewModel : BaseViewModel
{
    private HouseholdSeparation _separation;
    public HouseholdSeparation Separation
    {
        get => _separation;
        set { _separation = value; OnPropertyChanged(nameof(Separation)); }
    }

    public ICommand ApproveCommand { get; }
    public ICommand RejectCommand { get; }

    public HouseholdSeparationDetailsViewModel(HouseholdSeparation separation)
    {
        Separation = separation;

        ApproveCommand = new LocalRelayCommand(o => ApproveSeparation());
        RejectCommand = new LocalRelayCommand(o => RejectSeparation());
    }

    private void ApproveSeparation()
    {
        try
        {
            // 1) If currently Pending => preliminary approval by Area Leader
            if (Separation.Status == Status.Pending.ToString())
            {
                // Preliminary approval only: set status = ApprovedByLeader
                Separation.Status = Status.ApprovedByLeader.ToString();
                UpdateSeparation(Separation);

                MessageBox.Show($"Tách hộ ID = {Separation.SeparationId} đã được duyệt sơ bộ.\n" +
                                "Chờ xử lý cuối cùng từ phía Công an (Police).");
            }
            // 2) If currently ApprovedByLeader => final approval by Police
            else if (Separation.Status == Status.ApprovedByLeader.ToString())
            {
                using (var context = new PrnContext())
                {
                    // Reload from DB (including members)
                    var dbSep = context.HouseholdSeparations
                        .Include(s => s.SeparationMembers)
                        .FirstOrDefault(s => s.SeparationId == Separation.SeparationId);

                    if (dbSep == null)
                        throw new Exception("Separation not found in DB.");

                    // Possibly create a new household if needed
                    if (dbSep.NewHouseholdId == null)
                    {
                        var newHouse = new Household
                        {
                            // For example, you might have an AddressId in the separation or a default:
                            AddressId = 1039,
                            CreatedDate = DateOnly.FromDateTime(DateTime.Now)
                        };
                        context.Households.Add(newHouse);
                        context.SaveChanges();

                        dbSep.NewHouseholdId = newHouse.HouseholdId;
                    }

                    // Move each separation member from OriginalHousehold to the new household
                    foreach (var member in dbSep.SeparationMembers)
                    {
                        var hhMember = context.HouseholdMembers
                            .FirstOrDefault(m => m.UserId == member.UserId &&
                                                 m.HouseholdId == dbSep.OriginalHouseholdId);
                        if (hhMember != null)
                        {
                            // effectively "removing" from old by reassigning to the new household
                            hhMember.HouseholdId = dbSep.NewHouseholdId.Value;
                            context.HouseholdMembers.Update(hhMember);
                        }
                    }

                    // Mark as fully Approved
                    dbSep.Status = Status.Approved.ToString();
                    dbSep.ApprovedBy = /* policeman’s userId if you have it */ null;
                    dbSep.ApprovalDate = DateTime.Now;
                    context.HouseholdSeparations.Update(dbSep);
                    context.SaveChanges();

                    // Update local copy
                    Separation.Status = Status.Approved.ToString();
                }

                MessageBox.Show($"Tách hộ ID={Separation.SeparationId} đã được phê duyệt (Police). " +
                                "Thành viên đã di chuyển khỏi hộ cũ.");
            }
            else
            {
                // If it's already Approved or Rejected, or some unexpected status, do nothing or show error
                MessageBox.Show($"Không thể phê duyệt tách hộ ở trạng thái {Separation.Status}.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error approving household separation: " + ex.Message);
        }
    }

    private void RejectSeparation()
    {
        try
        {
            Separation.Status = Status.Rejected.ToString();
            UpdateSeparation(Separation);
            MessageBox.Show($"Tách hộ ID = {Separation.SeparationId} đã bị từ chối.");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error rejecting household separation: " + ex.Message);
        }
    }

    private void UpdateSeparation(HouseholdSeparation separation)
    {
        // A small helper to do quick updates (like status changes) 
        using (var context = new PrnContext())
        {
            context.HouseholdSeparations.Update(separation);
            context.SaveChanges();
        }
    }
}
