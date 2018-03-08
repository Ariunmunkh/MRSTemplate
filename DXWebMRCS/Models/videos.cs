using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DXWebMRCS.Models
{
    public class videos
    {
        [Key]
        public int VID { get; set; }
        [Required]
        [Display(Name = "Бичлэгийн нэр")]
        public string title { get; set; }
        [Required]
        [Display(Name = "Холбоос")]
        public string Link { get; set; }
    }
}