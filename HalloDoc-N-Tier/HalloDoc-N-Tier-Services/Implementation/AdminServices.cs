using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using HalloDoc_N_Tier_Entity.DataModels;
using HalloDoc_N_Tier_Entity.ViewModel;
using HalloDoc_N_Tier_Repository.Interface;
using HalloDoc_N_Tier_Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HalloDoc.Entity.ViewModels;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;

namespace HalloDoc_N_Tier_Services.Implementation
{
    public class AdminServices : IAdminServices
    {
        private readonly IAdminRepo _repository;
        public AdminServices(IAdminRepo repository)
        {
            _repository = repository;
        }

        public GenetareTokenViewModel AdminLogin(LoginViewModel userdata)
        {
            AspNetUser aspuser = _repository.GetAspNetUser(userdata);
            GenetareTokenViewModel vm = new();
            if (aspuser.Email != null)
            {
                AspNetUserRole userRole = _repository.GetAspNetUserRoleById(aspuser.Id);
                AspNetRole role = _repository.GetOnlyRole(userRole.RoleId);
                vm.Email = aspuser.Email;
                vm.Role = role.Name;
                Admin admin = _repository.GetAdminByAspId(aspuser.Id);
                vm.User_Id = admin.AdminId;
                vm.Username = aspuser.UserName;
                return vm;
            }
            return null;
        }

        public DashboardViewModel DashboardView()
        {
            return _repository.DashboardView();
        }

        public List<AdminAllCaseViewModel> AdminNewCase(int id)
        {
            var query = (from req in _repository.RequestData()
                         join reqclient in _repository.RequestClientData()
                         on req.RequestId equals reqclient.RequestId
                         where (req.Status == 1)
                         select new
                         {
                             req,
                             reqclient
                         }).ToList();

            var query1 = _repository.DashboardData();

            if (id != 0)
            {
                query = query.Where(r => r.req.RequestTypeId == id).ToList();
            }
            List<AdminAllCaseViewModel> adminNewCaseViewModels = new List<AdminAllCaseViewModel>();
            foreach (var data in query)
            {
                //var dateofbirth = 
                DateOnly? date = new DateOnly(data.reqclient.IntYear!.Value, DateOnly.ParseExact(data.reqclient.StrMonth!, "MMMM", CultureInfo.InvariantCulture).Month, data.reqclient.IntDate!.Value);
                string address = data.reqclient.Street + " " + data.reqclient.City + " " + data.reqclient.State + "," + data.reqclient.ZipCode;
                string typeOfRequestor = "";
                switch (data.req.RequestTypeId)
                {
                    case 1:
                        typeOfRequestor = "Business";
                        break;
                    case 2:
                        typeOfRequestor = "Patient";
                        break;
                    case 3:
                        typeOfRequestor = "Family/Friend";
                        break;
                    case 4:
                        typeOfRequestor = "Concierge";
                        break;
                    case 5:
                        typeOfRequestor = "VIP";
                        break;
                }
                adminNewCaseViewModels.Add(new AdminAllCaseViewModel
                {
                    TypeOfRequestor = typeOfRequestor,
                    ReqTypeId = data.req.RequestTypeId,
                    FirstName = data.reqclient.FirstName,
                    LastName = data.reqclient.LastName,
                    DateOfBirth = date,
                    RequestorName = data.req.FirstName,
                    RequestedDate = data.req.CreatedDate,
                    Mobile = data.reqclient.PhoneNumber,
                    RequestorMobile = data.req.PhoneNumber,
                    Address = address,
                    Notes = data.reqclient.Notes,
                    RequestId = data.req.RequestId,
                    Region = data.reqclient.State!.ToLower(),
                    Email = data.reqclient.Email

                });
            }
            return adminNewCaseViewModels;
        }

        public List<AdminAllCaseViewModel> AdminPendingCase(int id)
        {
            var query = (from req in _repository.RequestData()
                         join reqclient in _repository.RequestClientData()
                         on req.RequestId equals reqclient.RequestId
                         join phy in _repository.PhysicianData()
                         on req.PhysicianId equals phy.PhysicianId
                         where (req.Status == 2)
                         select new
                         {
                             phy,
                             req,
                             reqclient
                         }).ToList();
            if (id != 0)
            {
                query = query.Where(r => r.req.RequestTypeId == id).ToList();
            }
            List<AdminAllCaseViewModel> adminPendingCase = new List<AdminAllCaseViewModel>();
            foreach (var data in query)
            {
                //var dateofbirth = 
                DateOnly? date = new DateOnly(data.reqclient.IntYear!.Value, DateOnly.ParseExact(data.reqclient.StrMonth!, "MMMM", CultureInfo.InvariantCulture).Month, data.reqclient.IntDate!.Value);
                string address = data.reqclient.Street + " " + data.reqclient.City + " " + data.reqclient.State + "," + data.reqclient.ZipCode;
                string typeOfRequestor = "";
                switch (data.req.RequestTypeId)
                {
                    case 1:
                        typeOfRequestor = "Business";
                        break;
                    case 2:
                        typeOfRequestor = "Patient";
                        break;
                    case 3:
                        typeOfRequestor = "Family/Friend";
                        break;
                    case 4:
                        typeOfRequestor = "Concierge";
                        break;
                    case 5:
                        typeOfRequestor = "VIP";
                        break;
                }
                string tnotes;
                List<string> notes = new List<string>();
                List<RequestStatusLog> reqlog = _repository.GetReqLogList(data.req.RequestId);
                foreach (var item in reqlog)
                {
                    if (item.TransToPhysicianId != null)
                    {
                        Physician phy = _repository.GetPhysiciianById(item.TransToPhysicianId);
                        string Createddate = item.CreatedDate.ToString();
                        if (item.AdminId != null)
                        {
                            tnotes = "Admin transferred to Dr." + phy.FirstName + " on " + Createddate.Substring(10) + " at " + Createddate.Substring(Createddate.Length - 9);
                            notes.Add(tnotes);
                        }
                        Physician phy1 = _repository.GetPhysiciianById(item.TransToPhysicianId);
                        if (item.PhysicianId != null)
                        {
                            tnotes = phy1.FirstName + " transferred to Dr." + phy.FirstName + " on " + Createddate.Substring(10) + " at " + Createddate.Substring(Createddate.Length - 9);
                            notes.Add(tnotes);
                        }
                    }
                }
                adminPendingCase.Add(new AdminAllCaseViewModel
                {
                    RequestId = data.req.RequestId,
                    TypeOfRequestor = typeOfRequestor,
                    ReqTypeId = data.req.RequestTypeId,
                    FirstName = data.reqclient.FirstName,
                    LastName = data.reqclient.LastName,
                    DateOfBirth = date,
                    PhysicianName = data.phy.FirstName,
                    RequestorName = data.req.FirstName,
                    RequestedDate = data.req.CreatedDate,
                    Mobile = data.reqclient.PhoneNumber,
                    RequestorMobile = data.req.PhoneNumber,
                    Address = address,
                    Region = data.reqclient.State!.ToLower(),
                    Email = data.reqclient.Email,
                    TransferNotes = notes
                });
            }
            return adminPendingCase;
        }

