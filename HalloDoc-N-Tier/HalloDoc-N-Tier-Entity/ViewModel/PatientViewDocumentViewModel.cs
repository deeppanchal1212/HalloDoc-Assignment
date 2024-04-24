using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class PatientViewDocumentViewModel
    {
        public string? ConfirmationNumber { get; set; }

        public int RequestId { get; set; }

        public List<DocumentsViewModel>? Documents { get; set; }

        public List<IFormFile>? file { get; set; }
    }
}
