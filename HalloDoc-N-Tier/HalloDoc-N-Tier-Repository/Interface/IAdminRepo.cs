using HalloDoc.Entity.ViewModels;
using HalloDoc_N_Tier_Entity.DataModels;
using HalloDoc_N_Tier_Entity.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Repository.Interface
{
    public interface IAdminRepo
    {
        //public GenetareTokenViewModel? AdminLogin(LoginViewModel userdata);
        AspNetUser GetAspNetUser(LoginViewModel userdata);
        AspNetUserRole GetAspNetUserRoleById(int id);
        AspNetRole GetOnlyRole(int roleId);
        IQueryable DashboardData();

        public DashboardViewModel DashboardView();

        public List<Region> AssignCaseRegion();

        public List<Physician> GetPhysicianRegionWise(int Id);

        public Request GetRequestById(int id);

        public void UpdateRequest(Request req);

        void SaveRequestStatusLog(RequestStatusLog reqlog);

        public List<CaseTag> GetAllTags();

        public string GetPatientName(int id);

        string? GetCaseTagNameById(int caseTagId);

        void SaveRequestNotes(RequestNote reqnotes);

        public Region GetRegionById(int? id);

        public RequestClient AdminViewCase(int Id);
        RequestNote GetRequestNotesById(int id);
        void SaveBlockRequest(BlockRequest blockreq);
        List<RequestStatusLog> GetReqStatusLogById(int id);
        Physician GetPhysiciianById(int? transToPhysicianId);
        List<RequestWiseFile> GetReqWiseFile(int id);
        RequestClient GetReqClientById(int requestId);
        List<HealthProfessionalType>? GetAllProfession();
        List<HealthProfessional> GetHealthProfession(int healthprofessionalid);
        HealthProfessional GetVenderDetails(int healthvendorid);
        void AddOrder(OrderDetail order);
        Admin GetAdminById(int? adminId);
        void AddReqWiseFile(RequestWiseFile requestWiseFile);
        RequestWiseFile GetReqWiseFileById(int id);
        void SaveChangeReqFile();
        void UpdateReqClient(RequestClient reqclient);
        DbSet<Request> RequestData();
        DbSet<RequestClient> RequestClientData();
        DbSet<Physician> PhysicianData();
        IQueryable<NewStateAdminViewModel> ExportAll();
        List<RequestStatusLog> GetReqLogList(int requestId);
        AspNetUser GetAspNetUserById(int aspNetUserId);
        List<Region>? GetAllRegions();
        bool GetAdminRegionInBool(int? regionId, int? adminId1);
        void UpdateAspNetUser(AspNetUser aspuser);
        void DeleteAdminRegion(int? adminId);
        void UpdateAdmin(Admin admin);
        void AddAdminRegion(AdminRegion adminRegion);
        void AddReqNotes(RequestNote reqnote1);
        void UpdateReqNotes(RequestNote reqNote);
        List<AspNetRole>? GetAllRoles();
        AspNetUser GetAspNetUserByEmail(string? email);
        void AddAspNetUser(AspNetUser aspuser1);
        Physician GetPhysiciianByEmail(string? email);
        void AddPhysician(Physician physician);
        void UpdatePhysician(Physician physician);
        void AddPhysicianRegion(PhysicianRegion phyregion);
        List<Menu> GetMenu(int AccType);
        List<Menu> GetMenuByRoleId(int AccType);
        void SaveRole(Role role);
        void SaveRoleMenu(RoleMenu rolemenu);
        List<Role> GetAllRolesFromTable();
        void AddShift(Shift shift);
        void AddShiftDetail(ShiftDetail shiftdetail);
        IQueryable<Physician> GetAllPhysician();
        Admin GetAdminByAspId(int id);
    }
}
