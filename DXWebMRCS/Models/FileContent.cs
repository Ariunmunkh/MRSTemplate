using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DXWebMRCS.Models
{
    public class FileContent
    {
        public int Id { get; set; }
        public string TitleEng { get; set; }
        public string TitleMon { get; set; }
        public string DescriptionEng { get; set; }
        public string DescriptionMon { get; set; }
        public string Image { get; set; }
        public string FilePath { get; set; }
        [NotMapped]
        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Select File")]
        public HttpPostedFileBase File { get; set; }

    }
}