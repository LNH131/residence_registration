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
                              .Include(r => r.RegistrationMembers) // Ensure navigation property exists
                              .Include(r => r.Address)              // If you want to load address details
                              .FirstOrDefault(r => r.RegistrationId == regId);
            }
        }


        public void ApproveRegistration(Registration registration, User currentUser)
        {
            using (var context = new PrnContext())
            {
                // Re-load the registration (with its RegistrationMembers) from the current context.
                var reg = context.Registrations
                                 .Include(r => r.RegistrationMembers)
                                 .FirstOrDefault(r => r.RegistrationId == registration.RegistrationId);
                if (reg == null)
                {
                    throw new Exception("Registration not found in the database.");
                }

                // Extract simple values.
                int regId = reg.RegistrationId;
                int addressId = reg.AddressId;

                // Get the registration's user (the person who registered).
                var regUser = context.Users.FirstOrDefault(u => u.UserId == reg.UserId);
                if (regUser == null)
                {
                    throw new Exception("The user who registered was not found in the database.");
                }

                // 1) Create a new Household record.
                var newHousehold = new Household
                {
                    AddressId = addressId,
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now)
                };
                context.Households.Add(newHousehold);
                context.SaveChanges();  // newHousehold.HouseholdId is generated

                // 2) Create the HeadOfHouseHold record for the registration's user.
                var head = new HeadOfHouseHold
                {
                    UserId = regUser.UserId, // Use the registration's user id.
                    HouseholdId = newHousehold.HouseholdId,
                    RegisteredDate = DateTime.Now
                };
                context.HeadOfHouseHolds.Add(head);
                context.SaveChanges();  // head.HeadOfHouseHoldId is generated

                // Optionally update the Household navigation property.
                newHousehold.HeadOfHouseHold = head;
                context.Households.Update(newHousehold);
                context.SaveChanges();

                // 3) Copy each RegistrationMember (from the reloaded reg) into a HouseholdMember.
                // Use the already loaded RegistrationMembers from 'reg' to avoid duplicate tracking.
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

                // 4) Update the registration status to Approved.
                reg.Status = Status.Approved.ToString();
                context.Registrations.Update(reg);

                // 5) Save all changes.
                context.SaveChanges();
            }
        }





    }
}
