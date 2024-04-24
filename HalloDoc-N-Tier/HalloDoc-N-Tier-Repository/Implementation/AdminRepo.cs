using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalloDoc_N_Tier_Repository.Interface;
using HalloDoc_N_Tier_Entity.DataModels;
using HalloDoc_N_Tier_Entity.ViewModel;
using HalloDoc_N_Tier_Entity.DataContext;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Net;
using HalloDoc.Entity.ViewModels;

namespace HalloDoc_N_Tier_Repository.Implementation
{
    public class AdminRepo : IAdminRepo
    {
        private readonly ApplicationDbContext _context;
        public AdminRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public AspNetUser GetAspNetUser(LoginViewModel userdata)
        {
            return _context.AspNetUsers.FirstOrDefault(u => u.Email == userdata.Email && u.PasswordHash == userdata.Password) ?? new();
        }

        public AspNetUserRole GetAspNetUserRoleById(int id)
        {
            return _context.AspNetUserRoles.FirstOrDefault(u => u.UserId == id) ?? new();
        }

        public AspNetRole GetOnlyRole(int roleId)
        {
            return _context.AspNetRoles.FirstOrDefault(u => u.Id == roleId) ?? new();
        }

        public Admin GetAdminByAspId(int id)
        {
            return _context.Admins.FirstOrDefault(u => u.AspNetUserId == id) ?? new();
        }

        public DashboardViewModel DashboardView()
        {
            DashboardViewModel result = new DashboardViewModel();
            result.NewCount = _context.Requests.Count(x => x.Status == 1);
            result.PendingCount = _context.Requests.Count(x => x.Status == 2);
            result.ActiveCount = _context.Requests.Count(x => x.Status == 4 || x.Status == 5);
            result.ConcludeCount = _context.Requests.Count(x => x.Status == 6);
            result.ToCloseCount = _context.Requests.Count(x => x.Status == 3 || x.Status == 7 || x.Status == 8);
            result.UnpaidCount = _context.Requests.Count(x => x.Status == 9);
            result.RegionName = _context.Regions.ToList();
            return result;
        }

        public IQueryable DashboardData()
        {
            return from req in _context.Requests
                   join reqclient in _context.RequestClients
                   on req.RequestId equals reqclient.RequestId
                   select req;
        }


        public DbSet<Request> RequestData()
        {
            return _context.Requests;
        }

        public DbSet<RequestClient> RequestClientData()
        {
            return _context.RequestClients;
        }

        public DbSet<Physician> PhysicianData()
        {
            return _context.Physicians;
        }

        public List<Region> AssignCaseRegion()
        {
            return _context.Regions.ToList();
        }

        public List<Physician> GetPhysicianRegionWise(int Id)
        {
            return _context.Physicians.Where(p => p.RegionId == Id).ToList();
        }

        public Request GetRequestById(int id)
        {
            return _context.Requests.FirstOrDefault(r => r.RequestId == id) ?? new Request();
        }
        public void UpdateRequest(Request req)
        {
            _context.Requests.Update(req);
            _context.SaveChanges();
        }

        public void SaveRequestStatusLog(RequestStatusLog reqlog)
        {
            _context.RequestStatusLogs.Add(reqlog);
            _context.SaveChanges();
        }

        public List<CaseTag> GetAllTags()
        {
            return _context.CaseTags.ToList();
        }

        public string GetPatientName(int id)
        {
            var query = _context.Requests.Include(r => r.RequestClients).FirstOrDefault(r => r.RequestId == id);
            string name = query.FirstName + " " + query.LastName;
            return name;
        }

        public string GetCaseTagNameById(int caseTagId)
        {
            return _context.CaseTags.FirstOrDefault(u => u.CaseTagId == caseTagId).Name;
        }

        public void SaveRequestNotes(RequestNote reqnotes)
        {
            _context.RequestNotes.Add(reqnotes);
            _context.SaveChanges();
        }