        public List<AdminAllCaseViewModel> AdminActiveCase(int id)
        {
            var query = (from req in _repository.RequestData()
                         join reqclient in _repository.RequestClientData()
                         on req.RequestId equals reqclient.RequestId
                         where (req.Status == 4 || req.Status == 5)
                         select new
                         {
                             req,
                             reqclient
                         }).ToList();
            if (id != 0)
            {
                query = query.Where(r => r.req.RequestTypeId == id).ToList();
            }
            List<AdminAllCaseViewModel> adminActiveCase = new List<AdminAllCaseViewModel>();
            foreach (var data in query)
            {
                //var dateofbirth = 
                DateOnly? date = new DateOnly(data.reqclient.IntYear!.Value, DateOnly.ParseExact(data.reqclient.StrMonth!, "MMMM", CultureInfo.InvariantCulture).Month, data.reqclient.IntDate!.Value);
                string address = data.reqclient.Street + " " + data.reqclient.City + " " + data.reqclient.State + "," + data.reqclient.ZipCode;
                string typeOfRequestor = "";
                switch (data.req.RequestTypeId)
                {
                    case 1:
                        typeOfRequestor = "Business";
                        break;
                    case 2:
                        typeOfRequestor = "Patient";
                        break;
                    case 3:
                        typeOfRequestor = "Family/Friend";
                        break;
                    case 4:
                        typeOfRequestor = "Concierge";
                        break;
                    case 5:
                        typeOfRequestor = "VIP";
                        break;
                }
                string tnotes;
                List<string> notes = new List<string>();
                List<RequestStatusLog> reqlog = _repository.GetReqLogList(data.req.RequestId);
                foreach (var item in reqlog)
                {
                    if (item.TransToPhysicianId != null)
                    {
                        Physician phy = _repository.GetPhysiciianById(item.TransToPhysicianId);
                        string Createddate = item.CreatedDate.ToString();
                        if (item.AdminId != null)
                        {
                            tnotes = "Admin transferred to Dr." + phy.FirstName + " on " + Createddate.Substring(10) + " at " + Createddate.Substring(Createddate.Length - 9);
                            notes.Add(tnotes);
                        }
                        Physician phy1 = _repository.GetPhysiciianById(item.TransToPhysicianId);
                        if (item.PhysicianId != null)
                        {
                            tnotes = phy1.FirstName + " transferred to Dr." + phy.FirstName + " on " + Createddate.Substring(10) + " at " + Createddate.Substring(Createddate.Length - 9);
                            notes.Add(tnotes);
                        }
                    }
                }
                adminActiveCase.Add(new AdminAllCaseViewModel
                {
                    RequestId = data.req.RequestId,
                    TypeOfRequestor = typeOfRequestor,
                    ReqTypeId = data.req.RequestTypeId,
                    FirstName = data.reqclient.FirstName,
                    LastName = data.reqclient.LastName,
                    DateOfBirth = date,
                    RequestorName = data.req.FirstName,
                    RequestedDate = data.req.CreatedDate,
                    Mobile = data.reqclient.PhoneNumber,
                    RequestorMobile = data.req.PhoneNumber,
                    Address = address,
                    TransferNotes = notes,
                    Region = data.reqclient.State!.ToLower(),
                    Email = data.reqclient.Email
                });
            }
            return adminActiveCase;
        }

        public List<AdminAllCaseViewModel> AdminConcludeCase(int id)
        {
            var query = (from req in _repository.RequestData()
                         join reqclient in _repository.RequestClientData()
                         on req.RequestId equals reqclient.RequestId
                         where (req.Status == 6)
                         select new
                         {
                             req,
                             reqclient
                         }).ToList();
            if (id != 0)
            {
                query = query.Where(r => r.req.RequestTypeId == id).ToList();
            }
            List<AdminAllCaseViewModel> adminConcludeCase = new();
            foreach (var data in query)
            {
                //var dateofbirth = 
                DateOnly? date = new DateOnly(data.reqclient.IntYear!.Value, DateOnly.ParseExact(data.reqclient.StrMonth!, "MMMM", CultureInfo.InvariantCulture).Month, data.reqclient.IntDate!.Value);
                string address = data.reqclient.Street + " " + data.reqclient.City + " " + data.reqclient.State + "," + data.reqclient.ZipCode;
                string typeOfRequestor = "";
                switch (data.req.RequestTypeId)
                {
                    case 1:
                        typeOfRequestor = "Business";
                        break;
                    case 2:
                        typeOfRequestor = "Patient";
                        break;
                    case 3:
                        typeOfRequestor = "Family/Friend";
                        break;
                    case 4:
                        typeOfRequestor = "Concierge";
                        break;
                    case 5:
                        typeOfRequestor = "VIP";
                        break;
                }
                string tnotes;
                List<string> notes = new List<string>();
                List<RequestStatusLog> reqlog = _repository.GetReqLogList(data.req.RequestId);
                foreach (var item in reqlog)
                {
                    if (item.TransToPhysicianId != null)
                    {
                        Physician phy = _repository.GetPhysiciianById(item.TransToPhysicianId);
                        string Createddate = item.CreatedDate.ToString();
                        if (item.AdminId != null)
                        {
                            tnotes = "Admin transferred to Dr." + phy.FirstName + " on " + Createddate.Substring(10) + " at " + Createddate.Substring(Createddate.Length - 9);
                            notes.Add(tnotes);
                        }
                        Physician phy1 = _repository.GetPhysiciianById(item.TransToPhysicianId);
                        if (item.PhysicianId != null)
                        {
                            tnotes = phy1.FirstName + " transferred to Dr." + phy.FirstName + " on " + Createddate.Substring(10) + " at " + Createddate.Substring(Createddate.Length - 9);
                            notes.Add(tnotes);
                        }
                    }
                }
                adminConcludeCase.Add(new AdminAllCaseViewModel
                {
                    RequestId = data.req.RequestId,
                    TypeOfRequestor = typeOfRequestor,
                    ReqTypeId = data.req.RequestTypeId,
                    FirstName = data.reqclient.FirstName,
                    LastName = data.reqclient.LastName,
                    DateOfBirth = date,
                    RequestorName = data.req.FirstName,
                    RequestedDate = data.req.CreatedDate,
                    Mobile = data.reqclient.PhoneNumber,
                    RequestorMobile = data.req.PhoneNumber,
                    Address = address,
                    Region = data.reqclient.State!.ToLower(),
                    Email = data.reqclient.Email,
                    TransferNotes = notes
                });
            }
            return adminConcludeCase;
        }

