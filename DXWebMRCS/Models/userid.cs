using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DXWebMRCS.Models
{
    public class userid
    {
        [Key]
        public int UID { get; set; }
        [Required]
        [Display(Name = @"Сургагч багшийн нэр")]
        public string uname { get; set; }
        public virtual ICollection<Questionnaire> Questionnaire { get; set; }
    }
}