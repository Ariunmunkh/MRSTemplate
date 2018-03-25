using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DXWebMRCS.Models
{
    public class jobdetail
    {
        [Key]
        public int JDID { get; set; }
        [Required]
        [Display(Name = "Овог")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Нэр")]
        public string FirstName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "И-мэйл")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Утасны дугаар")]
        public string phone { get; set; }
        [Required]
        [Display(Name = "Хүйс")]
        public string gender { get; set; }
        [Display(Name = "Сонирхсон ажлын байр")]
        public string JobName { get; set; }
        [Required]
        [Display(Name = "Тайлбар")]
        public string description { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Анкет ирсэн өдөр")]
        public System.DateTime DateEnd { get; set; }

    }
}