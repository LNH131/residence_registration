using Microsoft.EntityFrameworkCore;
using Resident.Enums;
using Resident.Models;

namespace Resident.Services
{
    public interface IPoliceProcessingService
    {
        Task ProcessRegistrationAsync(Registration registration);
        Task ProcessHouseholdTransferAsync(HouseholdTransfer transfer);
        Task ProcessHouseholdSeparationAsync(HouseholdSeparation separation);
    }

    public class PoliceProcessingService : IPoliceProcessingService
    {
        private readonly PrnContext _context;

        public PoliceProcessingService(PrnContext context)
        {
            _context = context;
        }

        public async Task ProcessRegistrationAsync(Registration registration)
        {
            // Reload registration with necessary navigation properties (RegistrationMembers, User)
            var reg = await _context.Registrations
                .Include(r => r.RegistrationMembers)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.RegistrationId == registration.RegistrationId);
            if (reg == null)
            {
                throw new Exception("Registration not found.");
            }

            // 1. Create a new Household record using the AddressId from the registration.
            var newHousehold = new Household
            {
                AddressId = reg.AddressId,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now)
            };
            _context.Households.Add(newHousehold);
            await _context.SaveChangesAsync();  // newHousehold.HouseholdId is generated here

            // 2. Create HeadOfHouseHold record for the registration's user.
            var head = new HeadOfHouseHold
            {
                UserId = reg.UserId, // or reg.User.UserId
                HouseholdId = newHousehold.HouseholdId,
                RegisteredDate = DateTime.Now
            };
            _context.HeadOfHouseHolds.Add(head);
            await _context.SaveChangesAsync();  // head.HeadOfHouseHoldId is generated

            // 3. Update the new Household to link the head of household.
            newHousehold.HeadOfHouseHold = head;
            _context.Households.Update(newHousehold);
            await _context.SaveChangesAsync();

            // 4. For each RegistrationMember, attempt to find the corresponding User (by IdentityCard)
            // and then add a HouseholdMember record.
            foreach (var rm in reg.RegistrationMembers)
            {
                User memberUser = null;
                if (!string.IsNullOrEmpty(rm.IdentityCard))
                {
                    memberUser = await _context.Users
                        .FirstOrDefaultAsync(u => u.IdentityCard == rm.IdentityCard);
                }

                // Nếu tìm thấy User tương ứng, thêm HouseholdMember.
                if (memberUser != null)
                {
                    var householdMember = new HouseholdMember
                    {
                        HouseholdId = newHousehold.HouseholdId,
                        UserId = memberUser.UserId,
                        Relationship = rm.Relationship
                    };
                    _context.HouseholdMembers.Add(householdMember);
                }
            }

            // 5. Cập nhật trạng thái của Registration thành "Approved"
            reg.Status = Status.Approved.ToString();
            _context.Registrations.Update(reg);

            // 6. Lưu tất cả các thay đổi.
            await _context.SaveChangesAsync();
        }




        public async Task ProcessHouseholdTransferAsync(HouseholdTransfer transfer)
        {
            // If the transfer already has the Household navigation property loaded, use it.
            if (transfer.Household != null)
            {
                // Update the already tracked instance.
                transfer.Household.AddressId = transfer.ToAddressId;
            }
            else
            {
                // Otherwise, attempt to retrieve it from the local cache or the database.
                var household = _context.Households.Local
                    .FirstOrDefault(h => h.HouseholdId == transfer.HouseholdId)
                    ?? await _context.Households.FirstOrDefaultAsync(h => h.HouseholdId == transfer.HouseholdId);
                if (household != null)
                {
                    household.AddressId = transfer.ToAddressId;
                }
            }

            // Update the transfer status to "Approved" (using your enum)
            transfer.Status = Status.Approved.ToString();
            _context.HouseholdTransfers.Update(transfer);

            await _context.SaveChangesAsync();
        }




        public async Task ProcessHouseholdSeparationAsync(HouseholdSeparation separation)
        {
            // Với mỗi SeparationMember, xóa các HouseholdMember thuộc hộ ban đầu có UserId trùng với sepMember.UserId.
            var separationMembers = separation.SeparationMembers.ToList();
            foreach (var sepMember in separationMembers)
            {
                var householdMember = await _context.HouseholdMembers
                    .FirstOrDefaultAsync(hm => hm.HouseholdId == separation.OriginalHouseholdId &&
                                                hm.UserId == sepMember.UserId);
                if (householdMember != null)
                {
                    _context.HouseholdMembers.Remove(householdMember);
                }
            }

            // Cập nhật các thông tin của separation sau khi xử lý:
            separation.Status = Status.Approved.ToString();
            separation.ApprovalDate = DateTime.Now;
            // Nếu có thông tin người duyệt (ví dụ: currentPoliceUserId), bạn có thể set:
            // separation.ApprovedBy = currentPoliceUserId;

            _context.HouseholdSeparations.Update(separation);
            await _context.SaveChangesAsync();
        }


    }
}
