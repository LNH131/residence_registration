using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Resident.Enums;
using Resident.Models;

namespace Resident.Service
{
    public interface IPoliceProcessingService
    {
        Task ProcessRegistrationAsync(Registration registration, int approverUserId);
        Task ProcessHouseholdTransferAsync(HouseholdTransfer transfer, int approverUserId);
        Task ProcessHouseholdSeparationAsync(HouseholdSeparation separation, int approverUserId);
    }

    public class PoliceProcessingService : IPoliceProcessingService
    {
        private readonly PrnContext _context;

        public PoliceProcessingService(PrnContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Finalize a registration from ApprovedByLeader -> Approved.
        /// </summary>
        public async Task ProcessRegistrationAsync(Registration registration, int approverUserId)
        {
            Debug.WriteLine($"Processing registration ID={registration.AddressId}");
            // Load fresh from DB to avoid stale data
            var dbReg = await _context.Registrations
            .Include(r => r.RegistrationMembers)
            .FirstOrDefaultAsync(r => r.RegistrationId == registration.RegistrationId);

            Debug.WriteLine($"Loaded registration ID={dbReg.AddressId}");
            Debug.WriteLine($"Members: {dbReg.RegistrationMembers.Count}");

            if (dbReg == null)
            {
                throw new Exception("Không tìm thấy đơn đăng ký.");
            }

            var household = new Household
            {
                AddressId = registration.AddressId,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Households.Add(household);
            await _context.SaveChangesAsync(); // Để nhận được HouseholdId

            var headHousehold = new HeadOfHouseHold
            {
                UserId = registration.UserId,
                HouseholdId = household.HouseholdId,
                RegisteredDate = DateTime.Now
            };

            _context.HeadOfHouseHolds.Add(headHousehold);
            await _context.SaveChangesAsync(); // Để nhận được HouseholdId

            var members = dbReg.RegistrationMembers.ToList();

            Debug.WriteLine($"Processing {members} members");

            foreach (var regMember in members)
            {

                int userId = GetUserIdByRegistrationMember(regMember);
                if (userId == 0)
                {
                    // Xử lý tình huống không tìm thấy người dùng, ví dụ: thông báo lỗi hoặc tạo mới user.
                    throw new Exception($"Không tìm thấy người dùng tương ứng với thành viên {regMember.FullName}");
                }

                Debug.WriteLine($"thành viên {regMember.FullName}");

                var householdMember = new HouseholdMember
                {
                    HouseholdId = household.HouseholdId,
                    UserId = userId,
                    Relationship = regMember.Relationship
                };

                _context.HouseholdMembers.Add(householdMember);
            }

            await _context.SaveChangesAsync();

            // Final approval
            dbReg.Status = Status.Approved.ToString();
            dbReg.ApprovedBy = approverUserId;


            // Save changes
            _context.Registrations.Update(dbReg);
            await _context.SaveChangesAsync();
        }

        private int GetUserIdByRegistrationMember(RegistrationMember regMember)
        {
            using (var context = new PrnContext())
            {
                // Giả sử bạn tra cứu theo IdentityCard:
                var user = context.Users.FirstOrDefault(u => u.IdentityCard == regMember.IdentityCard);
                return user != null ? user.UserId : 0;
            }
        }

        /// <summary>
        /// Finalize a household transfer from ApprovedByLeader -> Approved.
        /// Moves the household to the new address.
        /// </summary>
        public async Task ProcessHouseholdTransferAsync(HouseholdTransfer transfer, int approverUserId)
        {
            // Load fresh from DB
            var dbTransfer = await _context.HouseholdTransfers
                .Include(t => t.Household)
                .FirstOrDefaultAsync(t => t.TransferId == transfer.TransferId);

            if (dbTransfer == null)
                throw new Exception($"HouseholdTransfer (ID={transfer.TransferId}) not found.");

            // Move household to the new address
            if (dbTransfer.Household != null)
            {
                dbTransfer.Household.AddressId = dbTransfer.ToAddressId;
            }
            else
            {
                // fallback: load the household from DB if not tracked
                var household = await _context.Households
                    .FirstOrDefaultAsync(h => h.HouseholdId == dbTransfer.HouseholdId);
                if (household != null)
                {
                    household.AddressId = dbTransfer.ToAddressId;
                }
            }

            // Mark transfer as Approved
            dbTransfer.Status = Status.Approved.ToString();
            dbTransfer.ApprovedBy = approverUserId;

            _context.HouseholdTransfers.Update(dbTransfer);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Finalize a household separation from ApprovedByLeader -> Approved.
        /// Removes the separation members from the original household. 
        /// If a new household is specified, creates/updates that household and assigns members.
        /// </summary>
        public async Task ProcessHouseholdSeparationAsync(HouseholdSeparation separation, int approverUserId)
        {
            // Load fresh from DB, including SeparationMembers
            var dbSeparation = await _context.HouseholdSeparations
                .Include(s => s.SeparationMembers)
                .FirstOrDefaultAsync(s => s.SeparationId == separation.SeparationId);

            if (dbSeparation == null)
                throw new Exception($"HouseholdSeparation (ID={separation.SeparationId}) not found.");

            // Also load OriginalHousehold & NewHousehold if assigned
            await _context.Entry(dbSeparation)
                          .Reference(s => s.OriginalHousehold)
                          .LoadAsync();

            if (dbSeparation.NewHouseholdId.HasValue)
            {
                await _context.Entry(dbSeparation)
                              .Reference(s => s.NewHousehold)
                              .LoadAsync();
            }

            // 1) Remove separation members from the original household
            foreach (var sepMember in dbSeparation.SeparationMembers)
            {
                Debug.WriteLine($"Removing member {sepMember.UserId} from household {dbSeparation.OriginalHouseholdId}");
                var householdMember = await _context.HouseholdMembers
                    .FirstOrDefaultAsync(hm => hm.HouseholdId == dbSeparation.OriginalHouseholdId &&
                                               hm.UserId == sepMember.UserId);

                if (householdMember != null)
                {
                    _context.HouseholdMembers.Remove(householdMember);
                }
            }

            //// 2) If a new household is assigned, add these members to that new household
            ////    If no NewHouseholdId, skip this step
            //if (dbSeparation.NewHouseholdId.HasValue && dbSeparation.NewHouseholdId.Value > 0)
            //{
            //    // Make sure we have a valid Household record
            //    var newHousehold = dbSeparation.NewHousehold;
            //    if (newHousehold == null)
            //    {
            //        // Possibly the user didn't create a new Household record in DB yet
            //        // or we want to create it on-the-fly. 
            //        // For now, assume it already exists in the DB if NewHouseholdId is set.
            //        newHousehold = await _context.Households
            //            .FirstOrDefaultAsync(h => h.HouseholdId == dbSeparation.NewHouseholdId.Value);

            //        if (newHousehold == null)
            //        {
            //            // Optionally, create a new household record if needed
            //            // This depends on your business logic.
            //            throw new Exception("New household record not found in DB. " +
            //                                "Please create it before final approval.");
            //        }
            //    }

            //    // Add each separation member to the new household
            //    foreach (var sepMember in dbSeparation.SeparationMembers)
            //    {
            //        // Create new HouseholdMember for the new household
            //        var newMember = new HouseholdMember
            //        {
            //            HouseholdId = newHousehold.HouseholdId,
            //            UserId = sepMember.UserId,
            //            Relationship = sepMember.NewRelationship ?? "Other"
            //        };
            //        _context.HouseholdMembers.Add(newMember);

            //        // If isNewHeadOfHousehold == true, create or update HeadOfHouseHold
            //        if (sepMember.IsNewHeadOfHousehold)
            //        {
            //            // There's a unique constraint on HouseholdID in HeadOfHouseHold,
            //            // so we need to remove any existing head or update if needed.
            //            var existingHoH = await _context.HeadOfHouseHolds
            //                .FirstOrDefaultAsync(hh => hh.HouseholdId == newHousehold.HouseholdId);

            //            if (existingHoH != null)
            //            {
            //                // Possibly reassign or throw an error
            //                existingHoH.UserId = sepMember.UserId;
            //                existingHoH.RegisteredDate = DateTime.Now;
            //                _context.HeadOfHouseHolds.Update(existingHoH);
            //            }
            //            else
            //            {
            //                var newHoH = new HeadOfHouseHold
            //                {
            //                    UserId = sepMember.UserId,
            //                    HouseholdId = newHousehold.HouseholdId,
            //                    RegisteredDate = DateTime.Now
            //                };
            //                _context.HeadOfHouseHolds.Add(newHoH);
            //            }
            //        }
            //    }
            //}

            // 3) Mark separation as Approved
            dbSeparation.Status = Status.Approved.ToString();
            dbSeparation.ApprovalDate = DateTime.Now;
            dbSeparation.ApprovedBy = approverUserId;
            _context.HouseholdSeparations.Update(dbSeparation);

            // Save all changes
            await _context.SaveChangesAsync();
        }
    }
}