        public List<AdminAllCaseViewModel> AdminToCloseCase(int id)
        {
            var query = (from req in _repository.RequestData()
                         join reqclient in _repository.RequestClientData()
                         on req.RequestId equals reqclient.RequestId
                         where (req.Status == 3 || req.Status == 7 || req.Status == 8)
                         select new
                         {
                             req,
                             reqclient
                         }).ToList();
            if (id != 0)
            {
                query = query.Where(r => r.req.RequestTypeId == id).ToList();
            }
            List<AdminAllCaseViewModel> adminToCloseCase = new List<AdminAllCaseViewModel>();
            foreach (var data in query)
            {
                //var dateofbirth = 
                DateOnly? date = new DateOnly(data.reqclient.IntYear!.Value, DateOnly.ParseExact(data.reqclient.StrMonth!, "MMMM", CultureInfo.InvariantCulture).Month, data.reqclient.IntDate!.Value);
                string address = data.reqclient.Street + " " + data.reqclient.City + " " + data.reqclient.State + "," + data.reqclient.ZipCode;
                string typeOfRequestor = "";
                switch (data.req.RequestTypeId)
                {
                    case 1:
                        typeOfRequestor = "Business";
                        break;
                    case 2:
                        typeOfRequestor = "Patient";
                        break;
                    case 3:
                        typeOfRequestor = "Family/Friend";
                        break;
                    case 4:
                        typeOfRequestor = "Concierge";
                        break;
                    case 5:
                        typeOfRequestor = "VIP";
                        break;
                }
                string tnotes;
                List<string> notes = new List<string>();
                List<RequestStatusLog> reqlog = _repository.GetReqLogList(data.req.RequestId);
                foreach (var item in reqlog)
                {
                    if (item.TransToPhysicianId != null)
                    {
                        Physician phy = _repository.GetPhysiciianById(item.TransToPhysicianId);
                        string Createddate = item.CreatedDate.ToString();
                        if (item.AdminId != null)
                        {
                            tnotes = "Admin transferred to Dr." + phy.FirstName + " on " + Createddate.Substring(10) + " at " + Createddate.Substring(Createddate.Length - 9);
                            notes.Add(tnotes);
                        }
                        Physician phy1 = _repository.GetPhysiciianById(item.TransToPhysicianId);
                        if (item.PhysicianId != null)
                        {
                            tnotes = phy1.FirstName + " transferred to Dr." + phy.FirstName + " on " + Createddate.Substring(10) + " at " + Createddate.Substring(Createddate.Length - 9);
                            notes.Add(tnotes);
                        }
                    }
                }
                adminToCloseCase.Add(new AdminAllCaseViewModel
                {
                    RequestId = data.req.RequestId,
                    TypeOfRequestor = typeOfRequestor,
                    ReqTypeId = data.req.RequestTypeId,
                    FirstName = data.reqclient.FirstName,
                    LastName = data.reqclient.LastName,
                    DateOfBirth = date,
                    RequestorName = data.req.FirstName,
                    RequestedDate = data.req.CreatedDate,
                    Region = data.reqclient.State!.ToLower(),
                    Address = address,
                    TransferNotes = notes,
                    Email = data.reqclient.Email
                });
            }
            return adminToCloseCase;
        }

