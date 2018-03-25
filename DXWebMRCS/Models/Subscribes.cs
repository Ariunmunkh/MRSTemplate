using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DXWebMRCS.Models
{
    public class Subscribes
    {
        [Key]
        public int SubID { get; set; }
        [Required]
        [Display(Name = "И-мэйл")]
        public string email { get; set; }
    }
}