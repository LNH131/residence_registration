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
            set
            {
                _transfer = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanModify));
            }
        }

        // Only allow Approve/Reject if not Approved or Rejected
        public bool CanModify =>
            Transfer.Status != Status.Approved.ToString() &&
            Transfer.Status != Status.Rejected.ToString();

        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

        private readonly HouseholdTransferService _transferService;

        public HouseholdTransferDetailsViewModel(HouseholdTransfer transfer)
        {
            _transferService = new HouseholdTransferService(); // or inject if needed
            Transfer = transfer;

            ApproveCommand = new LocalRelayCommand(async _ => await ApproveTransferAsync(), _ => CanModify);
            RejectCommand = new LocalRelayCommand(async _ => await RejectTransferAsync(), _ => CanModify);
        }

        private async Task ApproveTransferAsync()
        {
            try
            {
                Transfer.Status = Status.Approved.ToString();
                _transferService.UpdateHouseholdTransfer(Transfer);

                MessageBox.Show($"Household Transfer ID = {Transfer.TransferId} approved.",
                                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                OnPropertyChanged(nameof(CanModify));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error approving transfer: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task RejectTransferAsync()
        {
            try
            {
                Transfer.Status = Status.Rejected.ToString();
                _transferService.UpdateHouseholdTransfer(Transfer);

                MessageBox.Show($"Household Transfer ID = {Transfer.TransferId} rejected.",
                                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                OnPropertyChanged(nameof(CanModify));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error rejecting transfer: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
