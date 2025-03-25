using Resident.Enums;
using Resident.Models;
using Resident.Service;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class HouseholdTransferDetailsViewModel : BaseViewModel
    {
        private HouseholdTransfer _transfer;
        public HouseholdTransfer Transfer
        {
            get => _transfer;
            set { _transfer = value; OnPropertyChanged(); }
        }

        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

        private readonly HouseholdTransferService _transferService;

        public HouseholdTransferDetailsViewModel(HouseholdTransfer transfer)
        {
            Transfer = transfer;
            _transferService = new HouseholdTransferService(); // You could inject via DI

            ApproveCommand = new LocalRelayCommand(_ => ApproveTransfer());
            RejectCommand = new LocalRelayCommand(_ => RejectTransfer());
        }

        private void ApproveTransfer()
        {
            // Final approval by the Police: update household's AddressId to Transfer.ToAddressId
            // Then set status to Approved
            try
            {
                Transfer.Status = Status.Approved.ToString();
                _transferService.UpdateHouseholdTransfer(Transfer);
                MessageBox.Show($"Chuyển hộ ID = {Transfer.TransferId} đã được phê duyệt.\nĐịa chỉ hộ đã được cập nhật.");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error approving transfer: " + ex.Message);
            }
        }

        private void RejectTransfer()
        {
            try
            {
                Transfer.Status = Status.Rejected.ToString();
                _transferService.UpdateHouseholdTransfer(Transfer);
                MessageBox.Show($"Chuyển hộ ID = {Transfer.TransferId} đã bị từ chối.");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error rejecting transfer: " + ex.Message);
            }
        }
    }
}