        public List<AdminAllCaseViewModel> AdminUnpaidCase(int id)
        {
            var query = (from req in _repository.RequestData()
                         join reqclient in _repository.RequestClientData()
                         on req.RequestId equals reqclient.RequestId
                         where (req.Status == 9)
                         select new
                         {
                             req,
                             reqclient
                         }).ToList();
            if (id != 0)
            {
                query = query.Where(r => r.req.RequestTypeId == id).ToList();
            }
            List<AdminAllCaseViewModel> adminUnpaidCase = new List<AdminAllCaseViewModel>();
            foreach (var data in query)
            {
                //var dateofbirth = 
                DateOnly? date = new DateOnly(data.reqclient.IntYear!.Value, DateOnly.ParseExact(data.reqclient.StrMonth!, "MMMM", CultureInfo.InvariantCulture).Month, data.reqclient.IntDate!.Value);
                string address = data.reqclient.Street + " " + data.reqclient.City + " " + data.reqclient.State + "," + data.reqclient.ZipCode;
                string typeOfRequestor = "";
                switch (data.req.RequestTypeId)
                {
                    case 1:
                        typeOfRequestor = "Business";
                        break;
                    case 2:
                        typeOfRequestor = "Patient";
                        break;
                    case 3:
                        typeOfRequestor = "Family/Friend";
                        break;
                    case 4:
                        typeOfRequestor = "Concierge";
                        break;
                    case 5:
                        typeOfRequestor = "VIP";
                        break;
                }
                string tnotes;
                List<string> notes = new List<string>();
                List<RequestStatusLog> reqlog = _repository.GetReqLogList(data.req.RequestId);
                foreach (var item in reqlog)
                {
                    if (item.TransToPhysicianId != null)
                    {
                        Physician phy = _repository.GetPhysiciianById(item.TransToPhysicianId);
                        string Createddate = item.CreatedDate.ToString();
                        if (item.AdminId != null)
                        {
                            tnotes = "Admin transferred to Dr." + phy.FirstName + " on " + Createddate.Substring(10) + " at " + Createddate.Substring(Createddate.Length - 9);
                            notes.Add(tnotes);
                        }
                        Physician phy1 = _repository.GetPhysiciianById(item.TransToPhysicianId);
                        if (item.PhysicianId != null)
                        {
                            tnotes = phy1.FirstName + " transferred to Dr." + phy.FirstName + " on " + Createddate.Substring(10) + " at " + Createddate.Substring(Createddate.Length - 9);
                            notes.Add(tnotes);
                        }
                    }
                }
                adminUnpaidCase.Add(new AdminAllCaseViewModel
                {
                    RequestId = data.req.RequestId,
                    TypeOfRequestor = typeOfRequestor,
                    ReqTypeId = data.req.RequestTypeId,
                    FirstName = data.reqclient.FirstName,
                    LastName = data.reqclient.LastName,
                    DateOfBirth = date,
                    RequestorName = data.req.FirstName,
                    RequestedDate = data.req.CreatedDate,
                    Mobile = data.reqclient.PhoneNumber,
                    RequestorMobile = data.req.PhoneNumber,
                    Address = address,
                    TransferNotes = notes,
                    Region = data.reqclient.State!.ToLower(),
                    Email = data.reqclient.Email
                });
            }
            return adminUnpaidCase;
        }

        public List<Region> AssignCaseRegion()
        {
            return _repository.AssignCaseRegion();
        }

        public AdminAssignCaseViewModel GetPhysicianRegionWise(int Id)
        {
            AdminAssignCaseViewModel vm = new();
            List<Region> regions = _repository.AssignCaseRegion();
            vm.AllRegion = regions;

            List<Physician> physicians = _repository.GetPhysicianRegionWise(Id);
            vm.AllPhysician = physicians;
            return vm;
        }

        public void PostAssignCase(int id, AdminAssignCaseViewModel vm, int? adminid)
        {
            Request req = _repository.GetRequestById(id);
            req.Status = 2;
            req.PhysicianId = vm.PhysicianId;
            req.ModifiedDate = DateTime.Now;
            _repository.UpdateRequest(req);

            RequestStatusLog reqlog = new();
            reqlog.Status = 2;
            reqlog.TransToPhysicianId = vm.PhysicianId;
            reqlog.AdminId = adminid;
            reqlog.CreatedDate = DateTime.Now;
            reqlog.RequestId = id;
            reqlog.Notes = vm.Description;
            _repository.SaveRequestStatusLog(reqlog);
        }

        public AdminCancleCaseViewModel CancelCaseData(int id)
        {
            AdminCancleCaseViewModel vm = new();
            vm.CaseTags = _repository.GetAllTags();
            vm.PatientName = _repository.GetPatientName(id);
            return vm;
        }

        public void PostCancleCase(int id, AdminCancleCaseViewModel vm, int? Admin_id)
        {
            Request req = _repository.GetRequestById(id);
            req.Status = 3;
            req.ModifiedDate = DateTime.Now;
            req.CaseTag = _repository.GetCaseTagNameById(vm.CaseTagId);
            _repository.UpdateRequest(req);

            RequestStatusLog reqlog = new();
            reqlog.Status = 3;
            reqlog.RequestId = id;
            reqlog.Notes = vm.AdditionalNotes;
            reqlog.CreatedDate = DateTime.Now;
            reqlog.AdminId = Admin_id;
            _repository.SaveRequestStatusLog(reqlog);
        }

        public AdminViewCaseViewModel AdminViewCase(int id)
        {
            Request req = _repository.GetRequestById(id);
            RequestClient data = _repository.AdminViewCase(id);
            Region region = _repository.GetRegionById(data.RegionId);
            DateOnly date = new DateOnly(data.IntYear.Value, DateOnly.ParseExact(data.StrMonth, "MMMM", CultureInfo.InvariantCulture).Month, data.IntDate.Value);
            AdminViewCaseViewModel vm = new()
            {
                RequestId = req.RequestId,
                ConformationNumber = req.ConfirmationNumber,
                Status = req.Status,
                RequestTypeId = req.RequestTypeId,
                PatientNotes = data.Notes,
                PatientFirstName = data.FirstName,
                PatientLastName = data.LastName,
                PatientBirthDate = date,
                PatientMobile = data.PhoneNumber,
                PatientEmail = data.Email,
                PatientRegion = region.Name,
                PatientAddress = data.Address,
            };
            return vm;
        }

        public AdminViewNotesViewModel AdminViewNotes(int id)
        {
            List<RequestStatusLog> reqstatuslog = _repository.GetReqStatusLogById(id);
            AdminViewNotesViewModel vm = new();
            List<string> temp = new List<string>();
            foreach (RequestStatusLog log in reqstatuslog)
            {
                if (log != null && log.TransToPhysicianId != null)
                {
                    Physician physician = _repository.GetPhysiciianById(log.TransToPhysicianId);
                    string date = log.CreatedDate.ToString();
                    string? tnote = "Admin transferred to Dr." + physician.FirstName + " on " + date.Substring(0, 10) + " at " + date.Substring(11);
                    temp.Add(tnote);
                }
            }
            vm.TransferNotes = temp;

            RequestNote reqnote = _repository.GetRequestNotesById(id);
            if (reqnote.PhysicianNotes != null)
            {
                vm.PhysicianNotes = reqnote.PhysicianNotes;
            }
            if (reqnote.AdminNotes != null)
            {
                vm.AdminNotes = reqnote.AdminNotes;
            }
            return vm;
        }