        public Region GetRegionById(int? id)
        {
            return _context.Regions.FirstOrDefault(u => u.RegionId == id);
        }

        public RequestClient AdminViewCase(int Id)
        {
            RequestClient reqclient = _context.RequestClients.FirstOrDefault(u => u.RequestId == Id) ?? new();
            return reqclient;
        }
        public RequestNote GetRequestNotesById(int id)
        {
            return _context.RequestNotes.FirstOrDefault(u => u.RequestId == id) ?? new();
        }

        public void SaveBlockRequest(BlockRequest blockreq)
        {
            _context.BlockRequests.Add(blockreq);
            _context.SaveChanges();
        }

        public List<RequestStatusLog> GetReqStatusLogById(int id)
        {
            List<RequestStatusLog> reqlog = _context.RequestStatusLogs.Where(u => u.RequestId == id).ToList();
            return reqlog;
        }
        public Physician GetPhysiciianById(int? transToPhysicianId)
        {
            return _context.Physicians.FirstOrDefault(u => u.PhysicianId == transToPhysicianId) ?? new();
        }
        public List<RequestWiseFile> GetReqWiseFile(int id)
        {
            return _context.RequestWiseFiles.Where(u => u.RequestId == id).ToList();
        }
        public RequestClient GetReqClientById(int requestId)
        {
            return _context.RequestClients.FirstOrDefault(_context => _context.RequestId == requestId);
        }
        public List<HealthProfessionalType>? GetAllProfession()
        {
            return _context.HealthProfessionalTypes.ToList();
        }
        public List<HealthProfessional> GetHealthProfession(int healthprofessionalid)
        {
            return _context.HealthProfessionals.Where(u => u.Profession == healthprofessionalid).ToList();
        }
        public HealthProfessional GetVenderDetails(int healthvendorid)
        {
            return _context.HealthProfessionals.FirstOrDefault(u => u.VendorId == healthvendorid);
        }
        public void AddOrder(OrderDetail order)
        {
            _context.OrderDetails.Add(order);
            _context.SaveChanges();
        }
        public Admin GetAdminById(int? adminId)
        {
            return _context.Admins.FirstOrDefault(u => u.AdminId == adminId);
        }
        public void AddReqWiseFile(RequestWiseFile requestWiseFile)
        {
            _context.RequestWiseFiles.Add(requestWiseFile);
            _context.SaveChanges();
        }
        public RequestWiseFile GetReqWiseFileById(int id)
        {
            return _context.RequestWiseFiles.FirstOrDefault(u => u.RequestWiseFileId == id);
        }
        public void SaveChangeReqFile()
        {
            _context.SaveChanges();
        }
        public void UpdateReqClient(RequestClient reqclient)
        {
            _context.RequestClients.Update(reqclient);
            _context.SaveChanges();
        }
        public List<RequestStatusLog> GetReqLogList(int requestId)
        {
            return _context.RequestStatusLogs.Where(u => u.RequestId == requestId).ToList();
        }

        public IQueryable<NewStateAdminViewModel> ExportAll()
        {
            var jkb = from r in _context.Requests
                      join rc in _context.RequestClients on r.RequestId equals rc.RequestId
                      select new NewStateAdminViewModel
                      {
                          Firstname = rc.FirstName,
                          Lastname = rc.LastName,
                          Intdate = rc.IntDate,
                          Intyear = rc.IntYear,
                          Strmonth = rc.StrMonth,
                          RequestorFirstname = r.FirstName,
                          RequestorLastname = r.LastName,
                          Createddate = r.CreatedDate,
                          Phonenumber = rc.PhoneNumber,
                          City = rc.City,
                          State = rc.State,
                          Street = rc.Street,
                          Zipcode = rc.ZipCode,
                          Notes = rc.Notes,
                          Status = r.Status,
                          Email = rc.Email,
                          RequestTypeId = r.RequestTypeId,
                          RequestId = r.RequestId,
                          ConfirmationNumber = r.ConfirmationNumber
                      };

            return jkb;
        }
        public AspNetUser GetAspNetUserById(int aspNetUserId)
        {
            return _context.AspNetUsers.FirstOrDefault(u => u.Id == aspNetUserId) ?? new();
        }
        public List<Region>? GetAllRegions()
        {
            return _context.Regions.ToList();
        }

