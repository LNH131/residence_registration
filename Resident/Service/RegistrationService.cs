using Microsoft.EntityFrameworkCore;
using Resident.Enums;
using Resident.Models;

namespace Resident.Service
{
    public class RegistrationService
    {
        public List<Registration> GetPendingRegistrations()
        {
            using (var context = new PrnContext())
            {
                return context.Registrations
                              .Include(r => r.User)
                              .Where(r => r.Status == Status.Pending.ToString())
                              .ToList();
            }
        }

        public void UpdateRegistration(Registration registration)
        {
            using (var context = new PrnContext())
            {
                context.Registrations.Update(registration);
                context.SaveChanges();
            }
        }

        public Registration GetRegistrationDetails(Registration registration)
        {
            using (var context = new PrnContext())
            {
                int regId = registration.RegistrationId; // Extract to a local variable
                return context.Registrations
                              .Include(r => r.User)
                              .Include(r => r.RegistrationMembers)
                              .Include(r => r.Address)
                              .FirstOrDefault(r => r.RegistrationId == regId);
            }
        }

        public void ApproveRegistration(Registration registration, User currentUser)
        {
            using (var context = new PrnContext())
            {
                // 1. Reload the registration (with its RegistrationMembers) from the database.
                var reg = context.Registrations
                                 .Include(r => r.RegistrationMembers)
                                 .FirstOrDefault(r => r.RegistrationId == registration.RegistrationId);
                if (reg == null)
                {
                    throw new Exception("Registration not found in the database.");
                }

                // 2. Retrieve the registration's user (the person who registered).
                // Optionally, you can use the currentUser passed in if you're sure it's correct.
                var regUser = context.Users.FirstOrDefault(u => u.UserId == reg.UserId);
                if (regUser == null)
                {
                    throw new Exception("The user who registered was not found in the database.");
                }

                // 3. Create a new Household record.
                var newHousehold = new Household
                {
                    AddressId = reg.AddressId,
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now)
                };
                context.Households.Add(newHousehold);
                context.SaveChanges();  // newHousehold.HouseholdId is generated here

                // 4. Create the HeadOfHouseHold record for the registration's user.
                var head = new HeadOfHouseHold
                {
                    UserId = regUser.UserId,
                    HouseholdId = newHousehold.HouseholdId,
                    RegisteredDate = DateTime.Now
                };
                context.HeadOfHouseHolds.Add(head);
                context.SaveChanges();  // head.HeadOfHouseHoldId is generated here

                // 5. Link the HeadOfHouseHold to the Household.
                newHousehold.HeadOfHouseHold = head;
                context.Households.Update(newHousehold);
                context.SaveChanges();

                // 6. Copy each RegistrationMember into a new HouseholdMember.
                foreach (var rm in reg.RegistrationMembers.ToList())
                {
                    // Try to find an existing user by IdentityCard.
                    User memberUser = null;
                    if (!string.IsNullOrEmpty(rm.IdentityCard))
                    {
                        memberUser = context.Users.FirstOrDefault(u => u.IdentityCard == rm.IdentityCard);
                    }

                    // If no matching user is found, skip adding this member.
                    if (memberUser == null)
                    {
                        continue;
                    }

                    // Create a HouseholdMember for this registration member.
                    var householdMember = new HouseholdMember
                    {
                        HouseholdId = newHousehold.HouseholdId,
                        UserId = memberUser.UserId,
                        Relationship = rm.Relationship
                    };
                    context.HouseholdMembers.Add(householdMember);
                }

                // 7. Update the registration status to Approved.
                reg.Status = Status.Approved.ToString();
                context.Registrations.Update(reg);

                // 8. Save all changes.
                context.SaveChanges();
            }
        }
    }
}
