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
            // Load fresh from DB to avoid stale data
            var reg = await _context.Registrations
                .FirstOrDefaultAsync(r => r.RegistrationId == registration.RegistrationId);

            if (reg == null)
                throw new Exception($"Registration (ID={registration.RegistrationId}) not found.");

            reg.Status = Status.Approved.ToString();
            reg.ApprovedBy = approverUserId;
            _context.Registrations.Update(reg);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Finalize a household transfer from ApprovedByLeader -> Approved.
        /// Move the household to the new address.
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
        /// Remove the separation members from the original household. 
        /// (Optionally, if a new household is created, assign them there.)
        /// </summary>
        public async Task ProcessHouseholdSeparationAsync(HouseholdSeparation separation, int approverUserId)
        {
            // Load fresh from DB
            var dbSeparation = await _context.HouseholdSeparations
                .Include(s => s.SeparationMembers)
                .FirstOrDefaultAsync(s => s.SeparationId == separation.SeparationId);

            if (dbSeparation == null)
                throw new Exception($"HouseholdSeparation (ID={separation.SeparationId}) not found.");

            // Remove separation members from the original household
            foreach (var sepMember in dbSeparation.SeparationMembers)
            {
                var householdMember = await _context.HouseholdMembers
                    .FirstOrDefaultAsync(hm => hm.HouseholdId == dbSeparation.OriginalHouseholdId &&
                                               hm.UserId == sepMember.UserId);
                if (householdMember != null)
                {
                    _context.HouseholdMembers.Remove(householdMember);
                }
            }

            // Mark separation as Approved
            dbSeparation.Status = Status.Approved.ToString();
            dbSeparation.ApprovalDate = DateTime.Now;
            dbSeparation.ApprovedBy = approverUserId;
            _context.HouseholdSeparations.Update(dbSeparation);
            await _context.SaveChangesAsync();
        }
    }
}
