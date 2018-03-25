using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DXWebMRCS.Models
{
    public class jobs
    {
        [Key]
        public int JID { get; set; }
        [Required]
        [Display(Name = "Ажлын байрны нэр")]
        public string JobName { get; set; }
        [Required]
        [Display(Name = "Дэлгэрэнгүй мэдээлэл")]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дуусах хугацаа")]
        public System.DateTime DateEnd { get; set; }
    }

    public class JobDViewModel
    {
        public jobs jobs { get; set; }
        public jobdetail jobdetail { get; set; }
    }
}