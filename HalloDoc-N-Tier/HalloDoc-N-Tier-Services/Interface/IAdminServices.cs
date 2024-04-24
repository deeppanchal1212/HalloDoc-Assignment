using HalloDoc.Entity.ViewModels;
using HalloDoc_N_Tier_Entity.DataModels;
using HalloDoc_N_Tier_Entity.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Services.Interface
{
    public interface IAdminServices
    {
        public GenetareTokenViewModel AdminLogin(LoginViewModel userdata);

        public DashboardViewModel DashboardView();

        public List<AdminAllCaseViewModel> AdminNewCase(int id);

        public List<AdminAllCaseViewModel> AdminPendingCase(int id);

        public List<AdminAllCaseViewModel> AdminActiveCase(int id);

        public List<AdminAllCaseViewModel> AdminConcludeCase(int id);

        public List<AdminAllCaseViewModel> AdminToCloseCase(int id);

        public List<AdminAllCaseViewModel> AdminUnpaidCase(int id);

        public List<Region> AssignCaseRegion();

        public AdminAssignCaseViewModel GetPhysicianRegionWise(int Id);

        void PostAssignCase(int id,AdminAssignCaseViewModel vm,int? adminid);

        public AdminCancleCaseViewModel CancelCaseData(int id);
        void PostCancleCase(int id, AdminCancleCaseViewModel vm,int? Admin_id);
        AdminViewCaseViewModel AdminViewCase(int id);
        AdminViewNotesViewModel AdminViewNotes(int id);
        AdminBlockCaseViewModel GetBlockCaseData(int id);
        void AdminBlockCase(int id,AdminBlockCaseViewModel vm);
        AdminViewUploadsViewModel AdminViewUploads(int id);
        Request GetRequestById(int id);
        void AdminClearCase(int id);
        void SendMail(string email,string token);
        RequestClient GetReqClientById(int requestId);
        List<HealthProfessionalType>? GetAllProfession();
        List<HealthProfessional> GetHealthProfession(int healthprofessionalid);
        HealthProfessional GetVenderDetails(int healthvendorid);
        void ApplyForOrder(AdminSendOrderViewModel vm, int selectedPhysician);
        void UploadNewFiles(List<IFormFile>? files, int reqid, int? adminId);
        void DeleteReqFile(int id);
        void DeleteMultiple(int id, string reqwid);
        AdminCloseCaseViewModel AdminCloseCase(int id);
        void EditReqClient(int id,AdminCloseCaseViewModel vm);
        IQueryable<NewStateAdminViewModel> ExportAll();
        void PostCloseCase(int id, int? AdminId);
        AdminProfileViewModel AdminProfile(int? adminId);
        void AdminResetPassword(int? adminId, string password);
        void AdminInfoReset(AdminProfileViewModel vm, int? adminId);
        void AdminMailingBillingInfoReset(AdminProfileViewModel vm, int? adminId);
        void SavingAdminNotes(int id, AdminViewNotesViewModel vm, int? adminId);
        AdminCreateProviderAccountViewModel AdminCreateProviderAccount();
        void AdminCreateProviderAccount(AdminCreateProviderAccountViewModel vm, int? adminid);
        List<AdminProviderInfoViewModel> AdminProviderInformation();
        List<Menu> GetMenu(int AccType);
        void AdminCreateRole(AdminCreateRoleViewModel vm,int? AdminId);
        List<AdminAccountAccessViewModel> AdminAccountAccess();
        AdminSchedulingViewModel AdminScheduling();
        void AdminCreateShift(AdminSchedulingViewModel vm, int? adminId);
        List<AdminPhysicianShiftDetailsViewModel> AdminDayWiseScheduling(int day, int month, int year);
    }
}
