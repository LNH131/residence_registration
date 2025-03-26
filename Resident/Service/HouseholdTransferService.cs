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
                              .Include(t => t.FromAddress)
                              .Include(t => t.ToAddress)
                              .Where(t => t.Status == "Pending" || t.Status == Status.ApprovedByLeader.ToString())
                              .ToList();
            }
        }

        public void UpdateHouseholdTransfer(HouseholdTransfer transfer)
        {
            using (var context = new PrnContext())
            {
                context.HouseholdTransfers.Update(transfer);
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

                // Set the ApprovedBy field and update the status to Approved.
                trans.ApprovedBy = currentUser.UserId;
                trans.Status = Status.Approved.ToString();

                context.HouseholdTransfers.Update(trans);
                context.SaveChanges();
            }
        }
    }
}
