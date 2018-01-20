using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DXWebMRCS.Models
{
    public class eService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int eServiceID { get; set; }
        [Required]
        [Display(Name = "Хэрэглэгч нэр")]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Хичээлийн нэр")]
        public int LessonId { get; set; }

        [Required]
        [Display(Name = "Хичээлийн оноо")]
        public int Score { get; set; }

        [Display(Name = "Огноо")]
        public string DateTime { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual Elearn Elearn { get; set; }
    }

    public class eServiceModel
    {
        public int eServiceid { get; set; }
        public int UserId { get; set; }
        public string LessonName { get; set; }
        public string LessonBody { get; set; }
        public string Image { get; set; }
        public int totalscore { get; set; }
        public string nowDate { get; set; }
    }
}
