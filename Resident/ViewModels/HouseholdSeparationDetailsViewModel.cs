using Resident.Enums;
using Resident.Models;
using Resident.Service;
using System.Windows;
using System.Windows.Input;

namespace Resident.ViewModels
{
    public class HouseholdSeparationDetailsViewModel : BaseViewModel
    {
        private HouseholdSeparation _separation;
        public HouseholdSeparation Separation
        {
            get => _separation;
            set { _separation = value; OnPropertyChanged(); }
        }

        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

        private readonly HouseholdSeparationService _separationService;

        public HouseholdSeparationDetailsViewModel(HouseholdSeparation separation)
        {
            Separation = separation;
            _separationService = new HouseholdSeparationService(); // or inject via DI

            ApproveCommand = new LocalRelayCommand(_ => ApproveSeparation());
            RejectCommand = new LocalRelayCommand(_ => RejectSeparation());
        }

        private void ApproveSeparation()
        {
            // On final approval by Police: move members from original household to new household
            // or create new household if needed. Then set Status = Approved.
            try
            {
                Separation.Status = Status.Approved.ToString();
                _separationService.UpdateHouseholdSeparation(Separation);
                MessageBox.Show($"Tách hộ ID = {Separation.SeparationId} đã được phê duyệt.\nCác thành viên đã được cập nhật.");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error approving separation: " + ex.Message);
            }
        }

        private void RejectSeparation()
        {
            try
            {
                Separation.Status = Status.Rejected.ToString();
                _separationService.UpdateHouseholdSeparation(Separation);
                MessageBox.Show($"Tách hộ ID = {Separation.SeparationId} đã bị từ chối.");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error rejecting separation: " + ex.Message);
            }
        }
    }
}