        public AdminBlockCaseViewModel GetBlockCaseData(int id)
        {
            AdminBlockCaseViewModel vm = new();
            Request req = _repository.GetRequestById(id);
            vm.Name = req.FirstName + " " + req.LastName;
            return vm;
        }

        public void AdminBlockCase(int id, AdminBlockCaseViewModel vm)
        {
            Request req = _repository.GetRequestById(id);
            req.ModifiedDate = DateTime.Now;
            req.Status = 11;
            _repository.UpdateRequest(req);

            RequestStatusLog reqlog = new();
            reqlog.RequestId = id;
            reqlog.Status = 11;
            reqlog.Notes = vm.ReasonForBlock;
            reqlog.CreatedDate = DateTime.Now;
            _repository.SaveRequestStatusLog(reqlog);

            BlockRequest blockreq = new();
            blockreq.PhoneNumber = req.PhoneNumber;
            blockreq.Email = req.Email;
            blockreq.Reason = vm.ReasonForBlock;
            blockreq.RequestId = id.ToString();
            blockreq.CreatedDate = DateTime.Now;
            _repository.SaveBlockRequest(blockreq);
        }

        public AdminViewUploadsViewModel AdminViewUploads(int id)
        {
            Request req = _repository.GetRequestById(id);
            AdminViewUploadsViewModel vm = new();
            vm.PatientName = req.FirstName + " " + req.LastName;
            vm.ConfirmationNumber = req.ConfirmationNumber;
            vm.RequestId = req.RequestId;
            List<RequestWiseFile> reqwisefile = _repository.GetReqWiseFile(id);
            if (reqwisefile != null)
            {
                List<DocumentsViewModel> docs = new List<DocumentsViewModel>();
                foreach (RequestWiseFile file in reqwisefile)
                {
                    docs.Add(new DocumentsViewModel()
                    {
                        DocumentName = file.FileName,
                        UploadDate = DateOnly.FromDateTime(file.CreatedDate),
                        ReqWiseFileId = file.RequestWiseFileId,
                        Isdeleted = file.IsDeleted
                    });
                }
                vm.Documents = docs;
            }
            return vm;
        }
        public Request GetRequestById(int id)
        {
            return _repository.GetRequestById(id);
        }

        public void AdminClearCase(int id)
        {
            Request req = _repository.GetRequestById(id);
            req.Status = 10;
            req.ModifiedDate = DateTime.Now;
            _repository.UpdateRequest(req);

            RequestStatusLog reqlog = new();
            reqlog.RequestId = req.RequestId;
            reqlog.Status = 10;
            reqlog.CreatedDate = DateTime.Now;
            _repository.SaveRequestStatusLog(reqlog);
        }

        public void SendMail(string email, string token)
        {
            var receiver = email;
            var subject = "HalloDoc-Agreement";
            var message = "Tap on link to Confirm the Agreement:https://localhost:7018/Patient/PatientSendAgreement/?id=" + token;//change link by original link with token appended

            var mail = "tatva.dotnet.deeppanchal@outlook.com";
            var password = "Deep@2829";
            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
        }
        public RequestClient GetReqClientById(int requestId)
        {
            return _repository.GetReqClientById(requestId);
        }
        public List<HealthProfessionalType>? GetAllProfession()
        {
            return _repository.GetAllProfession();
        }
        public List<HealthProfessional> GetHealthProfession(int healthprofessionalid)
        {
            return _repository.GetHealthProfession(healthprofessionalid);
        }
        public HealthProfessional GetVenderDetails(int healthvendorid)
        {
            return _repository.GetVenderDetails(healthvendorid);
        }
        public void ApplyForOrder(AdminSendOrderViewModel abc, int selectedPhysician)
        {
            var data = _repository.GetVenderDetails(abc.VenderId);
            data.FaxNumber = abc.FaxNumber;
            data.Email = abc.Email;
            data.VendorName = abc.BusinessContact;

            OrderDetail order = new()
            {
                VendorId = abc.VenderId,
                RequestId = abc.RequestId,
                FaxNumber = abc.FaxNumber,
                Email = abc.Email,
                BusinessContact = abc.BusinessContact,
                Prescription = abc.Prescription,
                NoOfRefill = selectedPhysician,
                CreatedDate = DateTime.Now,
            };
            _repository.AddOrder(order);
        }
        public void UploadNewFiles(List<IFormFile>? files, int reqid, int? adminId)
        {
            Admin admin = _repository.GetAdminById(adminId);
            foreach (var newfile in files)
            {
                string filename = admin.FirstName + admin.LastName + Path.GetExtension(newfile.FileName); //abc.txt
                string path = Path.Combine("D:\\Projects\\HalloDoc\\HalloDoc\\wwwroot\\Files1\\", filename); //Files1/abc.txt
                                                                                                             //Request req = await _context.Requests.FirstOrDefaultAsync(u => u.RequestId == newfile.ReqId);
                using (FileStream stream = new FileStream(path, FileMode.Create)) //only create but empty file
                {
                    newfile.CopyToAsync(stream).Wait(); //copy orignal file to File1 folder
                }

                RequestWiseFile requestWiseFile = new RequestWiseFile();
                requestWiseFile.FileName = filename;
                requestWiseFile.RequestId = reqid;
                requestWiseFile.DocType = 1;
                requestWiseFile.CreatedDate = DateTime.Now;
                requestWiseFile.AdminId = adminId;
                requestWiseFile.IsDeleted = false;
                _repository.AddReqWiseFile(requestWiseFile);
            }
        }
        public void DeleteReqFile(int id)
        {
            RequestWiseFile reqfile = _repository.GetReqWiseFileById(id);

            reqfile.IsDeleted = true;
            _repository.SaveChangeReqFile();
        }
        public void DeleteMultiple(int requestid, string fileId)
        {
            //RequestWiseFile rwf = _context.RequestWiseFiles.Where(r => r.RequestId == requestid).FirstOrDefault();
            //RequestWiseFile rwf = _adminRepository.ReturnRequestWiseFile(requestid);
            string[] fileid = fileId.Split(',').Select(x => x.Trim()).ToArray();
            for (int i = 0; i < fileid.Length; i++)
            {
                RequestWiseFile r = _repository.GetReqWiseFileById(int.Parse(fileid[i]));
                r.IsDeleted = true;
                _repository.SaveChangeReqFile();
            }
            //TempData["success"] = "File(s) deleted successfully";
        }
        public AdminCloseCaseViewModel AdminCloseCase(int id)
        {
            RequestClient reqclient = _repository.GetReqClientById(id);
            AdminCloseCaseViewModel vm = new();
            vm.FirstName = reqclient.FirstName;
            vm.LastName = reqclient.LastName;
            vm.Mobile = reqclient.PhoneNumber;
            vm.Email = reqclient.Email;
            vm.DateOfBirth = new DateOnly(reqclient.IntYear.Value, DateOnly.ParseExact(reqclient.StrMonth, "MMMM", CultureInfo.InvariantCulture).Month, reqclient.IntDate.Value);
            Request req = _repository.GetRequestById(id);
            vm.ConfirmationNumber = req.ConfirmationNumber;
            List<RequestWiseFile> reqwisefile = _repository.GetReqWiseFile(id);
            if (reqwisefile != null)
            {
                List<DocumentsViewModel> docs = new List<DocumentsViewModel>();
                foreach (RequestWiseFile file in reqwisefile)
                {
                    docs.Add(new DocumentsViewModel()
                    {
                        DocumentName = file.FileName,
                        UploadDate = DateOnly.FromDateTime(file.CreatedDate),
                        ReqWiseFileId = file.RequestWiseFileId,
                        Isdeleted = file.IsDeleted
                    });
                }
                vm.Documents = docs;
            }
            return vm;
        }
        public void EditReqClient(int id, AdminCloseCaseViewModel vm)
        {
            RequestClient reqclient = _repository.GetReqClientById(id);
            if (vm.Email != null)
            {
                reqclient.Email = vm.Email;
            }
            if (vm.Mobile != null)
            {
                reqclient.PhoneNumber = vm.Mobile;
            }
            _repository.UpdateReqClient(reqclient);
        }
        public IQueryable<NewStateAdminViewModel> ExportAll()
        {
            return _repository.ExportAll();
        }