        public bool GetAdminRegionInBool(int? regionId, int? adminId)
        {
            AdminRegion adregion = _context.AdminRegions.FirstOrDefault(u => u.AdminId == adminId && u.RegionId == regionId);
            if (adregion == null)
            {
                return false;
            }
            return true;
        }

        public void UpdateAspNetUser(AspNetUser aspuser)
        {
            _context.AspNetUsers.Update(aspuser);
            _context.SaveChanges();
        }
        public void DeleteAdminRegion(int? adminId)
        {
            List<AdminRegion> adminregion = _context.AdminRegions.Where(u => u.AdminId == adminId).ToList();
            foreach (var item in adminregion)
            {
                _context.AdminRegions.Remove(item);
            }
        }
        public void UpdateAdmin(Admin admin)
        {
            _context.Admins.Update(admin);
            _context.SaveChanges();
        }
        public void AddAdminRegion(AdminRegion adminRegion)
        {
            _context.AdminRegions.Add(adminRegion);
            _context.SaveChanges();
        }

        public void AddReqNotes(RequestNote reqnote1)
        {
            _context.RequestNotes.Add(reqnote1);
            _context.SaveChanges();
        }
        public void UpdateReqNotes(RequestNote reqNote)
        {
            _context.RequestNotes.Update(reqNote);
            _context.SaveChanges();
        }
        public List<AspNetRole>? GetAllRoles()
        {
            return _context.AspNetRoles.ToList();
        }
        public AspNetUser GetAspNetUserByEmail(string? email)
        {
            return _context.AspNetUsers.FirstOrDefault(u => u.Email == email) ?? new();
        }

        public void AddAspNetUser(AspNetUser aspuser1)
        {
            _context.AspNetUsers.Add(aspuser1);
            _context.SaveChanges();
        }

        public Physician GetPhysiciianByEmail(string? email)
        {
            return _context.Physicians.FirstOrDefault(u => u.Email == email) ?? new();
        }
        public void AddPhysician(Physician physician)
        {
            _context.Physicians.Add(physician);
            _context.SaveChanges();
        }
        public void UpdatePhysician(Physician physician)
        {
            _context.Physicians.Update(physician);
        }
        public void AddPhysicianRegion(PhysicianRegion phyregion)
        {
            _context.PhysicianRegions.Add(phyregion);
        }

        public List<Menu> GetMenu(int AccType)
        {
            return _context.Menus.ToList();
        }
        public List<Menu> GetMenuByRoleId(int AccType)
        {
            return _context.Menus.Where(u => u.AccountType == AccType).ToList();
        }

        public void SaveRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        public void SaveRoleMenu(RoleMenu rolemenu)
        {
            _context.RoleMenus.Add(rolemenu);
            _context.SaveChanges();
        }
        public List<Role> GetAllRolesFromTable()
        {
            return _context.Roles.ToList();
        }
        public void AddShift(Shift shift)
        {
            _context.Shifts.Add(shift);
            _context.SaveChanges();
        }
        public void AddShiftDetail(ShiftDetail shiftdetail)
        {
            _context.ShiftDetails.Add(shiftdetail);
            _context.SaveChanges();
        }
        //public void abc()
        //{
        //    _context.Physicians.Include(x => x.Shifts).ThenInclude(x => x.ShiftDetails);
        //}
        public IQueryable<Physician> GetAllPhysician()
        {
            return _context.Physicians.Include(x => x.Shifts).ThenInclude(x => x.ShiftDetails);
        }






    }
}
