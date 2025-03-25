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
                int regId = registration.RegistrationId;
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
                var reg = context.Registrations
                                 .Include(r => r.RegistrationMembers)
                                 .FirstOrDefault(r => r.RegistrationId == registration.RegistrationId);
                if (reg == null)
                {
                    throw new Exception("Registration not found in the database.");
                }
                reg.Status = Status.Approved.ToString();
                context.Registrations.Update(reg);
                context.SaveChanges();
            }
        }
        public List<Registration> GetAllRegistrations()
        {
            using (var context = new PrnContext())
            {
                return context.Registrations
                              .Include(r => r.User)
                              .Include(r => r.RegistrationMembers)
                              .Include(r => r.Address)
                              .ToList();
            }
        }
    }
}
