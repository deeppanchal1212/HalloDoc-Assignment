using HalloDoc_N_Tier_Entity.DataModels;
using HalloDoc_N_Tier_Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Repository.Interface
{
    public interface IPatientRepo
    {
        AspNetUser GetAspNetUser(string email);
        User GetUserByAspId(int id);
        AspNetUserRole GetRoleByAspId(int id);
        AspNetRole GetRole(int roleId);

        public bool CheckEmail(string email);

        public AspNetUser CheckAspNetUser(string useremail);

        public void AddAspUser(AspNetUser aspnetuser1);

        public Region CheckRegion(string state);

        public User CheckUser(string useremail);

        public void AddUser(User user);

        public void AddRequest(Request request);

        public void AddRequestClient(RequestClient reqclient);

        public void AddRequestWiseFile(RequestWiseFile reqwisefile);

        public void AddConcierge(Concierge concierge);
        List<Request> GetDashboardData(int? id);
        int GetFileCount(int requestId);
        AspNetUserRole CheckAspNetUserRole(int id);
        void AddAspNetUserRole(AspNetUserRole userrole);
        int GetRequestToday();
        List<RequestWiseFile> GetReqWiseFileById(int id);
        public Request GetRequest(int id);
        void UpdateReqTable(Request req1);
        void AddReqStatusLog(RequestStatusLog reqlog);
        Admin GetAdminById(int? adminId);
        Physician GetPhysicianById(int? physicianId);
        User GetUserById(int? userId);
        Region GetRegionByName(string state);
        AspNetUser GetAspNetUserById(int? aspNetUserId);
        void UpdateUser(User user);
        void UpdateAspNetUser(AspNetUser aspuser);
        List<Request> GetRequestsByUserId(int? userId);
        RequestClient GetReqClientByReqId(int requestId);
        void UpdateReqClient(RequestClient reqClient);
    }
}