        public void PostCloseCase(int id, int? AdminId)
        {
            Request req = _repository.GetRequestById(id);
            req.ModifiedDate = DateTime.Now;
            req.Status = 8;
            _repository.UpdateRequest(req);

            RequestStatusLog reqlog = new();
            reqlog.RequestId = id;
            reqlog.Status = 8;
            reqlog.AdminId = AdminId;
            reqlog.CreatedDate = DateTime.Now;
            _repository.SaveRequestStatusLog(reqlog);
        }
        public AdminProfileViewModel AdminProfile(int? adminId)
        {
            AdminProfileViewModel vm = new AdminProfileViewModel();
            Admin admin = _repository.GetAdminById(adminId);
            AspNetUser aspuser = _repository.GetAspNetUserById(admin.AspNetUserId);

            vm.Username = aspuser.UserName;
            vm.AdminFirstName = admin.FirstName;
            vm.AdminLastName = admin.LastName;
            vm.AdminEmail = admin.Email;
            vm.AdminConfirmEmail = admin.Email;
            vm.AdminPhone = admin.Mobile;
            vm.AdminPhone2 = admin.AltPhone;
            vm.Address1 = admin.Address1;
            vm.Address2 = admin.Address2;
            vm.City = admin.City;
            vm.State = _repository.GetRegionById(admin.RegionId).Name;
            vm.Zipcode = admin.Zip;
            List<AdminSelectedRegionViewModel> adminSelectedRegionViewModels = new List<AdminSelectedRegionViewModel>();
            List<Region>? reg = _repository.GetAllRegions();
            foreach (Region regdata in reg)
            {
                bool adminreg = _repository.GetAdminRegionInBool(regdata.RegionId, adminId);

                adminSelectedRegionViewModels.Add(new AdminSelectedRegionViewModel()
                {
                    RegionId = regdata.RegionId,
                    RegionName = regdata.Name,
                    IsSelected = adminreg
                });
            }
            vm.AdminSelectedRegions = adminSelectedRegionViewModels;
            return vm;
        }
        public void AdminResetPassword(int? adminId, string password)
        {
            Admin admin = _repository.GetAdminById(adminId);
            AspNetUser aspuser = _repository.GetAspNetUserById(admin.AspNetUserId);
            aspuser.PasswordHash = password;
            _repository.UpdateAspNetUser(aspuser);
        }
        public void AdminInfoReset(AdminProfileViewModel vm, int? adminId)
        {
            Admin admin = _repository.GetAdminById(adminId);
            admin.FirstName = vm.AdminFirstName;
            admin.LastName = vm.AdminLastName;
            admin.Email = vm.AdminEmail;
            admin.Mobile = vm.AdminPhone;
            _repository.UpdateAdmin(admin);

            AspNetUser aspuser = _repository.GetAspNetUserById(admin.AspNetUserId);
            aspuser.Email = vm.AdminEmail;
            aspuser.PhoneNumber = vm.AdminPhone;
            _repository.UpdateAspNetUser(aspuser);

            if (vm.AdminChangedRegion != null)
            {
                _repository.DeleteAdminRegion(adminId);
                foreach (var selectedRegion in vm.AdminChangedRegion)
                {
                    AdminRegion adminRegion = new();
                    adminRegion.AdminId = (int)adminId;
                    adminRegion.RegionId = selectedRegion;
                    _repository.AddAdminRegion(adminRegion);
                }
            }
        }
        public void AdminMailingBillingInfoReset(AdminProfileViewModel vm, int? adminId)
        {
            Admin admin = _repository.GetAdminById(adminId);
            admin.Address1 = vm.Address1;
            admin.Address2 = vm.Address2;
            admin.City = vm.City;
            admin.Zip = vm.Zipcode;
            admin.AltPhone = vm.AdminPhone2;
            _repository.UpdateAdmin(admin);
        }
        public void SavingAdminNotes(int id, AdminViewNotesViewModel vm, int? adminId)
        {
            RequestNote reqNote = _repository.GetRequestNotesById(id);
            int aspuserid = _repository.GetAdminById((int)adminId).AspNetUserId;
            if (vm.AdminAdditionalNotes != null)
            {
                if (reqNote.RequestNotesId == 0)
                {
                    RequestNote reqnote1 = new();
                    reqnote1.AdminNotes = vm.AdminAdditionalNotes;
                    reqnote1.CreatedBy = aspuserid;
                    reqnote1.CreatedDate = DateTime.Now;
                    reqnote1.RequestId = id;
                    _repository.AddReqNotes(reqnote1);
                    return;
                }
                else
                {
                    reqNote.AdminNotes = vm.AdminAdditionalNotes;
                    reqNote.ModifiedBy = aspuserid;
                    reqNote.ModifiedDate = DateTime.Now;
                    reqNote.RequestId = id;
                    _repository.UpdateReqNotes(reqNote);
                }
            }
        }
        public AdminCreateProviderAccountViewModel AdminCreateProviderAccount()
        {
            AdminCreateProviderAccountViewModel vm = new();
            vm.Region = _repository.GetAllRegions();
            vm.Role = _repository.GetAllRoles();
            return vm;
        }

