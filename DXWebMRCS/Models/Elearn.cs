using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXWebMRCS.Models
{
    public class Elearn
    {
        [Key]
        public int ELID { get; set; }
        [Required]
        [Display(Name = "Хичээлийн нэр")]
        public string LessonName { get; set; }
        [Required]
        [Display(Name = "Тайлбар")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Зураг")]
        public string Image { get; set; }
        [Display(Name = "Хичээл агуулга")]
        [AllowHtml]
        public string LessonBody { get; set; }



    }
}