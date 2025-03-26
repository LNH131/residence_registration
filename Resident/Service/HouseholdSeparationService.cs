using Microsoft.EntityFrameworkCore;
using Resident.Enums;
using Resident.Models;

namespace Resident.Service
{
    public class HouseholdSeparationService
    {
        public List<HouseholdSeparation> GetPendingSeparations()
        {
            using (var context = new PrnContext())
            {
                // Retrieve separations with status "Pending" or "ApprovedByLeader"
                return context.HouseholdSeparations
                              .Include(s => s.OriginalHousehold)
                                  .ThenInclude(h => h.HeadOfHouseHold)
                              .Where(s => s.Status == "Pending" || s.Status == Status.ApprovedByLeader.ToString())
                              .ToList();
            }
        }

        public void UpdateHouseholdSeparation(HouseholdSeparation separation)
        {
            using (var context = new PrnContext())
            {
                context.HouseholdSeparations.Update(separation);
                context.SaveChanges();
            }
        }

        public void ApproveHouseholdSeparation(HouseholdSeparation separation, User currentUser)
        {
            using (var context = new PrnContext())
            {
                var sep = context.HouseholdSeparations.FirstOrDefault(s => s.SeparationId == separation.SeparationId);
                if (sep == null)
                    throw new Exception("Household Separation not found in the database.");

                // Set the ApprovedBy field using the current user and update the status.
                sep.ApprovedBy = currentUser.UserId;
                sep.Status = Status.Approved.ToString();

                context.HouseholdSeparations.Update(sep);
                context.SaveChanges();
            }
        }
    }
}