        public void AdminCreateProviderAccount(AdminCreateProviderAccountViewModel vm, int? adminid)
        {
            AspNetUser aspuser = _repository.GetAspNetUserByEmail(vm.Email);
            if (aspuser != null)
            {
                AspNetUser aspuser1 = new()
                {
                    UserName = vm.UserName,
                    PasswordHash = vm.Password,
                    Email = vm.Email,
                    PhoneNumber = vm.PhoneNumber,
                    CreatedDate = DateTime.Now,
                };
                aspuser = aspuser1;
                _repository.AddAspNetUser(aspuser1);
            }
            Physician phy = _repository.GetPhysiciianByEmail(vm.Email);
            if (phy != null)
            {
                Admin admin = _repository.GetAdminById(adminid);
                Physician physician = new()
                {
                    AspNetUserId = aspuser.Id,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    Email = vm.Email,
                    Mobile = vm.PhoneNumber,
                    MedicalLicense = vm.MedicalLicense,
                    AdminNotes = vm.AdminNotes,
                    Address1 = vm.Address1,
                    Address2 = vm.Address2,
                    City = vm.City,
                    RegionId = vm.RegionId,
                    Zip = vm.Zipcode,
                    AltPhone = vm.PhoneNumber2,
                    CreatedBy = admin.AspNetUserId,
                    CreatedDate = DateTime.Now,
                    BusinessName = vm.BusinessName,
                    BusinessWebsite = vm.BusinessWebsite,
                    RoleId = vm.RoleId,
                    Npinumber = vm.NPINumber,
                    IsAgreementDoc = vm.IndependentContractorAgrement != null ? true : false,
                    IsBackgroundDoc = vm.BackgroundCheck != null ? true : false,
                    IsTrainingDoc = vm.HIPAACompliance != null ? true : false,
                    IsNonDisclosureDoc = vm.NonDisclosureAgreement != null ? true : false,
                };
                _repository.AddPhysician(physician);
                string filename = physician.PhysicianId.ToString() + "Photo" + Path.GetExtension(vm.Photo.FileName);
                physician.Photo = filename;
                _repository.UpdatePhysician(physician);

                string path = Path.Combine("D:\\Projects\\HalloDoc-N-Tier\\HalloDoc-N-Tier\\wwwroot\\PhysicianData\\", filename); //Files1/abc.txt
                using (FileStream stream = new FileStream(path, FileMode.Create)) //only create but empty file
                {
                    vm.Photo.CopyToAsync(stream).Wait(); //copy orignal file to File1 folder
                }

                if (vm.IndependentContractorAgrement != null)
                {
                    filename = physician.PhysicianId.ToString() + "AgreementDoc" + Path.GetExtension(vm.IndependentContractorAgrement.FileName);
                    path = Path.Combine("D:\\Projects\\HalloDoc-N-Tier\\HalloDoc-N-Tier\\wwwroot\\PhysicianData\\", filename); //Files1/abc.txt
                    using (FileStream stream = new FileStream(path, FileMode.Create)) //only create but empty file
                    {
                        vm.IndependentContractorAgrement.CopyToAsync(stream).Wait(); //copy orignal file to File1 folder
                    }
                }

                if (vm.BackgroundCheck != null)
                {
                    filename = physician.PhysicianId.ToString() + "BackgroundCheck" + Path.GetExtension(vm.BackgroundCheck.FileName);
                    path = Path.Combine("D:\\Projects\\HalloDoc-N-Tier\\HalloDoc-N-Tier\\wwwroot\\PhysicianData\\", filename); //Files1/abc.txt
                    using (FileStream stream = new FileStream(path, FileMode.Create)) //only create but empty file
                    {
                        vm.BackgroundCheck.CopyToAsync(stream).Wait(); //copy orignal file to File1 folder
                    }
                }

                if (vm.HIPAACompliance != null)
                {
                    filename = physician.PhysicianId.ToString() + "LicenseDoc" + Path.GetExtension(vm.HIPAACompliance.FileName);
                    path = Path.Combine("D:\\Projects\\HalloDoc-N-Tier\\HalloDoc-N-Tier\\wwwroot\\PhysicianData\\", filename); //Files1/abc.txt
                    using (FileStream stream = new FileStream(path, FileMode.Create)) //only create but empty file
                    {
                        vm.HIPAACompliance.CopyToAsync(stream).Wait(); //copy orignal file to File1 folder
                    }
                }

                if (vm.NonDisclosureAgreement != null)
                {
                    filename = physician.PhysicianId.ToString() + "NonDisclosureDoc" + Path.GetExtension(vm.NonDisclosureAgreement.FileName);
                    path = Path.Combine("D:\\Projects\\HalloDoc-N-Tier\\HalloDoc-N-Tier\\wwwroot\\PhysicianData\\", filename); //Files1/abc.txt
                    using (FileStream stream = new FileStream(path, FileMode.Create)) //only create but empty file
                    {
                        vm.NonDisclosureAgreement.CopyToAsync(stream).Wait(); //copy orignal file to File1 folder
                    }
                }
                foreach (var regionid in vm.SelectedRegions)
                {
                    PhysicianRegion phyregion = new()
                    {
                        PhysicianId = physician.PhysicianId,
                        PhysicianRegionId = regionid.RegionId
                    };
                    _repository.AddPhysicianRegion(phyregion);
                }
            }
        }

