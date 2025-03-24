using Microsoft.EntityFrameworkCore;
using Resident.Models;

namespace Resident.Services
{
    public interface IHouseholdService
    {
        Task<HouseholdDetailViewModel> GetHouseholdDetailAsync(int householdId);
        Task<IEnumerable<HouseholdDetailViewModel>> GetAllHouseholdsAsync();
    }

    public class HouseholdService : IHouseholdService
    {
        private readonly PrnContext _context;

        public HouseholdService(PrnContext context)
        {
            _context = context;
        }

        public async Task<HouseholdDetailViewModel> GetHouseholdDetailAsync(int householdId)
        {
            var household = await _context.Households
                .Include(h => h.Address)
                .Include(h => h.HouseholdMembers)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(h => h.HouseholdId == householdId);

            if (household == null)
                return null;

            var viewModel = new HouseholdDetailViewModel
            {
                HouseholdId = household.HouseholdId,
                CreatedDate = household.CreatedDate,
                Address = new AddressViewModel
                {
                    AddressId = household.Address.AddressId,
                    Street = household.Address.Street,
                    City = household.Address.City,
                    State = household.Address.State,
                    ZipCode = household.Address.ZipCode,
                    Country = household.Address.Country,
                    Ward = household.Address.Ward,
                    District = household.Address.District
                },
                Members = household.HouseholdMembers.Select(m => new HouseholdMemberViewModel
                {
                    MemberId = m.MemberId,
                    UserId = m.UserId,
                    UserName = m.User.FullName,
                    Relationship = m.Relationship
                }).ToList()
            };

            return viewModel;
        }

        public async Task<IEnumerable<HouseholdDetailViewModel>> GetAllHouseholdsAsync()
        {
            var households = await _context.Households
                .Include(h => h.Address)
                .Include(h => h.HouseholdMembers)
                    .ThenInclude(m => m.User)
                .ToListAsync();

            return households.Select(h => new HouseholdDetailViewModel
            {
                HouseholdId = h.HouseholdId,
                CreatedDate = h.CreatedDate,
                Address = new AddressViewModel
                {
                    AddressId = h.Address.AddressId,
                    Street = h.Address.Street,
                    City = h.Address.City,
                    State = h.Address.State,
                    ZipCode = h.Address.ZipCode,
                    Country = h.Address.Country,
                    Ward = h.Address.Ward,
                    District = h.Address.District
                },
                Members = h.HouseholdMembers.Select(m => new HouseholdMemberViewModel
                {
                    MemberId = m.MemberId,
                    UserId = m.UserId,
                    UserName = m.User.FullName,
                    Relationship = m.Relationship
                }).ToList()
            });
        }
    }

    // View Models to support data binding in your MVVM layer
    public class HouseholdDetailViewModel
    {
        public int HouseholdId { get; set; }
        public DateOnly? CreatedDate { get; set; }
        public AddressViewModel Address { get; set; }
        public List<HouseholdMemberViewModel> Members { get; set; }
    }

    public class AddressViewModel
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
    }

    public class HouseholdMemberViewModel
    {
        public int MemberId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Relationship { get; set; }
    }
}
