using HalloDoc_N_Tier_Entity.DataContext;
using HalloDoc_N_Tier_Entity.DataModels;
using HalloDoc_N_Tier_Entity.ViewModel;
using HalloDoc_N_Tier_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDoc_N_Tier_Repository.Implementation
{
    public class PatientRepo : IPatientRepo
    {
        private readonly ApplicationDbContext _context;
        public PatientRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public AspNetUser GetAspNetUser(string email)
        {
            return _context.AspNetUsers.FirstOrDefault(u => u.Email == email) ?? new();
        }
        public User GetUserByAspId(int id)
        {
            return _context.Users.FirstOrDefault(u => u.AspNetUserId == id) ?? new();
        }
        public AspNetUserRole GetRoleByAspId(int id)
        {
            return _context.AspNetUserRoles.FirstOrDefault(u => u.UserId == id) ?? new();
        }

        public AspNetRole GetRole(int roleId)
        {
            return _context.AspNetRoles.FirstOrDefault(u => u.Id == roleId) ?? new();
        }



        public AspNetUser CheckAspNetUser(string useremail)
        {
            AspNetUser aspnetuser = _context.AspNetUsers.FirstOrDefault(u => u.Email == useremail);
            return aspnetuser;
        }

        public void AddAspUser(AspNetUser aspnetuser1)
        {
            _context.AspNetUsers.Add(aspnetuser1);
            _context.SaveChanges();
        }

        public Region CheckRegion(string state)
        {
            Region region = _context.Regions.FirstOrDefault(u => u.Name.ToLower() == state.ToLower());
            if (region != null)
            {
                return region;
            }
            return new();
        }

        public User CheckUser(string useremail)
        {
            User user1 = _context.Users.FirstOrDefault(u => u.Email == useremail);
            return user1;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void AddRequest(Request request)
        {
            _context.Requests.Add(request);
            _context.SaveChanges();
        }

        public void AddRequestClient(RequestClient reqclient)
        {
            _context.RequestClients.Add(reqclient);
            _context.SaveChanges();
        }

        public bool CheckEmail(string email)
        {
            AspNetUser aspuser = _context.AspNetUsers.FirstOrDefault(u => u.Email == email) ?? new();
            return aspuser.Email == null;
        }

        public void AddRequestWiseFile(RequestWiseFile reqwisefile)
        {
            _context.RequestWiseFiles.Add(reqwisefile);
            _context.SaveChanges();
        }

        public void AddConcierge(Concierge concierge)
        {
            _context.Concierges.Add(concierge);
            _context.SaveChanges();
        }
        public List<Request> GetDashboardData(int? id)
        {
            List<Request> req = _context.Requests.Where(u => u.UserId == id).ToList();
            return req;
        }
        public int GetFileCount(int requestId)
        {
            return _context.RequestWiseFiles.Count(u => u.RequestId == requestId);
        }
        public AspNetUserRole CheckAspNetUserRole(int id)
        {
            AspNetUserRole aspuserrole = _context.AspNetUserRoles.FirstOrDefault(u => u.UserId == id);
            return aspuserrole;
        }

        public void AddAspNetUserRole(AspNetUserRole userrole)
        {
            _context.AspNetUserRoles.Add(userrole);
            _context.SaveChanges();
        }

        public int GetRequestToday()
        {
            DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);
            List<Request> req = _context.Requests.ToList();
            int count = 1;
            foreach (Request request in req)
            {
                if (dateNow.Day == request.CreatedDate.Day && dateNow.Month == request.CreatedDate.Month && dateNow.Year == request.CreatedDate.Year)
                {
                    count++;
                }
            }
            return count;
        }

        public List<RequestWiseFile> GetReqWiseFileById(int id)
        {
            return _context.RequestWiseFiles.Where(u => u.RequestId == id).ToList();
        }

        public Request GetRequest(int id)
        {
            return _context.Requests.FirstOrDefault(u => u.RequestId == id) ?? new();
        }
        public void UpdateReqTable(Request req1)
        {
            _context.Requests.Update(req1);
            _context.SaveChanges();
        }
        public void AddReqStatusLog(RequestStatusLog reqlog)
        {
            _context.RequestStatusLogs.Add(reqlog);
            _context.SaveChanges();
        }
        public Admin GetAdminById(int? adminId)
        {
            return _context.Admins.FirstOrDefault(u => u.AdminId == adminId) ?? new();
        }
        public Physician GetPhysicianById(int? physicianId)
        {
            return _context.Physicians.FirstOrDefault(u => u.PhysicianId == physicianId) ?? new();
        }
        public User GetUserById(int? userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId) ?? new();
        }
        public Region GetRegionByName(string state)
        {
            return _context.Regions.FirstOrDefault(u => u.Name.ToLower() == state.ToLower()) ?? new();
        }
        public AspNetUser GetAspNetUserById(int? aspNetUserId)
        {
            return _context.AspNetUsers.FirstOrDefault(u => u.Id == aspNetUserId) ?? new();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public void UpdateAspNetUser(AspNetUser aspuser)
        {
            _context.AspNetUsers.Update(aspuser);
            _context.SaveChanges();
        }
        public List<Request> GetRequestsByUserId(int? userId)
        {
            return _context.Requests.Where(u => u.UserId == userId).ToList();
        }
        public RequestClient GetReqClientByReqId(int requestId)
        {
            return _context.RequestClients.FirstOrDefault(u => u.RequestId == requestId) ?? new();
        }

        public void UpdateReqClient(RequestClient reqClient)
        {
            _context.RequestClients.Update(reqClient);
            _context.SaveChanges();
        }


    }
}
