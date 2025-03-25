using Microsoft.EntityFrameworkCore;
using Resident.Models;
using Resident.Service;
using System.Collections.ObjectModel;

namespace Resident.ViewModels
{
    public class HouseholdMemberDetailsViewModel : BaseViewModel
    {
        private ObservableCollection<HouseholdMember> _householdMembers;
        public ObservableCollection<HouseholdMember> HouseholdMembers
        {
            get => _householdMembers;
            set { _householdMembers = value; OnPropertyChanged(nameof(HouseholdMembers)); }
        }

        public int HouseholdId { get; set; }

        public HouseholdMemberDetailsViewModel(int householdId)
        {
            HouseholdId = householdId;
            LoadMembers();
        }

        private void LoadMembers()
        {
            using (var context = new PrnContext())
            {
                var members = context.HouseholdMembers


                    .Include(m => m.User)
                    .Where(m => m.HouseholdId == HouseholdId)
                    .ToList();
                HouseholdMembers = new ObservableCollection<HouseholdMember>(members);
            }
        }
    }
}
