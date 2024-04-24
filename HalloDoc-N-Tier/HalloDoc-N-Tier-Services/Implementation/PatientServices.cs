using HalloDoc_N_Tier_Entity.DataModels;
using HalloDoc_N_Tier_Entity.ViewModel;
using HalloDoc_N_Tier_Repository.Interface;
using HalloDoc_N_Tier_Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace HalloDoc_N_Tier_Services.Implementation
{
    public class PatientServices : IPatientServices
    {
        private readonly IPatientRepo _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PatientServices(IPatientRepo repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }

        public GenetareTokenViewModel PatientLogin(LoginViewModel user)
        {
            AspNetUser aspuser = _repository.GetAspNetUser(user.Email);
            if(aspuser.Id != 0 && Crypto.VerifyHashedPassword(aspuser.PasswordHash,user.Password))
            {
                User user1 = _repository.GetUserByAspId(aspuser.Id);
                AspNetUserRole userrole = _repository.GetRoleByAspId(aspuser.Id);
                AspNetRole role = _repository.GetRole(userrole.RoleId);
                GenetareTokenViewModel vm = new();
                vm.Email = aspuser.Email;
                vm.Username = aspuser.UserName;
                vm.User_Id = user1.UserId;
                vm.Role = role.Name;
                return vm;
            }
            return new();
        }

        public Region CheckRegion(string state)
        {
            Region region = _repository.CheckRegion(state);
            return region;
        }


        public void PatientRequest(PatientRequestViewModel userpatient, Region region)
        {
            AspNetUser aspnetuser = _repository.CheckAspNetUser(userpatient.Email);
            if (aspnetuser == null)
            {
                AspNetUser aspnetuser1 = new()
                {
                    UserName = userpatient.FirstName + " " + userpatient.LastName,
                    PasswordHash = Crypto.HashPassword(userpatient.Password),
                    Email = userpatient.Email,
                    PhoneNumber = userpatient.Mobile,
                    CreatedDate = DateTime.Now
                };
                _repository.AddAspUser(aspnetuser1);
                aspnetuser = aspnetuser1;
            }

            AspNetUserRole aspuserrole = _repository.CheckAspNetUserRole(aspnetuser.Id);
            if (aspuserrole == null)
            {
                AspNetUserRole userrole = new()
                {
                    UserId = aspnetuser.Id,
                    RoleId = 2
                };
                _repository.AddAspNetUserRole(userrole);
            }

            User user1 = _repository.CheckUser(userpatient.Email);
            if (user1 == null)
            {
                User user2 = new()
                {
                    AspNetUserId = aspnetuser.Id,
                    FirstName = userpatient.FirstName,
                    LastName = userpatient.LastName,
                    Email = userpatient.Email,
                    Mobile = userpatient.Mobile,
                    Street = userpatient.Street,
                    City = userpatient.City,
                    State = userpatient.State,
                    ZipCode = userpatient.ZipCode,
                    RegionId = region.RegionId,
                    CreatedBy = aspnetuser.Id,
                    CreatedDate = DateTime.Now,
                    Status = 1,
                    IntDate = userpatient.DateOfBirth.Day,
                    IntYear = userpatient.DateOfBirth.Year,
                    StrMonth = userpatient.DateOfBirth.ToString("MMMM"),
                };
                _repository.AddUser(user2);
                user1 = user2;
            }
            int count = _repository.GetRequestToday();
            string number = "0000" + count.ToString();
            string userdate = "0" + DateTime.Now.Day.ToString();
            string usermonth = "0" + DateTime.Now.Month.ToString();
            string useryear = DateTime.Now.Year.ToString();
            string conNumber = region.Abbreviation + userdate.Substring(userdate.Length - 2) + usermonth.Substring(usermonth.Length - 2) + useryear.Substring(useryear.Length - 2) + userpatient.FirstName.Substring(0, 2).ToUpper() + userpatient.LastName.Substring(0, 2).ToUpper() + number.Substring(number.Length - 4);
            Request request = new()
            {
                RequestTypeId = 2, //1-Bussiness 2-Patient 3-Family/Friend 4-Concierge 5-VIP
                UserId = user1.UserId,
                FirstName = userpatient.FirstName,
                LastName = userpatient.LastName,
                PhoneNumber = userpatient.Mobile,
                Email = userpatient.Email,
                Status = 1,//this tells that case is unassigned
                CreatedDate = DateTime.Now,
                ConfirmationNumber = conNumber,
                PatientAccountId = aspnetuser.Id.ToString()
            };
            _repository.AddRequest(request);

            RequestClient reqclient = new()
            {
                RequestId = request.RequestId,
                FirstName = userpatient.FirstName,
                LastName = userpatient.LastName,
                PhoneNumber = userpatient.Mobile,
                Address = userpatient.Room + " " + userpatient.Street + " " + userpatient.City + " " + userpatient.State + "," + userpatient.ZipCode,
                RegionId = region.RegionId,
                Notes = userpatient.Symptoms,
                Email = userpatient.Email,
                Street = userpatient.Street,
                City = userpatient.City,
                State = userpatient.State,
                ZipCode = userpatient.ZipCode,
                StrMonth = userpatient.DateOfBirth.ToString("MMMM"),
                IntYear = userpatient.DateOfBirth.Year,
                IntDate = userpatient.DateOfBirth.Day,
            };
            _repository.AddRequestClient(reqclient);

            if (userpatient.file != null)
            {
                foreach (IFormFile files in userpatient.file)
                {
                    string filename = userpatient.FirstName + userpatient.LastName + Path.GetExtension(files.FileName);
                    string path = Path.Combine(_webHostEnvironment.WebRootPath + "Files\\", filename);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        files.CopyToAsync(stream).Wait();
                    }
                    RequestWiseFile reqwisefile = new()
                    {
                        RequestId = request.RequestId,
                        FileName = filename,
                        CreatedDate = DateTime.Now,
                        DocType = 1
                    };
                    _repository.AddRequestWiseFile(reqwisefile);
                }
            }
        }

        public bool CheckEmail(string email)
        {
            if (email != null)
            {
                return _repository.CheckEmail(email);
            }
            else
            {
                return false;
            }
        }

        public void FamilyFriendRequest(PatientFamilyFriendRequestViewModel userpatient, Region region)
        {
            AspNetUser aspuser = _repository.CheckAspNetUser(userpatient.RelativeEmail);
            if (aspuser == null)
            {
                //details for receiver
                var receiver = userpatient.RelativeEmail;
                var subject = "Create Assount";
                var message = "Tap on link to Create Account:link";//change link by original link with token appended

                var mail = "tatva.dotnet.deeppanchal@outlook.com";
                var password = "Deep@2829";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
            }

            AspNetUser aspuserpatient = _repository.CheckAspNetUser(userpatient.PatientEmail);
            if (aspuserpatient == null)
            {
                //details for receiver
                var receiver = userpatient.PatientEmail;
                var subject = "Create Assount";
                var message = "Tap on link to Create Account:link";//change link by original link with token appended

                var mail = "tatva.dotnet.deeppanchal@outlook.com";
                var password = "Deep@2829";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
            }
            int count = _repository.GetRequestToday();
            string number = "0000" + count.ToString();
            string userdate = "0" + DateTime.Now.Day;
            string usermonth = "0" + DateTime.Now.Month;
            string useryear = DateTime.Now.Year.ToString();
            string conNumber = region.Abbreviation + userdate.Substring(userdate.Length - 2) + usermonth.Substring(usermonth.Length - 2) + useryear.Substring(useryear.Length - 2) + userpatient.PatientFirstName.Substring(0, 2).ToUpper() + userpatient.PatientLastName.Substring(0, 2).ToUpper() + number.Substring(number.Length - 4);
            Request req = new()
            {
                RequestTypeId = 3,//Requested by Family/Friend
                FirstName = userpatient.RelativeFirstName,
                LastName = userpatient.RelativeLastName,
                PhoneNumber = userpatient.RelativeMobile,
                Email = userpatient.RelativeEmail,
                CreatedDate = DateTime.Now,
                ConfirmationNumber = conNumber,
                Status = 1,
            };
            _repository.AddRequest(req);

            RequestClient reqclient = new()
            {
                RequestId = req.RequestId,
                FirstName = userpatient.PatientFirstName,
                LastName = userpatient.PatientLastName,
                PhoneNumber = userpatient.PatientMobile,
                Address = userpatient.PatientRoom + " " + userpatient.PatientStreet + " " + userpatient.PatientCity + " " + userpatient.PatientState + "," + userpatient.PatientZipCode,
                RegionId = region.RegionId,
                Email = userpatient.PatientEmail,
                Notes = userpatient.PatientSymptoms,
                StrMonth = userpatient.PatientDateOfBirth.Value.ToString("MMMM"),
                IntYear = userpatient.PatientDateOfBirth.Value.Year,
                IntDate = userpatient.PatientDateOfBirth.Value.Day,
                Street = userpatient.PatientRoom + " " + userpatient.PatientStreet,
                City = userpatient.PatientCity,
                State = userpatient.PatientState,
                ZipCode = userpatient.PatientZipCode,
            };
            _repository.AddRequestClient(reqclient);

            if (userpatient.file != null)
            {
                foreach (IFormFile files in userpatient.file)
                {
                    string filename = userpatient.PatientFirstName + userpatient.PatientLastName + Path.GetExtension(files.FileName);
                    string path = Path.Combine("D:\\Projects\\HalloDoc-N-Tier\\HalloDoc-N-Tier\\wwwroot\\Files\\", filename);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        files.CopyToAsync(stream).Wait();
                    }
                    RequestWiseFile reqwisefile = new()
                    {
                        RequestId = req.RequestId,
                        FileName = filename,
                        CreatedDate = DateTime.Now,
                        DocType = 1
                    };
                    _repository.AddRequestWiseFile(reqwisefile);
                }
            }

        }

        public void ConciergeRequest(PatientConciergeRequestViewModel userpatient, Region region)
        {
            AspNetUser aspuserconcierge = _repository.CheckAspNetUser(userpatient.ConciergeEmail);
            if (aspuserconcierge == null)
            {
                //details for receiver
                var receiver = userpatient.ConciergeEmail;
                var subject = "Create Assount";
                var message = "Tap on link to Create Account:link";//change link by original link with token appended

                var mail = "tatva.dotnet.deeppanchal@outlook.com";
                var password = "Deep@2829";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
            }
            AspNetUser aspuserpatient = _repository.CheckAspNetUser(userpatient.PatientEmail);
            if (aspuserpatient == null)
            {
                //details for receiver
                var receiver = userpatient.PatientEmail;
                var subject = "Create Assount";
                var message = "Tap on link to Create Account:link";//change link by original link with token appended

                var mail = "tatva.dotnet.deeppanchal@outlook.com";
                var password = "Deep@2829";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
            }
            Concierge concierge = new Concierge()
            {
                ConciergeName = userpatient.ConciergeEmail,
                Address = userpatient.PatientRoom + " " + userpatient.ConciergeStreet + " " + userpatient.ConciergeCity + " " + userpatient.ConciergeState + "," + userpatient.ConciergeZipCode,
                Street = userpatient.ConciergeStreet,
                City = userpatient.ConciergeCity,
                State = userpatient.ConciergeState,
                ZipCode = userpatient.ConciergeZipCode,
                CreatedDate = DateTime.Now,
                RegionId = region.RegionId

            };
            _repository.AddConcierge(concierge);
            int count = _repository.GetRequestToday();
            string number = "0000" + count.ToString();
            string userdate = "0" + DateTime.Now.Day;
            string usermonth = "0" + DateTime.Now.Month;
            string useryear = DateTime.Now.Year.ToString();
            string conNumber = region.Abbreviation + userdate.Substring(userdate.Length - 2) + usermonth.Substring(usermonth.Length - 2) + useryear.Substring(useryear.Length - 2) + userpatient.PatientFirstName.Substring(0, 2).ToUpper() + userpatient.PatientLastName.Substring(0, 2).ToUpper() + number.Substring(number.Length - 4);
            Request req = new()
            {
                RequestTypeId = 4,//Requested by Concierge
                FirstName = userpatient.ConciergeFirstName,
                LastName = userpatient.ConciergeLastName,
                PhoneNumber = userpatient.ConciergeMobile,
                Email = userpatient.ConciergeEmail,
                CreatedDate = DateTime.Now,
                ConfirmationNumber = conNumber,
                Status = 1,
            };
            _repository.AddRequest(req);

            RequestClient reqclient = new()
            {
                RequestId = req.RequestId,
                FirstName = userpatient.PatientFirstName,
                LastName = userpatient.PatientLastName,
                PhoneNumber = userpatient.PatientMobile,
                Address = userpatient.PatientRoom + " " + userpatient.ConciergeStreet + " " + userpatient.ConciergeCity + " " + userpatient.ConciergeState + "," + userpatient.ConciergeZipCode,
                RegionId = region.RegionId,
                Email = userpatient.PatientEmail,
                Notes = userpatient.PatientSymptoms,
                StrMonth = userpatient.PatientDateOfBirth.Value.ToString("MMMM"),
                IntYear = userpatient.PatientDateOfBirth.Value.Year,
                IntDate = userpatient.PatientDateOfBirth.Value.Day,
                Street = userpatient.PatientRoom + " " + userpatient.ConciergeStreet,
                City = userpatient.ConciergeCity,
                State = userpatient.ConciergeState,
                ZipCode = userpatient.ConciergeZipCode,
            };
            _repository.AddRequestClient(reqclient);
        }

        public void BusinessRequest(PatientBusinessRequestViewModel userpatient, Region region)
        {
            AspNetUser aspuserbusiness = _repository.CheckAspNetUser(userpatient.BusinessEmail);
            if (aspuserbusiness == null)
            {
                //details for receiver
                var receiver = userpatient.BusinessEmail;
                var subject = "Create Assount";
                var message = "Tap on link to Create Account:link";//change link by original link with token appended

                var mail = "tatva.dotnet.deeppanchal@outlook.com";
                var password = "Deep@2829";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
            }
            AspNetUser aspuserpatient = _repository.CheckAspNetUser(userpatient.PatientEmail);
            if (aspuserpatient == null)
            {
                //details for receiver
                var receiver = userpatient.PatientEmail;
                var subject = "Create Assount";
                var message = "Tap on link to Create Account:link";//change link by original link with token appended

                var mail = "tatva.dotnet.deeppanchal@outlook.com";
                var password = "Deep@2829";

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
            }
            int count = _repository.GetRequestToday();
            string number = "0000" + count.ToString();
            string userdate = "0" + DateTime.Now.Day;
            string usermonth = "0" + DateTime.Now.Month;
            string useryear = DateTime.Now.Year.ToString();
            string conNumber = region.Abbreviation + userdate.Substring(userdate.Length - 2) + usermonth.Substring(usermonth.Length - 2) + useryear.Substring(useryear.Length - 2) + userpatient.PatientFirstName.Substring(0, 2).ToUpper() + userpatient.PatientLastName.Substring(0, 2).ToUpper() + number.Substring(number.Length - 4);
            Request req = new()
            {
                RequestTypeId = 1,//Requested by Business
                FirstName = userpatient.BusinessFirstName,
                LastName = userpatient.BusinessLastName,
                PhoneNumber = userpatient.BusinessMobile,
                Email = userpatient.BusinessEmail,
                CreatedDate = DateTime.Now,
                ConfirmationNumber = conNumber,
                Status = 1,
            };
            _repository.AddRequest(req);

            RequestClient reqclient = new()
            {
                RequestId = req.RequestId,
                FirstName = userpatient.PatientFirstName,
                LastName = userpatient.PatientLastName,
                PhoneNumber = userpatient.PatientMobile,
                Address = userpatient.PatientRoom + " " + userpatient.PatientStreet + " " + userpatient.PatientCity + " " + userpatient.PatientState + "," + userpatient.PatientZipCode,
                RegionId = region.RegionId,
                Email = userpatient.PatientEmail,
                Notes = userpatient.PatientSymptoms,
                StrMonth = userpatient.PatientDateOfBirth.Value.ToString("MMMM"),
                IntYear = userpatient.PatientDateOfBirth.Value.Year,
                IntDate = userpatient.PatientDateOfBirth.Value.Day,
                Street = userpatient.PatientRoom + " " + userpatient.PatientStreet,
                City = userpatient.PatientCity,
                State = userpatient.PatientState,
                ZipCode = userpatient.PatientZipCode,
            };
            _repository.AddRequestClient(reqclient);
        }

        public List<PatientDashboardViewModel> GetDashboardData(int? id)
        {
            List<Request> reqdata = _repository.GetDashboardData(id);
            List<PatientDashboardViewModel> vm = new();

            foreach (Request request in reqdata)
            {
                vm.Add(new PatientDashboardViewModel
                {
                    CreatedDate = request.CreatedDate,
                    CurrentStatus = request.Status,
                    RequestId = request.RequestId,
                    filecount = _repository.GetFileCount(request.RequestId),
                });
            }

            return vm;

        }
        public PatientViewDocumentViewModel GetDocument(int id)
        {
            List<RequestWiseFile> reqfile = _repository.GetReqWiseFileById(id);
            PatientViewDocumentViewModel vm = new();
            vm.ConfirmationNumber = _repository.GetRequest(id).ConfirmationNumber;
            List<DocumentsViewModel> document = new();
            foreach (RequestWiseFile req in reqfile)
            {
                if (req.AdminId != null)
                {
                    Admin adm = _repository.GetAdminById(req.AdminId);
                    document.Add(new()
                    {
                        Uploader = adm.FirstName,
                        UploadDate = DateOnly.FromDateTime(req.CreatedDate),
                        DocumentName = req.FileName,
                        ReqWiseFileId = req.RequestWiseFileId
                    });
                }
                else if (req.PhysicianId != null)
                {
                    Physician phy = _repository.GetPhysicianById(req.PhysicianId);
                    document.Add(new()
                    {
                        Uploader = phy.FirstName,
                        UploadDate = DateOnly.FromDateTime(req.CreatedDate),
                        DocumentName = req.FileName,
                        ReqWiseFileId = req.RequestWiseFileId
                    });
                }
                else
                {
                    Request request = _repository.GetRequest(req.RequestId);
                    document.Add(new()
                    {
                        Uploader = request.FirstName,
                        UploadDate = DateOnly.FromDateTime(req.CreatedDate),
                        DocumentName = req.FileName,
                        ReqWiseFileId = req.RequestWiseFileId
                    });
                }
            }
            vm.Documents = document;
            return vm;
        }

        public void AcceptSendAgreement(Request req)
        {
            req.Status = 4;
            req.ModifiedDate = DateTime.Now;
            _repository.UpdateReqTable(req);
            RequestStatusLog reqlog = new()
            {
                RequestId = req.RequestId,
                Status = 4,
                CreatedDate = DateTime.Now,
            };
            _repository.AddReqStatusLog(reqlog);
        }
        public PatientRequestViewModel PatientSubmitInformationMe(int? userId)
        {
            User user = _repository.GetUserById(userId);
            PatientRequestViewModel vm = new();
            if (user != null)
            {
                DateTime date = new DateTime(user.IntYear!.Value, DateTime.ParseExact(user.StrMonth!, "MMMM", CultureInfo.InvariantCulture).Month, user.IntDate!.Value);
                vm.FirstName = user.FirstName;
                vm.LastName = user.LastName;
                vm.DateOfBirth = date;
                vm.Email = user.Email;
                vm.Mobile = user.Mobile;
                vm.Street = user.Street;
                vm.City = user.City;
                vm.State = user.State!;
                vm.ZipCode = user.ZipCode;
            }
            return vm;
        }

        public void PatientSubmitInformationMe(PatientRequestViewModel vm, int? userId)
        {
            int count = _repository.GetRequestToday();
            string number = "0000" + count.ToString();
            string userdate = "0" + DateTime.Now.Day;
            string usermonth = "0" + DateTime.Now.Month;
            string useryear = DateTime.Now.Year.ToString();
            Region region = _repository.GetRegionByName(vm.State);
            string conNumber = region.Abbreviation + userdate.Substring(userdate.Length - 2) + usermonth.Substring(usermonth.Length - 2) + useryear.Substring(useryear.Length - 2) + vm.FirstName.Substring(0, 2).ToUpper() + vm.LastName.Substring(0, 2).ToUpper() + number.Substring(number.Length - 4);
            Request req = new()
            {
                RequestTypeId = 2,
                UserId = userId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                PhoneNumber = vm.Mobile,
                Email = vm.Email,
                Status = 1,
                ConfirmationNumber = conNumber,
                CreatedDate = DateTime.Now,
            };
            _repository.AddRequest(req);

            RequestClient requestClient = new()
            {
                RequestId = req.RequestId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                PhoneNumber = vm.Mobile,
                Address = vm.Room + " " + vm.Street + " " + vm.City + " " + vm.State + "," + vm.ZipCode,
                RegionId = region.RegionId,
                Notes = vm.Symptoms,
                Email = vm.Email,
                StrMonth = vm.DateOfBirth.ToString("MMMM"),
                IntDate = vm.DateOfBirth.Day,
                IntYear = vm.DateOfBirth.Year,
                Street = vm.Street,
                City = vm.City,
                State = vm.State,
                ZipCode = vm.ZipCode,
            };
            _repository.AddRequestClient(requestClient);
        }

        public void PatientSubmitInformationSomeoneElse(PatientFamilyFriendRequestViewModel vm, int? userId)
        {
            int count = _repository.GetRequestToday();
            string number = "0000" + count.ToString();
            string userdate = "0" + DateTime.Now.Day;
            string usermonth = "0" + DateTime.Now.Month;
            string useryear = DateTime.Now.Year.ToString();
            Region region = _repository.GetRegionByName(vm.PatientState);
            string conNumber = region.Abbreviation + userdate.Substring(userdate.Length - 2) + usermonth.Substring(usermonth.Length - 2) + useryear.Substring(useryear.Length - 2) + vm.PatientFirstName.Substring(0, 2).ToUpper() + vm.PatientLastName.Substring(0, 2).ToUpper() + number.Substring(number.Length - 4);
            Request req = new()
            {
                RequestTypeId = 3,//Requested by Family/Friend
                FirstName = vm.RelativeFirstName,
                LastName = vm.RelativeLastName,
                PhoneNumber = vm.RelativeMobile,
                Email = vm.RelativeEmail,
                CreatedDate = DateTime.Now,
                ConfirmationNumber = conNumber,
                Status = 1,
            };
            _repository.AddRequest(req);

            RequestClient reqclient = new()
            {
                RequestId = req.RequestId,
                FirstName = vm.PatientFirstName,
                LastName = vm.PatientLastName,
                PhoneNumber = vm.PatientMobile,
                Address = vm.PatientRoom + " " + vm.PatientStreet + " " + vm.PatientCity + " " + vm.PatientState + "," + vm.PatientZipCode,
                RegionId = region.RegionId,
                Email = vm.PatientEmail,
                Notes = vm.PatientSymptoms,
                StrMonth = vm.PatientDateOfBirth.Value.ToString("MMMM"),
                IntYear = vm.PatientDateOfBirth.Value.Year,
                IntDate = vm.PatientDateOfBirth.Value.Day,
                Street = vm.PatientRoom + " " + vm.PatientStreet,
                City = vm.PatientCity,
                State = vm.PatientState,
                ZipCode = vm.PatientZipCode,
            };
            _repository.AddRequestClient(reqclient);
        }

        public PatientProfileViewModel PatientProfile(int? userId)
        {
            User user = _repository.GetUserById(userId);
            DateOnly date = new DateOnly(user.IntYear.Value, DateOnly.ParseExact(user.StrMonth, "MMMM", CultureInfo.InvariantCulture).Month, user.IntDate.Value);
            PatientProfileViewModel vm = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = date,
                Mobile = user.Mobile,
                Email = user.Email,
                Street = user.Street,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode
            };
            return vm;
        }

        public void EditPatientProfile(PatientProfileViewModel vm, int? userId)
        {
            User user = _repository.GetUserById(userId);
            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.Email = vm.Email;
            user.Mobile = vm.Mobile;
            user.Street = vm.Street;
            user.City = vm.City;
            user.State = vm.State;
            user.ZipCode = vm.ZipCode;
            user.IntDate = vm.DateOfBirth.Value.Day;
            user.IntYear = vm.DateOfBirth.Value.Year;
            user.StrMonth = vm.DateOfBirth.Value.ToString("MMMM");
            _repository.UpdateUser(user);

            AspNetUser aspuser = _repository.GetAspNetUserById(user.AspNetUserId);
            aspuser.UserName = vm.FirstName + " " + vm.LastName;
            aspuser.Email = vm.Email;
            aspuser.PhoneNumber = vm.Mobile;
            _repository.UpdateAspNetUser(aspuser);

            List<Request> req = _repository.GetRequestsByUserId(userId);
            foreach(Request request in req)
            {
                request.FirstName = vm.FirstName;
                request.LastName = vm.LastName;
                request.PhoneNumber = vm.Mobile;
                request.Email = vm.Email;
                _repository.UpdateReqTable(request);

                RequestClient reqClient = _repository.GetReqClientByReqId(request.RequestId);
                reqClient.FirstName = vm.FirstName;
                reqClient.LastName = vm.LastName;
                reqClient.PhoneNumber = vm.Mobile;
                reqClient.Email = vm.Email;
                reqClient.StrMonth = vm.DateOfBirth.Value.ToString("MMMM");
                reqClient.IntDate = vm.DateOfBirth.Value.Day;
                reqClient.IntYear = vm.DateOfBirth.Value.Year;
                reqClient.Street = vm.Street;
                reqClient.City = vm.City;
                reqClient.State = vm.State;
                reqClient.ZipCode = vm.ZipCode;
                reqClient.Address = vm.Street +" "+ vm.City +" "+ vm.State +","+ vm.ZipCode;
                _repository.UpdateReqClient(reqClient);
            }

        }








    }
}
