using Microsoft.EntityFrameworkCore;
using Resident.Enums;
using Resident.Models;

namespace Resident.Service
{
    public class HouseholdTransferService
    {
        public List<HouseholdTransfer> GetPendingTransfers()
        {
            using (var context = new PrnContext())
            {
                // Retrieve transfers with status "Pending" or "ApprovedByLeader" (preliminarily approved)
                return context.HouseholdTransfers
                              .Include(t => t.Household)
                              .Include(t => t.FromAreaId)
                              .Include(t => t.FromAreaId)
                              .Where(t => t.Status == "Pending" || t.Status == Status.ApprovedByLeader.ToString())
                              .ToList();
            }
        }

        public void UpdateHouseholdTransfer(HouseholdTransfer updatedTransfer)
        {
            using (var context = new PrnContext())
            {
                // Reload from DB so EF sees the real object with correct references
                var dbTransfer = context.HouseholdTransfers
                    .FirstOrDefault(t => t.TransferId == updatedTransfer.TransferId);

                if (dbTransfer == null)
                    throw new Exception("Household Transfer not found in the database.");

                // Now only update the fields you need
                dbTransfer.Status = updatedTransfer.Status;
                dbTransfer.ApprovedBy = updatedTransfer.ApprovedBy;
                dbTransfer.Comments = updatedTransfer.Comments;

                context.SaveChanges();
            }
        }


        public void ApproveHouseholdTransfer(HouseholdTransfer transfer, User currentUser)
        {
            using (var context = new PrnContext())
            {
                var trans = context.HouseholdTransfers.FirstOrDefault(t => t.TransferId == transfer.TransferId);
                if (trans == null)
                    throw new Exception("Household Transfer not found in the database.");

                // Final approval: update the ApprovedBy field and change status to Approved.
                trans.ApprovedBy = currentUser.UserId;
                trans.Status = Status.Approved.ToString();

                context.HouseholdTransfers.Update(trans);
                context.SaveChanges();
            }
        }
    }
}
