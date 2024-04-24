using HalloDoc_N_Tier_Entity.DataModels;
using HalloDoc_N_Tier_Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Services.Interface
{
    public interface IPatientServices
    {
        public GenetareTokenViewModel PatientLogin(LoginViewModel user);

        public Region CheckRegion(string state);

        public void PatientRequest(PatientRequestViewModel userpatient, Region region);

        public bool CheckEmail(string email);

        public void FamilyFriendRequest(PatientFamilyFriendRequestViewModel userpatient, Region region);

        public void ConciergeRequest(PatientConciergeRequestViewModel userpatient,Region region);

        public void BusinessRequest(PatientBusinessRequestViewModel userpatient, Region region);
        List<PatientDashboardViewModel> GetDashboardData(int? id);
        PatientViewDocumentViewModel GetDocument(int id);
        void AcceptSendAgreement(Request req);
        PatientRequestViewModel PatientSubmitInformationMe(int? userId);
        void PatientSubmitInformationMe(PatientRequestViewModel vm, int? userId);
        void PatientSubmitInformationSomeoneElse(PatientFamilyFriendRequestViewModel vm, int? userId);
        PatientProfileViewModel PatientProfile(int? userId);
        void EditPatientProfile(PatientProfileViewModel vm, int? userId);
    }
}
