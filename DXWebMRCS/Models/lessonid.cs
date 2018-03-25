using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DXWebMRCS.Models
{
    public class lessonid
    {
        [Key]
        public int LID { get; set; }
        [Required]
        [Display(Name = @"Хичээлийн нэр")]
        public string lname { get; set; }
        public virtual ICollection<Questionnaire> Questionnaire { get; set; }

    }
}