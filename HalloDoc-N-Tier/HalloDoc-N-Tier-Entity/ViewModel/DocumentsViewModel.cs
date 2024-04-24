using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class DocumentsViewModel
    {
        public string? DocumentName { get; set; }

        public string? Uploader { get; set; }

        public DateOnly? UploadDate { get; set; }

        public int? ReqWiseFileId { get; set; }

        public bool? Isdeleted { get; set; }
    }
}
