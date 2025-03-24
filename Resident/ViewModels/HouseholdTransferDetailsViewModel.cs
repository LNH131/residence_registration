using Resident.Enums;
using Resident.Models;
using Resident.Service;
using System.Windows;
using System.Windows.Input;

public class HouseholdTransferDetailsViewModel : BaseViewModel
{
    private HouseholdTransfer _transfer;
    public HouseholdTransfer Transfer
    {
        get => _transfer;
        set { _transfer = value; OnPropertyChanged(nameof(Transfer)); }
    }

    public ICommand ApproveCommand { get; }
    public ICommand RejectCommand { get; }

    public HouseholdTransferDetailsViewModel(HouseholdTransfer transfer)
    {
        Transfer = transfer;

        ApproveCommand = new LocalRelayCommand(o => ApproveTransfer());
        RejectCommand = new LocalRelayCommand(o => RejectTransfer());
    }

    private void ApproveTransfer()
    {
        try
        {
            // 1) If currently Pending => area leader sets status = ApprovedByLeader
            if (Transfer.Status == Status.Pending.ToString())
            {
                Transfer.Status = Status.ApprovedByLeader.ToString();
                UpdateTransfer(Transfer);
                MessageBox.Show($"Chuyển hộ ID={Transfer.TransferId} đã được duyệt sơ bộ (Area Leader).");
            }
            // 2) If currently ApprovedByLeader => final police approval => update household address
            else if (Transfer.Status == Status.ApprovedByLeader.ToString())
            {
                using (var context = new PrnContext())
                {
                    var dbTransfer = context.HouseholdTransfers
                        .FirstOrDefault(t => t.TransferId == Transfer.TransferId);
                    if (dbTransfer == null)
                        throw new Exception("Transfer not found in DB.");

                    // 2a) Update the household's AddressId to the "ToAddressID".
                    var dbHousehold = context.Households
                        .FirstOrDefault(h => h.HouseholdId == dbTransfer.HouseholdId);
                    if (dbHousehold == null)
                        throw new Exception("Household not found in DB.");

                    dbHousehold.AddressId = dbTransfer.ToAddressId;
                    context.Households.Update(dbHousehold);

                    // 2b) Mark the transfer as fully Approved
                    dbTransfer.Status = Status.Approved.ToString();
                    dbTransfer.ApprovedBy = /* policeman’s userId if you have it */ null;
                    dbTransfer.Comments = "Transfer completed by police.";
                    context.HouseholdTransfers.Update(dbTransfer);

                    context.SaveChanges();

                    // Update local copy
                    Transfer.Status = Status.Approved.ToString();
                }

                MessageBox.Show($"Chuyển hộ ID={Transfer.TransferId} đã được phê duyệt (Police). " +
                                "Địa chỉ hộ đã được cập nhật!");
            }
            else
            {
                MessageBox.Show($"Không thể phê duyệt chuyển hộ ở trạng thái {Transfer.Status}.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error approving household transfer: " + ex.Message);
        }
    }

    private void RejectTransfer()
    {
        try
        {
            Transfer.Status = Status.Rejected.ToString();
            UpdateTransfer(Transfer);
            MessageBox.Show($"Chuyển hộ ID={Transfer.TransferId} đã bị từ chối.");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error rejecting household transfer: " + ex.Message);
        }
    }

    private void UpdateTransfer(HouseholdTransfer transfer)
    {
        using (var context = new PrnContext())
        {
            context.HouseholdTransfers.Update(transfer);
            context.SaveChanges();
        }
    }
}