        public List<AdminProviderInfoViewModel> AdminProviderInformation()
        {
            return new();
        }

        public List<HalloDoc_N_Tier_Entity.DataModels.Menu> GetMenu(int AccType)
        {
            if (AccType == 0)
            {
                return _repository.GetMenu(AccType);
            }
            else
            {
                return _repository.GetMenuByRoleId(AccType);
            }
        }

        public void AdminCreateRole(AdminCreateRoleViewModel vm, int? AdminId)
        {

            if (vm.RoleName != null && vm.RoleId != 0)
            {
                Role role = new()
                {
                    Name = vm.RoleName,
                    AccountType = (short)vm.RoleId,
                    CreatedBy = AdminId.ToString(),
                    CreatedDate = DateTime.Now,
                    IsDeleted = new BitArray(new[] { false })
                };
                _repository.SaveRole(role);

                foreach (var menu in vm.SelectedMenuId)
                {
                    RoleMenu rolemenu = new()
                    {
                        RoleId = role.RoleId,
                        MenuId = menu
                    };
                    _repository.SaveRoleMenu(rolemenu);
                }

            }
            else
            {

            }
        }

        public List<AdminAccountAccessViewModel> AdminAccountAccess()
        {
            List<Role> role = _repository.GetAllRolesFromTable();
            List<AdminAccountAccessViewModel> access = new();
            foreach (var roledata in role)
            {
                Dictionary<int, string> acctype = new Dictionary<int, string>
                {
                    {1,"Admin"},
                    {2,"Patient"},
                    {3,"Physician"}
                };
                access.Add(new AdminAccountAccessViewModel()
                {
                    AccountName = roledata.Name,
                    AccountType = acctype[roledata.AccountType],
                    AccountId = roledata.RoleId
                });
            }
            return access;
        }

        public AdminSchedulingViewModel AdminScheduling()
        {
            AdminSchedulingViewModel adminScheduling = new();
            adminScheduling.RegionList = _repository.GetAllRegions();
            return adminScheduling;
        }

        public void AdminCreateShift(AdminSchedulingViewModel vm, int? adminId)
        {
            Admin admin = _repository.GetAdminById(adminId);
            Shift shift = new()
            {
                PhysicianId = vm.SelectedPhysicianId,
                StartDate = DateOnly.FromDateTime(vm.ShiftDate),
                RepeatUpto = vm.NumberOfTimesToRepeat,
                CreatedBy = admin.AspNetUserId,
                CreatedDate = DateTime.Now,
                IsRepeat = vm.NumberOfTimesToRepeat > 1,
            };
            string repeat = "";
            if (vm.SelectedDays != null)
            {
                for (int i = 1; i <= 7; i++)
                {
                    bool flag = false;
                    foreach (int days in vm.SelectedDays)
                    {
                        if (days == i) { flag = true; break; }
                    }
                    if (flag)
                    {
                        repeat += 1;
                    }
                    else { repeat += 0; }
                }
            }
            else
            {
                repeat = "0000000";
            }
            shift.WeekDays = repeat;
            _repository.AddShift(shift);

            //Code to store data in shiftdetail table
            if (repeat == "0000000")
            {
                ShiftDetail shiftdetail = new()
                {
                    ShiftId = shift.ShiftId,
                    ShiftDate = vm.ShiftDate,
                    RegionId = vm.SelectedRegionId,
                    StartTime = vm.StartTime,
                    EndTime = vm.EndTime,
                    Status = 1,//status pending
                    //IsDeleted = new BitArray(new[] { false })
                };
                _repository.AddShiftDetail(shiftdetail);
                ShiftDetailRegion sdregion = new()
                {
                    ShiftDetailId = shiftdetail.ShiftDetailId,
                    RegionId = vm.SelectedRegionId
                };
            }
            else
            {
                for (int repeatcount = 0; repeatcount < vm.NumberOfTimesToRepeat; repeatcount++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        char day = repeat[j];
                        if (day == '1')
                        {
                            ShiftDetail shiftdetail = new()
                            {
                                ShiftId = shift.ShiftId,
                                ShiftDate = vm.ShiftDate.AddDays((7 * repeatcount) + j),
                                RegionId = vm.SelectedRegionId,
                                StartTime = vm.StartTime,
                                EndTime = vm.EndTime,
                                Status = 1,//status pending
                                //IsDeleted = new BitArray(new[] { false })
                            };
                            _repository.AddShiftDetail(shiftdetail);
                            ShiftDetailRegion sdregion = new()
                            {
                                ShiftDetailId = shiftdetail.ShiftDetailId,
                                RegionId = vm.SelectedRegionId
                            };
                        }
                    }
                }
            }
        }

        public List<AdminPhysicianShiftDetailsViewModel> AdminDayWiseScheduling(int day, int month, int year)
        {
            List<AdminPhysicianShiftDetailsViewModel> vm = new();
            var phy = _repository.GetAllPhysician();
            foreach (var phydata in phy)
            {
                AdminPhysicianShiftDetailsViewModel apsdvm = new()
                {
                    PhysicianDetails = phydata
                };
                if (phydata.Shifts.Count > 0)
                {
                    List<ShiftDetail> shiftdetail = new();
                    foreach (var shift in phydata.Shifts)
                    {
                        foreach (var shiftDetails in shift.ShiftDetails.Where(x => x.ShiftDate.Day == day && x.ShiftDate.Month == month && x.ShiftDate.Year == year && x.IsDeleted == null).ToList())
                        {
                            shiftdetail.Add(shiftDetails);
                        }
                    }
                    apsdvm.PhysicianShiftDetail = shiftdetail;
                }
                vm.Add(apsdvm);
            }
            return vm;
        }




    }
}
